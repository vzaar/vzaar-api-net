using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VzaarApi
{
	public class Playlist
	{

		internal Record record;

		//constructor
		public Playlist ()
		{
			record = new Record ("feeds/playlists");

		}

		public Playlist (Client client)
		{
			record = new Record ("feeds/playlists", client);
		}

		internal Playlist (Record item)
		{
			record = item;
		}

		public Client GetClient() {
			return record.RecordClient;
		}

		public object this[string index]{

			get { return record [index];}

			set { record [index] = value; }
		}

		public object ToTypeDef(Type type){

			return record.ToTypeDef (type);

		}

		public bool Edited {
			get { return record.Edited; }
		}

		//create
		public static Playlist Create(Dictionary<string,object> tokens) {

			var playlist = new Playlist ();

			playlist.record.Create (tokens);

			return playlist;
		}

		public static Playlist Create(Dictionary<string,object> tokens, Client client){

			var playlist = new Playlist (client);

			playlist.record.Create (tokens);

			return playlist;
		}

		//lookup
		public static Playlist Find(long id) {

			var playlist = new Playlist ();

			playlist.record.Read (id);

			return playlist;
		}

		public static Playlist Find(long id, Client client) {

			var playlist = new Playlist (client);

			playlist.record.Read (id);

			return playlist;
		}

		//update
		public virtual void Save() {

			record.Update ();

		}

		public virtual void Save(Dictionary<string,object> tokens) {

			record.Update (tokens);

		}

		//delete
		public virtual void Delete() {

			record.Delete ();

		}

	}
}

