using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using VzaarApi;

namespace tests
{
	internal class S3ClientMock : ClientMock
	{
		public S3ClientMock(MockResponse type) : base(type)
		{
		}

		internal void TestCreate_5mb()
		{
			Dictionary<string, object> tokens = new Dictionary<string, object>() {
				{ "title", "Example video" },
				{"filepath", "../../src/Fixture/movie-5mb.mp4"}
			};

			var task = Video.CreateAsync(tokens, new S3ClientMock(MockResponse.Signature));
			task.Wait();
		}

		internal void TestCreate_1mb()
		{
			Dictionary<string, object> tokens = new Dictionary<string, object>() {
				{ "title", "Example video" },
				{"filepath", "../../src/Fixture/movie-1mb.mp4"}
			};

			var task = Video.CreateAsync(tokens, new S3ClientMock(MockResponse.Signature));
			task.Wait();
		}

		internal override async Task HttpPostMfdcAsync(string hostname, string filename, Dictionary<string, string> fields, Stream stream)
		{
			var response = new HttpResponseMessage(HttpStatusCode.Created)
			{
				Content = new StringContent("")
			};

			if (fields.ContainsKey("x-amz-signature") == false)
				response.StatusCode = HttpStatusCode.Forbidden;

			if (fields.ContainsKey("key"))
			{
				if (String.IsNullOrEmpty(fields["key"]))
					response.StatusCode = HttpStatusCode.Forbidden;
			}

			if (filename == "movie-5mb.mp4")
			{
				// this should be MemoryStream
				if (!(stream is MemoryStream))
					response.StatusCode = HttpStatusCode.UnsupportedMediaType;
			}

			if (filename == "movie-1mb.mp4")
			{
				// this should be FileStream
				if (!(stream is FileStream))
					response.StatusCode = HttpStatusCode.UnsupportedMediaType;
			}

			switch (responseType)
			{
				case MockResponse.UploadFailed:
					response.StatusCode = HttpStatusCode.Forbidden;
					break;
			}

			await ValidateS3Response(response);
		}

	}//end class
}

