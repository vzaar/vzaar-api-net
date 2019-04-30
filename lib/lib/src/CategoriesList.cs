using System.Collections.Generic;

namespace VzaarApi
{
	public class CategoriesList : BaseResourceCollection<Category, CategoriesList>
	{
		public CategoriesList()
			: this(Client.GetClient())
		{
		}

		public CategoriesList(Client client)
			: base("categories", client)
		{
		}

		internal void FindSubtree(long id, Dictionary<string, string> query)
		{
			string path = "/" + id + "/subtree";

			records.Read(path, query);

			Initialize();
		}

		//get subtree
		public static CategoriesList Subtree(long id)
		{
			return Subtree(id, new Dictionary<string, string>(), Client.GetClient());
		}

		public static CategoriesList Subtree(long id, Client client)
		{
			return Subtree(id, new Dictionary<string, string>(), client);
		}

		public static CategoriesList Subtree(long id, Dictionary<string, string> query)
		{
			return Subtree(id, query, Client.GetClient());
		}

		public static CategoriesList Subtree(long id, Dictionary<string, string> query, Client client)
		{
			var categories = new CategoriesList(client);

			categories.FindSubtree(id, query);

			return categories;
		}
	}//end class
}//end namespace

