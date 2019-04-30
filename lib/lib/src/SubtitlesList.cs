using System.Collections.Generic;
using System.Threading.Tasks;

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
			return EachItemAsync(query, client).Result;
		}

		public new virtual Task<IEnumerable<Subtitle>> EachItemAsync()
		{
			return EachItemAsync(new Dictionary<string, string>(), Client.GetClient());
		}

		public new virtual Task<IEnumerable<Subtitle>> EachItemAsync(Client client)
		{
			return EachItemAsync(new Dictionary<string, string>(), client);
		}

		public new virtual Task<IEnumerable<Subtitle>> EachItemAsync(Dictionary<string, string> query)
		{
			return EachItemAsync(query, Client.GetClient());
		}

		public new virtual async Task<IEnumerable<Subtitle>> EachItemAsync(Dictionary<string, string> query, Client client)
		{
			await records.Read(query).ConfigureAwait(false);

			var resources = new List<Subtitle>();

			do
			{
				Initialize();

				resources.AddRange(Page);

			} while (await records.Next().ConfigureAwait(false));

			return resources;
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

		public new virtual Task<SubtitlesList> PaginateAsync()
		{
			return PaginateAsync(new Dictionary<string, string>());
		}

		public new virtual Task<SubtitlesList> PaginateAsync(Client client)
		{
			return PaginateAsync(new Dictionary<string, string>(), client);
		}

		public new virtual Task<SubtitlesList> PaginateAsync(Dictionary<string, string> query)
		{
			return PaginateAsync(query, Client.GetClient());
		}

		public new virtual async Task<SubtitlesList> PaginateAsync(Dictionary<string, string> query, Client client)
		{
			await records.Read(query).ConfigureAwait(false);
			Initialize();

			return this;
		}
	}//end class
}//end namespace