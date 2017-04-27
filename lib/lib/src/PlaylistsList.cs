using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace VzaarApi
{
	public class PlaylistsList
	{
		internal RecordsList records;

		public List<Playlist> Page { get; internal set;}

		public PlaylistsList ()
		{
			records = new RecordsList ("feeds/playlists");
			Page = new List<Playlist>();
		}

		public PlaylistsList (Client client)
		{
			records = new RecordsList ("feeds/playlists", client);
			Page = new List<Playlist>();
		}

		public Client GetClient() {
			return records.RecordClient;
		}

		internal void Initialize(){

			Page.Clear ();

			foreach (var item in records.List) {

				Playlist playlist = new Playlist (item);
				Page.Add (playlist);

			}
		}

		//get list
		public static IEnumerable<Playlist> EachItem() {
			return PlaylistsList.EachItem (new Dictionary<string, string> ());
		}

		public static IEnumerable<Playlist> EachItem(Client client) {
			return PlaylistsList.EachItem (new Dictionary<string, string> (), client);
		}

		public static IEnumerable<Playlist> EachItem(Dictionary<string, string> query) {

			var playlists = new PlaylistsList ();

			playlists.records.Read (query);

			do {

				playlists.Initialize ();

				foreach (var item in playlists.Page) {
					yield return item;
				}

			} while (playlists.records.Next());

		}

		public static IEnumerable<Playlist> EachItem(Dictionary<string, string> query, Client client) {

			var playlists = new PlaylistsList (client);

			playlists.records.Read (query);

			do {

				playlists.Initialize ();

				foreach (var item in playlists.Page) {
					yield return item;
				}

			} while (playlists.records.Next());

		}

		//paginate
		public static PlaylistsList Paginate(Dictionary<string,string> query) {

			var playlists = new PlaylistsList ();

			playlists.records.Read(query);
			playlists.Initialize ();

			return playlists;
		}

		public static PlaylistsList Paginate(Dictionary<string,string> query, Client client) {

			var playlists = new PlaylistsList (client);

			playlists.records.Read(query);
			playlists.Initialize ();

			return playlists;
		}

		public static PlaylistsList Paginate(){
			return PlaylistsList.Paginate (new Dictionary<string, string> ());
		}

		public static PlaylistsList Paginate(Client client){
			return PlaylistsList.Paginate (new Dictionary<string, string> (), client);
		}

		public virtual bool Next() {
			bool result = records.Next ();

			if (result)
				Initialize ();

			return result;
		}

		public virtual bool Prevous() {
			bool result = records.Previous ();

			if (result)
				Initialize ();

			return result;
		}

		public virtual bool First() {
			bool result = records.First ();

			if (result)
				Initialize ();

			return result;
		}

		public virtual bool Last() {
			bool result = records.Last ();

			if (result)
				Initialize ();

			return result;
		}

	}//end class
}//end namespace