using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VzaarApi;

namespace Examples
{
	public class VideosAsync
	{
		public VideosAsync ()
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

		public async static Task UsingVideoCreateUrlAsync(string id, string token, string url) {

			try {

				Console.WriteLine ("--Create()-from URL-");

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "url", url },
					{ "title", "My URL Movie"},
					{ "description", "Initial URL description"}
				};

				var video = await Video.CreateAsync(tokens);

				Console.WriteLine ("Id: " + video["id"] + " Title: " + video["title"]);

//				Console.WriteLine ("--Delete--");
//
//				var videoId = (long)video ["id"];
//				await video.DeleteAsync ();
//
//				Console.WriteLine ("--Find after delete: id: "+videoId+"--");
//				var video1 = await Video.FindAsync (videoId);

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

		public async static Task UsingVideoCreateFromFileAsync(string id, string token, string filepath) {

			try {

				Console.WriteLine ("--Create(filepath)--");

				Client client = Client.GetClient();
				client.UploadProgress += myDelegateUser.CheckProgress;

				var video = await Video.CreateAsync(filepath, client);

				Console.WriteLine ("Id: " + video["id"] + " Title: " + video["title"]);

//				Console.WriteLine ("--Delete--");
//
//				var videoId = (long)video ["id"];
//				await video.DeleteAsync ();
//
//				Console.WriteLine ("--Find after delete--");
//				video = await Video.FindAsync (videoId);

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

		public async static Task UsingVideoCreateGuidAsync(string id, string token, string filepath) {

			try {
				
				Console.WriteLine ("--Create(tokens) with GUID--");

				var signature = await Signature.CreateAsync(filepath);

				//

				//-> here the file should be uploaded with your AWS uploader
				//   according to received signature (single or multipart)

				//

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "guid", signature["guid"] },
					{ "title", "MyMovie"},
					{ "description", "Initial description"}
				};

				var video1 = await Video.CreateAsync(tokens);

				Console.WriteLine ("Id: " + video1["id"] + " Title: " + video1["title"]);

				//lookup
				Console.WriteLine ("--Find(id)--");

				var item = await Video.FindAsync((long)video1["id"]);

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


				await item.SaveAsync(tokens2);

				Console.WriteLine ("--After: Save(tokens)--");
				Console.WriteLine ("Id: " + item["id"] + " Description: " + item["description"]);


				Console.WriteLine ("--Update2--");

				Console.WriteLine ("--Before: Save()--");
				Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);

				item["title"] = "Updated title";

				item["title"] = null;

				if(item.Edited)
					await item.SaveAsync();

				item["title"] = "Updated title";

				if(item.Edited)
					await item.SaveAsync();

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

		public async static Task ReadingVideosListAsync(string id, string token) {

			try {

				Dictionary<string,string> query = new Dictionary<string, string>() {
					{"sort","id"},
					{"order", "desc"},
					{"per_page", "2"}
				};

				Console.WriteLine ("--Paginate--");

				var items1 = await VideosList.PaginateAsync ();
				foreach (var item in items1.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--Paginate(query)--");

				var items2 = await VideosList.PaginateAsync (query, new Client());

				Console.WriteLine ("--Paginate(query)--Initial-First--");
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				while(await items2.NextAsync()) {
					Console.WriteLine ("--Paginate(query)--Next--");

					foreach (var item in items2.Page) {
						Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
					}
				}

				Console.WriteLine ("--Paginate(query)--Previous--");
				await items2.PrevousAsync();
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--Paginate(query)--Last--");
				await items2.LastAsync();
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--Paginate(query)--First--");
				await items2.FirstAsync();
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

