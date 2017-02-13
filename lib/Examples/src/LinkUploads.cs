using System;
using System.Collections.Generic;
using VzaarApi;

namespace Examples
{
	public class LinkUploads
	{
		public LinkUploads ()
		{
		}

		public static void UsingLinkUploadParameters(string id, string token, string url) {

			try {

				Console.WriteLine ("--Create()-from parameters--");

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "url", url },
					{ "uploader", "My Uploader"},
					{ "title", "My URL Movie"},
					{ "description", "Initial URL description"}
				};

				var video = LinkUpload.Create(tokens);

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

		public static void UsingLinkUploadUrlString(string id, string token, string url) {

			try {

				Console.WriteLine ("--Create()-from URL string--");

				var video = LinkUpload.Create(url);

				Console.WriteLine ("Id: " + video["id"] + " Title: " + video["title"]);

				Console.WriteLine ("--Delete--");

				var videoId = (long)video ["id"];
				video.Delete ();

				Console.WriteLine ("--Find after delete: id: "+videoId+"--");
				var video1 = Video.Find (videoId);

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

