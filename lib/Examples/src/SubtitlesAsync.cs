using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VzaarApi;

namespace Examples
{
	public class SubtitlesAsync
	{
		public SubtitlesAsync ()
		{
		}

		public async static Task UsingSubtitleAsync(string id, string token, int videoId, string subtitlespath) {

			try {

				var video = await Video.FindAsync(videoId);

				Console.WriteLine ("--Create(tokens)--");

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "code", "en" },
					{ "content", "1\n00:02:17,440 --> 00:02:20,375\nSenator, we're making\nour final approach into Coruscant.\n\n2\n00:02:20,476 --> 00:02:22,501\nVery good, Lieutenant." }
				};
				
				var itemNew = await video.SubtitleCreateAsync(tokens);
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

				await video.SubtitleUpdateAsync ((long)itemNew["id"], tokens2);

				Console.WriteLine ("--After: Save(tokens)--");

				Console.WriteLine ("--EachItem()--");
				foreach(Subtitle item in video.Subtitles().EachItem()) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--Delete--");

				await video.SubtitleDeleteAsync ((long)itemNew["id"]);

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

				var itemNew2 = await video.SubtitleCreateAsync(tokens3);
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

				await video.SubtitleUpdateAsync ((long)itemNew2["id"], tokens4);

				Console.WriteLine ("--After: Save(tokens)--");

				Console.WriteLine ("--EachItem()--");
				foreach(Subtitle item in video.Subtitles().EachItem()) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--Delete--");

				await video.SubtitleDeleteAsync ((long)itemNew2["id"]);

				//delay the listing to consider deletion delay on the server
				System.Threading.Thread.Sleep(4000);

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

		public async static Task ReadingSubtitlesListAsync(string id, string token, int videoId) {

			try {

				var video = await Video.FindAsync(videoId);

				Console.WriteLine ("--Paginate--");

				var items1 = await video.Subtitles().PaginateAsync();
				foreach (var item in items1.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--Paginate(query)--");

				Dictionary<string,string> query = new Dictionary<string, string>();

				var items2 = await video.Subtitles().PaginateAsync(query);

				Console.WriteLine ("--Paginate(query)--Initial-First--");
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Title: " + item["title"]);
				}

				Console.WriteLine ("--Paginate(query)--Next--");
				while(await items2.NextAsync()) {

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
	}//end class
}//end namespace

