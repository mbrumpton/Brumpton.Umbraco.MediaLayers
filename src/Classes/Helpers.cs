using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using umbraco;
using umbraco.NodeFactory;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace MediaLayers.Umbraco.Classes
{
    public class Helpers
    {
        #region public methods

        public static List<Node> FindChildren(Node currentNode, Func<Node, bool> predicate)
        {
            var result = new List<Node>();

            var nodes = currentNode
                .Children
                .OfType<Node>()
                .Where(predicate).ToList();

            if (nodes.Count() != 0)
                result.AddRange(nodes);

            foreach (var child in currentNode.Children.OfType<Node>())
            {
                nodes = FindChildren(child, predicate);
                if (nodes.Count() != 0)
                    result.AddRange(nodes);
            }
            return result;
        }

        public static List<Node> FindParent(Node currentNode, Func<Node, bool> predicate)
        {
            return currentNode.GetAncestorOrSelfNodes().Where(predicate).ToList();
        }

        public static string GetMediaItemsFromCsvId(string csvMediaIds)
        {
            UmbracoHelper uHelper = new UmbracoHelper(UmbracoContext.Current);
            string mediaUrl = "";
  
                var mediaItem = uHelper.Media(csvMediaIds);
                mediaUrl = mediaItem.umbracoFile;
  
            return mediaUrl;
        }


        /// <summary>
        /// ReplaceSmartQuotesEtcInString method - usage as name implies.
        /// </summary>
        /// <param name="stringToConvert">The string to replace smart quotes etc in.</param>
        /// <returns>
        /// The converted string.
        /// </returns>
        public string ReplaceSmartQuotesEtcInString(string stringToConvert)
        {

            var convertedString = stringToConvert;

            // Replace string â€˜ (left smart quote) with apostrophe
            convertedString = convertedString.Replace("â€˜", "'");
            // Replace string â€™ (right smart quote) with apostrophe
            convertedString = convertedString.Replace("â€™", "'");

            // Replace string â€œ (left double quote) with quote
            convertedString = convertedString.Replace("â€œ", "\"");
            // Replace string â€ (right double quote) with quote
            convertedString = convertedString.Replace("â€	", "\"");

            // Replace string â€“ (emdash) with hyphen
            convertedString = convertedString.Replace("â€“", "-");
            // Replace string â€” (emdash) with hyphen
            convertedString = convertedString.Replace("â€”", "-");

            // Replace string â€‹ (whatever this is supposed to be) with nothing
            convertedString = convertedString.Replace("â€‹", "");

            return convertedString;
        }

        public static string GetImageUrl(int imageId)
        {
            IMediaService mediaService = ApplicationContext.Current.Services.MediaService;
            var mediaItem = mediaService.GetById(imageId);
            var serializer = new JavaScriptSerializer();
            dynamic jsonObject = serializer.Deserialize<ImagePathModel>(mediaItem.GetValue<string>("umbracoFile"));

            return jsonObject.src;
        }

        #endregion public methods

    }

    public class ImagePathModel
    {
        public string src { get; set; }
    }
}