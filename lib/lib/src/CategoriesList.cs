using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

		internal async Task FindSubtreeAsync(long id, Dictionary<string, string> query) {

			string path = "/" + id.ToString() + "/subtree";

			await records.ReadAsync (path, query).ConfigureAwait(false);

			Initialize ();
		}

		internal async Task FindSubtreeAsync(long id) {
			await FindSubtreeAsync (id, new Dictionary<string, string> ()).ConfigureAwait(false);
		}

		//ASYNC METHODS

		//get subtree
		public async static Task<CategoriesList> SubtreeAsync(long id) {
			var categories = new CategoriesList ();

			await categories.FindSubtreeAsync (id).ConfigureAwait(false);

			return categories;
		}

		public async static Task<CategoriesList> SubtreeAsync(long id, Client client) {
			var categories = new CategoriesList (client);

			await categories.FindSubtreeAsync (id).ConfigureAwait(false);

			return categories;
		}

		public async static Task<CategoriesList> SubtreeAsync(long id, Dictionary<string, string> query){
			var categories = new CategoriesList ();

			await categories.FindSubtreeAsync (id, query).ConfigureAwait(false);

			return categories;
		}

		public async static Task<CategoriesList> SubtreeAsync(long id, Dictionary<string, string> query, Client client){

			var categories = new CategoriesList (client);

			await categories.FindSubtreeAsync (id, query).ConfigureAwait(false);

			return categories;
		}

		//paginate
		public async static Task<CategoriesList> PaginateAsync(Dictionary<string,string> query) {

			var categories = new CategoriesList ();

			await categories.records.ReadAsync(query).ConfigureAwait(false);
			categories.Initialize ();

			return categories;
		}

		public async static Task<CategoriesList> PaginateAsync(Dictionary<string,string> query, Client client) {

			var categories = new CategoriesList (client);

			await categories.records.ReadAsync(query).ConfigureAwait(false);
			categories.Initialize ();

			return categories;
		}

		public async static Task<CategoriesList> PaginateAsync(){
			return await CategoriesList.PaginateAsync (new Dictionary<string, string> ()).ConfigureAwait(false);
		}

		public async static Task<CategoriesList> PaginateAsync(Client client){
			return await CategoriesList.PaginateAsync (new Dictionary<string, string> (), client).ConfigureAwait(false);
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


		// SYNCHRONOUS METHODS

		//get subtree
		public static CategoriesList Subtree(long id) {
			return CategoriesList.SubtreeAsync(id).Result;
		}

		public static CategoriesList Subtree(long id, Client client) {
			return CategoriesList.SubtreeAsync(id, client).Result;
		}

		public static CategoriesList Subtree(long id, Dictionary<string, string> query){
			return CategoriesList.SubtreeAsync(id, query).Result;
		}

		public static CategoriesList Subtree(long id, Dictionary<string, string> query, Client client){
			return CategoriesList.SubtreeAsync(id, query, client).Result;
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

			var task = categories.records.ReadAsync (query);
			task.Wait ();

			do {

				categories.Initialize ();

				foreach (var item in categories.Page) {
					yield return item;
				}

			} while (categories.records.NextAsync().Result);

		}

		public static IEnumerable<Category> EachItem(Dictionary<string, string> query, Client client) {

			var categories = new CategoriesList (client);

			var task = categories.records.ReadAsync (query);
			task.Wait ();

			do {

				categories.Initialize ();

				foreach (var item in categories.Page) {
					yield return item;
				}

			} while (categories.records.NextAsync().Result);

		}

		//paginate
		public static CategoriesList Paginate(Dictionary<string,string> query) {
			return CategoriesList.PaginateAsync (query).Result;
		}

		public static CategoriesList Paginate(Dictionary<string,string> query, Client client) {
			return CategoriesList.PaginateAsync (query, client).Result;
		}

		public static CategoriesList Paginate(){
			return CategoriesList.PaginateAsync (new Dictionary<string, string> ()).Result;
		}

		public static CategoriesList Paginate(Client client){
			return CategoriesList.PaginateAsync (new Dictionary<string, string> (), client).Result;
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

