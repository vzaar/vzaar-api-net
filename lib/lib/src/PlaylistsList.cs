namespace VzaarApi
{
	public class PlaylistsList : BaseResourceCollection<Playlist, PlaylistsList>
	{
		public PlaylistsList()
			: this(Client.GetClient())
		{
		}

		public PlaylistsList(Client client)
			: base("feeds/playlists", client)
		{
		}
	}//end class
}//end namespace