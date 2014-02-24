using System;

namespace VzaarAPI
{
    /// <summary>
    /// This class is responsible for making requests for video objects.
    /// </summary>
    public class VideoDetailsRequest : VzaarRequest
    {        
        #region Public Static Methods     

        /// <summary>
        /// Create a request object configured to request the full details of the object.
        /// </summary>
        /// <param name="videoId">The video number for the video</param>
        /// <returns>Video detail request for full details</returns>
        public static VideoDetailsRequest FullDetails(int videoId)
        {
            VideoDetailsRequest request = new VideoDetailsRequest(videoId);
            request.SetDescription(true);
            request.SetDuration(true);
            request.SetEmbedOnly(false);
            request.SetFramegrab(true);
            request.SetThumbnail(true);

            return request;
        }


        /// <summary>
        /// Create a request object configured to request the minimal details for embedding.
        /// </summary>
        /// <param name="videoId">The video number for the video</param>
        /// <returns>Video detail request for the minimal details</returns>
        public static VideoDetailsRequest EmbedOnly(int videoId)
        {
            VideoDetailsRequest request = new VideoDetailsRequest(videoId);
            
            request.SetEmbedOnly(true);
            
            return request;
        }

        #endregion
        
        #region Request Parameter Names         

        protected static string MAX_WIDTH = "maxwidth";
        protected static string MAX_HEIGHT = "maxheight";
        protected static string BORDERLESS = "borderless";
        protected static string THUMBNAIL = "thumbnail";
        protected static string FRAMEGRAB = "framegrab";
        protected static string EMBED_ONLY = "embed_only";
        protected static string DURATION = "duration";
        protected static string DESCRIPTION = "description";

        #endregion
        
        #region Private Members

        private int videoId;

        #endregion
        
        #region Public Methods         

        /// <summary>
        /// Construct a video details request for the given video id. This object 
        /// can be reused for different videos by calling setVideoId(). It is
        /// not thread safe obviously.
        /// </summary>
        /// <param name="videoId">The video number for the video</param>
        public VideoDetailsRequest(int videoId)
        {
            this.videoId = videoId;
        }

        /// <summary>
        /// The video number for the video.
        /// </summary>
        /// <returns>The video number for the video</returns>
        public int GetVideoId()
        {
            return videoId;
        }

        /// <summary>
        /// The video number for the video.
        /// </summary>
        /// <param name="videoId">The video number for the video</param>
        public void SetVideoId(int videoId)
        {
            this.videoId = videoId;
        }


        /// <summary>
        /// The maximum width of the video. Returns the closest smaller size 
        /// available when if not possible. Defaults to original size.
        /// </summary>
        /// <returns>The maximum width of the video</returns>
        public int? GetMaxWidth()
        {
            return GetInteger(MAX_WIDTH);
        }

        /// <summary>
        /// The maximum width of the video. Returns the closest smaller size 
        /// available when if not possible. Defaults to original size.
        /// </summary>
        /// <param name="maxWidth">The maximum width of the video</param>
        public void SetMaxWidth(int maxWidth)
        {
            PutParameter(MAX_WIDTH, maxWidth);
        }


        /// <summary>
        /// The maximum height of the video. Returns the closest smaller size 
        /// available when if not possible. Defaults to original size.
        /// </summary>
        /// <returns>The maximum height of the video</returns>
        public int? GetMaxHeight()
        {
            return GetInteger(MAX_HEIGHT);
        }

        /// <summary>
        /// The maximum height of the video. Returns the closest smaller size 
        /// available when if not possible. Defaults to original size.
        /// </summary>
        /// <param name="maxHeight">The maximum height of the video</param>
        public void setMaxHeight(int maxHeight)
        {
            PutParameter(MAX_HEIGHT, maxHeight);
        }


        /// <summary>
        /// If set to true and the user has sufficient privileges, the size and 
        /// embedded code returned will be be for a borderless player. Else ignored.
        /// </summary>
        /// <returns>Should return borderless player embed code</returns>
        public bool? GetBorderless()
        {
            return GetBoolean(BORDERLESS);
        }

        /// <summary>
        /// If set to true and the user has sufficient privileges, the size and 
        /// embedded code returned will be be for a borderless player. Else ignored.
        /// </summary>
        /// <param name="borderless">Should return borderless player embed code</param>
        public void SetBorderless(Boolean borderless)
        {
            PutParameter(BORDERLESS, borderless);
        }


        /// <summary>
        /// When returning data include information about the video thumbnail.
        /// </summary>
        /// <returns>Should return video thumbnail information</returns>
        public bool? GetThumbnail()
        {
            return GetBoolean(THUMBNAIL);
        }


        /// <summary>
        /// When returning data include information about the video thumbnail.
        /// </summary>
        /// <param name="thumbnail">Should return video thumbnail information</param>
        public void SetThumbnail(bool thumbnail)
        {
            PutParameter(THUMBNAIL, thumbnail);
        }

        /// <summary>
        /// When returning data include information about the video frame grab.
        /// </summary>
        /// <returns>Should return video frame grab information</returns>
        public bool? GetFramegrab()
        {
            return GetBoolean(FRAMEGRAB);
        }

        /// <summary>
        /// When returning data include information about the video frame grab.
        /// </summary>
        /// <param name="framegrab">Should return video frame grab information</param>
        public void SetFramegrab(bool framegrab)
        {
            PutParameter(FRAMEGRAB, framegrab);
        }


        /// <summary>
        /// When returning data, only include the minimum fields and embed code 
        /// possible. Use this if you want the quickest and smallest return code for 
        /// embedding in it.
        /// </summary>
        /// <returns>Return minum fields and embed code</returns>
        public bool? GetEmbedOnly()
        {
            return GetBoolean(EMBED_ONLY);
        }


        /// <summary>
        /// When returning data, only include the minimum fields and embed code 
        /// possible. Use this if you want the quickest and smallest return code for 
        /// embedding in it.
        /// </summary>
        /// <param name="embedOnly">Should return minum fields and embed code</param>
        public void SetEmbedOnly(Boolean embedOnly)
        {
            PutParameter(EMBED_ONLY, embedOnly);
        }


        /// <summary>
        /// Include (or not) duration of a video. Defaults to false.
        /// </summary>
        /// <returns>Include duration of video</returns>
        public bool? GetDuration()
        {
            return GetBoolean(DURATION);
        }


        /// <summary>
        /// Include (or not) duration of a video. Defaults to false.
        /// </summary>
        /// <param name="duration">Should include duration of video</param>
        public void SetDuration(bool duration)
        {
            PutParameter(DURATION, duration);
        }


        /// <summary>
        /// Include (or not) description of a video. Defaults to false.
        /// </summary>
        /// <returns>Include description of video</returns>
        public bool? GetDescription()
        {
            return GetBoolean(DESCRIPTION);
        }


        /// <summary>
        /// Include (or not) description of a video. Defaults to false.
        /// </summary>
        /// <param name="description">Should include description of video</param>
        public void SetDescription(bool description)
        {
            PutParameter(DESCRIPTION, description);
        }

        #endregion
    }
}
