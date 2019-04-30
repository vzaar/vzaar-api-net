using System.Collections.Generic;

namespace VzaarApi
{
	public class Recipe : BaseResource
	{
		//constructor
		public Recipe()
			: this(Client.GetClient())
		{
		}

		public Recipe(Client client)
			: base("ingest_recipes", client)
		{
		}

		/// <summary>
		/// Do not remove. This is required for use in BaseResourceCollection
		/// </summary>
		internal Recipe(Record item)
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

		//create
		public static Recipe Create(Dictionary<string, object> tokens)
		{
			return Create(tokens, Client.GetClient());
		}

		public static Recipe Create(Dictionary<string, object> tokens, Client client)
		{
			var recipe = new Recipe(client);

			recipe.record.Create(tokens);

			return recipe;
		}

		//lookup
		public static Recipe Find(long id)
		{
			return Find(id, Client.GetClient());
		}

		public static Recipe Find(long id, Client client)
		{
			var recipe = new Recipe(client);

			recipe.record.Read(id);

			return recipe;
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

