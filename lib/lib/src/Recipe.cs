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

		//create
		public static Recipe Create(Dictionary<string,object> tokens) {

			var recipe = new Recipe ();

			recipe.record.Create (tokens);

			return recipe;
		}

		public static Recipe Create(Dictionary<string,object> tokens, Client client){

			var recipe = new Recipe (client);

			recipe.record.Create (tokens);

			return recipe;
		}

		//lookup
		public static Recipe Find(long id) {
			
			var recipe = new Recipe ();

			recipe.record.Read (id);

			return recipe;
		}

		public static Recipe Find(long id, Client client) {

			var recipe = new Recipe (client);

			recipe.record.Read (id);

			return recipe;
		}

		//update
		public virtual void Save() {

			record.Update ();

		}

		public virtual void Save(Dictionary<string,object> tokens) {

			record.Update (tokens);

		}

		//delete
		public virtual void Delete() {

			record.Delete ();
			
		}
			
	}
}

