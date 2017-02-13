using System;
using System.Collections.Generic;

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

		public CategoriesList Subtree() {

			long id = (long)this ["id"];
			var query = new Dictionary<string,string> ();
			return CategoriesList.Subtree (id, query, record.RecordClient);
		}

		public CategoriesList Subtree(Dictionary<string, string> query) {

			long id = (long)this ["id"];
			return CategoriesList.Subtree (id, query, record.RecordClient);
		}

		//lookup
		public static Category Find(long id) {

			var category = new Category ();

			category.record.Read (id);

			return category;
		}

		public static Category Find(long id, Client client) {

			var category = new Category (client);

			category.record.Read (id);

			return category;
		}

		//create
		public static Category Create(Dictionary<string,object> tokens) {

			var category = new Category ();

			category.record.Create (tokens);

			return category;
		}

		public static Category Create(Dictionary<string,object> tokens, Client client){

			var category = new Category (client);

			category.record.Create (tokens);

			return category;
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

