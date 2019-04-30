using System;
using System.Net;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VzaarApi
{
	public class Client
	{
		public static string url = "https://api.vzaar.com/api/";
		public static string client_id = "id";
		public static string auth_token = "token";
		public static string version = "v2";
		public static bool urlAuth = false;

		public static readonly string UPLOADER = "Vzaar .NET SDK";
		public static readonly string VERSION = "2.1.0-alpha";

		public static readonly long ONE_MB = (1024 * 1024);
		public static readonly long MULTIPART_MIN_SIZE = (5 * ONE_MB);

		internal HttpClient httpClient;
		public HttpResponseHeaders httpHeaders { get; internal set; }

		public string CfgUrl { get; set; }
		public string CfgVerson { get; set; }
		public bool CfgUrlAuth { get; set; }
		public string CfgClientId { get; set; }
		public string CfgAuthToken { get; set; }

		public delegate void UploadProgressHandler(object sender, VideoUploadProgressEventArgs e);
		public event UploadProgressHandler UploadProgress = delegate { };

		public Client()
		{
			httpClient = new HttpClient();

			//initialize 
			CfgUrl = Client.url;
			CfgVerson = Client.version;
			CfgUrlAuth = Client.urlAuth;
			CfgClientId = Client.client_id;
			CfgAuthToken = Client.auth_token;
		}

		public TimeSpan HttpTimeout
		{
			set { httpClient.Timeout = value; }
			get { return httpClient.Timeout; }
		}

		public static Client GetClient()
		{
			return new Client();
		}

		public string RateLimit => CheckRates("X-RateLimit-Limit");

		public string RateRemaining => CheckRates("X-RateLimit-Remaining");

		public string RateReset => CheckRates("X-RateLimit-Reset");

		internal string CheckRates(string key)
		{
			string value = string.Empty;
			if (httpHeaders.TryGetValues(key, out var header))
				value = header.FirstOrDefault();

			return value;
		}

		internal async Task<string> HttpGetAsync(string endpoint, Dictionary<string, string> query)
		{
			Uri httpUri = BuildUri(endpoint);
			httpUri = BuildQuery(httpUri, query);

			var msg = new HttpRequestMessage
			{
				RequestUri = httpUri,
				Method = HttpMethod.Get
			};

			var jsonResponse = await HttpSendAsync(msg).ConfigureAwait(false);

			return jsonResponse;
		}

		internal async Task<string> HttpPostAsync(string path, string body)
		{
			Uri httpUri = BuildUri(path);

			var msg = new HttpRequestMessage
			{
				RequestUri = httpUri,
				Method = HttpMethod.Post
			};

			var content = new StringContent(body);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			msg.Content = content;

			var jsonResponse = await HttpSendAsync(msg).ConfigureAwait(false);

			return jsonResponse;
		}

		internal async Task<string> HttpPatchAsync(string path, string body)
		{
			Uri httpUri = BuildUri(path);

			var msg = new HttpRequestMessage
			{
				RequestUri = httpUri,
				Method = new HttpMethod("PATCH")
			};

			var content = new StringContent(body);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			msg.Content = content;

			var jsonResponse = await HttpSendAsync(msg).ConfigureAwait(false);

			return jsonResponse;

		}

		internal async Task HttpDeleteAsync(string path)
		{
			Uri httpUri = BuildUri(path);

			HttpRequestMessage msg = new HttpRequestMessage
			{
				RequestUri = httpUri,
				Method = HttpMethod.Delete
			};

			await HttpSendAsync(msg).ConfigureAwait(false);

			//if the request completed, means successful
			//no return value needed
		}

		internal virtual async Task<string> HttpSendAsync(HttpRequestMessage msg)
		{
			if (CfgUrlAuth)
			{
				var query = new Dictionary<string, string>
				{
					{"client_id", client_id},
					{ "auth_token", auth_token}
				};

				var uri = BuildQuery(msg.RequestUri, query);
				msg.RequestUri = uri;
			}
			else
			{
				msg.Headers.Add("X-Client-Id", client_id);
				msg.Headers.Add("X-Auth-Token", auth_token);
			}

#if DEBUG
			Debug.WriteLine ("\n\nRequest Method");
			Debug.WriteLine (msg.Method.ToString());
			Debug.WriteLine ("Request Uri");
			Debug.WriteLine (msg.RequestUri.AbsoluteUri);
			Debug.WriteLine ("Request Headers");
			Debug.WriteLine (msg.Headers.ToString().Trim());

			if (msg.Content != null) {
				Debug.WriteLine ("Request Content");
				string debug_content = await msg.Content.ReadAsStringAsync ().ConfigureAwait(false);
				Debug.WriteLine (debug_content);
			}
#endif

			var response = await httpClient.SendAsync(msg).ConfigureAwait(false);

			await ValidateHttpResponse(response).ConfigureAwait(false);

			httpHeaders = response.Headers;

			var bodyResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			return bodyResponse;
		}

		internal async Task ValidateHttpResponse(HttpResponseMessage response)
		{

#if DEBUG
			Debug.WriteLine ("Response StatusCode");
			Debug.WriteLine (response.StatusCode + ": "+ (int)response.StatusCode);
			Debug.WriteLine ("Response Headers");
			Debug.WriteLine (response.Headers.ToString().Trim());

			if (response.Content != null) {
				Debug.WriteLine ("Response Content");
				var debug_content = await response.Content.ReadAsStringAsync ().ConfigureAwait(false);
				Debug.WriteLine (debug_content);
			}
#endif

			switch (response.StatusCode)
			{
				case HttpStatusCode.OK:
				case HttpStatusCode.Created:
				case HttpStatusCode.NoContent:
				case HttpStatusCode.Accepted:
					break;
				case HttpStatusCode.BadRequest:
				case HttpStatusCode.Unauthorized:
				case HttpStatusCode.Forbidden:
				case HttpStatusCode.NotFound:
				case (HttpStatusCode)422: //Unprocessable Entity
				case HttpStatusCode.UpgradeRequired:
				case (HttpStatusCode)429: //Too many Requests
				case HttpStatusCode.InternalServerError:

					var error = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					var message = "StatusCode: " + response.StatusCode + "\r\n";
					message += error;

					throw new VzaarApiException(message);

				default:
					string unknown = "Unknown response. StatusCode: " + response.StatusCode;
					throw new VzaarApiException(unknown);
			}
		}

		internal async Task HttpPostS3Async(string filepath, Signature signature)
		{
			var file = new FileInfo(filepath);
			if (!file.Exists)
				throw new VzaarApiException("File does not exsist: " + filepath);

			var hostname = (string)signature["upload_hostname"];

			//move signature to Dictionary
			var postFields = new Dictionary<string, string>
			{
				{"x-amz-credential", (string) signature["x-amz-credential"]},
				{"x-amz-algorithm", (string) signature["x-amz-algorithm"]},
				{"x-amz-date", (string) signature["x-amz-date"]},
				{"x-amz-signature", (string) signature["x-amz-signature"]},
				{"x-amz-meta-uploader", Client.UPLOADER + Client.VERSION},
				{"acl", (string) signature["acl"]},
				{"bucket", (string) signature["bucket"]},
				{"policy", (string) signature["policy"]},
				{"success_action_status", (string) signature["success_action_status"]}
			};

			var key = (string)signature["key"];
			var fileKey = "";
			if (string.IsNullOrEmpty(key) == false)
				fileKey = key.Replace("${filename}", file.Name);

			postFields.Add("key", fileKey);

			long? parts = signature["parts"] as long?;

			if (parts == null)
			{
				//upload file in POST form
				using (var fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
				{
					await HttpPostMfdcAsync(hostname, file.Name, postFields, fileStream).ConfigureAwait(false);
					// response is validated in the HttpPostMfdcAsync
				}
			}
			else
			{
				if (signature["part_size_in_bytes"] == null)
					throw new VzaarApiException("File Upload: not valid 'part_size_in_bytes'");

				var chunkSize = Convert.ToInt32((long)signature["part_size_in_bytes"]);
				var buffer = new byte[chunkSize];

				var keyCache = postFields["key"];
				long chunk = 0;
				using (var fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
				{
					int bytesCount;
					while ((bytesCount = await fileStream.ReadAsync(buffer, 0, chunkSize).ConfigureAwait(false)) != 0)
					{
						string chunkKey = keyCache + "." + chunk;
						postFields.Remove("key");
						postFields.Add("key", chunkKey);

						using (var chunkStream = new MemoryStream(buffer, 0, bytesCount))
						{
							await HttpPostMfdcAsync(hostname, file.Name, postFields, chunkStream).ConfigureAwait(false);
							// response is validated in the HttpPostMfdcAsync
						}

						var progress = new VideoUploadProgressEventArgs()
						{
							totalParts = (long)parts,
							uploadedChunk = chunk
						};

						UploadProgress(this, progress);

						chunk++;
					}
				}
			}
		}

		internal async Task<string> HttpPostFormAsync(string endpoint, string filepath, Dictionary<string, string> postFields)
		{
			var file = new FileInfo(filepath);
			if (!file.Exists)
				throw new VzaarApiException("File does not exsist: " + filepath);

			//change the filepath in Dictionary to filename
			var fields = new Dictionary<string, string>();
			foreach (var item in postFields)
				fields.Add(item.Key, item.Value == filepath ? file.Name : item.Value);

			//upload file in POST form
			var httpMethod = HttpMethod.Post;
			using (var fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
			{
				return await HttpSendMfdcAsync(httpMethod, endpoint, file.Name, fields, fileStream).ConfigureAwait(false);
				// response is validated in the HttpPostMfdcAsync
			}
		}

		internal async Task<string> HttpPatchFormAsync(string endpoint, string filepath, Dictionary<string, string> postFields)
		{
			var file = new FileInfo(filepath);
			if (!file.Exists)
				throw new VzaarApiException("File does not exsist: " + filepath);

			//change the filepath in Dictionary to filename
			var fields = new Dictionary<string, string>();
			foreach (var item in postFields)
				fields.Add(item.Key, item.Value == filepath ? file.Name : item.Value);

			//upload file in PATCH method
			var httpMethod = new HttpMethod("PATCH");
			using (var fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
			{
				return await HttpSendMfdcAsync(httpMethod, endpoint, file.Name, fields, fileStream).ConfigureAwait(false);
				// response is validated in the HttpPostMfdcAsync
			}
		}

		internal virtual async Task<string> HttpSendMfdcAsync(HttpMethod httpMethod, string endpoint, string filename, Dictionary<string, string> fields, Stream stream)
		{
			var httpUri = BuildUri(endpoint);

			var msg = new HttpRequestMessage
			{
				RequestUri = httpUri,
				Method = httpMethod
			};

			var mfdc = new MultipartFormDataContent();

			//data in POST form
			StringContent postField;

			Debug.WriteLine("\n\nRequest Method");
			Debug.WriteLine(msg.Method.ToString());
			Debug.WriteLine("Request Uri");
			Debug.WriteLine(msg.RequestUri.AbsoluteUri);
			Debug.WriteLine("Request Headers");
			Debug.WriteLine(msg.Headers.ToString().Trim());
			Debug.WriteLine("Request Content");

			string fileField = ""; //get the post Field name for the file
			foreach (var item in fields)
			{
				if (item.Value != filename)
				{
					postField = new StringContent(item.Value);
					mfdc.Add(postField, '"' + item.Key + '"');
				}
				else
				{
					fileField = item.Key;
				}

				Debug.WriteLine(item.Key + ": " + item.Value);
			}

			StreamContent postFile = new StreamContent(stream);
			mfdc.Add(postFile, '"' + fileField + '"', filename);

			Debug.WriteLine("file: " + filename);

			msg.Content = mfdc;

			return await HttpSendAsync(msg).ConfigureAwait(false);
		}

		internal virtual async Task HttpPostMfdcAsync(string hostname, string filename, Dictionary<string, string> fields, Stream stream)
		{
			var msg = new HttpRequestMessage
			{
				RequestUri = new Uri(hostname),
				Method = HttpMethod.Post
			};

			var mfdc = new MultipartFormDataContent();

			//upload signature data in POST form
			StringContent postField;

			Debug.WriteLine("\n\nRequest Method");
			Debug.WriteLine(msg.Method.ToString());
			Debug.WriteLine("Request Uri");
			Debug.WriteLine(msg.RequestUri.AbsoluteUri);
			Debug.WriteLine("Request Headers");
			Debug.WriteLine(msg.Headers.ToString().Trim());
			Debug.WriteLine("Request Content");

			foreach (var item in fields)
			{
				postField = new StringContent(item.Value);
				mfdc.Add(postField, item.Key);

				Debug.WriteLine(item.Key + ": " + item.Value);
			}

			var postFile = new StreamContent(stream);
			postFile.Headers.ContentType = new MediaTypeHeaderValue("binary/octet-stream");
			mfdc.Add(postFile, "file", filename);

			Debug.WriteLine("file: " + filename);

			msg.Content = mfdc;

			var response = await this.httpClient.SendAsync(msg).ConfigureAwait(false);

			await ValidateS3Response(response).ConfigureAwait(false);
		}

		internal async Task ValidateS3Response(HttpResponseMessage response)
		{
			string bodyResponse = "";

			if (response.Content != null)
				bodyResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			Debug.WriteLine("Response StatusCode");
			Debug.WriteLine(response.StatusCode + ": " + (int)response.StatusCode);
			Debug.WriteLine("Response Headers");
			Debug.WriteLine(response.Headers.ToString().Trim());
			Debug.WriteLine("Response Content");
			Debug.WriteLine(bodyResponse);

			switch (response.StatusCode)
			{
				case HttpStatusCode.Created:
					break;
				default:
					var message = "StatusCode: " + response.StatusCode + "\r\n";
					message += bodyResponse;

					throw new VzaarApiException(message);
			}
		}

		internal Uri BuildUri(string endpoint)
		{
			//build uri base
			var uri = CfgUrl.TrimEnd('/') + "/" + CfgVerson;
			//add endpoint
			uri += "/" + endpoint;

			return new Uri(uri);
		}

		internal Uri BuildQuery(Uri uri, Dictionary<string, string> query)
		{
			var reqUrl = new UriBuilder(uri);

			if (query != null)
			{
				string queryString = "";
				foreach (string key in query.Keys)
				{
					string item = key + "=" + query[key];

					if (queryString == "")
						queryString = item;
					else
						queryString += "&" + item;
				}

				if (reqUrl.Query.Length > 1)
					reqUrl.Query = reqUrl.Query.Substring(1) + "&" + queryString;
				else
					reqUrl.Query = queryString;
			}

			return reqUrl.Uri;

		}

	}//end class
}//end namespace 