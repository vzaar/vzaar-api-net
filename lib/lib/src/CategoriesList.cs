using System;
using System.Collections.Generic;

namespace VzaarApi
{
	public class CategoriesList
	{

		internal RecordsList records;

		public List<Category> Page { get; internal set;}

		public CategoriesList ()
		{
			records = new RecordsList ("categories");
			Page = new List<Category>();
		}

		public CategoriesList (Client client)
		{
			records = new RecordsList ("categories", client);
			Page = new List<Category>();
		}

		public Client GetClient() {
			return records.RecordClient;
		}

		//this method creates Category objects from Records
		internal void Initialize(){

			Page.Clear ();

			foreach (var item in records.List) {

				Category category = new Category (item);
				Page.Add (category);

			}
		}

		internal void FindSubtree(long id, Dictionary<string, string> query) {

			string path = "/" + id.ToString() + "/subtree";

			records.Read (path, query);

			Initialize ();
		}

		internal void FindSubtree(long id) {
			FindSubtree (id, new Dictionary<string, string> ());
		}

		//get subtree
		public static CategoriesList Subtree(long id) {
			var categories = new CategoriesList ();

			categories.FindSubtree (id);

			return categories;
		}

		public static CategoriesList Subtree(long id, Client client) {
			var categories = new CategoriesList (client);

			categories.FindSubtree (id);

			return categories;
		}

		public static CategoriesList Subtree(long id, Dictionary<string, string> query){
			var categories = new CategoriesList ();

			categories.FindSubtree (id, query);

			return categories;
		}

		public static CategoriesList Subtree(long id, Dictionary<string, string> query, Client client){

			var categories = new CategoriesList (client);

			categories.FindSubtree (id, query);

			return categories;
		}

		//get list
		public static IEnumerable<Category> EachItem() {
			return CategoriesList.EachItem (new Dictionary<string, string> ());
		}

		public static IEnumerable<Category> EachItem(Client client) {
			return CategoriesList.EachItem (new Dictionary<string, string> (),client);
		}

		public static IEnumerable<Category> EachItem(Dictionary<string, string> query) {

			var categories = new CategoriesList ();

			categories.records.Read (query);

			do {

				categories.Initialize ();

				foreach (var item in categories.Page) {
					yield return item;
				}

			} while (categories.records.Next());

		}

		public static IEnumerable<Category> EachItem(Dictionary<string, string> query, Client client) {

			var categories = new CategoriesList (client);

			categories.records.Read (query);

			do {

				categories.Initialize ();

				foreach (var item in categories.Page) {
					yield return item;
				}

			} while (categories.records.Next());

		}

		//paginate
		public static CategoriesList Paginate(Dictionary<string,string> query) {

			var categories = new CategoriesList ();

			categories.records.Read(query);
			categories.Initialize ();

			return categories;
		}

		public static CategoriesList Paginate(Dictionary<string,string> query, Client client) {

			var categories = new CategoriesList (client);

			categories.records.Read(query);
			categories.Initialize ();

			return categories;
		}

		public static CategoriesList Paginate(){
			return CategoriesList.Paginate (new Dictionary<string, string> ());
		}

		public static CategoriesList Paginate(Client client){
			return CategoriesList.Paginate (new Dictionary<string, string> (), client);
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

