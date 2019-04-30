using NUnit.Framework;
using System;
using VzaarApi;

namespace tests
{
	[TestFixture ()]
	public class PresetTest
	{
		[Test ()]
		public void NewPreset ()
		{
			var preset = new Preset ();

			Assert.AreEqual (preset.record.RecordEndpoint, "encoding_presets");
			Assert.IsInstanceOf<Client> (preset.record.RecordClient);

			var client = new ClientMock (MockResponse.Preset);
			preset = new Preset (client);

			Assert.AreEqual (preset.record.RecordEndpoint, "encoding_presets");
			Assert.IsInstanceOf<Record> (preset.record);
			Assert.IsInstanceOf<Client> (preset.record.RecordClient);
			Assert.That (preset.record.RecordClient, Is.SameAs (client));
		}

		[Test()]
		public void GetClient() {

			var preset = new Preset ();

			Assert.That (preset.GetClient (), Is.SameAs (preset.record.RecordClient));
		}

		[Test()]
		public void RateLimits() {

			var preset = Preset.Find (1, new ClientMock (MockResponse.Preset));

			var client = preset.GetClient ();

			Assert.AreEqual ("222",client.RateLimit);
			Assert.AreEqual ("122",client.RateRemaining);
			Assert.AreEqual ("33333",client.RateReset);
		}

		[Test()]
		public void Find() {

			var preset = Preset.Find (1, new ClientMock(MockResponse.Preset));

			Assert.AreEqual (0, preset.record.Parameters.Count);
			Assert.IsFalse (preset.record.Edited);
			Assert.AreEqual (0, preset.record.cache.Count);

			Assert.IsNotNull (preset["id"]);
			Assert.IsNotNull (preset ["name"]);
		}

		[Test()]
		public void ToTypeDef() {

			var preset = Preset.Find (1, new ClientMock(MockResponse.Preset));

			var data = (EncodingPresetType)preset.ToTypeDef (typeof(EncodingPresetType));

			Assert.AreEqual (data.id,(long)preset["id"]);
			Assert.AreEqual (data.name,(string)preset["name"]);

		}
			
	}
}

