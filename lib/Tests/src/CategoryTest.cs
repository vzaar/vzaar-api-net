using NUnit.Framework;
using System;
using System.Collections.Generic;
using VzaarApi;

namespace tests
{
	[TestFixture ()]
	public class CategoryTest
	{
		[Test ()]
		public void NewCategory ()
		{
			var category = new Category ();

			Assert.AreEqual (category.record.RecordEndpoint, "categories");
			Assert.IsInstanceOf<Client> (category.record.RecordClient);

			var client = new ClientMock (MockResponse.Category);
			category = new Category (client);

			Assert.AreEqual (category.record.RecordEndpoint, "categories");
			Assert.IsInstanceOf<Record> (category.record);
			Assert.IsInstanceOf<Client> (category.record.RecordClient);
			Assert.That (category.record.RecordClient, Is.SameAs (client));
		}

		[Test()]
		public void GetClient() {

			var category = new Category ();

			Assert.That (category.GetClient (), Is.SameAs (category.record.RecordClient));
		}

		[Test()]
		public void RateLimits() {

			var category = Category.Find (1, new ClientMock (MockResponse.Category));

			var client = category.GetClient ();

			Assert.AreEqual ("222",client.RateLimit);
			Assert.AreEqual ("122",client.RateRemaining);
			Assert.AreEqual ("33333",client.RateReset);
		}

		[Test()]
		public void Find() {

			var category = Category.Find (1, new ClientMock(MockResponse.Category));

			Assert.AreEqual (0, category.record.Parameters.Count);
			Assert.IsFalse (category.record.Edited);
			Assert.AreEqual (0, category.record.cache.Count);

			Assert.IsNotNull (category["id"]);
			Assert.IsNotNull (category ["name"]);
		}

		[Test()]
		public void Subtree() {

			var category = Category.Find (1, new ClientMock(MockResponse.Category));

			CategoriesList subtree = category.Subtree ();

			Assert.That (category.record.RecordClient, Is.SameAs (subtree.records.RecordClient));

			do {
				foreach (var item in subtree.Page) {
					Assert.IsInstanceOf<Category> (item);
					Assert.That (item.record.RecordClient, Is.SameAs (category.record.RecordClient));
				}
			} while(subtree.Next ());
		}

		[Test()]
		public void Create() {

			Dictionary<string, object> tokens = new Dictionary<string, object> () {
				{ "name", "Example Category" },
			};

			var category = Category.Create (tokens, new ClientMock(MockResponse.Category));

			Assert.AreEqual (0, category.record.Parameters.Count);
			Assert.IsFalse (category.record.Edited);
			Assert.AreEqual (0, category.record.cache.Count);

			Assert.IsNotNull (category ["id"]);
			Assert.IsNotNull (category ["name"]);
		}

		[Test()]
		public void Save() {

			Dictionary<string, object> tokens = new Dictionary<string, object> () {
				{ "name", "Example Category" },
			};

			var category = Category.Find (1, new ClientMock(MockResponse.Category));

			category.Save (tokens);

			Assert.AreEqual (0, category.record.Parameters.Count);
			Assert.IsFalse (category.record.Edited);
			Assert.AreEqual (0, category.record.cache.Count);

			Assert.IsNotNull (category["id"]);
			Assert.IsNotNull (category ["name"]);

			category ["move_to_root"] = true;

			Assert.IsTrue (category.Edited);

			Assert.AreEqual(true, (bool)category["move_to_root"]);

			category ["move_to_root"] = null;

			Assert.IsFalse (category.Edited);

			category ["move_to_root"] = true;

			category.Save ();

			Assert.IsFalse (category.Edited);

			Assert.AreEqual (0, category.record.Parameters.Count);
			Assert.IsFalse (category.record.Edited);
			Assert.AreEqual (0, category.record.cache.Count);

			Assert.IsNotNull (category["id"]);
			Assert.IsNotNull (category["name"]);
		}

		[Test()]
		public void Delete() {

			var category = Category.Find (1, new ClientMock(MockResponse.Category));

			category["name"] = "test name";

			Assert.IsNotNull (category["id"]);
			Assert.IsNotNull (category ["name"]);

			Assert.IsTrue (category.Edited);
			Assert.AreNotEqual (0, category.record.cache.Count);

			category.Delete ();

			Assert.IsNull (category ["name"]);

			Assert.IsFalse (category.Edited);
			Assert.AreEqual (0, category.record.cache.Count);

			var expected = "missing 'id'";
			try {

				var id = category["id"];

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
							StringAssert.Contains (expected, fe.Message);
					}
				}
			}


		}

		[Test()]
		public void ToTypeDef() {

			var category = Category.Find (1, new ClientMock(MockResponse.Category));

			var data = (VideoCategoryType)category.ToTypeDef (typeof(VideoCategoryType));

			Assert.AreEqual (data.id,(long)category["id"]);
			Assert.AreEqual (data.name,(string)category["name"]);

		}

	}//end class
}//end namespace

