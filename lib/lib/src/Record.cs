using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

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

		public virtual void Create(Dictionary<string,object> tokens){

			string body = JsonConvert.SerializeObject (tokens);
			var task = RecordClient.HttpPostAsync (RecordEndpoint, body);
			task.Wait ();

			UpdateRecord(task.Result);
		}

		public virtual void Create(Dictionary<string,object> tokens, string endponit){

			string body = JsonConvert.SerializeObject (tokens);
			var task = RecordClient.HttpPostAsync (RecordEndpoint + endponit, body);
			task.Wait ();

			UpdateRecord(task.Result);
		}

		public virtual void Read(long id){

			string endpoint = RecordEndpoint + "/" + id.ToString();

			var task = RecordClient.HttpGetAsync (endpoint,new Dictionary<string, string>());
			task.Wait ();

			UpdateRecord(task.Result);
		}


		public virtual void Update(Dictionary<string,object> tokens){
			
			string endpoint = RecordEndpoint + "/" + this["id"].ToString();
			string body = JsonConvert.SerializeObject(tokens);

			var task = RecordClient.HttpPatchAsync (endpoint, body);
			task.Wait ();

			UpdateRecord(task.Result);
		}

		public virtual void Update(){

			string endpoint = RecordEndpoint + "/" + this["id"].ToString();
			string body = JsonConvert.SerializeObject(cache);

			var task = RecordClient.HttpPatchAsync (endpoint, body);
			task.Wait ();

			UpdateRecord(task.Result);
		}

		public virtual void Delete(){
			
			string endpoint = RecordEndpoint + "/" + this["id"].ToString();

			var task = RecordClient.HttpDeleteAsync (endpoint);
			task.Wait ();

			UpdateRecord (empty_record);
		}
	}
}