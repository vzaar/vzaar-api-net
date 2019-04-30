using System.Collections.Generic;
using System.Threading.Tasks;

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

		internal async Task FindSubtree(long id, Dictionary<string, string> query)
		{
			string path = "/" + id + "/subtree";

			await records.Read(path, query).ConfigureAwait(false);

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
			return SubtreeAsync(id, query, client).Result;
		}

		public static Task<CategoriesList> SubtreeAsync(long id)
		{
			return SubtreeAsync(id, new Dictionary<string, string>(), Client.GetClient());
		}

		public static Task<CategoriesList> SubtreeAsync(long id, Client client)
		{
			return SubtreeAsync(id, new Dictionary<string, string>(), client);
		}

		public static Task<CategoriesList> SubtreeAsync(long id, Dictionary<string, string> query)
		{
			return SubtreeAsync(id, query, Client.GetClient());
		}

		public static async Task<CategoriesList> SubtreeAsync(long id, Dictionary<string, string> query, Client client)
		{
			var categories = new CategoriesList(client);

			await categories.FindSubtree(id, query).ConfigureAwait(false);

			return categories;
		}
	}//end class
}//end namespace

