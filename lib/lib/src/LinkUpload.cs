using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

		internal async Task<Video> LinkCreateAsync(Dictionary<string,object> tokens) {

			if (tokens.ContainsKey ("uploader") == false) {
				tokens.Add ("uploader", Client.UPLOADER + Client.VERSION);
			}

			await record.CreateAsync (tokens).ConfigureAwait(false);
			record.RecordEndpoint = "videos";

			var video = new Video (record);

			return video;
		}

		//ASYNCHRONOUS METHODS

		//create from url
		public async static Task<Video> CreateAsync(string url) {

			var link = new LinkUpload ();

			var tokens = new Dictionary<string, object> ();
			tokens.Add ("url", url);

			var video = await link.LinkCreateAsync (tokens).ConfigureAwait(false);

			return video;
		}

		public async static Task<Video> CreateAsync(string url, Client client) {

			var link = new LinkUpload (client);

			var tokens = new Dictionary<string, object> ();
			tokens.Add ("url", url);

			var video = await link.LinkCreateAsync (tokens).ConfigureAwait(false);

			return video;
		}

		//create from url with additional parameters
		public async static Task<Video> CreateAsync(Dictionary<string,object> tokens) {

			var link = new LinkUpload ();

			var video = await link.LinkCreateAsync (tokens).ConfigureAwait(false);

			return video;
		}

		public async static Task<Video> CreateAsync(Dictionary<string,object> tokens, Client client){

			var link = new LinkUpload (client);

			var video = await link.LinkCreateAsync (tokens).ConfigureAwait(false);

			return video;
		}

		//SYNCHRONOUS METHODS

		//create from url
		public static Video Create(string url) {
			return LinkUpload.CreateAsync(url).Result;
		}

		public static Video Create(string url, Client client) {
			return LinkUpload.CreateAsync(url, client).Result;
		}

		//create from url with additional parameters
		public static Video Create(Dictionary<string,object> tokens) {
			return LinkUpload.CreateAsync(tokens).Result;
		}

		public static Video Create(Dictionary<string,object> tokens, Client client){
			return LinkUpload.CreateAsync (tokens, client).Result;
		}
	}
}

