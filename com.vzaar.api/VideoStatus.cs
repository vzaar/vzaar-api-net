
namespace com.vzaar.api
{
	public enum VideoStatus
	{
		PROCESSING = 1, //Processing not complete
		AVAILABLE = 2, //Available (processing complete, video ready)
		EXPIRED = 3, //Expired
		ON_HOLD = 4, //On Hold (waiting for encoding to be available)
		FAILED = 5, //Encoding Failed
		ENCODING_UNAVAILABLE = 6, //Encoding Unavailable
		NOT_AVAILABLE = 7, //n/a
		REPLACED = 8, //Replaced
		DELETED = 9 //Deleted
	}
}
