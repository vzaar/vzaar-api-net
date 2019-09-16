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

		//ASYNC METHODS

		//create
		public async static Task<Playlist> CreateAsync(Dictionary<string,object> tokens) {

			var playlist = new Playlist ();

			await playlist.record.CreateAsync (tokens).ConfigureAwait(false);

			return playlist;
		}

		public async static Task<Playlist> CreateAsync(Dictionary<string,object> tokens, Client client){

			var playlist = new Playlist (client);

			await playlist.record.CreateAsync (tokens).ConfigureAwait(false);

			return playlist;
		}

		//lookup
		public async static Task<Playlist> FindAsync(long id) {

			var playlist = new Playlist ();

			await playlist.record.ReadAsync (id).ConfigureAwait(false);

			return playlist;
		}

		public async static Task<Playlist> FindAsync(long id, Client client) {

			var playlist = new Playlist (client);

			await playlist.record.ReadAsync (id).ConfigureAwait(false);

			return playlist;
		}

		//update
		public async Task SaveAsync() {

			await record.UpdateAsync ().ConfigureAwait(false);

		}

		public async Task SaveAsync(Dictionary<string,object> tokens) {

			await record.UpdateAsync (tokens).ConfigureAwait(false);

		}

		//delete
		public async Task DeleteAsync() {

			await record.DeleteAsync ().ConfigureAwait(false);

		}

		//SYNCHRONOUS METHODS

		//create
		public static Playlist Create(Dictionary<string,object> tokens) {
			return Playlist.CreateAsync (tokens).Result;
		}

		public static Playlist Create(Dictionary<string,object> tokens, Client client){
			return Playlist.CreateAsync (tokens, client).Result;
		}

		//lookup
		public static Playlist Find(long id) {
			return Playlist.FindAsync (id).Result;
		}

		public static Playlist Find(long id, Client client) {
			return Playlist.FindAsync (id, client).Result;
		}

		//update
		public void Save() {
			SaveAsync().Wait();
		}

		public void Save(Dictionary<string,object> tokens) {
			SaveAsync (tokens).Wait();
		}

		//delete
		public void Delete() {
			DeleteAsync ().Wait();
		}

	}
}

