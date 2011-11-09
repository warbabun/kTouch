using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;

namespace KTouch.Units {

    /// <summary>
    /// Classe dédiée au chargement des éléments .
    /// </summary>
    public class kLoader {

        public EventHandler LoadCompleted;
        private Thread _loadingThread = null;
        private ObservableCollection<kItem> _itemList = new ObservableCollection<kItem>();

        public static XDocument kDocument {
            get;
            private set;
        }

        /// <summary>
        /// Path to the directory with the content.
        /// All the sake of thumbnails files have to be placed in the separate directories.
        /// </summary>
        /// <param name="path">Full path.</param>
        public kLoader(string path) {
            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(LoadItems);
            _loadingThread = new Thread(threadStart);
            _loadingThread.Start(path);
        }

        /// <summary>
        /// Callback function to receive kItems.
        /// </summary>
        /// <param name="data">XDocument reference for the content.</param>
        public void LoadItems(object data) {
            Application app = Application.Current;
            if(app != null) {
                DispatcherOperation dispatcherOp = app.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(Parse), data);
                if(LoadCompleted != null) {
                    dispatcherOp.Completed += new EventHandler(LoadCompleted);
                }
            }
        }

        /// <summary>
        /// Parsing logic of the reference file.
        /// </summary>
        /// <param name="data">XDocument reference for the content.</param>
        /// <returns>Null.</returns>
        private object Parse(object data) {
            kDocument = this.LoadXDocument((string)data);
            return null;
        }

        /*/// <summary>
        /// Bindable collection of kItems.
        /// </summary>
        public IList<kItem> ItemList {
            get { return _itemList; }
        }*/

        /// <summary>
        /// Creates and XDocument (XML) declaration file from the content of a directory.
        /// </summary>
        /// <param name="directory">Full path of the directory.</param>
        /// <returns>XDocument (XML) declaration file.</returns>
        public XDocument LoadXDocument(string directory) {
            Dictionary<string, string> dicoFileThumbnail = new Dictionary<string, string>();
            List<string> fileList = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories)
                    .Where(f => SupportedExtensions.SupportedExtensionList.Contains(Path.GetExtension(f)))
                    .ToList();
            foreach(string file in fileList) {
                string thumbnail = string.Empty;
                string parentDirectory = Directory.GetParent(file).FullName;
                ICollection<string> existingThumbs = Directory.GetFiles(parentDirectory, "*.*")
                    .Where(f => SupportedExtensions.SupportedThumbnailExtensionList.Contains(Path.GetExtension(f)))
                    .ToList();
                if(existingThumbs.Any()) {
                    thumbnail = Path.GetFileName(existingThumbs.FirstOrDefault());
                } else {
                    thumbnail = ThumbnailCreator.CreateThumbnail(file);
                }
                dicoFileThumbnail.Add(file, thumbnail);
            }
            XNamespace dir = new Uri(directory).ToString();
            XDocument kContent =
                new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("kTouch content declarations file."),
                    new XElement(dir + "kContent",
                        new XAttribute(XNamespace.Xmlns + "dir", dir),
                        from f in fileList
                        select new XElement(dir + "kItem",
                                    new XAttribute("FileName", Path.GetFileNameWithoutExtension(f)),
                                    new XAttribute("FileType", Path.GetExtension(f)),
                                    new XAttribute("Thumbnail", dicoFileThumbnail[f])
                        )
                    )
            );
            return kContent;
        }
    }
}
