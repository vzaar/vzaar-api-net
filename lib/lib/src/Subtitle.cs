using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VzaarApi
{
	public class Subtitle
	{

		internal Record record;

		//constructor
		internal Subtitle(long videoId, Client client)
		{
			record = new Record("videos/" + videoId.ToString() + "/subtitles", client);
		}

		internal Subtitle(long videoId, Client client, long subtitleId)
		{
			record = new Record("videos/" + videoId.ToString() + "/subtitles", client);
			/*
            The endpoint does not provide Lookup, thus
            it is not possible to read data to 
            initialize the object before it is used.
            */
			record["id"] = subtitleId;
		}

		internal Subtitle(Record item)
		{
			record = item;
		}

		public object this[string index]
		{

			get { return record[index]; }

		}

		public object ToTypeDef(Type type)
		{

			return record.ToTypeDef(type);

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