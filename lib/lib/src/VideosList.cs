using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VzaarApi
{
	public class VideosList
	{
		internal RecordsList records;

		public List<Video> Page { get; internal set;}

		public VideosList ()
		{
			records = new RecordsList ("videos");
			Page = new List<Video>();
		}

		public VideosList (Client client)
		{
			records = new RecordsList ("videos", client);
			Page = new List<Video>();
		}

		public Client GetClient() {
			return records.RecordClient;
		}

		internal void Initialize(){

			Page.Clear ();

			foreach (var item in records.List) {

				Video video = new Video (item);
				Page.Add (video);

			}
		}

		// ASYNC METHODS

		//paginate
		public async static Task<VideosList> PaginateAsync(Dictionary<string,string> query) {

			var videos = new VideosList ();

			await videos.records.ReadAsync(query).ConfigureAwait(false);
			videos.Initialize ();

			return videos;
		}

		public async static Task<VideosList> PaginateAsync(Dictionary<string,string> query, Client client) {

			var videos = new VideosList (client);

			await videos.records.ReadAsync(query).ConfigureAwait(false);
			videos.Initialize ();

			return videos;
		}

		public async static Task<VideosList> PaginateAsync(){
			return await VideosList.PaginateAsync (new Dictionary<string, string> ()).ConfigureAwait(false);
		}

		public async static Task<VideosList> PaginateAsync(Client client){
			return await VideosList.PaginateAsync (new Dictionary<string, string> (), client).ConfigureAwait(false);
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

		//get all list items
		public static IEnumerable<Video> EachItem() {
			return VideosList.EachItem (new Dictionary<string,string> ());
		}

		public static IEnumerable<Video> EachItem(Client client) {
			return VideosList.EachItem (new Dictionary<string,string> (), client);
		}

		public static IEnumerable<Video> EachItem(Dictionary<string,string> query) {

			var videos = new VideosList ();

			var task = videos.records.ReadAsync (query);
			task.Wait ();

			do {

				videos.Initialize ();

				foreach (var item in videos.Page) {
					yield return item;
				}

			} while (videos.records.NextAsync().Result);

		}

		public static IEnumerable<Video> EachItem(Dictionary<string,string> query, Client client) {

			var videos = new VideosList (client);

			var task = videos.records.ReadAsync (query);
			task.Wait ();

			do {

				videos.Initialize ();

				foreach (var item in videos.Page) {
					yield return item;
				}

			} while (videos.records.NextAsync().Result);

		}

		//paginate
		public static VideosList Paginate(Dictionary<string,string> query) {
			return VideosList.PaginateAsync (query).Result;
		}

		public static VideosList Paginate(Dictionary<string,string> query, Client client) {
			return VideosList.PaginateAsync (query, client).Result;
		}

		public static VideosList Paginate(){
			return VideosList.PaginateAsync (new Dictionary<string, string> ()).Result;
		}

		public static VideosList Paginate(Client client){
			return VideosList.PaginateAsync (new Dictionary<string, string> (), client).Result;
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

