using System.Collections.Generic;
using System.Threading.Tasks;

namespace VzaarApi
{
	public class Subtitle : BaseResource
	{
		//constructor
		internal Subtitle(long videoId, Client client)
			: this(videoId, client, null)
		{
		}

		internal Subtitle(long videoId, Client client, long? subtitleId)
			: base("videos/" + videoId + "/subtitles", client)
		{
			/*
            The endpoint does not provide Lookup, thus
            it is not possible to read data to 
            initialize the object before it is used.
            */
			if (subtitleId.HasValue)
				record["id"] = subtitleId;
		}

		/// <summary>
		/// Do not remove. This is required for use in BaseResourceCollection
		/// </summary>
		internal Subtitle(Record item)
			: base(item)
		{
			record = item;
		}

		public object this[string index]
		{
			get => record[index];
		}

		//create
		internal virtual void Create(Dictionary<string, object> tokens)
		{
			CreateAsync(tokens).Wait();
		}

		internal virtual async Task CreateAsync(Dictionary<string, object> tokens)
		{
			if (tokens.ContainsKey("file"))
			{
				var filepath = tokens["file"].ToString();

				await record.Create(tokens, null, filepath).ConfigureAwait(false);
			}
			else
			{
				await record.Create(tokens).ConfigureAwait(false);
			}
		}

		//update
		internal virtual void Save(Dictionary<string, object> tokens)
		{
			SaveAsync(tokens).Wait();
		}

		internal virtual async Task SaveAsync(Dictionary<string, object> tokens)
		{
			if (tokens.ContainsKey("file"))
			{
				var filepath = tokens["file"].ToString();

				await record.Update(tokens, null, filepath).ConfigureAwait(false);
			}
			else
			{
				await record.Update(tokens).ConfigureAwait(false);
			}
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

	}//end class
}//end namespace