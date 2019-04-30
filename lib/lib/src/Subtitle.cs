using System.Collections.Generic;

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
			if (tokens.ContainsKey("file"))
			{
				var filepath = tokens["file"].ToString();

				record.Create(tokens, null, filepath);
			}
			else
			{
				record.Create(tokens);
			}
		}

		//update
		internal virtual void Save(Dictionary<string, object> tokens)
		{
			if (tokens.ContainsKey("file"))
			{
				var filepath = tokens["file"].ToString();

				record.Update(tokens, null, filepath);
			}
			else
			{
				record.Update(tokens);
			}
		}

		//delete
		internal virtual void Delete()
		{
			record.Delete();
		}

	}//end class
}//end namespace