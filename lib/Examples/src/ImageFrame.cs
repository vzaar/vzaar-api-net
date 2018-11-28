using System;
using System.Collections.Generic;
using VzaarApi;

namespace Examples
{
	public class ImageFrame
	{
		public ImageFrame ()
		{
		}

		public static void UsingImageFrame(string id, string token, int videoId, string imagepath) {

			try {

				var video = Video.Find(videoId);

				Console.WriteLine ("--Set(time)--");

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "time", 0.20 }
				};

				video.SetImageFrame(tokens);
				Console.WriteLine ("Id: " + video["id"] + " Title: " + video["title"]);

				Console.WriteLine ("--Set(image)--");

				Dictionary<string, object> tokens2 = new Dictionary<string, object> () {
					{ "image", imagepath }
				};

				video.SetImageFrame(tokens2);
				Console.WriteLine ("Id: " + video["id"] + " Title: " + video["title"]);

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

	}//enc class
}//end namespace

