using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

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

		//ASYNCHRONOUS METHODS

		//paginate
		public async static Task<PlaylistsList> PaginateAsync(Dictionary<string,string> query) {

			var playlists = new PlaylistsList ();

			await playlists.records.ReadAsync(query).ConfigureAwait(false);
			playlists.Initialize ();

			return playlists;
		}

		public async static Task<PlaylistsList> PaginateAsync(Dictionary<string,string> query, Client client) {

			var playlists = new PlaylistsList (client);

			await playlists.records.ReadAsync(query).ConfigureAwait(false);
			playlists.Initialize ();

			return playlists;
		}

		public async static Task<PlaylistsList> PaginateAsync(){
			return await PlaylistsList.PaginateAsync (new Dictionary<string, string> ()).ConfigureAwait(false);
		}

		public async static Task<PlaylistsList> PaginateAsync(Client client){
			return await PlaylistsList.PaginateAsync (new Dictionary<string, string> (), client).ConfigureAwait(false);
		}

		public async Task<bool> NextAsync() {
			bool result = await records.NextAsync ().ConfigureAwait(false);

			if (result)
				Initialize ();

			return result;
		}

		public async Task<bool> PrevousAsync() {
			bool result = await records.PreviousAsync ().ConfigureAwait(false);

			if (result)
				Initialize ();

			return result;
		}

		public async Task<bool> FirstAsync() {
			bool result = await records.FirstAsync ().ConfigureAwait(false);

			if (result)
				Initialize ();

			return result;
		}

		public async Task<bool> LastAsync() {
			bool result = await records.LastAsync ().ConfigureAwait(false);

			if (result)
				Initialize ();

			return result;
		}

		//SYNCHRONOUS METHODS

		//get list
		public static IEnumerable<Playlist> EachItem() {
			return PlaylistsList.EachItem (new Dictionary<string, string> ());
		}

		public static IEnumerable<Playlist> EachItem(Client client) {
			return PlaylistsList.EachItem (new Dictionary<string, string> (), client);
		}

		public static IEnumerable<Playlist> EachItem(Dictionary<string, string> query) {

			var playlists = new PlaylistsList ();

			var task = playlists.records.ReadAsync (query);
			task.Wait ();

			do {

				playlists.Initialize ();

				foreach (var item in playlists.Page) {
					yield return item;
				}

			} while (playlists.records.NextAsync().Result);

		}

		public static IEnumerable<Playlist> EachItem(Dictionary<string, string> query, Client client) {

			var playlists = new PlaylistsList (client);

			var task = playlists.records.ReadAsync (query);
			task.Wait ();

			do {

				playlists.Initialize ();

				foreach (var item in playlists.Page) {
					yield return item;
				}

			} while (playlists.records.NextAsync().Result);

		}

		//paginate
		public static PlaylistsList Paginate(Dictionary<string,string> query) {
			return PlaylistsList.PaginateAsync (query).Result;
		}

		public static PlaylistsList Paginate(Dictionary<string,string> query, Client client) {
			return PlaylistsList.PaginateAsync (query, client).Result;
		}

		public static PlaylistsList Paginate(){
			return PlaylistsList.PaginateAsync (new Dictionary<string, string> ()).Result;
		}

		public static PlaylistsList Paginate(Client client){
			return PlaylistsList.PaginateAsync (new Dictionary<string, string> (), client).Result;
		}

		public bool Next() {
			return NextAsync ().Result;
		}

		public virtual bool Prevous() {
			return PrevousAsync ().Result;
		}

		public virtual bool First() {
			return FirstAsync ().Result;
		}

		public virtual bool Last() {
			return LastAsync ().Result;
		}

	}//end class
}//end namespace