using System;
using System.Collections.Generic;
using VzaarApi;

namespace Examples
{
	public class Subtitles
	{
		public Subtitles ()
		{
		}

		public static void UsingSubtitle(string id, string token, int videoId, string subtitlespath) {

			try {

				var video = Video.Find(videoId);

				Console.WriteLine ("--Create(tokens)--");

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "code", "en" },
					{ "content", "1\n00:02:17,440 --> 00:02:20,375\nSenator, we're making\nour final approach into Coruscant.\n\n2\n00:02:20,476 --> 00:02:22,501\nVery good, Lieutenant." }
				};
				
				var itemNew = video.SubtitleCreate(tokens);
				Console.WriteLine ("Code: " + itemNew["code"] + " Title: " + itemNew["title"]);

				//display current subtitles
				Console.WriteLine ("--EachItem()--");
				foreach(Subtitle item in video.Subtitles().EachItem()) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				//update
				Console.WriteLine ("--Update1--");

				Console.WriteLine ("--Before: Save(tokens)--");
				Console.WriteLine ("Code: " + itemNew["code"] + " Title: " + itemNew["title"]);

				Dictionary<string, object> tokens2 = new Dictionary<string, object> () {
					{ "code", "en" },
					{ "content", "1\n00:02:17,440 --> 00:02:20,375\nSenator, we're making\nour final approach into Coruscant.\n\n2\n00:02:20,476 --> 00:02:22,501\nVery bad, Lieutenant." }
				};

				video.SubtitleUpdate ((long)itemNew["id"], tokens2);

				Console.WriteLine ("--After: Save(tokens)--");

				Console.WriteLine ("--EachItem()--");
				foreach(Subtitle item in video.Subtitles().EachItem()) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--Delete--");

				video.SubtitleDelete ((long)itemNew["id"]);

				//delay the listing to consider deletion delay on the server
				System.Threading.Thread.Sleep(4000);

				Console.WriteLine ("--EachItem()--");
				foreach(Subtitle item in video.Subtitles().EachItem()) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--Create(from file)--");

				Dictionary<string, object> tokens3 = new Dictionary<string, object> () {
					{ "code", "en" },
					{ "file", subtitlespath }
				};

				var itemNew2 = video.SubtitleCreate(tokens3);
				Console.WriteLine ("Code: " + itemNew2["code"] + " Title: " + itemNew2["title"]);

				Console.WriteLine ("--EachItem()--");
				foreach(Subtitle item in video.Subtitles().EachItem()) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				//update
				Console.WriteLine ("--Update2--");

				Console.WriteLine ("--Before: Save(tokens)--");
				Console.WriteLine ("Code: " + itemNew2["code"] + " Title: " + itemNew2["title"]);

				Dictionary<string, object> tokens4 = new Dictionary<string, object> () {
					{ "code", "en" },
					{ "file", subtitlespath }
				};

				video.SubtitleUpdate ((long)itemNew2["id"], tokens4);

				Console.WriteLine ("--After: Save(tokens)--");

				Console.WriteLine ("--EachItem()--");
				foreach(Subtitle item in video.Subtitles().EachItem()) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--Delete--");

				video.SubtitleDelete ((long)itemNew2["id"]);

				//delay the listing to consider deletion delay on the server
				System.Threading.Thread.Sleep(1000);

				Console.WriteLine ("--EachItem()--");
				foreach(Subtitle item in video.Subtitles().EachItem()) {
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

		public static void ReadingSubtitlesList(string id, string token, int videoId) {

			try {

				var video = Video.Find(videoId);

				Console.WriteLine ("--EachItem()--");
				foreach(Subtitle item in video.Subtitles().EachItem()) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				} 

				Console.WriteLine ("--Paginate--");

				var items1 = video.Subtitles().Paginate();
				foreach (var item in items1.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--Paginate(query)--");

				Dictionary<string,string> query = new Dictionary<string, string>();

				var items2 = video.Subtitles().Paginate(query);

				Console.WriteLine ("--Paginate(query)--Initial-First--");
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--Paginate(query)--Next--");
				while(items2.Next()) {

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
	}//end class
}//end namespace

