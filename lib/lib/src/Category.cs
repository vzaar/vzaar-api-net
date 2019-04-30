using System.Collections.Generic;
using System.Threading.Tasks;

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
			return FindAsync(id, client).Result;
		}

		public static Task<Category> FindAsync(long id)
		{
			return FindAsync(id, Client.GetClient());
		}

		public static async Task<Category> FindAsync(long id, Client client)
		{
			var item = new Category(client);

			await item.record.Read(id).ConfigureAwait(false);

			return item;
		}

		//create
		public static Category Create(Dictionary<string, object> tokens)
		{
			return Create(tokens, Client.GetClient());
		}

		public static Category Create(Dictionary<string, object> tokens, Client client)
		{
			return CreateAsync(tokens, client).Result;
		}

		public static Task<Category> CreateAsync(Dictionary<string, object> tokens)
		{
			return CreateAsync(tokens, Client.GetClient());
		}

		public static async Task<Category> CreateAsync(Dictionary<string, object> tokens, Client client)
		{
			var resource = new Category(client);

			await resource.record.Create(tokens).ConfigureAwait(false);

			return resource;
		}

		//update
		public virtual void Save()
		{
			SaveAsync().Wait();
		}

		public virtual void Save(Dictionary<string, object> tokens)
		{
			SaveAsync(tokens).Wait();
		}

		public virtual async Task SaveAsync()
		{
			await record.Update().ConfigureAwait(false);
		}

		public virtual async Task SaveAsync(Dictionary<string, object> tokens)
		{
			await record.Update(tokens).ConfigureAwait(false);
		}

		//delete
		public virtual void Delete()
		{
			DeleteAsync().Wait();
		}

		public virtual async Task DeleteAsync()
		{
			await record.Delete().ConfigureAwait(false);
		}
	}
}

