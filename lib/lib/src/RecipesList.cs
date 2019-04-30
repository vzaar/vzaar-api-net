namespace VzaarApi
{
	public class RecipesList : BaseResourceCollection<Recipe, RecipesList>
	{
		public RecipesList()
			: this(Client.GetClient())
		{
		}

		public RecipesList(Client client)
			: base("ingest_recipes", client)
		{
		}
	}//end class
}//end namespace