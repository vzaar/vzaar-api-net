namespace VzaarApi
{
	public class PresetsList : BaseResourceCollection<Preset, PresetsList>
	{
		public PresetsList()
			: this(Client.GetClient())
		{
		}

		public PresetsList(Client client)
			: base("encoding_presets", client)
		{
		}
	}//enc class
}//end namespace

