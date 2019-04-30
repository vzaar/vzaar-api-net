using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace VzaarApi
{
	public class Video : BaseResource
	{
		//constructor
		public Video()
			: this(Client.GetClient())
		{
		}

		public Video(Client client)
			: base("videos", client)
		{
		}

		internal Video(Record item)
			: base(item)
		{
			record = item;
		}

		public bool Edited => record.Edited;

		public object this[string index]
		{
			get => record[index];
			set => record[index] = value;
		}

		private async Task<Video> createDataAsync(Dictionary<string, object> tokens)
		{
			bool containsfile = tokens.ContainsKey("filepath");
			bool containsguid = tokens.ContainsKey("guid");
			bool containsurl = tokens.ContainsKey("url");

			if (!(containsfile ^ containsguid ^ containsurl))
				throw new VzaarApiException("Need to specify one of the following parameters: guid or url or filepath");

			if (containsfile & containsguid & containsurl)
				throw new VzaarApiException("Only one of the parameters: guid or url or filepath expected");

			if (containsurl)
				return LinkUpload.Create(tokens, record.RecordClient);

			if (containsguid)
				record.Create(tokens);

			if (containsfile)
			{
				string filepath = (string)tokens["filepath"];

				FileInfo file = new FileInfo(filepath);

				if (file.Exists == false)
					throw new VzaarApiException("File does not exist: " + filepath);

				Signature signature = Signature.Create(filepath, record.RecordClient);

				await record.RecordClient.HttpPostS3Async(filepath, signature);

				tokens.Remove("filepath");
				tokens.Add("guid", (string)signature["guid"]);

				record.Create(tokens);
			}

			return this;
		}

		//create
		public static Task<Video> CreateAsync(string filepath)
		{
			return CreateAsync(filepath, Client.GetClient());
		}

		public static Task<Video> CreateAsync(string filepath, Client client)
		{
			return CreateAsync(new Dictionary<string, object> { { "filepath", filepath } }, client);
		}

		public static Task<Video> CreateAsync(Dictionary<string, object> tokens)
		{
			return CreateAsync(tokens, Client.GetClient());
		}

		public static async Task<Video> CreateAsync(Dictionary<string, object> tokens, Client client)
		{
			var video = new Video(client);

			return await video.createDataAsync(tokens);
		}

		//lookup
		public static Video Find(long id)
		{
			return Find(id, Client.GetClient());
		}

		public static Video Find(long id, Client client)
		{
			var video = new Video(client);

			video.record.Read(id);

			return video;
		}

		//update
		public virtual void Save()
		{
			record.Update();
		}

		public virtual void Save(Dictionary<string, object> tokens)
		{
			record.Update(tokens);
		}

		//delete
		public virtual void Delete()
		{
			record.Delete();
		}

		//SubtitlesList get
		public virtual SubtitlesList Subtitles()
		{
			return new SubtitlesList((long)record["id"], record.RecordClient);
		}

		//Subtitle create
		public virtual Subtitle SubtitleCreate(Dictionary<string, object> tokens)
		{
			var subtitle = new Subtitle((long)record["id"], record.RecordClient);
			subtitle.Create(tokens);

			//refresh video object
			record.Read((long)record["id"]);

			return subtitle;
		}

		//Subtitle update
		public virtual Subtitle SubtitleUpdate(long subtitleId, Dictionary<string, object> tokens)
		{
			var subtitle = new Subtitle((long)record["id"], record.RecordClient, subtitleId);
			subtitle.Save(tokens);

			//refresh video object
			record.Read((long)record["id"]);

			return subtitle;
		}

		//Subtitle delete
		public virtual Subtitle SubtitleDelete(long subtitleId)
		{
			var subtitle = new Subtitle((long)record["id"], record.RecordClient, subtitleId);
			subtitle.Delete();

			//refresh video object
			record.Read((long)record["id"]);

			return subtitle;
		}

		//Set Image Frame
		public virtual void SetImageFrame(Dictionary<string, object> tokens)
		{
			if (tokens.ContainsKey("image"))
			{
				var videoId = (long)record["id"];
				var filepath = tokens["image"].ToString();
				record.Create(tokens, $"/{videoId}/image", filepath);
			}
			else
			{
				record.Update(tokens, "/image");
			}
		}
	}
}

