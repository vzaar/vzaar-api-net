namespace VzaarAPI
{
    /// <summary>
    /// This class represents a Vzaar video object. It contains information about the video such as
    /// the title, description, thumbnail URl and the play count as well as other details.
    /// </summary>
    public class Video
    {
        #region Private Members 
        
        private double _version;
        private int _id;
        private string _title;
        private string _description;
        private string _createdAt;
        private string _url;
        private string _thumbnailUrl;
        private int _playCount;
        private string _authorName;
        private string _authorUrl;
        private int _authorAccount;
        private int _videoCount;
        private double _duration;
        private int _width;
        private int _height;

        #endregion
        
        #region Public and Package Protected Methods               

        /// <summary>
        /// Protected constructor.
        /// </summary>
        /// <param name="version">The vzaar API version number</param>
        /// <param name="id">The video ID number</param>
        /// <param name="title">The video title. It may be null</param>
        /// <param name="description">The video description. It may be null</param>
        /// <param name="createdAt">The date time the video was uploaded</param>
        /// <param name="url">The link to the video page</param>
        /// <param name="thumbnailUrl">The URL link that points to the video thumbnail</param>
        /// <param name="playCount">The number of times the video has been played</param>
        /// <param name="authorName">The vzaar user name (i.e. their login)</param>
        /// <param name="authorUrl">The link to the vzaar user summary page </param>
        /// <param name="authorAccount">The number representing the users vzaar account</param>
        /// <param name="videoCount">The number of active videos in the users account </param>
        /// <param name="duration">The duration of the video</param>
        /// <param name="width">The width of the video</param>
        /// <param name="height">The height of the video</param>
        public Video(
            double version,
            int id,
            string title,
            string description,
            string createdAt,
            string url,
            string thumbnailUrl,
            int playCount,
            string authorName,
            string authorUrl,
            int authorAccount,
            int videoCount,
            double duration,
            int width,
            int height)
        {
            _version = version;
            _id = id;
            _title = title;
            _description = description;
            _createdAt = createdAt;
            _url = url;
            _thumbnailUrl = thumbnailUrl;
            _playCount = playCount;
            _authorName = authorName;
            _authorUrl = authorUrl;
            _authorAccount = authorAccount;
            _videoCount = videoCount;
            _duration = duration;
            _width = width;
            _height = height;
        }

        /// <summary>
        /// The vzaar user name (i.e. their login)
        /// </summary>
        /// <returns>The authorName</returns>
        public string GetAuthorName()
        {
            return _authorName;
        }


        /// <summary>
        /// The link to the vzaar user summary page.
        /// </summary>
        /// <returns>The link to the vzaar user summary page</returns>
        public string GetAuthorUrl()
        {
            return _authorUrl;
        }


        /// <summary>
        /// The number representing the users vzaar account. 1 represents a free account.
        /// </summary>
        /// <returns>The number representing the users vzaar account</returns>
        public int GetAuthorAccount()
        {
            return _authorAccount;
        }

        /// <summary>
        /// The number of active videos in the users account.
        /// </summary>
        /// <returns>The number of active videos in the users account</returns>
        public int GetVideoCount()
        {
            return _videoCount;
        }

        /// <summary>
        /// The vzaar API version number.
        /// </summary>
        /// <returns>The vzaar API version number</returns>
        public double GetVersion()
        {
            return _version;
        }

        /// <summary>
        /// The video ID number.
        /// </summary>
        /// <returns>The video ID number</returns>
        public int GetId()
        {
            return _id;
        }

        /// <summary>
        /// The video title. It may be null.
        /// </summary>
        /// <returns>The video title</returns>
        public string GetTitle()
        {
            return _title;
        }


        /// <summary>
        /// The video description. It may be null.
        /// </summary>
        /// <returns>The video description</returns>
        public string GetDescription()
        {
            return _description;
        }


        /// <summary>
        /// The date time the video was uploaded. Will be in UTC format.
        /// </summary>
        /// <returns>The date time the video was uploaded</returns>
        public string GetCreatedAt()
        {
            return _createdAt;
        }


        /// <summary>
        /// The link to the video page.
        /// </summary>
        /// <returns>The link to the video page</returns>
        public string GetUrl()
        {
            return _url;
        }


        /// <summary>
        /// The URL link that points to the video thumbnail. This is usually 120x90px.
        /// </summary>
        /// <returns>The URL link that points to the video thumbnail</returns>
        public string GetThumbnailUrl()
        {
            return _thumbnailUrl;
        }

        /// <summary>
        /// The number of times the video has been played.
        /// </summary>
        /// <returns>The number of times the video has been played</returns>
        public int GetPlayCount()
        {
            return _playCount;
        }

        /// <summary>
        /// The duration of the video.
        /// </summary>
        /// <returns>The duration of the video</returns>
        public double GetDuration()
        {
            return _duration;
        }

        /// <summary>
        /// The width of the video.
        /// </summary>
        /// <returns>The width of the video</returns>
        public int GetWidth()
        {
            return _width;
        }


        /// <summary>
        /// The height of the video.
        /// </summary>
        /// <returns>The height of the video</returns>
        public int GetHeight()
        {
            return _height;
        }


        /// <summary>
        /// String representation of the video bean.
        /// </summary>
        /// <returns>The string representing the beans details.</returns>
        public override string ToString()
        {
            string retString = 
                string.Format("version={0}, id={1}, title={2}, description={3}, createdAt={4}, url={5}, thumbnailUrl={6}", 
                _version, _id, _title, _description, _createdAt, _url, _thumbnailUrl);
            retString +=
                string.Format(", playCount={0}, duration={1}, width={2}, height={3}, authorName={4}, authorAccount={5}, authorUrl={6}, videoCount={7}",
                _playCount, _duration, _width, _height, _authorName, _authorAccount, _authorUrl, _videoCount);

            return retString;
        }

        #endregion
    }
}
