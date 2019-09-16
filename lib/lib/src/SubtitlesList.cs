using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace VzaarApi
{
	public class SubtitlesList
	{
		internal RecordsList records;

		public List<Subtitle> Page { get; internal set;}

		internal SubtitlesList (long videoId, Client client)
		{
			records = new RecordsList ("videos/" + videoId.ToString() + "/subtitles", client);
			Page = new List<Subtitle>();
		}

		internal void Initialize(){

			Page.Clear ();

			foreach (var item in records.List) {

				Subtitle subtitle = new Subtitle (item);
				Page.Add (subtitle);

			}
		}

		//ASYNC METHODS

		//paginate
		public async Task<SubtitlesList> PaginateAsync(Dictionary<string,string> query) {

			await records.ReadAsync(query).ConfigureAwait(false);
			Initialize ();

			return this;
		}

		public async Task<SubtitlesList> PaginateAsync(){
			return await PaginateAsync (new Dictionary<string, string> ()).ConfigureAwait(false);
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
		public IEnumerable<Subtitle> EachItem() {
			return EachItem (new Dictionary<string, string> ());
		}

		public IEnumerable<Subtitle> EachItem(Dictionary<string, string> query) {

			var task = records.ReadAsync (query);
			task.Wait ();

			do {

				Initialize ();

				foreach (var item in Page) {
					yield return item;
				}

			} while (records.NextAsync().Result);
		}

		//paginate
		public SubtitlesList Paginate(Dictionary<string,string> query) {
			return PaginateAsync (query).Result;
		}

		public virtual SubtitlesList Paginate(){
			return PaginateAsync (new Dictionary<string, string> ()).Result;
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