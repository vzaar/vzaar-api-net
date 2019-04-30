using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VzaarApi
{
	internal class RecordsList
	{
		internal Client RecordClient { get; set; }
		internal string RecordEndpoint { get; set; }

		internal JObject Data { get; set; }
		internal List<Record> List { get; }

		internal RecordsList(string endpoint)
			: this(endpoint, Client.GetClient())
		{
		}

		internal RecordsList(string endpoint, Client client)
		{
			Data = new JObject();
			List = new List<Record>();

			RecordClient = client;
			RecordEndpoint = endpoint;
		}

		internal virtual void UpdateList(string json)
		{
			//set new data
			Data = JObject.Parse(json);

			//validate data object
			JToken token;
			if (Data.TryGetValue("data", out token) == false)
				throw new VzaarApiException("Received data malformed: Missing 'data' token");

			if (token.Type != JTokenType.Array)
				throw new VzaarApiException("Received data malformed: 'data' value is not array");

			if (Data.TryGetValue("meta", out token) == false)
				throw new VzaarApiException("Received data malformed: Missing 'meta' token");

			JObject item = JObject.Parse(token.ToString());

			if (item.TryGetValue("links", out token) == false)
				throw new VzaarApiException("Received data malformed: Missing 'links' token");

			item = JObject.Parse(token.ToString());

			if (item.TryGetValue("first", out token) == false)
				throw new VzaarApiException("Received data malformed: Missing 'first' token");

			if (item.TryGetValue("last", out token) == false)
				throw new VzaarApiException("Received data malformed: Missing 'last' token");

			if (item.TryGetValue("next", out token) == false)
				throw new VzaarApiException("Received data malformed: Missing 'next' token");

			if (item.TryGetValue("previous", out token) == false)
				throw new VzaarApiException("Received data malformed: Missing 'previous' token");

			//initialize object
			List.Clear();

			var recordsArray = (JArray)Data["data"];

			foreach (var element in recordsArray)
			{
				var record = new JObject
				{
					["data"] = element
				};

				Record toList = new Record(RecordEndpoint, RecordClient)
				{
					Data = record
				};

				List.Add(toList);
			}
		}

		internal virtual Task Read(Dictionary<string, string> query)
		{
			string path = "";

			return Read(path, query);
		}

		internal virtual async Task Read(string endpoint, Dictionary<string, string> query)
		{
			var responseJson = await RecordClient.HttpGetAsync(RecordEndpoint + endpoint, query).ConfigureAwait(false);

			UpdateList(responseJson);
		}

		internal Dictionary<string, string> ExtractUriQuery(string link)
		{
			var linkUri = new UriBuilder((string)link);
			var linkString = linkUri.Query.Substring(1);
			var linkQuery = linkString.Split('&');

			var query = new Dictionary<string, string>();

			foreach (string q in linkQuery)
			{
				string[] item = q.Split('=');

				string name = item[0].Trim();
				string value = item[1].Trim();

				if (query.ContainsKey(name) == false)
					query.Add(name, value);
			}

			return query;
		}

		internal virtual Task<bool> First()
		{
			return JumpToPage("first");
		}

		internal virtual Task<bool> Next()
		{
			return JumpToPage("next");
		}

		internal virtual Task<bool> Previous()
		{
			return JumpToPage("previous");
		}

		internal virtual Task<bool> Last()
		{
			return JumpToPage("last");
		}

		private async Task<bool> JumpToPage(string linkName)
		{
			var link = Data["meta"]["links"][linkName];

			if (link.Type != JTokenType.Null)
			{
				await GetPage((string)link).ConfigureAwait(false);

				return true;
			}

			return false;
		}

		private Task GetPage(string link)
		{
			var query = ExtractUriQuery(link);

			return Read(query);
		}
	}
}