using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Web;
using VzaarAPI.Transport.S3Amazon;

namespace VzaarAPI.Transport.HttpClient
{
	/// <summary>
	/// This class is responsible for managing the specifics of the HTTP protocol.
	/// </summary>
	public class HttpClientTransport : IVzaarTransport
	{
		#region Private Members

		private HttpWebRequest _client;
		private OAuthConsumer _consumer;
		private string _apiUrl;

		#endregion

		#region Methods

		/// <summary>
		/// Send any get request. If OAuth credentials have been added then OAuth
		/// authentication headers are sent.
		/// </summary>
		/// <param name="uri">The uri of the request which is appended to the API URL</param>
		/// <param name="parameters">Request parameters.</param>
		/// <returns></returns>
		public VzaarTransportResponse SendGetRequest(string uri, Dictionary<string, object> parameters)
		{
			try
			{
				var url = ConstructGetUrl(uri, parameters);

				Debug.WriteLine("\n>> Request URL = " + url + "\n");
				Debug.WriteLine(">> Request Method = GET\n");

				ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

				ServicePointManager.Expect100Continue = false;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
				var request = (HttpWebRequest)WebRequest.Create(url);

				request.Method = "GET";

				if(_consumer != null)
					_consumer.Sign(request);
				
				return ExcuteMethod(request);

			}
			catch(VzaarException ex)
			{
				// Rethrow the Vzaar Exception
				throw new VzaarException(ex.Message);
			}
			catch(Exception ex)
			{
				throw new VzaarException(ex.Message);
			}
		}

		/// <summary>
		/// Send a post request with an XML body. If OAuth credentials have been 
		/// added then OAuth authentication headers are sent.
		/// </summary>
		/// <param name="uri">The request URI</param>
		/// <param name="xml">The XML content for the body of the request</param>
		/// <returns>The call response </returns>       
		public VzaarTransportResponse SendPostXmlRequest(string uri,string xml)
		{
			try
			{
				var url = _apiUrl + uri;

				Debug.WriteLine("\n>> Request URL = " + url + "\n");
				Debug.WriteLine(">> Request Method = POST\n");
				Debug.WriteLine(">> Request Body = >> " + xml);

				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

				// Create the URL, and assign a post request with XML content type 
				_client = (HttpWebRequest)WebRequest.Create(url);
				_client.Method = "POST";
				_client.ContentType = "text/xml";

				if(_consumer != null)
					_consumer.Sign(_client);

				// Encode the xml and convert it to bytes for the request stream
				var bytes = Encoding.UTF8.GetBytes(xml);
				_client.ContentLength = bytes.Length;

				// Write the XML request to the request stream
				using(var requestStream = _client.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}

				// Send the request to be executed
				var resp = ExcuteMethod(_client);

				return resp;
			}
			catch(Exception e)
			{
				Debug.WriteLine(e.Message);
				throw new VzaarException(e.Message);
			}
		}

        public VzaarTransportResponse SendPostXmlRequest(string uri, string xml, string method)
        {
            try
            {
                var url = _apiUrl + uri;

                Debug.WriteLine("\n>> Request URL = " + url + "\n");
                Debug.WriteLine(">> Request Method = POST\n");
                Debug.WriteLine(">> Request Body = >> " + xml);

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                // Create the URL, and assign a post request with XML content type 
                _client = (HttpWebRequest)WebRequest.Create(url);
                _client.Method = method.ToUpper();
                _client.ContentType = "text/xml";

                if (_consumer != null)
                    _consumer.Sign(_client);

                // Encode the xml and convert it to bytes for the request stream
                var bytes = Encoding.UTF8.GetBytes(xml);
                _client.ContentLength = bytes.Length;

                // Write the XML request to the request stream
                using (var requestStream = _client.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }

                // Send the request to be executed
                var resp = ExcuteMethod(_client);

                return resp;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new VzaarException(e.Message);
            }
        }

		/// <summary>
		/// Upload a file to S3. This request doesn't use OAuth. If the callback is
		/// not null then the request should send progress reports to the callback.
		/// There is no need to call the error or complete calls on the callback
		/// as these are done by the Vzaar calling class.
		/// </summary>
		/// <param name="url">The full url to the S3 bucket</param>
		/// <param name="parameters">The query parameters to send</param>
		/// <param name="file">The file to upload</param>        
		/// <param name="signature">The signature object</param>
		/// <returns>The call response </returns>
		public VzaarTransportResponse UploadToS3(string url, Dictionary<string, object> parameters, string file, UploadSignature signature)
		{
			VzaarTransportResponse response;

			try
			{
				response = UploadFileToRemoteUrl(url, parameters, signature);
			}
			catch(Exception e)
			{
				throw new VzaarException(e.Message, e);
			}

			return response;
		}

		/// <summary>
		/// Set the base API URL, such as http://vzaar.com/api/
		/// </summary>
		/// <param name="url">The base API URL</param>
		public void SetUrl(string url)
		{
			if(!url.EndsWith("/"))
			{
				url += "/";
			}

			if(!url.EndsWith("api/"))
			{
				url += "api/";
			}

			_apiUrl = url;
		}

		/// <summary>
		/// Set the OAuth 2 party tokens. If these are supplied then all calls to
		/// <a href="http://vzaar.com/">vzaar.com</a> will send OAuth credentials.
		/// </summary>
		/// <param name="token">The secret token supplied by vzaar</param>
		/// <param name="secret">The users login name</param>
		public void SetOAuthTokens(string token, string secret)
		{
			if(token == null)
			{
				_consumer = null;
			}
			else
			{
				_consumer = new OAuthConsumer("", "", OAuth.OAuthBase.SignatureTypes.HMACSHA1);
				_consumer.SetTokenWithSecret(token, secret);
			}
		}
		#endregion

		#region Private Methods

		/// <summary>
		/// Upload the file to a remote Url, typically Amazon's AWS S3 servers using a
		/// multipart/form post.
		/// </summary>
		/// <param name="url">The url to upload the file to</param>
		/// <param name="nvc">The collection of parameters to use in the form post operation</param>
		/// <param name="signature">The signature object</param>
		/// <returns>A response wrapper</returns>
		private static VzaarTransportResponse UploadFileToRemoteUrl(string url, Dictionary<string, object> nvc, UploadSignature signature)
		{
			var response = FormUpload.MultipartFormDataPost(url, nvc, signature.GetAccessKeyId(), signature.GetSignature());

			return response;
		}

		/// <summary>
		/// Construct the url from the apiUrl and uri and parameters. The parameter
		/// values are encoded with UTF-8.
		/// </summary>
		/// <param name="uri">The request uri</param>
		/// <param name="parameters">The query parameters</param>
		/// <returns></returns>
		private string ConstructGetUrl(string uri, Dictionary<string, object> parameters)
		{
			var url = new StringBuilder(_apiUrl);
			url.Append(uri);

			if(parameters != null)
			{
				var first = true;

				foreach(var entry in parameters)
				{
					var value = (string)entry.Value;

					if(value == null || value.Equals("null"))
						continue;

					try
					{
						url.Append(first ? '?' : '&')
						   .Append(entry.Key)
						   .Append('=')
						   .Append(HttpUtility.UrlEncode(value, Encoding.UTF8));

					}
					catch(Exception)
					{
						url.Append(entry.Value);
					}
					first = false;
				}
			}
			
			return url.ToString();
		}


		/// <summary>
		/// Execute an HttpMethod, if OAuth credentials have been added then OAuth
		/// authentication headers are sent. 
		/// </summary>
		/// <param name="request">The request to execute</param>
		/// <returns>The response wrapper</returns>
		private static VzaarTransportResponse ExcuteMethod(HttpWebRequest request)
		{
			VzaarTransportResponse transportResponse;
			var exception = false;

			try
			{
				// Get HTTP response
				var response = (HttpWebResponse)request.GetResponse();
				var stream = response.GetResponseStream();
				transportResponse = new VzaarTransportResponse(response.StatusCode, response.StatusDescription, stream);
			}
			catch(WebException ex)
			{
				exception = true;
				transportResponse = ex.Response != null ? new VzaarTransportResponse(((HttpWebResponse)ex.Response).StatusCode, ex.Message, ex.Response.GetResponseStream()) : new VzaarTransportResponse(HttpStatusCode.BadRequest, ex.Message, null);
			}
			catch(Exception ex)
			{
				exception = true;
				transportResponse = new VzaarTransportResponse(HttpStatusCode.BadRequest, ex.Message, null);
			}

			if(exception)
				Debug.WriteLine("<< Exception = " + transportResponse.GetStatusLine() + "\n");
			Debug.WriteLine("<< Response Code = " + transportResponse.GetStatusCode() + "\n");
			Debug.WriteLine("<< Response Line = " + transportResponse.GetStatusLine() + "\n");

			return transportResponse;
		}

		#endregion
	}
}