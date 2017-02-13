using System;
using System.Collections.Generic;
using VzaarApi;

namespace Examples
{
	public class Videos
	{
		public Videos ()
		{
		}

		class myDelegateUser {

			public static void CheckProgress(object sender, VideoUploadProgressEventArgs progress) {

				Console.WriteLine ("Progress: Total Parts: " + progress.totalParts + " Uploaded chunks: "+progress.uploadedChunk);
			}
		}

		class myType {

			public long? id;
			public string title;

			//parameter not existing in record
			public string mydata;
		}

		public static void UsingVideoCreateUrl(string id, string token, string url) {

			try {

				Console.WriteLine ("--Create()-from URL-");

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "url", url },
					{ "title", "My URL Movie"},
					{ "description", "Initial URL description"}
				};

				var task = Video.CreateAsync(tokens);
				task.Wait();

				var video = task.Result;

				Console.WriteLine ("Id: " + video["id"] + " Title: " + video["title"]);

//				Console.WriteLine ("--Delete--");
//
//				var videoId = (long)video ["id"];
//				video.Delete ();
//
//				Console.WriteLine ("--Find after delete: id: "+videoId+"--");
//				var video1 = Video.Find (videoId);

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

		public static void UsingVideoCreateFromFile(string id, string token, string filepath) {

			try {

				Console.WriteLine ("--Create(filepath)--");

				Client client = Client.GetClient();
				client.UploadProgress += myDelegateUser.CheckProgress;

				var task = Video.CreateAsync(filepath,client);
				task.Wait();

				var video = task.Result;

				Console.WriteLine ("Id: " + video["id"] + " Title: " + video["title"]);

//				Console.WriteLine ("--Delete--");
//
//				var videoId = (long)video ["id"];
//				video.Delete ();
//
//				Console.WriteLine ("--Find after delete--");
//				video = Video.Find (videoId);

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

		public static void UsingVideoCreateGuid(string id, string token, string filepath) {

			try {
				
				Console.WriteLine ("--Create(tokens) with GUID--");

				var signature = Signature.Create(filepath);

				//

				//-> here the file should be uploaded with your AWS uploader
				//   according to received signature (single or multipart)

				//

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "guid", signature["guid"] },
					{ "title", "MyMovie"},
					{ "description", "Initial description"}
				};

				var task2 = Video.CreateAsync(tokens);
				var video1 = task2.Result;


				Console.WriteLine ("Id: " + video1["id"] + " Title: " + video1["title"]);

				//lookup
				Console.WriteLine ("--Find(id)--");

				var item = Video.Find((long)video1["id"]);

				Console.WriteLine ("--Access with property name--");
				Console.WriteLine ("Id: " + video1["id"] + " Title: " + video1["title"]);

				Console.WriteLine ("--Access with the record Type--");

				var record1 = (VideoType)item.ToTypeDef(typeof(VideoType));
				Console.WriteLine ("Id: " + record1.id + " Title: "+ record1.title +" Created: " + record1.created_at);

				Console.WriteLine ("--Access with custom Type--");

				var record2 = (myType)item.ToTypeDef(typeof(myType));
				Console.WriteLine ("Id: " + record2.id + " Title: " + record2.title);


				//update
				Console.WriteLine ("--Update1--");

				Console.WriteLine ("--Before: Save(tokens)--");
				Console.WriteLine ("Id: " + item["id"] + " Description: " + item["description"]);

				Dictionary<string, object> tokens2 = new Dictionary<string, object> () {
					{"description", null }
				};


				item.Save(tokens2);

				Console.WriteLine ("--After: Save(tokens)--");
				Console.WriteLine ("Id: " + item["id"] + " Description: " + item["description"]);


				Console.WriteLine ("--Update2--");

				Console.WriteLine ("--Before: Save()--");
				Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);

				item["title"] = "Updated title";

				item["title"] = null;

				if(item.Edited)
					item.Save();

				item["title"] = "Updated title";

				if(item.Edited)
					item.Save();

				Console.WriteLine ("--After: Save()--");
				Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);


//				Console.WriteLine ("--Delete--");
//
//				var videoId = (long)item ["id"];
//				item.Delete ();
//
//				Console.WriteLine ("--Find after delete--");
//				item = Video.Find (videoId);
			

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

		public static void ReadingVideosList(string id, string token) {

			try {

				Console.WriteLine ("--EachItem()--");
				foreach(Video item in VideosList.EachItem()) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--EachItem(query)--");

				Dictionary<string,string> query = new Dictionary<string, string>() {
					{"sort","id"},
					{"order", "desc"},
					{"per_page", "2"}
				};

				Client client = new Client() {CfgClientId = id, CfgAuthToken = token};
				foreach(var item in VideosList.EachItem(query,client)) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--Paginate--");

				var items1 = VideosList.Paginate ();
				foreach (var item in items1.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--Paginate(query)--");

				var items2 = VideosList.Paginate (query, new Client());

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

