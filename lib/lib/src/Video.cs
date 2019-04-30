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

		private async Task<Video> CreateDataAsync(Dictionary<string, object> tokens)
		{
			var containsFile = tokens.ContainsKey("filepath");
			var containsGuid = tokens.ContainsKey("guid");
			var containsUrl = tokens.ContainsKey("url");

			if (!(containsFile ^ containsGuid ^ containsUrl))
				throw new VzaarApiException("Need to specify one of the following parameters: guid or url or filepath");

			if (containsFile & containsGuid & containsUrl)
				throw new VzaarApiException("Only one of the parameters: guid or url or filepath expected");

			if (containsUrl)
				return await LinkUpload.CreateAsync(tokens, record.RecordClient).ConfigureAwait(false);

			if (containsGuid)
				await record.Create(tokens).ConfigureAwait(false);

			if (containsFile)
			{
				var filepath = (string)tokens["filepath"];

				var file = new FileInfo(filepath);

				if (file.Exists == false)
					throw new VzaarApiException("File does not exist: " + filepath);

				var signature = await Signature.CreateAsync(filepath, record.RecordClient).ConfigureAwait(false);

				await record.RecordClient.HttpPostS3Async(filepath, signature).ConfigureAwait(false);

				tokens.Remove("filepath");
				tokens.Add("guid", (string)signature["guid"]);

				await record.Create(tokens).ConfigureAwait(false);
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

			return await video.CreateDataAsync(tokens).ConfigureAwait(false);
		}

		//lookup
		public static Video Find(long id)
		{
			return Find(id, Client.GetClient());
		}

		public static Video Find(long id, Client client)
		{
			return FindAsync(id, client).Result;
		}

		public static Task<Video> FindAsync(long id)
		{
			return FindAsync(id, Client.GetClient());
		}

		public static async Task<Video> FindAsync(long id, Client client)
		{
			var resource = new Video(client);

			await resource.record.Read(id).ConfigureAwait(false);

			return resource;
		}

		//update
		public virtual void Save()
		{
			SaveAsync().Wait();
		}

		public virtual void Save(Dictionary<string, object> tokens)
		{
			SaveAsync(tokens).Wait();
		}

		public virtual async Task SaveAsync()
		{
			await record.Update().ConfigureAwait(false);
		}

		public virtual async Task SaveAsync(Dictionary<string, object> tokens)
		{
			await record.Update(tokens).ConfigureAwait(false);
		}

		//delete
		public virtual void Delete()
		{
			DeleteAsync().Wait();
		}

		public virtual async Task DeleteAsync()
		{
			await record.Delete().ConfigureAwait(false);
		}

		//SubtitlesList get
		public virtual SubtitlesList Subtitles()
		{
			return new SubtitlesList((long)record["id"], record.RecordClient);
		}

		//Subtitle create
		public virtual Subtitle SubtitleCreate(Dictionary<string, object> tokens)
		{
			return SubtitleCreateAsync(tokens).Result;
		}

		public virtual async Task<Subtitle> SubtitleCreateAsync(Dictionary<string, object> tokens)
		{
			var subtitle = new Subtitle((long)record["id"], record.RecordClient);
			subtitle.Create(tokens);

			//refresh video object
			await record.Read((long)record["id"]).ConfigureAwait(false);

			return subtitle;
		}

		//Subtitle update
		public virtual Subtitle SubtitleUpdate(long subtitleId, Dictionary<string, object> tokens)
		{
			return SubtitleUpdateAsync(subtitleId, tokens).Result;
		}

		public virtual async Task<Subtitle> SubtitleUpdateAsync(long subtitleId, Dictionary<string, object> tokens)
		{
			var subtitle = new Subtitle((long)record["id"], record.RecordClient, subtitleId);
			subtitle.Save(tokens);

			//refresh video object
			await record.Read((long)record["id"]).ConfigureAwait(false);

			return subtitle;
		}

		//Subtitle delete
		public virtual Subtitle SubtitleDelete(long subtitleId)
		{
			return SubtitleDeleteAsync(subtitleId).Result;
		}

		public virtual async Task<Subtitle> SubtitleDeleteAsync(long subtitleId)
		{
			var subtitle = new Subtitle((long)record["id"], record.RecordClient, subtitleId);
			subtitle.Delete();

			//refresh video object
			await record.Read((long)record["id"]).ConfigureAwait(false);

			return subtitle;
		}

		//Set Image Frame
		public virtual void SetImageFrame(Dictionary<string, object> tokens)
		{
			SetImageFrameAsync(tokens).Wait();
		}

		public virtual async Task SetImageFrameAsync(Dictionary<string, object> tokens)
		{
			if (tokens.ContainsKey("image"))
			{
				var videoId = (long)record["id"];
				var filepath = tokens["image"].ToString();
				await record.Create(tokens, $"/{videoId}/image", filepath).ConfigureAwait(false);
			}
			else
			{
				await record.Update(tokens, "/image").ConfigureAwait(false);
			}
		}
	}
}

