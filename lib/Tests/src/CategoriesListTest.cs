using NUnit.Framework;
using System;
using System.Collections.Generic;
using VzaarApi;

namespace tests
{
	[TestFixture ()]
	public class CategoriesListTest
	{
		[Test ()]
		public void NewCategoriesList ()
		{
			var list = new CategoriesList ();

			Assert.AreEqual (list.records.RecordEndpoint, "categories");
			Assert.IsInstanceOf<List<Record>> (list.records.List);
			Assert.IsInstanceOf<List<Category>> (list.Page);

			var client = new ClientMock (MockResponse.CategoriesList);
			list = new CategoriesList (client);

			Assert.AreEqual (list.records.RecordEndpoint, "categories");
			Assert.IsInstanceOf<List<Record>> (list.records.List);
			Assert.IsInstanceOf<List<Category>> (list.Page);

			Assert.IsInstanceOf<Client> (list.records.RecordClient);
		}

		[Test ()]
		public void Paginate ()
		{
			var list = CategoriesList.Paginate (new ClientMock(MockResponse.CategoriesList));

			var listClient = list.records.RecordClient;

			Assert.AreEqual (list.records.RecordEndpoint, "categories");

			foreach (var item in list.Page) {

				Assert.AreEqual (item.record.RecordEndpoint, "categories");
			}

			var per_page = "1";
			var query = new Dictionary<string,string>() {
				{"per_page",per_page}
			};

			var list2 = CategoriesList.Paginate (query, new ClientMock(MockResponse.CategoriesList));

			var countItem = 0;
			foreach (var item in list2.Page) {
				countItem++;
			}

			Assert.AreEqual (countItem.ToString(), per_page);

			list = CategoriesList.Paginate (new ClientMock (MockResponse.RecordPaginate));

			Assert.IsTrue (list.Next ());
			Assert.IsTrue (list.Prevous ());
			Assert.IsTrue (list.First ());
			Assert.IsTrue (list.Last ());

			list = CategoriesList.Paginate (new ClientMock (MockResponse.RecordPaginateEmpty));

			Assert.IsFalse (list.Next ());
			Assert.IsFalse (list.Prevous ());
			Assert.IsFalse (list.First ());
			Assert.IsFalse (list.Last ());

		}

		[Test()]
		public void EachItem ()
		{
			Client listClient = new ClientMock (MockResponse.CategoriesList);

			foreach (Category item in CategoriesList.EachItem (listClient)) {

				Assert.That (listClient, Is.SameAs (item.record.RecordClient));
				Assert.AreEqual (item.record.RecordEndpoint, "categories");

			}

			var per_page = "1";
			var query = new Dictionary<string,string>() {
				{"per_page",per_page}
			};

			int countItem = 0;
			foreach (Category item in CategoriesList.EachItem (query,listClient)) {

				countItem++;

			}

			Assert.AreEqual (countItem.ToString(), per_page);

		} 

		[Test()]
		public void Subtree() {

			CategoriesList subtree = CategoriesList.Subtree (1, new ClientMock (MockResponse.CategoriesList));

			do {
				foreach (var item in subtree.Page) {
					Assert.IsInstanceOf<Category> (item);
				}
			} while(subtree.Next ());
		}

	}
}

