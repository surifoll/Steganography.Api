using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace Steganography.WebApi.Models
{
    public sealed class DataImage
    {
        private static readonly Regex DataUriPattern =
            new Regex(@"^data\:(?<type>image\/(png|tiff|jpg|gif|jpeg));base64,(?<data>[A-Z0-9\+\/\=]+)$",
                RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private DataImage(string mimeType, byte[] rawData)
        {
            MimeType = mimeType;
            RawData = rawData;
        }

        public string MimeType { get; }
        public byte[] RawData { get; }

        public Image Image => Image.FromStream(new MemoryStream(RawData));

        public static DataImage TryParse(string dataUri)
        {
            if (string.IsNullOrWhiteSpace(dataUri)) return null;

            var match = DataUriPattern.Match(dataUri);
            if (!match.Success) return null;

            var mimeType = match.Groups["type"].Value;
            var base64Data = match.Groups["data"].Value;

            try
            {
                var rawData = Convert.FromBase64String(base64Data);
                return rawData.Length == 0 ? null : new DataImage(mimeType, rawData);
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }

    public class DataModel
    {
        public bool IsExtraction { get; set; }
        public string Info { get; set; }
        public string ImageBase64 { get; set; }
    }
}