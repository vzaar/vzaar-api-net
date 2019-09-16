using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VzaarApi
{
	public class Recipe
	{

		internal Record record;

		//constructor
		public Recipe ()
		{
			record = new Record ("ingest_recipes");

		}

		public Recipe (Client client)
		{
			record = new Record ("ingest_recipes", client);
		}

		internal Recipe (Record item)
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

		// ASYNC methods 

		//create
		public async static Task<Recipe> CreateAsync(Dictionary<string,object> tokens) {

			var recipe = new Recipe ();

			await recipe.record.CreateAsync (tokens).ConfigureAwait(false);

			return recipe;
		}

		public async static Task<Recipe> CreateAsync(Dictionary<string,object> tokens, Client client){

			var recipe = new Recipe (client);

			await recipe.record.CreateAsync (tokens).ConfigureAwait(false);

			return recipe;
		}

		//lookup
		public async static Task<Recipe> FindAsync(long id) {
			
			var recipe = new Recipe ();

			await recipe.record.ReadAsync (id).ConfigureAwait(false);

			return recipe;
		}

		public async static Task<Recipe> FindAsync(long id, Client client) {

			var recipe = new Recipe (client);

			await recipe.record.ReadAsync (id).ConfigureAwait(false);

			return recipe;
		}

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

		//Synchronous methods

		//create
		public static Recipe Create(Dictionary<string,object> tokens) {
			return Recipe.CreateAsync (tokens).Result;
		}

		public static Recipe Create(Dictionary<string,object> tokens, Client client){
			return Recipe.CreateAsync (tokens, client).Result;
		}

		//lookup
		public static Recipe Find(long id) {
			return Recipe.FindAsync (id).Result;
		}

		public static Recipe Find(long id, Client client) {
			return Recipe.FindAsync (id, client).Result;
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

