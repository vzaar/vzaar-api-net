using System;
using System.Collections.Generic;

namespace VzaarApi
{
	public class LinkUpload
	{
		internal Record record;

		public LinkUpload ()
		{
			record = new Record ("link_uploads");

		}

		public LinkUpload (Client client)
		{
			record = new Record ("link_uploads", client);
		}

		internal Video LinkCreate(Dictionary<string,object> tokens) {

			if (tokens.ContainsKey ("uploader") == false) {
				tokens.Add ("uploader", Client.UPLOADER + Client.VERSION);
			}

			record.Create (tokens);
			record.RecordEndpoint = "videos";

			var video = new Video (record);

			return video;
		}

		//create from url
		public static Video Create(string url) {

			var link = new LinkUpload ();

			var tokens = new Dictionary<string, object> ();
			tokens.Add ("url", url);

			var video = link.LinkCreate (tokens);

			return video;
		}

		public static Video Create(string url, Client client) {

			var link = new LinkUpload (client);

			var tokens = new Dictionary<string, object> ();
			tokens.Add ("url", url);

			var video = link.LinkCreate (tokens);

			return video;
		}

		//create from url with additional parameters
		public static Video Create(Dictionary<string,object> tokens) {

			var link = new LinkUpload ();

			var video = link.LinkCreate (tokens);

			return video;
		}

		public static Video Create(Dictionary<string,object> tokens, Client client){

			var link = new LinkUpload (client);

			var video = link.LinkCreate (tokens);

			return video;
		}
	}
}

