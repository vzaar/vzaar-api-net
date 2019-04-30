using System.Collections.Generic;

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
			var playlist = new Playlist(client);

			playlist.record.Create(tokens);

			return playlist;
		}

		//lookup
		public static Playlist Find(long id)
		{
			return Find(id, Client.GetClient());
		}

		public static Playlist Find(long id, Client client)
		{
			var playlist = new Playlist(client);

			playlist.record.Read(id);

			return playlist;
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
	}
}

