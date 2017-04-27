using System;
using System.Collections.Generic;
using VzaarApi;

namespace Examples
{
	public class Playlists
	{
		public Playlists ()
		{
		}

		class myType {

			public long? id;
			public string title;

			//parameter not existing in record
			public string mydata;
		}

		public static void UsingPlaylist(string id, string token, int category_id) {

			try {
				Console.WriteLine ("--Create(tokens)--");

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "title", "Example Playlist" },
					{ "category_id", category_id }
				};

				var itemNew = Playlist.Create(tokens);
				Console.WriteLine ("Id: " + itemNew["id"] + " Title: " + itemNew["title"]);

				//lookup
				Console.WriteLine ("--Find(id)--");

				var item = Playlist.Find((long)itemNew["id"]);

				Console.WriteLine ("--Access with property name--");
				Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);

				Console.WriteLine ("--Access with the record Type--");

				Client client = Client.GetClient();

				var item2 = Playlist.Find ((long)item["id"],client);

				var record1 = (PlaylistType)item2.ToTypeDef(typeof(PlaylistType));
				Console.WriteLine ("Id: " + record1.id + " Title: " + record1.title+" Created: " + record1.created_at);

				Console.WriteLine ("--Access with custom Type--");

				var record2 = (myType)item2.ToTypeDef(typeof(myType));
				Console.WriteLine ("Id: " + record2.id + " Title: " + record2.title);

				//update
				Console.WriteLine ("--Update1--");

				Console.WriteLine ("--Before: Save(tokens)--");
				Console.WriteLine ("Id: " + item2["id"] + " Autoplay: " + item2["autoplay"].ToString());

				Dictionary<string, object> tokens2 = new Dictionary<string, object> () {
					{"autoplay", true }
				};

				item2.Save (tokens2);

				Console.WriteLine ("--After: Save(tokens)--");
				Console.WriteLine ("Id: " + item2["id"] + " Autoplay: " + item2["autoplay"].ToString());


				Console.WriteLine ("--Delete--");

				var deleteId = (long)item2 ["id"];
				item2.Delete ();

				Console.WriteLine ("--Find after delete--");
				item2 = Playlist.Find (deleteId);

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

		public static void ReadingPlaylistsList(string id, string token) {

			try {

				Console.WriteLine ("--EachItem()--");
				foreach(Playlist item in PlaylistsList.EachItem()) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--EachItem(query)--");

				Dictionary<string,string> query = new Dictionary<string, string>() {
					{"sort","created_at"},
					{"order", "desc"},
					{"per_page", "2"}
				};

				Client client = new Client() {CfgClientId = id, CfgAuthToken = token};
				foreach(var item in PlaylistsList.EachItem(query, client)) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--Paginate--");

				var items1 = PlaylistsList.Paginate ();
				foreach (var item in items1.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--Paginate(query)--");

				var items2 = PlaylistsList.Paginate (query, new Client());

				Console.WriteLine ("--Paginate(query)--Initial-First--");
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				while(items2.Next()) {
					Console.WriteLine ("--Paginate(query)--Next--");

					foreach (var item in items2.Page) {
						Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
					}
				}

				Console.WriteLine ("--Paginate(query)--Previous--");
				items2.Prevous();
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--Paginate(query)--Last--");
				items2.Last();
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--Paginate(query)--First--");
				items2.First();
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
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

