using NUnit.Framework;
using System;
using System.Collections.Generic;
using VzaarApi;

namespace tests
{
	[TestFixture ()]
	public class RecipesListTest
	{
		[Test ()]
		public void NewRecipesList ()
		{
			var list = new RecipesList ();

			Assert.AreEqual (list.records.RecordEndpoint, "ingest_recipes");
			Assert.IsInstanceOf<List<Record>> (list.records.List);
			Assert.IsInstanceOf<List<Recipe>> (list.Page);

			var client = new ClientMock (MockResponse.RecipesList);
			list = new RecipesList (client);

			Assert.AreEqual (list.records.RecordEndpoint, "ingest_recipes");
			Assert.IsInstanceOf<List<Record>> (list.records.List);
			Assert.IsInstanceOf<List<Recipe>> (list.Page);

			Assert.IsInstanceOf<Client> (list.records.RecordClient);
		}
		
		[Test ()]
		public void Paginate ()
		{
			var list = RecipesList.Paginate (new ClientMock(MockResponse.RecipesList));

			var listClient = list.records.RecordClient;

			Assert.AreEqual (list.records.RecordEndpoint, "ingest_recipes");

			foreach (var item in list.Page) {
				
				Assert.AreEqual (item.record.RecordEndpoint, "ingest_recipes");
			}

			var per_page = "1";
			var query = new Dictionary<string,string>() {
				{"per_page",per_page}
			};

			var list2 = RecipesList.Paginate (query, new ClientMock(MockResponse.RecipesList));

			var countItem = 0;
			foreach (var item in list2.Page) {
				countItem++;
			}

			Assert.AreEqual (countItem.ToString(), per_page);

			list = RecipesList.Paginate (new ClientMock (MockResponse.RecordPaginate));

			Assert.IsTrue (list.Next ());
			Assert.IsTrue (list.Prevous ());
			Assert.IsTrue (list.First ());
			Assert.IsTrue (list.Last ());

			list = RecipesList.Paginate (new ClientMock (MockResponse.RecordPaginateEmpty));

			Assert.IsFalse (list.Next ());
			Assert.IsFalse (list.Prevous ());
			Assert.IsFalse (list.First ());
			Assert.IsFalse (list.Last ());

		}

		[Test()]
		public void EachItem ()
		{
			Client listClient = new ClientMock (MockResponse.RecipesList);

			foreach (Recipe item in RecipesList.EachItem (listClient)) {

				Assert.That (listClient, Is.SameAs (item.record.RecordClient));
				Assert.AreEqual (item.record.RecordEndpoint, "ingest_recipes");

			}

			var per_page = "1";
			var query = new Dictionary<string,string>() {
				{"per_page",per_page}
			};

			int countItem = 0;
			foreach (Recipe item in RecipesList.EachItem (query,listClient)) {

				countItem++;

			}

			Assert.AreEqual (countItem.ToString(), per_page);

		}

		[Test()]
		public void ExtractUriQuery() {

			var link = "http://api.vzaar.com/api/v2/ingest_recipes?page=1&per_page=2";

			var list = new RecordsList ("endpoint");

			var query = list.ExtractUriQuery (link);

			string value;
			Assert.IsTrue(query.TryGetValue("page", out value));
			Assert.AreEqual ("1", value);

			Assert.IsTrue(query.TryGetValue("per_page", out value));
			Assert.AreEqual ("2", value);
		}

		[Test ()]
		public void RecordsListValidation ()
		{
			var expected = "Missing 'data' token";
			try {
				var list = RecipesList.Paginate (new ClientMock (MockResponse.MissingData));

				//if the exception is not thrown, the below assert fails
				bool assert = true;
				Assert.IsFalse(assert);

			} catch(VzaarApiException ve) {

				StringAssert.Contains (expected,ve.Message);

			} catch (Exception e){

				if (e is AggregateException) {
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten ();

					foreach (var fe in flatten.InnerExceptions) {
						if (fe is VzaarApiException)
							StringAssert.Contains (expected,fe.Message);
					}
				}
				
			}

			expected = "'data' value is not array";
			try {

				var list = RecipesList.Paginate (new ClientMock (MockResponse.DataNotArray));

				//if the exception is not thrown, the below assert fails
				bool assert = true;
				Assert.IsFalse(assert);

			} catch(VzaarApiException ve) {

				StringAssert.Contains (expected,ve.Message);

			} catch (Exception e){

				if (e is AggregateException) {
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten ();

					foreach (var fe in flatten.InnerExceptions) {
						if (fe is VzaarApiException)
							StringAssert.Contains (expected,fe.Message);
					}
				}

			}

			expected = "Missing 'meta' token";
			try {

				var list = RecipesList.Paginate (new ClientMock (MockResponse.MissingMeta));

				//if the exception is not thrown, the below assert fails
				bool assert = true;
				Assert.IsFalse(assert);

			} catch(VzaarApiException ve) {

				StringAssert.Contains (expected,ve.Message);

			} catch (Exception e){

				if (e is AggregateException) {
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten ();

					foreach (var fe in flatten.InnerExceptions) {
						if (fe is VzaarApiException)
							StringAssert.Contains (expected,fe.Message);
					}
				}

			}

			expected = "Missing 'links' token";
			try {

				var list = RecipesList.Paginate (new ClientMock (MockResponse.MissingLinks));

				//if the exception is not thrown, the below assert fails
				bool assert = true;
				Assert.IsFalse(assert);

			} catch(VzaarApiException ve) {

				StringAssert.Contains (expected,ve.Message);

			} catch (Exception e){

				if (e is AggregateException) {
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten ();

					foreach (var fe in flatten.InnerExceptions) {
						if (fe is VzaarApiException)
							StringAssert.Contains (expected,fe.Message);
					}
				}

			}

			expected = "Missing 'next' token";
			try {

				var list = RecipesList.Paginate (new ClientMock (MockResponse.MissingNext));

				//if the exception is not thrown, the below assert fails
				bool assert = true;
				Assert.IsFalse(assert);

			} catch(VzaarApiException ve) {

				StringAssert.Contains (expected,ve.Message);

			} catch (Exception e){

				if (e is AggregateException) {
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten ();

					foreach (var fe in flatten.InnerExceptions) {
						if (fe is VzaarApiException)
							StringAssert.Contains (expected,fe.Message);
					}
				}

			}

			expected = "Missing 'previous' token";
			try {

				var list = RecipesList.Paginate (new ClientMock (MockResponse.MissingPrevious));

				//if the exception is not thrown, the below assert fails
				bool assert = true;
				Assert.IsFalse(assert);

			} catch(VzaarApiException ve) {

				StringAssert.Contains (expected,ve.Message);

			} catch (Exception e){

				if (e is AggregateException) {
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten ();

					foreach (var fe in flatten.InnerExceptions) {
						if (fe is VzaarApiException)
							StringAssert.Contains (expected,fe.Message);
					}
				}

			}

			expected = "Missing 'last' token";
			try {

				var list = RecipesList.Paginate (new ClientMock (MockResponse.MissingLast));

				//if the exception is not thrown, the below assert fails
				bool assert = true;
				Assert.IsFalse(assert);

			} catch(VzaarApiException ve) {

				StringAssert.Contains (expected,ve.Message);

			} catch (Exception e){

				if (e is AggregateException) {
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten ();

					foreach (var fe in flatten.InnerExceptions) {
						if (fe is VzaarApiException)
							StringAssert.Contains (expected,fe.Message);
					}
				}

			}

			expected = "Missing 'first' token";
			try {

				var list = RecipesList.Paginate (new ClientMock (MockResponse.MissingFirst));

				//if the exception is not thrown, the below assert fails
				bool assert = true;
				Assert.IsFalse(assert);

			} catch(VzaarApiException ve) {

				StringAssert.Contains (expected,ve.Message);

			} catch (Exception e){

				if (e is AggregateException) {
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten ();

					foreach (var fe in flatten.InnerExceptions) {
						if (fe is VzaarApiException)
							StringAssert.Contains (expected,fe.Message);
					}
				}

			}

		}// end 
	}
}

