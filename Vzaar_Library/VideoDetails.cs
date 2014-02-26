

using System;
namespace VzaarAPI
{
    /// <summary>
    /// The VideoDetails class holds more detailed information about a video than that of the Video class.
    /// Further information like thumbnail width/height and HTML embedding is stored.
    /// </summary>
    public class VideoDetails
    {
        #region " Properties "

        private string _type;
        public string type
        {
            get { return _type; }
            set { _type = value; }
        }
        private double _version;
        public double version
        {
            get { return _version; }
            set { _version = value; }
        }
        private string _title;
        public string title
        {
            get { return _title; }
            set { _title = value; }
        }
        private string _description;

        public string description
        {
            get { return _description; }
            set { _description = value; }
        }
        private string _authorName;

        public string authorName
        {
            get { return _authorName; }
            set { _authorName = value; }
        }
        private string _authorUrl;

        public string authorUrl
        {
            get { return _authorUrl; }
            set { _authorUrl = value; }
        }
        private int _authorAccount;

        public int authorAccount
        {
            get { return _authorAccount; }
            set { _authorAccount = value; }
        }
		private int _playCount;

		public int playCount
		{
			get { return _playCount; }
			set { _playCount = value; }
		}

        private string _providerName;

        public string providerName
        {
            get { return _providerName; }
            set { _providerName = value; }
        }
        private string _providerUrl;

        public string providerUrl
        {
            get { return _providerUrl; }
            set { _providerUrl = value; }
        }
        private string _thumbnailUrl;

        public string thumbnailUrl
        {
            get { return _thumbnailUrl; }
            set { _thumbnailUrl = value; }
        }
        private int _thumbnailWidth;

        public int thumbnailWidth
        {
            get { return _thumbnailWidth; }
            set { _thumbnailWidth = value; }
        }
        private int _thumbnailHeight;

        public int thumbnailHeight
        {
            get { return _thumbnailHeight; }
            set { _thumbnailHeight = value; }
        }
        private string _framegrabUrl;

        public string framegrabUrl
        {
            get { return _framegrabUrl; }
            set { _framegrabUrl = value; }
        }
        private int _framegrabWidth;

        public int framegrabWidth
        {
            get { return _framegrabWidth; }
            set { _framegrabWidth = value; }
        }
        private int _framegrabHeight;

        public int framegrabHeight
        {
            get { return _framegrabHeight; }
            set { _framegrabHeight = value; }
        }
        private string _html;

        public string html
        {
            get { return _html; }
            set { _html = value; }
        }
        private int _height;

        public int height
        {
            get { return _height; }
            set { _height = value; }
        }
        private int _width;

        public int width
        {
            get { return _width; }
            set { _width = value; }
        }
        private bool _borderless;

        public bool borderless
        {
            get { return _borderless; }
            set { _borderless = value; }
        }
        private double _duration;

        public double duration
        {
            get { return _duration; }
            set { _duration = value; }
        }
        private int _videoStatus;

        public int videoStatus
        {
            get { return _videoStatus; }
            set { _videoStatus = value; }
        }
        private string _videoStatusDescription;

        public string videoStatusDescription
        {
            get { return _videoStatusDescription; }
            set { _videoStatusDescription = value; }
        }

        #endregion

        #region Public and Protected Methods

        /// <summary>
        /// The oEmbed resource type. For vzaar video assets this will always 
        /// be video
        /// </summary>
        /// <returns>The oEmbed resource type</returns>
        [Obsolete("Deprecated")]
        public string _GetType()
        {
            return _type;
        }


        /// <summary>
        /// The provider name. This will always be vzaar.
        /// </summary>
        /// <returns>The provider name</returns>
        [Obsolete("Deprecated")]
        public string GetProviderName()
        {
            return _providerName;
        }


        /// <summary>
        /// The provider URL. This will always be http://vzaar.com.
        /// </summary>
        /// <returns>The provider URL</returns>
        [Obsolete("Deprecated")]
        public string GetProviderUrl()
        {
            return _providerUrl;
        }


        /// <summary>
        /// The width of the thumbnail in pixels. This is usually 120px
        /// </summary>
        /// <returns>The thumbnail width in pixels</returns>
        public int GetThumbnailWidth()
        {
            return _thumbnailWidth;
        }

        /// <summary>
        /// The height of the thumbnail in pixels. This is usually 90px.
        /// </summary>
        /// <returns>The thumbnail height in pixels</returns>
        [Obsolete("Deprecated")]
        public int GetThumbnailHeight()
        {
            return _thumbnailHeight;
        }


        /// <summary>
        /// The URL that points to a frame grab of the video. This is the same as 
        /// the thumbnail normally but a bigger size, and represents the still 
        /// image on sees before clicking play in the video player.
        /// </summary>
        /// <returns>The URL that points to a frame grab of the video</returns>
        [Obsolete("Deprecated")]
        public string GetFramegrabUrl()
        {
            return _framegrabUrl;
        }


        /// <summary>
        /// The width of the frame grab image in pixels. This will be normally be 
        /// the same size as the video, but not necessarily the same size as the 
        /// video player which may be larger. The default size is 320px.
        /// </summary>
        /// <returns>The width of the frame grab image in pixels</returns>
        [Obsolete("Deprecated")]
        public int GetFramegrabWidth()
        {
            return _framegrabWidth;
        }


        /// <summary>
        /// The height of the frame grab image in pixels. This will be normally be 
        /// the same size as the video, but not necessarily the same size as the 
        /// video player which may be larger. The default size is 240px.
        /// </summary>
        /// <returns>The height of the frame grab image in pixels</returns>
        [Obsolete("Deprecated")]
        public int GetFramegrabHeight()
        {
            return _framegrabHeight;
        }


        /// <summary>
        /// Return the HTML
        /// </summary>
        /// <returns>The HTML</returns>
        [Obsolete("Deprecated")]
        public string GetHtml()
        {
            return _html;
        }


        /// <summary>
        /// This will return the exact HTML you need to use to embed the video 
        /// into a web page. This should work for all standard web pages. The HTML 
        /// will be encoded as follows JSON or XML.
        /// </summary>
        /// <returns>The exact HTML you need to use to embed the video into a web page</returns>
        [Obsolete("Deprecated")]
        public string GetAuthorName()
        {
            return _authorName;
        }

        /// <summary>
        /// The link to the vzaar user summary page
        /// </summary>
        /// <returns>The link to the vzaar user summary page</returns>
        [Obsolete("Deprecated")]
        public string GetAuthorUrl()
        {
            return _authorUrl;
        }

        /// <summary>
        /// The number representing the users vzaar account. 1 represents a free account.
        /// </summary>
        /// <returns>The number representing the users vzaar account</returns>
        [Obsolete("Deprecated")]
        public int GetAuthorAccount()
        {
            return _authorAccount;
        }


        /// <summary>
        /// The vzaar API version number.
        /// </summary>
        /// <returns>The vzaar API version number</returns>
        [Obsolete("Deprecated")]
        public double GetVersion()
        {
            return _version;
        }


        /// <summary>
        /// The video title. It may be null.
        /// </summary>
        /// <returns>The video title</returns>
        [Obsolete("Deprecated")]
        public string GetTitle()
        {
            return _title;
        }

        /// <summary>
        /// The video description. It may be null.
        /// </summary>
        /// <returns>The video description</returns>
        [Obsolete("Deprecated")]
        public string GetDescription()
        {
            return _description;
        }

        /// <summary>
        /// The URL link that points to the video thumbnail. This is usually 
        /// 120x90px.
        /// </summary>
        /// <returns>The URL link that points to the video thumbnail</returns>
        [Obsolete("Deprecated")]
        public string GetThumbnailUrl()
        {
            return _thumbnailUrl;
        }

        /// <summary>
        /// The duration of the video.
        /// </summary>
        /// <returns>The duration of the video</returns>
        [Obsolete("Deprecated")]
        public double GetDuration()
        {
            return _duration;
        }

        /// <summary>
        /// The width of the video.
        /// </summary>
        /// <returns>The width of the video</returns>
        [Obsolete("Deprecated")]
        public int GetWidth()
        {
            return _width;
        }


        /// <summary>
        /// The height of the video.
        /// </summary>
        /// <returns>The height of the video</returns>
        [Obsolete("Deprecated")]
        public int GetHeight()
        {
            return _height;
        }


        /// <summary>
        /// Is the video borderless.
        /// </summary>
        /// <returns>Is the video borderless</returns>
        [Obsolete("Deprecated")]
        public bool IsBorderless()
        {
            return _borderless;
        }
        #endregion
    }
}
