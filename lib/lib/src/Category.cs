using System.Collections.Generic;

namespace VzaarApi
{
	public class Category : BaseResource
	{
		//constructor
		public Category()
			: this (Client.GetClient())
		{
		}

		public Category(Client client)
			: base("categories", client)
		{
		}

		/// <summary>
		/// Do not remove. This is required for use in BaseResourceCollection
		/// </summary>
		internal Category(Record item)
			: base(item)
		{
			record = item;
		}

		public bool Edited => record.Edited;

		public object this[string index]
		{
			get => record[index];
			set => record[index] = value;
		}

		public CategoriesList Subtree()
		{
			return Subtree(new Dictionary<string, string>());
		}

		public CategoriesList Subtree(Dictionary<string, string> query)
		{
			long id = (long)this["id"];
			return CategoriesList.Subtree(id, query, record.RecordClient);
		}

		//lookup
		public static Category Find(long id)
		{
			return Find(id, Client.GetClient());
		}

		public static Category Find(long id, Client client)
		{
			var category = new Category(client);

			category.record.Read(id);

			return category;
		}

		//create
		public static Category Create(Dictionary<string, object> tokens)
		{
			return Create(tokens, Client.GetClient());
		}

		public static Category Create(Dictionary<string, object> tokens, Client client)
		{
			var category = new Category(client);

			category.record.Create(tokens);

			return category;
		}

		//update
		public virtual void Save()
		{
			record.Update();
		}

		public virtual void Save(Dictionary<string, object> tokens)
		{
			record.Update(tokens);
		}

		//delete
		public virtual void Delete()
		{
			record.Delete();
		}
	}
}

