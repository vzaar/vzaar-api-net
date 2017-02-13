using System;
using System.Collections.Generic;
using VzaarApi;

namespace Examples
{
	public class IngestRecipes
	{
		public IngestRecipes ()
		{
		}

		class myType {

			public long? id;
			public string name;

			//parameter not existing in record
			public string mydata;
		}

		public static void UsingIngestRecipe(string id, string token) {

			try {
				Console.WriteLine ("--Create(tokens)--");

				long[] presetIds = { 3, 10 };

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "name", "Example Recipe" },
					{ "encoding_preset_ids", presetIds},
					{ "multipass", false }
				};

				var itemNew = Recipe.Create(tokens);
				Console.WriteLine ("Id: " + itemNew["id"] + " Name: " + itemNew["name"]);

				//lookup
				Console.WriteLine ("--Find(id)--");

				var item = Recipe.Find((long)itemNew["id"]);

				Console.WriteLine ("--Access with property name--");
				Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);

				Console.WriteLine ("--Access with the record Type--");

				Client client = Client.GetClient();

				var item2 = Recipe.Find ((long)item["id"],client);

				var record1 = (IngestRecipeType)item2.ToTypeDef(typeof(IngestRecipeType));
				Console.WriteLine ("Id: " + record1.id + " Name: " + record1.name+" Created: " + record1.created_at);

				Console.WriteLine ("--Access with custom Type--");

				var record2 = (myType)item2.ToTypeDef(typeof(myType));
				Console.WriteLine ("Id: " + record2.id + " Name: " + record2.name);

				//update
				Console.WriteLine ("--Update1--");

				Console.WriteLine ("--Before: Save(tokens)--");
				Console.WriteLine ("Id: " + item2["id"] + " Multipass: " + item2["multipass"].ToString());

				Dictionary<string, object> tokens2 = new Dictionary<string, object> () {
					{"multipass", true }
				};

				item2.Save (tokens2);

				Console.WriteLine ("--After: Save(tokens)--");
				Console.WriteLine ("Id: " + item2["id"] + " Multipass: " + item2["multipass"].ToString());

				Console.WriteLine ("--Update2--");

				Console.WriteLine ("--Before: Save()--");
				Console.Write ("Id: " + item2["id"]+ "Encoding Presets: ");

				var presets = (List<Dictionary<string,object>>)item2["encoding_presets"];

				List<long> newEncoding = new List<long>();

				foreach(var p in presets) {
					
					newEncoding.Add((long)p["id"]);

					Console.Write(" "+p["id"]);
				}

				Console.WriteLine();

				//add new encoding to the current list
				newEncoding.Add(2);

				item2 ["encoding_preset_ids"] = newEncoding;

				item2.Save ();

				Console.WriteLine ("--After: Save()--");
				Console.WriteLine ("Id: " + item2["id"]+ "Encoding Presets: ");

				var record3 = (IngestRecipeType)item2.ToTypeDef(typeof(IngestRecipeType)); 

				foreach(var p in record3.encoding_presets) {

					Console.Write(" "+p.id);
				}

				Console.WriteLine();


				Console.WriteLine ("--Update3--");

				Console.WriteLine ("--Before: Save()--");
				Console.Write ("Id: " + item2["id"]+ "Encoding Presets: ");

				var record = (IngestRecipeType)item2.ToTypeDef(typeof(IngestRecipeType));

				newEncoding = new List<long>();

				foreach(var p in record.encoding_presets) {

					newEncoding.Add((long)p.id);

					Console.Write(" "+p.id);
				}

				Console.WriteLine();

				//remove encoding from the current list
				newEncoding.Remove(2);

				item2 ["encoding_preset_ids"] = newEncoding;

				item2.Save ();

				Console.WriteLine ("--After: Save()--");
				Console.WriteLine ("Id: " + item2["id"]+ "Encoding Presets: ");

				record = (IngestRecipeType)item2.ToTypeDef(typeof(IngestRecipeType));

				foreach(var p in record.encoding_presets) {

					Console.Write(" "+p.id);
				}

				Console.WriteLine();



				Console.WriteLine ("--Delete--");

				var deleteId = (long)item2 ["id"];
				item2.Delete ();

				Console.WriteLine ("--Find after delete--");
				item2 = Recipe.Find (deleteId);

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
			
		public static void ReadingIngestRecipesList(string id, string token) {

			try {

				Console.WriteLine ("--EachItem()--");
				foreach(Recipe item in RecipesList.EachItem()) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

				Console.WriteLine ("--EachItem(query)--");

				Dictionary<string,string> query = new Dictionary<string, string>() {
					{"sort","id"},
					{"order", "desc"},
					{"per_page", "2"}
				};

				Client client = new Client() {CfgClientId = id, CfgAuthToken = token};
				foreach(var item in RecipesList.EachItem(query,client)) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

				Console.WriteLine ("--Paginate--");

				var items1 = RecipesList.Paginate ();
				foreach (var item in items1.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

				Console.WriteLine ("--Paginate(query)--");

				var items2 = RecipesList.Paginate (query, new Client());

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

