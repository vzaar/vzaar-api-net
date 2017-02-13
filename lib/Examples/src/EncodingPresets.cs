using System;
using System.Collections.Generic;
using VzaarApi;

namespace Examples
{
	public class EncodingPresets
	{
		public EncodingPresets() {}

		//custom type class
		class myType {

			public long? id;
			public string name;

			//parameter not existing in record
			public string mydata;
		}

		public static void UsingEncodingPreset(string id, string token, long recordId) {

			try {
				//lookup
			
				Console.WriteLine ("--Find(id)--");
				Preset item = Preset.Find(recordId);

				Console.WriteLine ("--Access with property name--");
				Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);

				Console.WriteLine ("--Access with the record Type--");

				Client client = new Client () { CfgClientId = id, CfgAuthToken = token };

				var item2 = Preset.Find (recordId,client);

				var record1 = (EncodingPresetType)item2.ToTypeDef(typeof(EncodingPresetType));
				Console.WriteLine ("Id: " + record1.id + " Name: " + record1.name+" Created: " + record1.created_at);

				Console.WriteLine ("--Access with custom Type--");

				var record2 = (myType)item2.ToTypeDef(typeof(myType));
				Console.WriteLine ("Id: " + record2.id + " Name: " + record2.name);

			} catch(VzaarApiException ve) {

				Console.Write ("!!!!!!!!! EXCEPTION !!!!!!!!!");
				Console.WriteLine (ve.Message);

			} catch (Exception e) {

				Console.Write ("!!!!!!!!! EXCEPTION !!!!!!!!!");
				Console.WriteLine (e.Message);

				if (e is AggregateException) {
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten ();

					foreach (var fe in flatten.InnerExceptions) {
						if (fe is VzaarApiException)
							Console.WriteLine (fe.Message);
					}
				}

			}
		}

		//Read EncodingPresets
		public static void ReadingEncodingPresetsList(string id, string token) {

			try {

				Console.WriteLine ("--EachItem()--");
				foreach(Preset item in PresetsList.EachItem()) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

				Console.WriteLine ("--EachItem(query)--");

				Dictionary<string,string> query = new Dictionary<string, string>() {
					{"sort","id"},
					{"order", "desc"},
					{"per_page", "2"}
				};
					
				Client client = new Client() {CfgClientId = id, CfgAuthToken = token};
				foreach(var item in PresetsList.EachItem(query,client)) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

				Console.WriteLine ("--Paginate--");

				var items1 = PresetsList.Paginate ();
				foreach (var item in items1.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

				Console.WriteLine ("--Paginate(query)--");

				var items2 = PresetsList.Paginate (query, new Client());

				Console.WriteLine ("--Paginate(query)--Initial-First--");
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

				while(items2.Next()) {
					Console.WriteLine ("--Paginate(query)--Next--");

					foreach (var item in items2.Page) {
						Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
					}
				}

				Console.WriteLine ("--Paginate(query)--Previous--");
				items2.Prevous();
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

				Console.WriteLine ("--Paginate(query)--Last--");
				items2.Last();
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

				Console.WriteLine ("--Paginate(query)--First--");
				items2.First();
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

				Console.WriteLine ("--Rate Information--");
				var client3 = items2.GetClient();

				Console.WriteLine("RateLimit: " + client3.RateLimit);
				Console.WriteLine("RateRemaining: " + client3.RateRemaining);
				Console.WriteLine("RateReset: " + client3.RateReset);


			} catch(VzaarApiException ve) {

				Console.Write ("!!!!!!!!! EXCEPTION !!!!!!!!!");
				Console.WriteLine (ve.Message);

			} catch (Exception e) {

				Console.Write ("!!!!!!!!! EXCEPTION !!!!!!!!!");
				Console.WriteLine (e.Message);

				if (e is AggregateException) {
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten ();

					foreach (var fe in flatten.InnerExceptions) {
						if (fe is VzaarApiException)
							Console.WriteLine (fe.Message);
					}
				}

			}
		
		}
	}
}

