using System;
using System.Collections.Generic;
using System.IO;
using VzaarApi;

namespace Examples
{
	public class UploadSignatures
	{
		public UploadSignatures ()
		{
		}

		public static void ReadingSignature(string id, string token, string filepath) {

			try {
				Console.WriteLine ("--Single()--");

				Signature single1 = Signature.Single ();

				Console.WriteLine ("guid: "+single1["guid"]);

				Console.WriteLine ("--Single(tokens)--");

				var tokens1 = new Dictionary<string,object> () {
					{"filename", "myMovie.mp4"},
					{"uploader", "myUploader"}
				};

				Signature single2 = Signature.Single (tokens1);

				var record1 = (UploadSignatureType)single2.ToTypeDef (typeof(UploadSignatureType));

				Console.WriteLine ("guid: "+ record1.guid);


				Console.WriteLine ("--Multipart(tokens)--");

				FileInfo file = new FileInfo (filepath);

				var tokens2 = new Dictionary<string,object> () {
					{"filename", file.Name},
					{"uploader", "myUploader"},
					{"filesize", 3221225472 },
				};

				var multipart1 = Signature.Multipart (tokens2);

				Console.WriteLine ("guid: "+ multipart1["guid"]+" Parts: "+ multipart1["parts"].ToString());

				Console.WriteLine ("--FromFile--");

				var signature = Signature.Create (filepath);

				if(signature["parts"] == null)
					Console.WriteLine ("guid: "+ signature["guid"]);
				else
					Console.WriteLine ("guid: "+ signature["guid"]+" Parts: "+ signature["parts"]);
				
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

