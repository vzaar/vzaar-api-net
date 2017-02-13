using System;
using System.Collections.Generic;

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

		//get list
		public static IEnumerable<Preset> EachItem() {
			return PresetsList.EachItem (new Dictionary<string, string> ());
		}

		public static IEnumerable<Preset> EachItem(Client client) {
			return PresetsList.EachItem (new Dictionary<string, string> (), client);
		}

		public static IEnumerable<Preset> EachItem(Dictionary<string, string> query) {

			var presets = new PresetsList ();

			presets.records.Read (query);

			do {

				presets.Initialize ();

				foreach (var item in presets.Page) {
					yield return item;
				}

			} while (presets.records.Next());

		}

		public static IEnumerable<Preset> EachItem(Dictionary<string, string> query, Client client) {

			var presets = new PresetsList (client);

			presets.records.Read (query);

			do {

				presets.Initialize ();

				foreach (var item in presets.Page) {
					yield return item;
				}

			} while (presets.records.Next());

		}

		//paginate
		public static PresetsList Paginate(Dictionary<string,string> query) {

			var presets = new PresetsList ();

			presets.records.Read(query);
			presets.Initialize ();

			return presets;
		}

		public static PresetsList Paginate(Dictionary<string,string> query, Client client) {

			var presets = new PresetsList (client);

			presets.records.Read(query);
			presets.Initialize ();

			return presets;
		}

		public static PresetsList Paginate(){
			return PresetsList.Paginate (new Dictionary<string, string> ());
		}

		public static PresetsList Paginate(Client client){
			return PresetsList.Paginate (new Dictionary<string, string> (), client);
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

	}//enc class
}//end namespace

