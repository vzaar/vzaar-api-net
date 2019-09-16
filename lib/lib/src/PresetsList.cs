using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VzaarApi
{
	public class PresetsList
	{
		internal RecordsList records;

		public List<Preset> Page { get; internal set;}

		public PresetsList ()
		{
			records = new RecordsList ("encoding_presets");
			Page = new List<Preset>();
		}

		public PresetsList (Client client)
		{
			records = new RecordsList ("encoding_presets", client);
			Page = new List<Preset>();
		}

		public Client GetClient() {
			return records.RecordClient;
		}

		internal void Initialize(){

			Page.Clear ();

			foreach (var item in records.List) {

				Preset preset = new Preset (item);
				Page.Add (preset);

			}
		}

		//ASYNC METHODS

		//paginate
		public async static Task<PresetsList> PaginateAsync(Dictionary<string,string> query) {

			var presets = new PresetsList ();

			await presets.records.ReadAsync(query).ConfigureAwait(false);
			presets.Initialize ();

			return presets;
		}

		public async static Task<PresetsList> PaginateAsync(Dictionary<string,string> query, Client client) {

			var presets = new PresetsList (client);

			await presets.records.ReadAsync(query).ConfigureAwait(false);
			presets.Initialize ();

			return presets;
		}

		public async static Task<PresetsList> PaginateAsync(){
			return await PresetsList.PaginateAsync (new Dictionary<string, string> ()).ConfigureAwait(false);
		}

		public async static Task<PresetsList> PaginateAsync(Client client){
			return await PresetsList.PaginateAsync (new Dictionary<string, string> (), client).ConfigureAwait(false);
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


		//SYNC METHODS

		//get list
		public static IEnumerable<Preset> EachItem() {
			return PresetsList.EachItem (new Dictionary<string, string> ());
		}

		public static IEnumerable<Preset> EachItem(Client client) {
			return PresetsList.EachItem (new Dictionary<string, string> (), client);
		}

		public static IEnumerable<Preset> EachItem(Dictionary<string, string> query) {

			var presets = new PresetsList ();

			var task = presets.records.ReadAsync (query);
			task.Wait ();

			do {

				presets.Initialize ();

				foreach (var item in presets.Page) {
					yield return item;
				}

			} while (presets.records.NextAsync().Result);

		}

		public static IEnumerable<Preset> EachItem(Dictionary<string, string> query, Client client) {

			var presets = new PresetsList (client);

			var task = presets.records.ReadAsync (query);
			task.Wait ();

			do {

				presets.Initialize ();

				foreach (var item in presets.Page) {
					yield return item;
				}

			} while (presets.records.NextAsync().Result);

		}

		//paginate
		public static PresetsList Paginate(Dictionary<string,string> query) {
			return PresetsList.PaginateAsync (query).Result;
		}

		public static PresetsList Paginate(Dictionary<string,string> query, Client client) {
			return PresetsList.PaginateAsync (query, client).Result;
		}

		public static PresetsList Paginate(){
			return PresetsList.PaginateAsync (new Dictionary<string, string> ()).Result;
		}

		public static PresetsList Paginate(Client client){
			return PresetsList.PaginateAsync (new Dictionary<string, string> (), client).Result;
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

	}//enc class
}//end namespace

