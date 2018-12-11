using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

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

		//get list
		public virtual IEnumerable<Subtitle> EachItem() {
			return EachItem (new Dictionary<string, string> ());
		}

		public virtual IEnumerable<Subtitle> EachItem(Dictionary<string, string> query) {

			records.Read (query);

			do {

				Initialize ();

				foreach (var item in Page) {
					yield return item;
				}

			} while (records.Next());

		}

		//paginate
		public virtual SubtitlesList Paginate(Dictionary<string,string> query) {

			records.Read(query);
			Initialize ();

			return this;
		}

		public virtual SubtitlesList Paginate(){
			return Paginate (new Dictionary<string, string> ());
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