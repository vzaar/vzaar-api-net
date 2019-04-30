using NUnit.Framework;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using VzaarApi;

namespace tests
{
	[TestFixture ()]
	public class RecipeTest
	{
		[Test ()]
		public void NewRecipe ()
		{
			var recipe = new Recipe ();

			Assert.AreEqual (recipe.record.RecordEndpoint, "ingest_recipes");
			Assert.IsInstanceOf<Client> (recipe.record.RecordClient);

			var client = new ClientMock (MockResponse.Recipe);
			recipe = new Recipe (client);

			Assert.AreEqual (recipe.record.RecordEndpoint, "ingest_recipes");
			Assert.IsInstanceOf<Record> (recipe.record);
			Assert.IsInstanceOf<Client> (recipe.record.RecordClient);
			Assert.That (recipe.record.RecordClient, Is.SameAs (client));
		}

		[Test()]
		public void GetClient() {

			var recipe = new Recipe ();

			Assert.That (recipe.GetClient (), Is.SameAs (recipe.record.RecordClient));
		}

		[Test()]
		public void RateLimits() {

			var recipe = Recipe.Find (1, new ClientMock (MockResponse.Recipe));

			var client = recipe.GetClient ();

			Assert.AreEqual ("222",client.RateLimit);
			Assert.AreEqual ("122",client.RateRemaining);
			Assert.AreEqual ("33333",client.RateReset);
		}

		[Test()]
		public void Find() {

			var recipe = Recipe.Find (1, new ClientMock(MockResponse.Recipe));

			Assert.AreEqual (0, recipe.record.Parameters.Count);
			Assert.IsFalse (recipe.record.Edited);
			Assert.AreEqual (0, recipe.record.cache.Count);

			Assert.IsNotNull (recipe["id"]);
			Assert.IsNotNull (recipe ["name"]);
		}

		[Test()]
		public void Create() {

			Dictionary<string, object> tokens = new Dictionary<string, object> () {
				{ "name", "Example Recipe" },
			};

			var recipe = Recipe.Create (tokens, new ClientMock(MockResponse.Recipe));

			Assert.AreEqual (0, recipe.record.Parameters.Count);
			Assert.IsFalse (recipe.record.Edited);
			Assert.AreEqual (0, recipe.record.cache.Count);

			Assert.IsNotNull (recipe["id"]);
			Assert.IsNotNull (recipe ["name"]);
		}

		[Test()]
		public void Save() {

			Dictionary<string, object> tokens = new Dictionary<string, object> () {
				{ "name", "Example Recipe" },
			};

			var recipe = Recipe.Find (1, new ClientMock(MockResponse.Recipe));

			recipe.Save (tokens);

			Assert.AreEqual (0, recipe.record.Parameters.Count);
			Assert.IsFalse (recipe.record.Edited);
			Assert.AreEqual (0, recipe.record.cache.Count);

			Assert.IsNotNull (recipe["id"]);
			Assert.IsNotNull (recipe ["name"]);

			recipe ["multipass"] = true;

			Assert.IsTrue (recipe.Edited);

			Assert.AreEqual(true, (bool)recipe["multipass"]);

			recipe ["multipass"] = null;

			Assert.IsFalse (recipe.Edited);

			recipe ["multipass"] = true;

			recipe.Save ();

			Assert.IsFalse (recipe.Edited);

			Assert.AreEqual (0, recipe.record.Parameters.Count);
			Assert.IsFalse (recipe.record.Edited);
			Assert.AreEqual (0, recipe.record.cache.Count);

			Assert.IsNotNull (recipe["id"]);
			Assert.IsNotNull (recipe ["name"]);
		}

		[Test()]
		public void Delete() {

			var recipe = Recipe.Find (1, new ClientMock(MockResponse.Recipe));

			recipe["name"] = "test name";

			Assert.IsNotNull (recipe["id"]);
			Assert.IsNotNull (recipe ["name"]);

			Assert.IsTrue (recipe.Edited);
			Assert.AreNotEqual (0, recipe.record.cache.Count);

			recipe.Delete ();

			Assert.IsNull (recipe ["name"]);

			Assert.IsFalse (recipe.Edited);
			Assert.AreEqual (0, recipe.record.cache.Count);

			var expected = "missing 'id'";
			try {

				var id = recipe["id"];

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
		public void IndexerGet() {

			var recipe = Recipe.Find (1, new ClientMock (MockResponse.Recipe));

			var id = (long)recipe ["id"];
			var name = (string)recipe ["name"];

			Assert.AreEqual (id,(long)recipe.record.Data["data"]["id"]);
			Assert.AreEqual (name,(string)recipe.record.Data["data"]["name"]);

		}

		[Test()]
		public void IndexerSet() {

			var recipe = Recipe.Find (1, new ClientMock (MockResponse.Recipe));

			object value;
			Assert.AreEqual (0, recipe.record.Parameters.Count);
			Assert.IsFalse(recipe.record.Parameters.TryGetValue("multipass",out value));

			recipe ["multipass"] = true;

			JToken token;
			Assert.IsFalse(recipe.record.Data.TryGetValue("multipass",out token));
			Assert.IsTrue ((bool)recipe["multipass"]);

			Assert.AreEqual (1, recipe.record.Parameters.Count);
			Assert.IsTrue(recipe.record.Parameters.TryGetValue("multipass",out value));
			Assert.AreEqual (true, (bool)value);

			recipe ["multipass"] = false;

			Assert.IsFalse(recipe.record.Data.TryGetValue("multipass",out token));
			Assert.IsFalse ((bool)recipe["multipass"]);

			Assert.AreEqual (1, recipe.record.Parameters.Count);
			Assert.IsTrue(recipe.record.Parameters.TryGetValue("multipass",out value));
			Assert.AreEqual (false, (bool)value);

			Assert.IsTrue (recipe.Edited);

			recipe ["multipass"] = null;

			Assert.IsFalse(recipe.record.Data.TryGetValue("multipass",out token));
			Assert.IsFalse ((bool)recipe["multipass"]);

			Assert.AreEqual (0, recipe.record.Parameters.Count);
			Assert.IsFalse(recipe.record.Parameters.TryGetValue("multipass",out value));

			Assert.IsFalse (recipe.Edited);
		}

		[Test()]
		public void ToTypeDef() {

			var recipe = Recipe.Find (1, new ClientMock(MockResponse.Recipe));

			var data = (IngestRecipeType)recipe.ToTypeDef (typeof(IngestRecipeType));

			Assert.AreEqual (data.id,(long)recipe.record.Data["data"]["id"]);
			Assert.AreEqual (data.name,(string)recipe.record.Data["data"]["name"]);

		}

		[Test()] 
		public void RecordValidate() {

			var expected = "Missing 'data' token";
			try {
				
				Recipe.Find (1, new ClientMock (MockResponse.MissingData));

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
		public void RecordIndexerGetValidate() {

			var expected = "missing 'id'";
			try {

				var recipe = Recipe.Find (1, new ClientMock (MockResponse.MissingId));

				var id = recipe["id"];

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

	}//end class
}//end namespace