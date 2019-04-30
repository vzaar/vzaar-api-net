using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VzaarApi
{
	internal class Record
	{
		internal Client RecordClient { get; set; }
		internal string RecordEndpoint { get; set; }

		internal JObject Data { get; set; }
		internal Dictionary<string, object> Parameters { get; set; }
		internal JObject cache;

		internal string empty_record = @"{'data':{}}";

		internal Record(string endpoint, Client client)
		{
			Data = JObject.Parse(empty_record);
			Parameters = new Dictionary<string, object>();
			cache = new JObject();

			RecordClient = client;
			RecordEndpoint = endpoint;
		}

		internal object ToTypeDef(Type type)
		{
			var data = Data["data"];

			var item = data.ToObject(type);

			return item;
		}

		internal bool Edited => Parameters.Any();

		internal object this[string index]
		{
			get
			{

				var data = JObject.Parse(Data["data"].ToString());

				object cast = null;

				JToken token;
				if (cache.TryGetValue(index, out token) || data.TryGetValue(index, out token))
				{

					switch (token.Type)
					{
						case JTokenType.String:
							cast = (string)token;
							break;
						case JTokenType.Integer:
							cast = (long)token;
							break;
						case JTokenType.Float:
							cast = (double)token;
							break;
						case JTokenType.Null:
							cast = null;
							break;
						case JTokenType.Date:
							cast = (DateTime)token;
							break;
						case JTokenType.Array:
							cast = (List<Dictionary<string, object>>)token.ToObject(typeof(List<Dictionary<string, object>>));
							break;
						case JTokenType.Boolean:
							cast = (bool)token;
							break;
					}
				}

				if (index == "id" && cast == null)
					throw new VzaarApiException("Record corrupted: missing 'id'");

				return cast;
			}

			set
			{

				if (value == null)
				{
					if (Parameters.ContainsKey(index))
						Parameters.Remove(index);
				}
				else
				{
					if (Parameters.ContainsKey(index))
						Parameters.Remove(index);

					Parameters.Add(index, value);
				}

				cache = JObject.Parse(JsonConvert.SerializeObject(Parameters));
			}
		}

		internal void UpdateRecord(string json)
		{
			// reset body parameters
			Parameters = new Dictionary<string, object>();
			cache = new JObject();

			//set new data
			Data = JObject.Parse(json);

			//validate data object
			if (!Data.TryGetValue("data", out _))
				throw new VzaarApiException("Received data malformed: Missing 'data' token");
		}

		internal async Task Create(Dictionary<string, object> tokens)
		{
			string body = JsonConvert.SerializeObject(tokens);
			var responseJson = await RecordClient.HttpPostAsync(RecordEndpoint, body).ConfigureAwait(false);

			UpdateRecord(responseJson);
		}

		internal async Task Create(Dictionary<string, object> tokens, string subEndpoint = null, string filepath = null)
		{
			string endpoint = RecordEndpoint;

			if (subEndpoint != null)
				endpoint += subEndpoint;

			if (filepath == null)
			{
				string body = JsonConvert.SerializeObject(tokens);
				var responseJson = await RecordClient.HttpPostAsync(endpoint, body).ConfigureAwait(false);

				UpdateRecord(responseJson);
			}
			else
			{
				var postFields = new Dictionary<string, string>();
				foreach (var entry in tokens)
					postFields.Add(entry.Key, entry.Value.ToString());

				var responseJson = await RecordClient.HttpPostFormAsync(endpoint, filepath, postFields).ConfigureAwait(false);

				UpdateRecord(responseJson);
			}
		}

		internal async Task Read(long id)
		{
			string endpoint = RecordEndpoint + "/" + id;

			var responseJson = await RecordClient.HttpGetAsync(endpoint, new Dictionary<string, string>()).ConfigureAwait(false);

			UpdateRecord(responseJson);
		}

		internal async Task Update(Dictionary<string, object> tokens, string subEndpoint = null, string filepath = null)
		{
			string endpoint = RecordEndpoint + "/" + this["id"];

			if (subEndpoint != null)
				endpoint += subEndpoint;

			if (filepath == null)
			{
				string body = JsonConvert.SerializeObject(tokens);

				var responseJson = await RecordClient.HttpPatchAsync(endpoint, body).ConfigureAwait(false);

				UpdateRecord(responseJson);
			}
			else
			{
				var postFields = new Dictionary<string, string>();
				foreach (var entry in tokens)
					postFields.Add(entry.Key, entry.Value.ToString());

				var responseJson = await RecordClient.HttpPatchFormAsync(endpoint, filepath, postFields).ConfigureAwait(false);

				UpdateRecord(responseJson);
			}
		}

		internal async Task Update()
		{
			string endpoint = RecordEndpoint + "/" + this["id"];
			string body = JsonConvert.SerializeObject(cache);

			var responseJson = await RecordClient.HttpPatchAsync(endpoint, body).ConfigureAwait(false);

			UpdateRecord(responseJson);
		}

		internal async Task Delete()
		{
			string endpoint = RecordEndpoint + "/" + this["id"];

			await RecordClient.HttpDeleteAsync(endpoint).ConfigureAwait(false);

			UpdateRecord(empty_record);
		}
	}
}