using System.Collections.Generic;

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

		internal Video LinkCreate(Dictionary<string, object> tokens)
		{
			if (tokens.ContainsKey("uploader") == false)
				tokens.Add("uploader", Client.UPLOADER + Client.VERSION);

			record.Create(tokens);
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
			var link = new LinkUpload(client);

			var video = link.LinkCreate(tokens);

			return video;
		}
	}
}

