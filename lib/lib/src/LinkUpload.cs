using System.Collections.Generic;
using System.Threading.Tasks;

namespace VzaarApi
{
	public class LinkUpload : BaseResource
	{
		public LinkUpload()
			: this(Client.GetClient())
		{
		}

		public LinkUpload(Client client)
			: base("link_uploads", client)
		{
		}

		internal async Task<Video> LinkCreateAsync(Dictionary<string, object> tokens)
		{
			if (tokens.ContainsKey("uploader") == false)
				tokens.Add("uploader", Client.UPLOADER + Client.VERSION);

			await record.Create(tokens).ConfigureAwait(false);
			record.RecordEndpoint = "videos";

			var video = new Video(record);

			return video;
		}

		//create
		public static Video Create(string url)
		{
			return Create(url, Client.GetClient());
		}

		public static Video Create(string url, Client client)
		{
			return Create(new Dictionary<string, object> {{"url", url}}, client);
		}

		public static Video Create(Dictionary<string, object> tokens)
		{
			return Create(tokens, Client.GetClient());
		}

		public static Video Create(Dictionary<string, object> tokens, Client client)
		{
			return CreateAsync(tokens, client).Result;
		}

		public static Task<Video> CreateAsync(string url)
		{
			return CreateAsync(url, Client.GetClient());
		}

		public static Task<Video> CreateAsync(string url, Client client)
		{
			return CreateAsync(new Dictionary<string, object> { { "url", url } }, client);
		}

		public static Task<Video> CreateAsync(Dictionary<string, object> tokens)
		{
			return CreateAsync(tokens, Client.GetClient());
		}

		public static async Task<Video> CreateAsync(Dictionary<string, object> tokens, Client client)
		{
			var link = new LinkUpload(client);

			var video = await link.LinkCreateAsync(tokens).ConfigureAwait(false);

			return video;
		}
	}
}

