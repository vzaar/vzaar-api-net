using System;
using System.Xml;
using System.IO;

namespace VzaarAPI
{
    /// <summary>
    /// An Xml Helper class to safely parse and extract node values
    /// </summary>
    public class XmlHelper
    {
	    /// <summary>
        /// Parses an XML file and returns a DOM document.
        /// </summary>
        /// <param name="response">The input stream of the XML file (typically from a HttpWebResponse object)</param>
        /// <returns>An XmlDocument</returns>
        public static XmlDocument ParseXml(Stream response)
        {
            var document = new XmlDocument();

            try
            {
                document.Load(response);
            }
            catch (Exception ex)
            {
                throw new VzaarException(ex.Message, ex);
            }

            return document;
        }


        /// <summary>
        /// Extract the value of the first child tag of the supplied node and
        /// parse as an integer.
        /// </summary>
        /// <param name="document">The parent XmlDocument node</param>
        /// <param name="tag">The node name to be returned</param>
        /// <returns>The value as an integer or 0 if not found.</returns>
        public static int GetInteger(XmlNode document, string tag)
        {
            var value = GetValue(document, tag);

            if (value != null)
            {
                try
                {
                    return Int32.Parse(value);
                }
                catch (InvalidCastException) { }
            }
            return 0;
        }


        /// <summary>
        /// Extract the value of the first child tag of the supplied node and
        /// parse as an long.
        /// </summary>
        /// <param name="document">The parent XmlDocument node</param>
        /// <param name="tag">The node name to be returned</param>
        /// <returns>The value as an integer or 0 if not found.</returns>
        public static long GetLong(XmlNode document, string tag)
        {
            var value = GetValue(document, tag);

            if (value != null)
            {
                try
                {
                    return Int64.Parse(value);
                }
                catch (InvalidCastException) { }
            }
            return 0;
        }


        /// <summary>
        /// Extract the value of the first child tag of the supplied node and
        /// parse as an double.
        /// </summary>
        /// <param name="document">The parent XmlDocument node</param>
        /// <param name="tag">The node name to be returned</param>
        /// <returns>The value as an double or 0 if not found.</returns>
        public static double GetDouble(XmlNode document, string tag)
        {
            var value = GetValue(document, tag);

            if (!String.IsNullOrEmpty(value))
            {
                try
                {
                    return Double.Parse(value);
                }
                catch (InvalidCastException) { }
            }
            return 0;
        }


        /// <summary>
        /// Extract the value of the first child tag of the supplied node and
        /// parse as an boolean.
        /// </summary>
        /// <param name="document">The parent XmlDocument node</param>
        /// <param name="tag">The node name to be returned</param>
        /// <returns>The value as an boolean or false if not found.</returns>
        public static bool GetBoolean(XmlNode document, string tag)
        {
            var value = GetValue(document, tag);

            if (value != null)
            {
                try
                {
                    return Boolean.Parse(value);
                }
                catch (Exception)
                { }
            }
            return false;
        }


        /// <summary>
        /// Extract the value of the first child tag of the supplied.
        /// </summary>
        /// <param name="document">The parent XmlDocument node</param>
        /// <param name="tag">The node name to be returned</param>
        /// <returns>The value or null if not found.</returns>
        public static string GetValue(XmlNode document, string tag)
        {
            string value = null;

            try
            {
                var node = document.SelectSingleNode(tag + " | */" + tag + "| */*/" + tag);
                if (node != null)
                {
                    value = node.InnerText;
                }
            }
            catch (Exception ex)
            {
                throw new VzaarException(ex.Message, ex);
            }

            return value;
        }

        /// <summary>
        /// Get a list of child nodes with the given tag.
        /// </summary>
        /// <param name="document">The parent XmlDocument node</param>
        /// <param name="tag">The tag of the type of child nodes to return</param>
        /// <returns>An XmlNodeList of the child nodes that match the tag</returns>
        public static XmlNodeList GetNodes(XmlDocument document, string tag)
        {
            try
            {
                return document.SelectNodes("//" + tag);
            }
            catch (Exception ex)
            {
                throw new VzaarException(ex.Message, ex);
            }
        }


        // Process request XML for use with formatter.
        // Takes four parameters - the GUI, the title, the description and
        // the profile type. 
        public const string PROCESS_VIDEO =
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
            "<vzaar-api>\n" +
            "   <video>\n" +
            "      <guid>{0}</guid>\n" +
            "      <title>{1}</title>\n" +
            "      <description>{2}</description>\n" +
            "      <profile>{3}</profile>\n" +
            "   </video>\n" +
            "</vzaar-api>\n";


        // Delete request XML.	    
        public const string DELETE_VIDEO ="<?xml version=\"1.0\" encoding=\"UTF-8\"?><vzaar-api><_method>delete</_method></vzaar-api>";


        // Change video request XML.
        // Takes two parameters -the title and the description. 	    
        public const string EDIT_VIDEO =
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
            "<vzaar_api>\n" +
            "   <_method>put</_method>\n" +
            "   <video>\n" +
            "      <title>{0}</title>\n" +
            "      <description>{1}</description>\n" +
            "   </video>\n" +
            "</vzaar_api>\n";

    }
}
