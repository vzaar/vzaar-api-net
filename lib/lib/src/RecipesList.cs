using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

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

		//ASYNC METHODS

		//paginate
		public async static Task<RecipesList> PaginateAsync(Dictionary<string,string> query) {

			var recipes = new RecipesList ();

			await recipes.records.ReadAsync(query).ConfigureAwait(false);
			recipes.Initialize ();

			return recipes;
		}

		public async static Task<RecipesList> PaginateAsync(Dictionary<string,string> query, Client client) {

			var recipes = new RecipesList (client);

			await recipes.records.ReadAsync(query).ConfigureAwait(false);
			recipes.Initialize ();

			return recipes;
		}

		public async static Task<RecipesList> PaginateAsync(){
			return await RecipesList.PaginateAsync (new Dictionary<string, string> ()).ConfigureAwait(false);
		}

		public async static Task<RecipesList> PaginateAsync(Client client){
			return await RecipesList.PaginateAsync (new Dictionary<string, string> (), client).ConfigureAwait(false);
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
		public static IEnumerable<Recipe> EachItem() {
			return RecipesList.EachItem (new Dictionary<string, string> ());
		}

		public static IEnumerable<Recipe> EachItem(Client client) {
			return RecipesList.EachItem (new Dictionary<string, string> (), client);
		}

		public static IEnumerable<Recipe> EachItem(Dictionary<string, string> query) {

			var recipes = new RecipesList ();

			var task = recipes.records.ReadAsync (query);
			task.Wait ();

			do {

				recipes.Initialize ();

				foreach (var item in recipes.Page) {
					yield return item;
				}

			} while (recipes.records.NextAsync().Result);

		}

		public static IEnumerable<Recipe> EachItem(Dictionary<string, string> query, Client client) {

			var recipes = new RecipesList (client);

			var task = recipes.records.ReadAsync (query);
			task.Wait ();

			do {

				recipes.Initialize ();

				foreach (var item in recipes.Page) {
					yield return item;
				}

			} while (recipes.records.NextAsync().Result);

		}

		//paginate
		public static RecipesList Paginate(Dictionary<string,string> query) {
			return RecipesList.PaginateAsync (query).Result;
		}

		public static RecipesList Paginate(Dictionary<string,string> query, Client client) {
			return RecipesList.PaginateAsync (query, client).Result;
		}

		public static RecipesList Paginate(){
			return RecipesList.PaginateAsync (new Dictionary<string, string> ()).Result;
		}

		public static RecipesList Paginate(Client client){
			return RecipesList.PaginateAsync (new Dictionary<string, string> (), client).Result;
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