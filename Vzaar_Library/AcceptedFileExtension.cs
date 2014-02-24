using System.Collections.Generic;
using System.Linq;

namespace VzaarAPI
{
    /// <summary>
    /// This class is responsible for identifying what extension types are regarded as trusted and accepted.
    /// </summary>
    public class AcceptedFileExtension
    {        
        #region Private Static Members

        private static Dictionary<string, AcceptedFileExtension> TRUSTED =
            new Dictionary<string, AcceptedFileExtension>();

        private static Dictionary<string, AcceptedFileExtension> ACCEPTED =
            new Dictionary<string, AcceptedFileExtension>();

        /// <summary>
        /// Constructor to create the list of accepted extension types.
        /// </summary>
        static AcceptedFileExtension()
        {
            TRUSTED.Add(".mp3", new AcceptedFileExtension(".mp3", "MPEG audio layer 3"));
            TRUSTED.Add(".asf", new AcceptedFileExtension(".asf", "asf format"));
            TRUSTED.Add(".avi", new AcceptedFileExtension(".avi", "avi format"));
            TRUSTED.Add(".flv", new AcceptedFileExtension(".flv", "flv video format"));
            TRUSTED.Add(".m4v", new AcceptedFileExtension(".m4v", "raw MPEG4 video format"));
            TRUSTED.Add(".mov", new AcceptedFileExtension(".mov", "QuickTime/MPEG4/Motion JPEG 2000 format"));
            TRUSTED.Add(".mp4", new AcceptedFileExtension(".mp4", "QuickTime/MPEG4/Motion JPEG 2000 format"));
            TRUSTED.Add(".m4a", new AcceptedFileExtension(".m4a", "QuickTime/MPEG4/Motion JPEG 2000 format"));
            TRUSTED.Add(".3gp", new AcceptedFileExtension(".3gp", "QuickTime/MPEG4/Motion JPEG 2000 format"));
            TRUSTED.Add(".3g2", new AcceptedFileExtension(".3g2", "QuickTime/MPEG4/Motion JPEG 2000 format"));
            TRUSTED.Add(".mj2", new AcceptedFileExtension(".mj2", "QuickTime/MPEG4/Motion JPEG 2000 format"));
            TRUSTED.Add(".wmv", new AcceptedFileExtension(".wmv", "Windows Media Video format"));

            ACCEPTED.Add(".asf", new AcceptedFileExtension(".asf", "asf format"));
            ACCEPTED.Add(".avi", new AcceptedFileExtension(".avi", "avi format"));
            ACCEPTED.Add(".flv", new AcceptedFileExtension(".flv", "flv video format"));
            ACCEPTED.Add(".m4v", new AcceptedFileExtension(".m4v", "raw MPEG4 video format"));
            ACCEPTED.Add(".mpg", new AcceptedFileExtension(".mpg", "mpg video format"));
            ACCEPTED.Add(".mov", new AcceptedFileExtension(".mov", "QuickTime/MPEG4/Motion JPEG 2000 format"));
            ACCEPTED.Add(".mp4", new AcceptedFileExtension(".mp4", "QuickTime/MPEG4/Motion JPEG 2000 format"));
            ACCEPTED.Add(".m4a", new AcceptedFileExtension(".m4a", "QuickTime/MPEG4/Motion JPEG 2000 format"));
            ACCEPTED.Add(".3gp", new AcceptedFileExtension(".3gp", "QuickTime/MPEG4/Motion JPEG 2000 format"));
            ACCEPTED.Add(".3g2", new AcceptedFileExtension(".3g2", "QuickTime/MPEG4/Motion JPEG 2000 format"));
            ACCEPTED.Add(".mj2", new AcceptedFileExtension(".mj2", "QuickTime/MPEG4/Motion JPEG 2000 format"));
            ACCEPTED.Add(".wmv", new AcceptedFileExtension(".wmv", "Windows Media Video format"));
            ACCEPTED.Add(".4xm", new AcceptedFileExtension(".4xm", "4X Technologies format"));
            ACCEPTED.Add(".MTV", new AcceptedFileExtension(".MTV", "MTV format"));
            ACCEPTED.Add(".RoQ", new AcceptedFileExtension(".RoQ", "Id RoQ format"));
            ACCEPTED.Add(".aac", new AcceptedFileExtension(".aac", "ADTS AAC"));
            ACCEPTED.Add(".ac3", new AcceptedFileExtension(".ac3", "raw ac3"));
            ACCEPTED.Add(".aiff", new AcceptedFileExtension(".aiff", "Audio IFF"));
            ACCEPTED.Add(".alaw", new AcceptedFileExtension(".alaw", "pcm A law format"));
            ACCEPTED.Add(".amr", new AcceptedFileExtension(".amr", "3gpp amr file format"));
            ACCEPTED.Add(".apc", new AcceptedFileExtension(".apc", "CRYO APC format"));
            ACCEPTED.Add(".ape", new AcceptedFileExtension(".ape", "Monkey's Audio"));
            ACCEPTED.Add(".au", new AcceptedFileExtension(".au", "SUN AU Format"));
            ACCEPTED.Add(".avs", new AcceptedFileExtension(".avs", "avs format"));
            ACCEPTED.Add(".bethsoftvid", new AcceptedFileExtension(".bethsoftvid", "Bethesda Softworks 'Daggerfall' VID format"));
            ACCEPTED.Add(".bktr", new AcceptedFileExtension(".bktr", "video grab"));
            ACCEPTED.Add(".c93", new AcceptedFileExtension(".c93", "Interplay C93"));
            ACCEPTED.Add(".daud", new AcceptedFileExtension(".daud", "D -Cinema audio format"));
            ACCEPTED.Add(".dsicin", new AcceptedFileExtension(".dsicin", "Delphine Software International CIN format"));
            ACCEPTED.Add(".dts", new AcceptedFileExtension(".dts", "raw dts"));
            ACCEPTED.Add(".dv", new AcceptedFileExtension(".dv", "DV video format"));
            ACCEPTED.Add(".dxa", new AcceptedFileExtension(".dxa", "dxa"));
            ACCEPTED.Add(".ea", new AcceptedFileExtension(".ea", "Electronic Arts Multimedia Format"));
            ACCEPTED.Add(".ffm", new AcceptedFileExtension(".ffm", "ffm format"));
            ACCEPTED.Add(".film_cpk", new AcceptedFileExtension(".film_cpk", "Sega FILM/CPK format"));
            ACCEPTED.Add(".flac", new AcceptedFileExtension(".flac", "raw flac"));
            ACCEPTED.Add(".flic", new AcceptedFileExtension(".flic", "FLI/FLC/FLX animation format"));
            ACCEPTED.Add(".gif", new AcceptedFileExtension(".gif", "GIF Animation"));
            ACCEPTED.Add(".gxf", new AcceptedFileExtension(".gxf", "GXF format"));
            ACCEPTED.Add(".h261", new AcceptedFileExtension(".h261", "raw h261"));
            ACCEPTED.Add(".h263", new AcceptedFileExtension(".h263", "raw h263"));
            ACCEPTED.Add(".h264", new AcceptedFileExtension(".h264", "raw H264 video format"));
            ACCEPTED.Add(".idcin", new AcceptedFileExtension(".idcin", "Id CIN format"));
            ACCEPTED.Add(".image2", new AcceptedFileExtension(".image2", "image2 sequence"));
            ACCEPTED.Add(".image2pipe", new AcceptedFileExtension(".image2pipe", "piped image2 sequence"));
            ACCEPTED.Add(".ingenient", new AcceptedFileExtension(".ingenient", "Ingenient MJPEG"));
            ACCEPTED.Add(".ipmovie", new AcceptedFileExtension(".ipmovie", "Interplay MVE format"));
            ACCEPTED.Add(".matroska", new AcceptedFileExtension(".matroska", "Matroska File Format"));
            ACCEPTED.Add(".mjpeg", new AcceptedFileExtension(".mjpeg", "MJPEG video"));
            ACCEPTED.Add(".mm", new AcceptedFileExtension(".mm", "American Laser Games MM format"));
            ACCEPTED.Add(".mmf", new AcceptedFileExtension(".mmf", "mmf format"));
            ACCEPTED.Add(".mp3", new AcceptedFileExtension(".mp3", "MPEG audio layer 3"));
            ACCEPTED.Add(".mpc", new AcceptedFileExtension(".mpc", "musepack"));
            ACCEPTED.Add(".mpeg", new AcceptedFileExtension(".mpeg", "MPEG1 System format"));
            ACCEPTED.Add(".mpegts", new AcceptedFileExtension(".mpegts", "MPEG2 transport stream format"));
            ACCEPTED.Add(".mpegtsraw", new AcceptedFileExtension(".mpegtsraw", "MPEG2 raw transport stream format"));
            ACCEPTED.Add(".mpegvideo", new AcceptedFileExtension(".mpegvideo", "MPEG video"));
            ACCEPTED.Add(".mulaw", new AcceptedFileExtension(".mulaw", "pcm mu law format"));
            ACCEPTED.Add(".mxf", new AcceptedFileExtension(".mxf", "MXF format"));
            ACCEPTED.Add(".nsv", new AcceptedFileExtension(".nsv", "NullSoft Video format"));
            ACCEPTED.Add(".nut", new AcceptedFileExtension(".nut", "nut format"));
            ACCEPTED.Add(".nuv", new AcceptedFileExtension(".nuv", "NuppelVideo format"));
            ACCEPTED.Add(".ogg", new AcceptedFileExtension(".ogg", "Ogg format"));
            ACCEPTED.Add(".oss", new AcceptedFileExtension(".oss", "audio grab and output"));
            ACCEPTED.Add(".psxstr", new AcceptedFileExtension(".psxstr", "Sony Playstation STR format"));
            ACCEPTED.Add(".rawvideo", new AcceptedFileExtension(".rawvideo", "raw video format"));
            ACCEPTED.Add(".redir", new AcceptedFileExtension(".redir", "Redirector format"));
            ACCEPTED.Add(".rm", new AcceptedFileExtension(".rm", "rm format"));
            ACCEPTED.Add(".rtsp", new AcceptedFileExtension(".rtsp", "RTSP input format"));
            ACCEPTED.Add(".s16be", new AcceptedFileExtension(".s16be", "pcm signed 16 bit big endian format"));
            ACCEPTED.Add(".s16le", new AcceptedFileExtension(".s16le", "pcm signed 16 bit little endian format"));
            ACCEPTED.Add(".s8", new AcceptedFileExtension(".s8", "pcm signed 8 bit format"));
            ACCEPTED.Add(".sdp", new AcceptedFileExtension(".sdp", "SDP"));
            ACCEPTED.Add(".shn", new AcceptedFileExtension(".shn", "raw shorten"));
            ACCEPTED.Add(".smk", new AcceptedFileExtension(".smk", "Smacker Video"));
            ACCEPTED.Add(".sol", new AcceptedFileExtension(".sol", "Sierra SOL Format"));
            ACCEPTED.Add(".swf", new AcceptedFileExtension(".swf", "Flash format"));
            ACCEPTED.Add(".thp", new AcceptedFileExtension(".thp", "THP"));
            ACCEPTED.Add(".tiertexseq", new AcceptedFileExtension(".tiertexseq", "Tiertex Limited SEQ format"));
            ACCEPTED.Add(".tta", new AcceptedFileExtension(".tta", "true -audio"));
            ACCEPTED.Add(".txd", new AcceptedFileExtension(".txd", "txd format"));
            ACCEPTED.Add(".u16be", new AcceptedFileExtension(".u16be", "pcm unsigned 16 bit big endian format"));
            ACCEPTED.Add(".u16le", new AcceptedFileExtension(".u16le", "pcm unsigned 16 bit little endian format"));
            ACCEPTED.Add(".u8", new AcceptedFileExtension(".u8", "pcm unsigned 8 bit format"));
            ACCEPTED.Add(".vc1", new AcceptedFileExtension(".vc1", "raw vc1"));
            ACCEPTED.Add(".vmd", new AcceptedFileExtension(".vmd", "Sierra VMD format"));
            ACCEPTED.Add(".voc", new AcceptedFileExtension(".voc", "Creative Voice File format"));
            ACCEPTED.Add(".wav", new AcceptedFileExtension(".wav", "wav format"));
            ACCEPTED.Add(".wc3movie", new AcceptedFileExtension(".wc3movie", "Wing Commander III movie format"));
            ACCEPTED.Add(".wsaud", new AcceptedFileExtension(".wsaud", "Westwood Studios audio format"));
            ACCEPTED.Add(".wsvqa", new AcceptedFileExtension(".wsvqa", "Westwood Studios VQA format"));
            ACCEPTED.Add(".wv", new AcceptedFileExtension(".wv", "WavPack"));
            ACCEPTED.Add(".yuv4mpegpipe", new AcceptedFileExtension(".yuv4mpegpipe", "YUV4MPEG pipe format"));
        }

        #endregion

        #region Public Static Methods 


        /// <summary>
        /// Provided with a file name or just the extension this method will 
        /// return true if the file format is a trusted format to be
        /// decoded by <a href="http://vzaar.com">vzaar.com</a>. 
        /// </summary>
        /// <param name="fileNameOrExtension">The full filename or just the extension</param>
        /// <returns>Returns true if a trusted and tested file format, false otherwise</returns>
        public static bool IsTrustedFormat(string fileNameOrExtension)
        {
            return TRUSTED.ContainsKey(NormaliseFileExtension(fileNameOrExtension));
        }


        /// <summary>
        /// Provided with a file name or just the extension this method will 
        /// return true if the file format is an accepted format. This includes
        /// trusted as well as untrusted formats. Untrusted formats are not
        /// well tested and are at your own risk. 
        /// </summary>
        /// <param name="fileNameOrExtension">The full filename or just the extension</param>
        /// <returns>True if a accepted file format, false otherwise</returns>
        public static bool IsAcceptedFormat(string fileNameOrExtension)
        {
            return ACCEPTED.ContainsKey(NormaliseFileExtension(fileNameOrExtension));
        }

        /// <summary>
        /// Returns a list of the trusted file formats that have been tested
        /// for decoding by <a href="http://vzaar.com">vzaar.com</a>.
        /// </summary>
        /// <returns>The list of trusted file extensions for formats.</returns>
        public static List<AcceptedFileExtension> GetTrustedFormats()
        {
	        return TRUSTED.Keys.Select(key => TRUSTED[key]).ToList();
        }

	    /// <summary>
        /// Returns a list of the accepted formats for decoding by 
        /// <a href="http://vzaar.com">vzaar.com</a>. Accepted formats include
        /// untested formats and are uploaded at your own risk. 
        /// </summary>
        /// <returns>The list of accepted file extensions for formats. </returns>
        public static List<AcceptedFileExtension> GetAcceptedFormats()
	    {
		    return ACCEPTED.Keys.Select(key => ACCEPTED[key]).ToList();
	    }

	    #endregion
        
        #region Private Static Methods 
       

        /// <summary>
        /// Normalise a filename or extension to enable lookup an entry.
        /// </summary>
        /// <param name="fileNameOrExtension">The full filename or just the extension</param>
        /// <returns>A string of the file extension</returns>
        private static string NormaliseFileExtension(string fileNameOrExtension)
        {
            return System.IO.Path.GetExtension(fileNameOrExtension).ToLower();
        }

        #endregion

        #region Private Members         

        private string _extension;
        private string _description;

        #endregion
        
        #region Private Methods 
  

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="extension">The extension as just the characters in lower case</param>
        /// <param name="description">The description of the file format</param>
        private AcceptedFileExtension(string extension, string description)
        {
            _extension = extension;
            _description = description;
        }

        #endregion
        
        #region Public Methods 

   
        /// <summary>
        /// Retrieve the file extension
        /// </summary>
        /// <returns>The file extension</returns>
        public string GetExtension()
        {
            return _extension;
        } 

        /// <summary>
        /// The file extension description.
        /// </summary>
        /// <returns>The description</returns>
        public string GetDescription()
        {
            return _description;
        }

        #endregion
    }
}
