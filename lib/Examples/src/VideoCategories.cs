using System;
using System.Collections.Generic;
using VzaarApi;

namespace Examples
{
	public class VideoCategories
	{
		public VideoCategories ()
		{
		}

		class myType {

			public long? id;
			public string name;

			//parameter not existing in record
			public string mydata;
		}

		public static void UsingVideoCategory(string id, string token) {

			try {

				Console.WriteLine ("--Create(tokens)-parent-");

				var parent_tokens = new Dictionary<string,object> () {
					{"name", "Parent Category"},
				};

				Category parent = Category.Create(parent_tokens);

				Console.WriteLine ("--Access with property name--");
				Console.WriteLine ("Id: " + parent["id"] + " Name: " + parent["name"]+" ParentId: "+parent["parent_id"]);

				var parentId = (long)parent["id"];

				Console.WriteLine ("--Access with the record Type--");

				var record1 = (VideoCategoryType)parent.ToTypeDef(typeof(VideoCategoryType));
				Console.WriteLine ("Id: " + record1.id + " Name: " + record1.name+" Created: " + record1.created_at);

				Console.WriteLine ("--Access with custom Type--");

				var record2 = (myType)parent.ToTypeDef(typeof(myType));
				Console.WriteLine ("Id: " + record2.id + " Name: " + record2.name);


				Console.WriteLine ("--Create(tokens)-child-");

				var child_tokens = new Dictionary<string,object> () {
					{"name", "Category Child"},
				};

				Category child = Category.Create(child_tokens);

				Console.WriteLine ("--Access with property name--");
				Console.WriteLine ("Id: " + child["id"] + " Name: " + child["name"]+" ParentId: "+child["parent_id"]);

				var childId = (long)parent["id"];


				Console.WriteLine ("--Save(tokens)--");

				var child_tokens2 = new Dictionary<string,object> () {
					{"parent_id", parentId}
				};

				child.Save(child_tokens2);

				Console.WriteLine ("--Save()--");

				child["name"] = "New Category Child Name";

				if(child.Edited)
					child.Save();
				else
					Console.WriteLine("Not edied.");

				Console.WriteLine ("Id: " + child["id"] + " Name: " + child["name"]+" ParentId: "+child["parent_id"]);

				Console.WriteLine ("--Find(id)-parent-");

				var parent2 = Category.Find(parentId);

				Console.WriteLine ("--Access with property name--");
				Console.WriteLine ("Id: " + parent2["id"] + " Name: " + parent2["name"]);
				Console.WriteLine("Node childrens: "+ parent2["node_children_count"]);

				Console.WriteLine ("--Delete()--");

				parent2.Delete();

				Console.WriteLine ("--Find(id)-parent-");

				try{
					var parent3 = Category.Find(parentId);

				} catch(VzaarApiException ve) {

					Console.Write ("!!!!!!!!! EXCEPTION !!!!!!!!!");
					Console.WriteLine (ve.Message);

				} catch (Exception e) {

					Console.Write ("!!!!!!!!! EXCEPTION !!!!!!!!!");
					Console.WriteLine (e.Message);

					if (e is AggregateException) {
						AggregateException ae = (AggregateException)e;

						var flatten = ae.Flatten ();

						foreach (var fe in flatten.InnerExceptions) {
							if (fe is VzaarApiException)
								Console.WriteLine (fe.Message);
						}
					}
				}

				Console.WriteLine ("--Find(id)-child-");

				try{
					var parent3 = Category.Find(childId);

				} catch(VzaarApiException ve) {

					Console.Write ("!!!!!!!!! EXCEPTION !!!!!!!!!");
					Console.WriteLine (ve.Message);

				} catch (Exception e) {

					Console.Write ("!!!!!!!!! EXCEPTION !!!!!!!!!");
					Console.WriteLine (e.Message);

					if (e is AggregateException) {
						AggregateException ae = (AggregateException)e;

						var flatten = ae.Flatten ();

						foreach (var fe in flatten.InnerExceptions) {
							if (fe is VzaarApiException)
								Console.WriteLine (fe.Message);
						}
					}

				}

			} catch(VzaarApiException ve) {

				Console.Write ("!!!!!!!!! EXCEPTION !!!!!!!!!");
				Console.WriteLine (ve.Message);

			} catch (Exception e) {

				Console.Write ("!!!!!!!!! EXCEPTION !!!!!!!!!");
				Console.WriteLine (e.Message);

				if (e is AggregateException) {
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten ();

					foreach (var fe in flatten.InnerExceptions) {
						if (fe is VzaarApiException)
							Console.WriteLine (fe.Message);
					}
				}

			}
		}

		public static void ReadingSubtreeCategoriesList(string id, string token, long recordId) {

			try {
				Console.WriteLine ("--Subtree--");

				CategoriesList subtree = CategoriesList.Subtree (recordId);

				foreach (var item in subtree.Page) {
					Console.WriteLine ("Id: " + item["id"] + " ParentId: "+item["parent_id"]);
				}

				while (subtree.Next ()) {
					foreach (var item in subtree.Page) {
						Console.WriteLine ("Id: " + item["id"] + " ParentId: "+item["parent_id"]);
					}
				}

				Dictionary<string,string> query = new Dictionary<string, string>() {
					{"per_page", "2"}
				};

				Console.WriteLine ("--Subtree(query)--");

				var subtree2 = CategoriesList.Subtree (3193,query,new Client());

				foreach (var item in subtree2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " ParentId: "+item["parent_id"]);
				}
			} catch(VzaarApiException ve) {

				Console.Write ("!!!!!!!!! EXCEPTION !!!!!!!!!");
				Console.WriteLine (ve.Message);

			} catch (Exception e) {

				Console.Write ("!!!!!!!!! EXCEPTION !!!!!!!!!");
				Console.WriteLine (e.Message);

				if (e is AggregateException) {
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten ();

					foreach (var fe in flatten.InnerExceptions) {
						if (fe is VzaarApiException)
							Console.WriteLine (fe.Message);
					}
				}

			}
		}

		public static void ReadingCategoriesList(string id, string token) {

			try {

				Console.WriteLine ("--EachItem()--");
				foreach(Category item in CategoriesList.EachItem()) {
					Console.WriteLine ("Id: " + item["id"] + " ParentId: "+ item["parent_id"]);
				}

				Console.WriteLine ("--EachItem(query)--");

				Dictionary<string,string> query = new Dictionary<string, string>() {
					{"sort","id"},
					{"order", "desc"},
					{"per_page", "2"}
				};

				Client client = new Client() {CfgClientId = id, CfgAuthToken = token};
				foreach(var item in CategoriesList.EachItem(query,client)) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

				Console.WriteLine ("--Paginate--");

				var items1 = CategoriesList.Paginate ();
				foreach (var item in items1.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

				Console.WriteLine ("--Paginate(query)--");

				var items2 = CategoriesList.Paginate (query, new Client());

				Console.WriteLine ("--Paginate(query)--Initial-First--");
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

				while(items2.Next()) {
					Console.WriteLine ("--Paginate(query)--Next--");

					foreach (var item in items2.Page) {
						Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
					}
				}

				Console.WriteLine ("--Paginate(query)--Previous--");
				items2.Prevous();
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

				Console.WriteLine ("--Paginate(query)--Last--");
				items2.Last();
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

				Console.WriteLine ("--Paginate(query)--First--");
				items2.First();
				foreach (var item in items2.Page) {
					Console.WriteLine ("Id: " + item["id"] + " Name: " + item["name"]);
				}

			} catch(VzaarApiException ve) {

				Console.Write ("!!!!!!!!! EXCEPTION !!!!!!!!!");
				Console.WriteLine (ve.Message);

			} catch (Exception e) {

				Console.Write ("!!!!!!!!! EXCEPTION !!!!!!!!!");
				Console.WriteLine (e.Message);

				if (e is AggregateException) {
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten ();

					foreach (var fe in flatten.InnerExceptions) {
						if (fe is VzaarApiException)
							Console.WriteLine (fe.Message);
					}
				}

			}

		}
	}
}

