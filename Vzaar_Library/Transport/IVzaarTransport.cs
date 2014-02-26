using System.Collections.Generic;

namespace VzaarAPI.Transport
{
	/// <summary>
	/// The vzaar transport interface
	/// </summary>
	public interface IVzaarTransport
	{
		/// <summary>
		/// Set the base API URL, such as http://vzaar.com/api/
		/// </summary>
		/// <param name="url">The base API URL</param>
		void SetUrl(string url);

		/// <summary>
		/// Send any get request. If OAuth credentials have been added then OAuth
		/// authentication headers should be sent with each request.
		/// </summary>
		/// <param name="uri">The method URI</param>
		/// <param name="parameters">Request parameters</param>
		/// <returns>The response details</returns>
		VzaarTransportResponse SendGetRequest(string uri, Dictionary<string, object> parameters);


		/// <summary>
		/// Set the OAuth 2 party tokens. If these are supplied then all calls to
		/// <a href="http://vzaar.com/">vzaar.com</a> will send OAuth credentials.
		/// </summary>
		/// <param name="token">The users login name</param>
		/// <param name="secret">The secret token supplied by vzaar</param>
		void SetOAuthTokens(string token, string secret);


		/// <summary>
		/// Send a post request with an XML body. If OAuth credentials have been 
		/// added then OAuth authentication headers are sent.
		/// </summary>
		/// <param name="uri">The method URI</param>
		/// <param name="xml">The XML content for the body of the request</param>
		/// <returns>The call response</returns>
		VzaarTransportResponse SendPostXmlRequest(string uri, string xml);

        VzaarTransportResponse SendPostXmlRequest(string uri, string xml, string method);


		/// <summary>
		/// Upload a file to S3. This method doesn't use OAuth. If the callback is
		/// not null then the method should send progress reports to the callback.
		/// There is no need to call the error or complete calls on the callback
		/// as these are done by the Vzaar calling class.
		/// </summary>
		/// <param name="url">The full url to the S3 bucket</param>
		/// <param name="parameters">The query parameters to send</param>
		/// <param name="file">The file to upload</param>       
		/// <param name="signature">The upload signature</param>
		/// <returns>The call response</returns>
		VzaarTransportResponse UploadToS3(string url, Dictionary<string, object> parameters, string file, UploadSignature signature);
	}
}
