using System.Collections.Generic;
using KTouch.Properties;
namespace KTouch.Utilities {
    public static class SupportedExtensions {

        /// <summary>
        /// Xps file.
        /// </summary>
        public const string XPS = ".xps";

        /// <summary>
        /// Wmv file.
        /// </summary>
        public const string WMV = ".wmv";

        /// <summary>
        /// Mp4 file.
        /// </summary>
        public const string MP4 = ".mp4";

        /// <summary>
        /// Mpg file.
        /// </summary>
        public const string MPG = ".mpg";

        /// <summary>
        /// Avi file.
        /// </summary>
        public const string AVI = ".avi";

        /// <summary>
        /// Jpg file.
        /// </summary>
        public const string JPG = ".jpg";

        /// <summary>
        /// Png file.
        /// </summary>
        public const string PNG = ".png";

        /// <summary>
        /// Gif file.
        /// </summary>
        public const string GIF = ".gif";

        /// <summary>
        /// Shortcut for directory.
        /// </summary>
        public const string DIR = "dir";

        private static List<string> _supportedExtensionList;
        private static List<string> _supportedDocumentExtensionList;
        private static List<string> _supportedMediaExtensionList;
        private static List<string> _supportedThumbnailExtensionList;

        /// <summary>
        /// Returns the list of supported file extensions.
        /// </summary>
        public static ICollection<string> SupportedExtensionList {
            get {
                if (_supportedExtensionList == null) {
                    _supportedExtensionList = new List<string>();
                    _supportedExtensionList.AddRange(SupportedDocumentExtensionList);
                    _supportedExtensionList.AddRange(SupportedMediaExtensionList);
                }
                return _supportedExtensionList;
            }
        }

        /// <summary>
        /// Returns the list of supported document file extensions.
        /// </summary>
        public static ICollection<string> SupportedDocumentExtensionList {
            get {
                if (_supportedDocumentExtensionList == null) {
                    _supportedDocumentExtensionList = new List<string> {
                        Extensions.XPS,                
                    };
                }
                return _supportedDocumentExtensionList;
            }
        }

        /// <summary>
        /// Returns the list of supported media file extensions.
        /// </summary>
        public static ICollection<string> SupportedMediaExtensionList {
            get {
                if (_supportedMediaExtensionList == null) {
                    _supportedMediaExtensionList = new List<string>{
                        Extensions.AVI,
                        Extensions.MP4,
                        Extensions.MPG,
                        Extensions.WMV,
                    };
                }
                return _supportedMediaExtensionList;
            }
        }

        /// <summary>
        /// Returns the list of output thumbnail image extensions.
        /// </summary>
        public static ICollection<string> SupportedThumbnailExtensionList {
            get {
                if (_supportedThumbnailExtensionList == null) {
                    _supportedThumbnailExtensionList = new List<string> {
                        Extensions.GIF,
                        Extensions.JPG,
                        Extensions.PNG,
                    };
                }
                return _supportedThumbnailExtensionList;
            }
        }
    }
}
