namespace VzaarAPI
{
    /// <summary>
    /// This class represents a Vzaar User, along with their account type, video quantity and login name.
    /// </summary>
    public class User
    {        
        #region Private Members        

        private double _version;
        private string _authorName;
        private int _authorId;
        private string _authorUrl;
        private int _authorAccount;
        private string _createdAt;
        private int _videoCount;
        private int _playCount;


        #endregion
        
        #region Public and Package Protected Methods        

        /// <summary>
        /// A parameterised constructor
        /// </summary>
        /// <param name="version">The vzaar API version number</param>
        /// <param name="authorName">The vzaar user name (i.e. their login)</param>
        /// <param name="authorId">The vzaar user id </param>
        /// <param name="authorUrl">A link to the vzaar user summary page</param>
        /// <param name="authorAccount">a number representing the users vzaar 
        /// account. 1 represents a free account</param>
        /// <param name="createdAt">the date time the video was uploaded. Will be 
        /// in UTC format</param>
        /// <param name="videoCount">the number of active videos in the users 
        /// account</param>
        /// <param name="playCount">the number of times all the users videos 
        /// have been played</param>
        public User(double version, 
            string authorName, 
            int authorId, 
            string authorUrl,
            int authorAccount, 
            string createdAt, 
            int videoCount, 
            int playCount)
        {
            _version = version;
            _authorName = authorName;
            _authorId = authorId;
            _authorUrl = authorUrl;
            _authorAccount = authorAccount;
            _createdAt = createdAt;
            _videoCount = videoCount;
            _playCount = playCount;
        }

        /// <summary>
        /// The vzaar API version number.
        /// </summary>
        /// <returns>The vzaar API version number.</returns>
        public double GetVersion()
        {
            return _version;
        }

        /// <summary>
        /// The vzaar user name (i.e. their login).
        /// </summary>
        /// <returns>The vzaar user name (i.e. their login)</returns>
        public string GetAuthorName()
        {
            return _authorName;
        }

        /// <summary>
        /// The vzaar user id
        /// </summary>
        /// <returns>The vzaar user id</returns>
        public int GetAuthorId()
        {
            return _authorId;
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
        /// A number representing the users vzaar account. 1 represents a free account.
        /// </summary>
        /// <returns>A number representing the users vzaar account</returns>
        public int GetAuthorAccount()
        {
            return _authorAccount;
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
        /// The number of active videos in the users account.
        /// </summary>
        /// <returns>The number of active videos in the users account</returns>
        public int GetVideoCount()
        {
            return _videoCount;
        }


        /// <summary>
        /// The number of times all the users videos have been played.
        /// </summary>
        /// <returns>The number of times all the users videos have been played</returns>
        public int GetPlayCount()
        {
            return _playCount;
        }


        /// <summary>
        /// String representation of the user bean.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            return
                "version=" + _version +
                ", authorName=" + _authorName +
                ", authorId=" + _authorId +
                ", authorUrl=" + _authorUrl +
                ", authorAccount=" + _authorAccount +
                ", createdAt=" + _createdAt +
                ", videoCount=" + _videoCount +
                ", playCount=" + _playCount;
        }
        #endregion
    }
}
