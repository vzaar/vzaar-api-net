using NUnit.Framework;
using System;
using System.IO;
using System.Collections.Generic;
using VzaarApi;

namespace tests
{
	[TestFixture ()]
	public class SignatureTest
	{
		[Test ()]
		public void NewSignature ()
		{
			var signature = new Signature ();

			Assert.AreEqual (signature.record.RecordEndpoint, "signature");
			Assert.IsInstanceOf<Client> (signature.record.RecordClient);

			var client = new ClientMock (MockResponse.Video);
			signature = new Signature (client);

			Assert.AreEqual (signature.record.RecordEndpoint, "signature");
			Assert.IsInstanceOf<Record> (signature.record);
			Assert.IsInstanceOf<Client> (signature.record.RecordClient);
			Assert.That (signature.record.RecordClient, Is.SameAs (client));

		}

		[Test()]
		public void GetClient() {

			var recipe = new Recipe ();

			Assert.That (recipe.GetClient (), Is.SameAs (recipe.record.RecordClient));
		}

		[Test()]
		public void CreateSingle() {

			var tokens = new Dictionary<string,object> (){
				{"uploader","my uploader nunit"}
			};

			Signature signature = Signature.Single (tokens, new ClientMock (MockResponse.Signature));

			Assert.IsNotNull (signature["x-amz-signature"]);
			Assert.IsNotNull (signature ["guid"]);
			Assert.IsNull (signature ["parts"]);

		}

		[Test()] 
		public void CreateMultipart() {

			FileInfo file = new FileInfo ("../../src/Fixture/movie-5mb.mp4");

			var tokens = new Dictionary<string,object> (){
				{"uploader","my uploader nunit"},
				{"filename","myFile"},
				{"filesize", file.Length}
			};

			Signature signature = Signature.Multipart (tokens, new ClientMock (MockResponse.Signature));

			Assert.IsNotNull (signature["x-amz-signature"]);
			Assert.IsNotNull (signature ["guid"]);
			Assert.IsNotNull (signature ["parts"]);

		}

		[Test()] 
		public void CreateMultipartFromFile() {

			var filepath = "../../src/Fixture/movie-5mb.mp4";

			Signature signature = Signature.Create (filepath, new ClientMock (MockResponse.Signature));

			Assert.IsNotNull (signature["x-amz-signature"]);
			Assert.IsNotNull (signature ["guid"]);
			Assert.IsNotNull (signature ["parts"]);

		}

		[Test()] 
		public void CreateSingleFromFile() {

			var filepath = "../../src/Fixture/movie-1mb.mp4";

			Signature signature = Signature.Create (filepath, new ClientMock (MockResponse.Signature));

			Assert.IsNotNull (signature["x-amz-signature"]);
			Assert.IsNotNull (signature ["guid"]);
			Assert.IsNull (signature ["parts"]);

		}

		[Test()]
		public void ToTypeDef() {

			var tokens = new Dictionary<string,object> (){
				{"uploader","my uploader nunit"}
			};

			var signature = Signature.Single (tokens, new ClientMock (MockResponse.Signature));

			var data = (UploadSignatureType)signature.ToTypeDef (typeof(UploadSignatureType));

			Assert.AreEqual (data.key,(string)signature["key"]);
			Assert.AreEqual (data.guid,(string)signature["guid"]);

		}
	}
}

