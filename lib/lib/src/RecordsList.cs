using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace VzaarApi
{
	internal class RecordsList
	{
		public Client RecordClient { get; set; }
		public string RecordEndpoint { get; set; }

		public JObject Data { get; set; }
		public List<Record> List { get; }

		public RecordsList(string endpoint)
			: this(endpoint, Client.GetClient())
		{
		}

		public RecordsList(string endpoint, Client client)
		{
			Data = new JObject();
			List = new List<Record>();

			RecordClient = client;
			RecordEndpoint = endpoint;
		}

		public virtual void UpdateList(string json)
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

			for (int i = 0; i < recordsArray.Count; i++)
			{

				var element = recordsArray[i];

				var record = new JObject();
				record["data"] = element;

				Record toList = new Record(RecordEndpoint, RecordClient);
				toList.Data = record;

				List.Add(toList);

			}

		}

		public virtual void Read(Dictionary<string, string> query)
		{

			string path = "";

			Read(path, query);
		}

		public virtual void Read(string endpoint, Dictionary<string, string> query)
		{

			var task = RecordClient.HttpGetAsync(RecordEndpoint + endpoint, query);
			task.Wait();

			UpdateList(task.Result);

		}

		internal Dictionary<string, string> ExtractUriQuery(string link)
		{

			UriBuilder linkUri = new UriBuilder((string)link);
			string linkString = linkUri.Query.Substring(1);
			string[] linkQuery = linkString.Split('&');

			Dictionary<string, string> query = new Dictionary<string, string>();

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

		internal void GetPage(string link)
		{

			var query = ExtractUriQuery(link);
			Read(query);

		}

		public virtual bool First()
		{

			var link = Data["meta"]["links"]["first"];

			if (link.Type != JTokenType.Null)
			{

				GetPage((string)link);

				return true;

			}
			else
			{

				return false;
			}
		}

		public virtual bool Next()
		{

			var link = Data["meta"]["links"]["next"];

			if (link.Type != JTokenType.Null)
			{

				GetPage((string)link);

				return true;

			}
			else
			{

				return false;

			}
		}

		public virtual bool Previous()
		{

			var link = Data["meta"]["links"]["previous"];

			if (link.Type != JTokenType.Null)
			{

				GetPage((string)link);

				return true;

			}
			else
			{

				return false;

			}
		}

		public virtual bool Last()
		{

			var link = Data["meta"]["links"]["last"];

			if (link.Type != JTokenType.Null)
			{

				GetPage((string)link);

				return true;

			}
			else
			{

				return false;

			}
		}

	}
}