using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using VzaarApi;

namespace tests
{
	[TestFixture()]
	public class ClientTest
	{
		[Test()]
		public void GetClient()
		{
			var client = Client.GetClient();

			Assert.AreEqual(client.CfgClientId, Client.client_id);
			Assert.AreEqual(client.CfgAuthToken, Client.auth_token);
			Assert.AreEqual(client.CfgUrlAuth, Client.urlAuth);
			Assert.AreEqual(client.CfgUrl, Client.url);
			Assert.AreEqual(client.CfgVerson, Client.version);

			Assert.IsInstanceOf<HttpClient>(client.httpClient);

		}

		[Test()]
		public void NewClient()
		{
			var client = new Client()
			{
				CfgClientId = "client_object",
				CfgAuthToken = "token_object",
				CfgUrlAuth = false,
				CfgVerson = "vY",
				CfgUrl = "https://example.object.com"
			};

			Assert.AreEqual(client.CfgClientId, "client_object");
			Assert.AreEqual(client.CfgAuthToken, "token_object");
			Assert.AreEqual(client.CfgUrlAuth, false);
			Assert.AreEqual(client.CfgUrl, "https://example.object.com");
			Assert.AreEqual(client.CfgVerson, "vY");

			Assert.IsInstanceOf<HttpClient>(client.httpClient);

			var client2 = new Client();

			Assert.AreEqual(client2.CfgClientId, Client.client_id);
			Assert.AreEqual(client2.CfgAuthToken, Client.auth_token);
			Assert.AreEqual(client2.CfgUrlAuth, Client.urlAuth);
			Assert.AreEqual(client2.CfgUrl, Client.url);
			Assert.AreEqual(client2.CfgVerson, Client.version);

			Assert.IsInstanceOf<HttpClient>(client2.httpClient);
		}

		[Test()]
		public void BuildUri()
		{
			var client = new Client()
			{
				CfgClientId = "client_object",
				CfgAuthToken = "token_object",
				CfgUrlAuth = false,
				CfgVerson = "vY",
				CfgUrl = "https://example.object.com"
			};

			var uri = client.BuildUri("endpoint");

			Assert.AreEqual(uri, new Uri("https://example.object.com/vY/endpoint"));

			var client2 = Client.GetClient();
			var uri2 = client2.BuildUri("endpoint");

			Assert.AreEqual(uri2, new Uri("https://api.vzaar.com/api/v2/endpoint"));
		}

		[Test()]
		public void BuildQuery()
		{

			var client = new Client();

			var uri = new Uri("https://example.static.com/vX/endpoint");

			var query = new Dictionary<string, string>() {
				{ "param1","value1" }
			};

			var queryUri = client.BuildQuery(uri, query);

			Assert.AreEqual(queryUri, new Uri("https://example.static.com/vX/endpoint?param1=value1"));

			var query2 = new Dictionary<string, string>() {
				{ "param2","value2" }
			};

			var queryAdd = client.BuildQuery(queryUri, query2);

			Assert.AreEqual(queryAdd, new Uri("https://example.static.com/vX/endpoint?param1=value1&param2=value2"));
		}

		[Test()]
		public async Task ValidateHttpResponse()
		{
			var client = new Client();

			HttpResponseMessage response = new HttpResponseMessage();
			response.Content = new StringContent("");

			try
			{
				response.StatusCode = HttpStatusCode.Created;
				await client.ValidateHttpResponse(response);
			}
			catch
			{
				//the StatusCode should NOT throw
				//the assert has to fail
				bool assert = true;
				Assert.IsFalse(assert);
			}

			try
			{
				response.StatusCode = HttpStatusCode.OK;
				await client.ValidateHttpResponse(response);
			}
			catch
			{
				//the StatusCode should NOT throw
				//the assert has to fail
				bool assert = true;
				Assert.IsFalse(assert);
			}

			try
			{
				response.StatusCode = HttpStatusCode.NoContent;
				await client.ValidateHttpResponse(response);
			}
			catch
			{
				//the StatusCode should NOT throw
				//the assert has to fail
				bool assert = true;
				Assert.IsFalse(assert);
			}

			try
			{
				response.StatusCode = HttpStatusCode.Accepted;
				await client.ValidateHttpResponse(response);
			}
			catch
			{
				//the StatusCode should NOT throw
				//the assert has to fail
				bool assert = true;
				Assert.IsFalse(assert);
			}
		}

		[Test()]
		public async Task ValidateHttpResponseThrows1()
		{
			var client = new Client();

			HttpResponseMessage response = new HttpResponseMessage();
			response.Content = new StringContent("");

			string expected = "StatusCode: " + HttpStatusCode.BadRequest;
			try
			{
				response.StatusCode = HttpStatusCode.BadRequest;
				await client.ValidateHttpResponse(response);

				//the StatusCode throws
				//the assert has to fail
				bool assert = true;
				Assert.IsFalse(assert);
			}
			catch (VzaarApiException ve)
			{
				StringAssert.Contains(expected, ve.Message);
			}
			catch (Exception e)
			{
				if (e is AggregateException)
				{
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten();

					foreach (var fe in flatten.InnerExceptions)
					{
						if (fe is VzaarApiException)
							StringAssert.Contains(expected, fe.Message);
					}
				}
			}
		}

		[Test()]
		public async Task ValidateHttpResponseThrows2()
		{
			var client = new Client();

			HttpResponseMessage response = new HttpResponseMessage();
			response.Content = new StringContent("");

			string expected = "StatusCode: " + HttpStatusCode.Unauthorized;
			try
			{
				response.StatusCode = HttpStatusCode.Unauthorized;
				await client.ValidateHttpResponse(response);

				//the StatusCode throws
				//the assert has to fail
				bool assert = true;
				Assert.IsFalse(assert);
			}
			catch (VzaarApiException ve)
			{
				StringAssert.Contains(expected, ve.Message);
			}
			catch (Exception e)
			{
				if (e is AggregateException)
				{
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten();

					foreach (var fe in flatten.InnerExceptions)
					{
						if (fe is VzaarApiException)
							StringAssert.Contains(expected, fe.Message);
					}
				}
			}
		}

		[Test()]
		public async Task ValidateHttpResponseThrows3()
		{
			var client = new Client();

			HttpResponseMessage response = new HttpResponseMessage();
			response.Content = new StringContent("");

			string expected = "StatusCode: " + HttpStatusCode.Forbidden;
			try
			{
				response.StatusCode = HttpStatusCode.Forbidden;
				await client.ValidateHttpResponse(response);

				//the StatusCode throws
				//the assert has to fail
				bool assert = true;
				Assert.IsFalse(assert);
			}
			catch (VzaarApiException ve)
			{
				StringAssert.Contains(expected, ve.Message);
			}
			catch (Exception e)
			{
				if (e is AggregateException)
				{
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten();

					foreach (var fe in flatten.InnerExceptions)
					{
						if (fe is VzaarApiException)
							StringAssert.Contains(expected, fe.Message);
					}
				}
			}
		}

		[Test()]
		public async Task ValidateHttpResponseThrows4()
		{
			var client = new Client();

			HttpResponseMessage response = new HttpResponseMessage();
			response.Content = new StringContent("");

			string expected = "StatusCode: " + HttpStatusCode.NotFound;
			try
			{
				response.StatusCode = HttpStatusCode.NotFound;
				await client.ValidateHttpResponse(response);

				//the StatusCode throws
				//the assert has to fail
				bool assert = true;
				Assert.IsFalse(assert);
			}
			catch (VzaarApiException ve)
			{
				StringAssert.Contains(expected, ve.Message);
			}
			catch (Exception e)
			{
				if (e is AggregateException)
				{
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten();

					foreach (var fe in flatten.InnerExceptions)
					{
						if (fe is VzaarApiException)
							StringAssert.Contains(expected, fe.Message);
					}
				}
			}
		}

		[Test()]
		public async Task ValidateHttpResponseThrows5()
		{
			var client = new Client();

			HttpResponseMessage response = new HttpResponseMessage();
			response.Content = new StringContent("");

			string expected = "StatusCode: 422";
			try
			{
				response.StatusCode = (HttpStatusCode)422;
				await client.ValidateHttpResponse(response);

				//the StatusCode throws
				//the assert has to fail
				bool assert = true;
				Assert.IsFalse(assert);
			}
			catch (VzaarApiException ve)
			{
				StringAssert.Contains(expected, ve.Message);
			}
			catch (Exception e)
			{
				if (e is AggregateException)
				{
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten();

					foreach (var fe in flatten.InnerExceptions)
					{
						if (fe is VzaarApiException)
							StringAssert.Contains(expected, fe.Message);
					}
				}
			}
		}

		[Test()]
		public async Task ValidateHttpResponseThrows6()
		{
			var client = new Client();

			HttpResponseMessage response = new HttpResponseMessage();
			response.Content = new StringContent("");

			string expected = "StatusCode: 429";
			try
			{
				response.StatusCode = (HttpStatusCode)429;
				await client.ValidateHttpResponse(response);

				//the StatusCode throws
				//the assert has to fail
				bool assert = true;
				Assert.IsFalse(assert);
			}
			catch (VzaarApiException ve)
			{
				StringAssert.Contains(expected, ve.Message);
			}
			catch (Exception e)
			{
				if (e is AggregateException)
				{
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten();

					foreach (var fe in flatten.InnerExceptions)
					{
						if (fe is VzaarApiException)
							StringAssert.Contains(expected, fe.Message);
					}
				}
			}
		}

		[Test()]
		public async Task ValidateHttpResponseThrows7()
		{
			var client = new Client();

			HttpResponseMessage response = new HttpResponseMessage();
			response.Content = new StringContent("");

			string expected = "StatusCode: " + HttpStatusCode.InternalServerError;

			try
			{
				response.StatusCode = HttpStatusCode.InternalServerError;
				await client.ValidateHttpResponse(response);

				//the StatusCode throws
				//the assert has to fail
				bool assert = true;
				Assert.IsFalse(assert);
			}
			catch (VzaarApiException ve)
			{
				StringAssert.Contains(expected, ve.Message);
			}
			catch (Exception e)
			{
				if (e is AggregateException)
				{
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten();

					foreach (var fe in flatten.InnerExceptions)
					{
						if (fe is VzaarApiException)
							StringAssert.Contains(expected, fe.Message);
					}
				}
			}
		}

		[Test()]
		public async Task ValidateHttpResponseThrows8()
		{
			var client = new Client();

			HttpResponseMessage response = new HttpResponseMessage();
			response.Content = new StringContent("");

			string expected = "Unknown response. StatusCode: " + HttpStatusCode.UnsupportedMediaType;

			try
			{
				response.StatusCode = HttpStatusCode.UnsupportedMediaType;
				await client.ValidateHttpResponse(response);

				//the StatusCode throws
				//the assert has to fail
				bool assert = true;
				Assert.IsFalse(assert);
			}
			catch (VzaarApiException ve)
			{
				StringAssert.Contains(expected, ve.Message);
			}
			catch (Exception e)
			{
				if (e is AggregateException)
				{
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten();

					foreach (var fe in flatten.InnerExceptions)
					{
						if (fe is VzaarApiException)
							StringAssert.Contains(expected, fe.Message);
					}
				}
			}
		}

		[Test()]
		public async Task ValidateHttpResponseThrows9()
		{
			var client = new Client();

			HttpResponseMessage response = new HttpResponseMessage();
			response.Content = new StringContent("");

			string expected = "StatusCode: " + HttpStatusCode.UpgradeRequired;

			try
			{
				response.StatusCode = HttpStatusCode.UpgradeRequired;
				await client.ValidateHttpResponse(response);

				//the StatusCode throws
				//the assert has to fail
				bool assert = true;
				Assert.IsFalse(assert);
			}
			catch (VzaarApiException ve)
			{
				StringAssert.Contains(expected, ve.Message);
			}
			catch (Exception e)
			{
				if (e is AggregateException)
				{
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten();

					foreach (var fe in flatten.InnerExceptions)
					{
						if (fe is VzaarApiException)
							StringAssert.Contains(expected, fe.Message);
					}
				}
			}
		}
	}
}