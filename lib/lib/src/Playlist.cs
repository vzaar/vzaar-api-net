using System.Collections.Generic;
using System.Threading.Tasks;

namespace VzaarApi
{
	public class Playlist : BaseResource
	{
		//constructor
		public Playlist()
			: this(Client.GetClient())
		{
		}

		public Playlist(Client client)
			: base("feeds/playlists", client)
		{
		}

		/// <summary>
		/// Do not remove. This is required for use in BaseResourceCollection
		/// </summary>
		internal Playlist(Record item)
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

		//create
		public static Playlist Create(Dictionary<string, object> tokens)
		{
			return Create(tokens, Client.GetClient());
		}

		public static Playlist Create(Dictionary<string, object> tokens, Client client)
		{
			return CreateAsync(tokens, client).Result;
		}

		public static Task<Playlist> CreateAsync(Dictionary<string, object> tokens)
		{
			return CreateAsync(tokens, Client.GetClient());
		}

		public static async Task<Playlist> CreateAsync(Dictionary<string, object> tokens, Client client)
		{
			var playlist = new Playlist(client);

			await playlist.record.Create(tokens).ConfigureAwait(false);

			return playlist;
		}

		//lookup
		public static Playlist Find(long id)
		{
			return Find(id, Client.GetClient());
		}

		public static Playlist Find(long id, Client client)
		{
			return FindAsync(id, client).Result;
		}

		public static Task<Playlist> FindAsync(long id)
		{
			return FindAsync(id, Client.GetClient());
		}

		public static async Task<Playlist> FindAsync(long id, Client client)
		{
			var resource = new Playlist(client);

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
	}
}

