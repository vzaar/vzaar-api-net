using System;
using System.Collections.Generic;

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

		//get all list items
		public static IEnumerable<Video> EachItem() {
			return VideosList.EachItem (new Dictionary<string,string> ());
		}

		public static IEnumerable<Video> EachItem(Client client) {
			return VideosList.EachItem (new Dictionary<string,string> (), client);
		}

		public static IEnumerable<Video> EachItem(Dictionary<string,string> query) {

			var videos = new VideosList ();

			videos.records.Read (query);

			do {

				videos.Initialize ();

				foreach (var item in videos.Page) {
					yield return item;
				}

			} while (videos.records.Next());

		}

		public static IEnumerable<Video> EachItem(Dictionary<string,string> query, Client client) {

			var videos = new VideosList (client);

			videos.records.Read (query);

			do {

				videos.Initialize ();

				foreach (var item in videos.Page) {
					yield return item;
				}

			} while (videos.records.Next());

		}

		//paginate
		public static VideosList Paginate(Dictionary<string,string> query) {

			var videos = new VideosList ();

			videos.records.Read(query);
			videos.Initialize ();

			return videos;
		}

		public static VideosList Paginate(Dictionary<string,string> query, Client client) {

			var videos = new VideosList (client);

			videos.records.Read(query);
			videos.Initialize ();

			return videos;
		}

		public static VideosList Paginate(){
			return VideosList.Paginate (new Dictionary<string, string> ());
		}

		public static VideosList Paginate(Client client){
			return VideosList.Paginate (new Dictionary<string, string> (), client);
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

