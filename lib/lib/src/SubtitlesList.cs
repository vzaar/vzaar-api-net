using System.Collections.Generic;

namespace VzaarApi
{
	public class SubtitlesList : BaseResourceCollection<Subtitle, SubtitlesList>
	{
		internal SubtitlesList(long videoId, Client client)
			: base("videos/" + videoId + "/subtitles", client)
		{
		}

		// NOTE: these methods hide BaseResourceCollection::EachItem and BaseResourceCollection::Paginate overloads
		// as this is the only entity that uses non-static methods for access. IMO this is actually the preferred approach

		//get list
		public new virtual IEnumerable<Subtitle> EachItem()
		{
			return EachItem(new Dictionary<string, string>(), Client.GetClient());
		}

		public new virtual IEnumerable<Subtitle> EachItem(Client client)
		{
			return EachItem(new Dictionary<string, string>(), client);
		}

		public new virtual IEnumerable<Subtitle> EachItem(Dictionary<string, string> query)
		{
			return EachItem(query, Client.GetClient());
		}

		public new virtual IEnumerable<Subtitle> EachItem(Dictionary<string, string> query, Client client)
		{
			records.Read(query);

			do
			{
				Initialize();

				foreach (var item in Page)
				{
					yield return item;
				}

			} while (records.Next());

		}

		//paginate
		public new virtual SubtitlesList Paginate()
		{
			return Paginate(new Dictionary<string, string>());
		}

		public new virtual SubtitlesList Paginate(Client client)
		{
			return Paginate(new Dictionary<string, string>(), client);
		}

		public new virtual SubtitlesList Paginate(Dictionary<string, string> query)
		{
			return Paginate(query, Client.GetClient());
		}

		public new virtual SubtitlesList Paginate(Dictionary<string, string> query, Client client)
		{
			records.Read(query);
			Initialize();

			return this;
		}
	}//end class
}//end namespace