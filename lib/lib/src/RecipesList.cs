using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace VzaarApi
{
	public class RecipesList
	{
		internal RecordsList records;

		public List<Recipe> Page { get; internal set;}

		public RecipesList ()
		{
			records = new RecordsList ("ingest_recipes");
			Page = new List<Recipe>();
		}

		public RecipesList (Client client)
		{
			records = new RecordsList ("ingest_recipes", client);
			Page = new List<Recipe>();
		}

		public Client GetClient() {
			return records.RecordClient;
		}

		internal void Initialize(){

			Page.Clear ();

			foreach (var item in records.List) {

				Recipe recipe = new Recipe (item);
				Page.Add (recipe);

			}
		}

		//get list
		public static IEnumerable<Recipe> EachItem() {
			return RecipesList.EachItem (new Dictionary<string, string> ());
		}

		public static IEnumerable<Recipe> EachItem(Client client) {
			return RecipesList.EachItem (new Dictionary<string, string> (), client);
		}

		public static IEnumerable<Recipe> EachItem(Dictionary<string, string> query) {

			var recipes = new RecipesList ();

			recipes.records.Read (query);

			do {
				
				recipes.Initialize ();

				foreach (var item in recipes.Page) {
					yield return item;
				}

			} while (recipes.records.Next());
	
		}

		public static IEnumerable<Recipe> EachItem(Dictionary<string, string> query, Client client) {

			var recipes = new RecipesList (client);

			recipes.records.Read (query);

			do {

				recipes.Initialize ();

				foreach (var item in recipes.Page) {
					yield return item;
				}

			} while (recipes.records.Next());

		}

		//paginate
		public static RecipesList Paginate(Dictionary<string,string> query) {

			var recipes = new RecipesList ();

			recipes.records.Read(query);
			recipes.Initialize ();

			return recipes;
		}

		public static RecipesList Paginate(Dictionary<string,string> query, Client client) {

			var recipes = new RecipesList (client);

			recipes.records.Read(query);
			recipes.Initialize ();

			return recipes;
		}

		public static RecipesList Paginate(){
			return RecipesList.Paginate (new Dictionary<string, string> ());
		}

		public static RecipesList Paginate(Client client){
			return RecipesList.Paginate (new Dictionary<string, string> (), client);
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