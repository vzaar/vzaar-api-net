using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VzaarApi;

namespace Examples
{
	public class EncodingPresetsAsync
	{
		public EncodingPresetsAsync() {}

		//custom type class
		class myType {

			public long? id;
			public string name;

			//parameter not existing in record
			public string mydata;
		}

		public async static Task UsingEncodingPresetAsync(string id, string token, long recordId) {

			try {
				//lookup
			
				Console.WriteLine ("--Find(id)--");
				Preset item = await Preset.FindAsync(recordId);

				Console.WriteLine ("--Access with property name--");
				Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);

				Console.WriteLine ("--Access with the record Type--");

				Client client = new Client () { CfgClientId = id, CfgAuthToken = token };

				var item2 = await Preset.FindAsync (recordId,client);

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
		public async static Task ReadingEncodingPresetsListAsyncCA(string id, string token) {

			try {

				Console.WriteLine ("--Paginate--");

				Dictionary<string,string> query = new Dictionary<string, string>() {
					{"sort","id"},
					{"order", "desc"},
					{"per_page", "2"}
				};

				var items1 = await PresetsList.PaginateAsync ().ConfigureAwait(false);
				foreach (var item in items1.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

				Console.WriteLine ("--Paginate(query)--");

				var items2 = await PresetsList.PaginateAsync (query, new Client()).ConfigureAwait(false);

				Console.WriteLine ("--Paginate(query)--Initial-First--");
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

				while(await items2.NextAsync().ConfigureAwait(false)) {
					Console.WriteLine ("--Paginate(query)--Next--");

					foreach (var item in items2.Page) {
						Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
					}
				}

				Console.WriteLine ("--Paginate(query)--Previous--");
				await items2.PrevousAsync().ConfigureAwait(false);
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

				Console.WriteLine ("--Paginate(query)--Last--");
				await items2.LastAsync().ConfigureAwait(false);
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

				Console.WriteLine ("--Paginate(query)--First--");
				await items2.FirstAsync().ConfigureAwait(false);
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

