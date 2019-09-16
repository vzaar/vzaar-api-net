using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VzaarApi
{
	public class Category
	{
		internal Record record;

		//constructor
		public Category ()
		{
			record = new Record ("categories");

		}

		public Category (Client client)
		{
			record = new Record ("categories", client);
		}

		internal Category (Record item)
		{
			record = item;
		}

		public Client GetClient() {
			return record.RecordClient;
		}

		public object this[string index]{

			get { return record [index];}

			set { record [index] = value; }
		}

		public object ToTypeDef(Type type){

			return record.ToTypeDef (type);

		}

		public bool Edited {
			get { return record.Edited; }
		}

		// ASYNC METHODS

		public async Task<CategoriesList> SubtreeAsync() {

			long id = (long)this ["id"];
			var query = new Dictionary<string,string> ();
			return await CategoriesList.SubtreeAsync (id, query, record.RecordClient).ConfigureAwait(false);
		}

		public async Task<CategoriesList> SubtreeAsync(Dictionary<string, string> query) {

			long id = (long)this ["id"];
			return await CategoriesList.SubtreeAsync (id, query, record.RecordClient).ConfigureAwait(false);
		}

		//lookup
		public async static Task<Category> FindAsync(long id) {

			var category = new Category ();

			await category.record.ReadAsync (id).ConfigureAwait(false);

			return category;
		}

		public async static Task<Category> FindAsync(long id, Client client) {

			var category = new Category (client);

			await category.record.ReadAsync (id).ConfigureAwait(false);

			return category;
		}

		//create
		public async static Task<Category> CreateAsync(Dictionary<string,object> tokens) {

			var category = new Category ();

			await category.record.CreateAsync (tokens).ConfigureAwait(false);

			return category;
		}

		public async static Task<Category> CreateAsync(Dictionary<string,object> tokens, Client client){

			var category = new Category (client);

			await category.record.CreateAsync (tokens).ConfigureAwait(false);

			return category;
		}

		//update
		public async Task SaveAsync() {

			await record.UpdateAsync ().ConfigureAwait(false);

		}

		public async Task SaveAsync(Dictionary<string,object> tokens) {

			await record.UpdateAsync (tokens).ConfigureAwait(false);

		}

		//delete
		public async Task DeleteAsync() {

			await record.DeleteAsync ().ConfigureAwait(false);

		}

		//SYNCH METHODS

		public CategoriesList Subtree() {

			long id = (long)this ["id"];
			var query = new Dictionary<string,string> ();

			return CategoriesList.SubtreeAsync (id, query, record.RecordClient).Result;
		}

		public CategoriesList Subtree(Dictionary<string, string> query) {

			long id = (long)this ["id"];
			return CategoriesList.SubtreeAsync (id, query, record.RecordClient).Result;
		}

		//lookup
		public static Category Find(long id) {
			return Category.FindAsync(id).Result;
		}

		public static Category Find(long id, Client client) {
			return Category.FindAsync(id, client).Result;
		}

		//create
		public static Category Create(Dictionary<string,object> tokens) {
			return Category.CreateAsync (tokens).Result;
		}

		public static Category Create(Dictionary<string,object> tokens, Client client){
			return Category.CreateAsync (tokens, client).Result;
		}

		//update
		public void Save() {
			SaveAsync().Wait();
		}

		public void Save(Dictionary<string,object> tokens) {
			SaveAsync (tokens).Wait();
		}

		//delete
		public void Delete() {
			DeleteAsync ().Wait();
		}

	}
}

