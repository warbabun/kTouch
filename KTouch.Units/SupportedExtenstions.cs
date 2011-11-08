using System.Collections.Generic;
namespace KTouch.Units {
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

        private List<string> _supportedExtensionList;
        private List<string> _supportedDocumentExtensionList;
        private List<string> _supportedMediaExtensionList;
        private List<string> _supportedThumbnailExtensionList;

        /// <summary>
        /// Returns the list of supported file extensions.
        /// </summary>
        private ICollection<string> SupportedExtensionList {
            get {
                if(_supportedExtensionList == null) {
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
        private ICollection<string> SupportedDocumentExtensionList {
            get {
                if(_supportedDocumentExtensionList == null) {
                    _supportedDocumentExtensionList = new List<string> {
                        SupportedExtensions.XPS,                
                    };
                }
                return _supportedDocumentExtensionList;
            }
        }

        /// <summary>
        /// Returns the list of supported media file extensions.
        /// </summary>
        private ICollection<string> SupportedMediaExtensionList {
            get {
                if(_supportedMediaExtensionList == null) {
                    _supportedMediaExtensionList = new List<string>{
                        SupportedExtensions.AVI,
                        SupportedExtensions.MP4,
                        SupportedExtensions.MPG,
                        SupportedExtensions.WMV,
                    };
                }
                return _supportedMediaExtensionList;
            }
        }

        /// <summary>
        /// Returns the list of output thumbnail image extensions.
        /// </summary>
        private ICollection<string> SupportedThumbnailExtensionList {
            get {
                if(_supportedThumbnailExtensionList == null) {
                    _supportedThumbnailExtensionList = new List<string> {
                        SupportedExtensions.GIF,
                        SupportedExtensions.JPG,
                        SupportedExtensions.PNG,
                    };
                }
                return _supportedThumbnailExtensionList;
            }
        }
    }
}
