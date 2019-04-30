namespace VzaarApi
{
	public class VideosList : BaseResourceCollection<Video, VideosList>
	{
		public VideosList()
			: this(Client.GetClient())
		{
		}

		public VideosList(Client client)
			: base("videos", client)
		{
		}
	}//end class
}//end namespace

