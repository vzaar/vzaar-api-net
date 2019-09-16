using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VzaarApi
{
	internal class Record
	{

		public Client RecordClient { get; set; }
		public string RecordEndpoint { get; set; }

		public JObject Data { get; set; }
		public Dictionary<string, object> Parameters { get; set;}
		public JObject cache;

		public string empty_record = @"{'data':{}}";

		public Record (string endpoint)
		{
			Data = JObject.Parse(empty_record);
			Parameters = new Dictionary<string, object> ();
			cache = new JObject ();

			RecordClient = Client.GetClient ();
			RecordEndpoint = endpoint;
		}

		public Record (string endpoint, Client client)
		{
			Data = JObject.Parse(empty_record);
			Parameters = new Dictionary<string, object> ();
			cache = new JObject ();

			RecordClient = client;
			RecordEndpoint = endpoint;
		}

		public object ToTypeDef(Type type){ 

				var data = Data ["data"];

				var item = data.ToObject(type);

				return item;
		}

		public bool Edited { 

			get { 
				if (Parameters.Count == 0)
					return false;
				else
					return true;
			} 

			internal set { }
		}

		public object this[string index] {

			get { 

				var data = JObject.Parse (Data ["data"].ToString ());

				object cast = null;

				JToken token; 
				if (cache.TryGetValue (index, out token) || data.TryGetValue (index, out token)) {

					switch (token.Type) {
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
						cast = (List<Dictionary<string,object>>)token.ToObject(typeof(List<Dictionary<string,object>>));
						break;
					case JTokenType.Boolean:
						cast = (bool)token;
						break;
					}
				}

				if (index == "id" && cast == null)
					throw new VzaarApiException ("Record corrupted: missing 'id'");

				return cast;
			}

			set { 

				if (value == null) {
					
					if(Parameters.ContainsKey(index))
						Parameters.Remove (index);
					
				} else {
					
					if(Parameters.ContainsKey(index))
						Parameters.Remove (index);
					
					Parameters.Add (index, value);
				}

				cache = JObject.Parse(JsonConvert.SerializeObject (Parameters));
			
			}

		}
			

		public virtual void UpdateRecord(string json) {

			// reset body parameters
			Parameters = new Dictionary<string, object> ();
			cache = new JObject ();

			//set new data
			Data = JObject.Parse(json);

			//validate data object
			JToken token;
			if (Data.TryGetValue ("data", out token) == false)
				throw new VzaarApiException ("Received data malformed: Missing 'data' token");

		}

		public async virtual Task CreateAsync(Dictionary<string,object> tokens){

				string body = JsonConvert.SerializeObject (tokens);
			    var result = await RecordClient.HttpPostAsync (RecordEndpoint, body).ConfigureAwait(false);

				UpdateRecord(result);
		}

		public async virtual Task CreateAsync(Dictionary<string,object> tokens, string subEndpoint = null, string filepath = null){

			string endpoint = RecordEndpoint;

			if( subEndpoint != null ){
				endpoint += subEndpoint;
			}

			if ( filepath == null) {
				string body = JsonConvert.SerializeObject (tokens);
				var result = await RecordClient.HttpPostAsync (endpoint, body).ConfigureAwait(false);

				UpdateRecord(result);
			} else {

				Dictionary<string,string> postFields = new Dictionary<string, string> ();
				foreach (KeyValuePair<string,object> entry in tokens) {
					postFields.Add (entry.Key, entry.Value.ToString ());
				}

				var result = await RecordClient.HttpPostFormAsync (endpoint, filepath, postFields).ConfigureAwait(false);

				UpdateRecord(result);
			}
		}

		public async virtual Task ReadAsync(long id){

			string endpoint = RecordEndpoint + "/" + id.ToString();

			var result = await RecordClient.HttpGetAsync (endpoint,new Dictionary<string, string>()).ConfigureAwait(false);

			UpdateRecord(result);
		}


		public async virtual Task UpdateAsync(Dictionary<string,object> tokens, string subEndpoint = null, string filepath = null){
			
			string endpoint = RecordEndpoint + "/" + this["id"].ToString();

			if( subEndpoint != null ){
				endpoint += subEndpoint;
			}
			
			if( filepath == null ) {

				string body = JsonConvert.SerializeObject(tokens);

				var result = await RecordClient.HttpPatchAsync (endpoint, body).ConfigureAwait(false);

				UpdateRecord(result);

			} else {

				Dictionary<string,string> postFields = new Dictionary<string, string> ();
				foreach (KeyValuePair<string,object> entry in tokens) {
					postFields.Add (entry.Key, entry.Value.ToString ());
				}

				var result = await RecordClient.HttpPatchFormAsync (endpoint, filepath, postFields).ConfigureAwait(false);

				UpdateRecord(result);

			}
		}

		public async virtual Task UpdateAsync(){

			string endpoint = RecordEndpoint + "/" + this["id"].ToString();
			string body = JsonConvert.SerializeObject(cache);

			var result = await RecordClient.HttpPatchAsync (endpoint, body).ConfigureAwait(false);

			UpdateRecord(result);
		}

		public async virtual Task DeleteAsync(){
			
			string endpoint = RecordEndpoint + "/" + this["id"].ToString();

			await RecordClient.HttpDeleteAsync (endpoint).ConfigureAwait(false);

			UpdateRecord (empty_record);
		}
	}
}