using System;

namespace VzaarAPI
{
    /// <summary>
    /// This class is responsible of retrieving a list of videos.
    /// </summary>
    public class VideoListRequest : VzaarRequest
    {        
        #region Request Parameter Names         

        protected static string COUNT = "count";
        protected static string PAGE = "page";
        protected static string LIST_ONLY = "list_only";
        protected static string SORT = "sort";
        protected static string SIZE = "size";
        protected static string TITLE = "title";
		protected static string STATUS = "status";

        #endregion
        
        #region " Properties "         

	    public string username { get; set; }

	    #endregion
        
        #region Public Methods 

        /// <summary>
        /// Construct a video list request for the given username. This object 
        /// can be reused for different users by calling setUsername(). It is
        /// not thread safe obviously.
        /// </summary>
        /// <param name="username">The vzaar login name for the user</param>
        public VideoListRequest(String username)
        {
            this.username = username;
        }
        
        /// <summary>
        /// Specifies the number of videos to retrieve per page. Default is 20. 
        /// Maximum is 100.
        /// </summary>
        /// <returns>Number of videos to retrieve per page</returns>
		public int count
		{
			get { return (GetInteger(COUNT) != -1) ? GetInteger(COUNT) : 0; }
			set { PutParameter(COUNT, value); }
		}

        /// <summary>
        /// Specifies the page number to retrieve. Default is 1.
        /// </summary>
        /// <returns>Page number to retrieve</returns>
		public int? page
		{
			get { return GetInteger(PAGE); }
			set { PutParameter(PAGE, value); }
		}

        /// <summary>
        /// When returning data, only include the minimum fields (those marked 
        /// as 'always')
        /// </summary>
        /// <returns>Only include the minimum fields</returns>
		public bool? listOnly
		{
			get { return GetBoolean(LIST_ONLY); }
			set {  PutParameter(LIST_ONLY, value); }
		}


        /// <summary>
        /// Should sort ascending (least recent) or descending (most recent).
        /// </summary>
        /// <returns>Should sort ascending</returns>
		public bool sortAscending
		{
			get { return "asc".Equals(GetBoolean(SORT)); }
			set { PutParameter(SORT, value ? "asc" : "desc"); }
		}


        /// <summary>
        /// Include (or not) size (width and height) of a video. Defaults to false.
        /// </summary>
        /// <returns>The size of the video be returned</returns>
		public bool? includeSize
		{
			get { return GetBoolean(SIZE); }
			set {  PutParameter(SIZE,value); }
		}

        /// <summary>
        /// Return only videos with title containing given string.
        /// </summary>
        /// <returns>Title search string</returns>

		public string title
		{
			get { return GetParameter(TITLE); }
			set { PutParameter(TITLE, value); }
		}

		public string status
		{
			get { return GetParameter(STATUS); }
			set { PutParameter(STATUS, value); }
		}
        
        #endregion
    }
}
