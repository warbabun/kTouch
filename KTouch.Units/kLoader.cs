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

        public delegate void MikeEventHandler(kItem obj);
        public static event MikeEventHandler ItemReceived;

        public static event EventHandler DocumentLoaded;
        public static event EventHandler ItemCollectionCreated;
        private Thread _loadingThread = null;
        private ObservableCollection<kItem> _itemList = new ObservableCollection<kItem>();

        private Dispatcher CurrentDispatcher {
            get {
                return Application.Current.Dispatcher;
            }
        }

        /// <summary>
        /// Path to the directory with the content.
        /// All the sake of thumbnails files have to be placed in the separate directories.
        /// </summary>
        /// <param name="path">Full path of the content directory.</param>
        public kLoader(string path) {
            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(LoadItems);
            _loadingThread = new Thread(threadStart);
            _loadingThread.Start(path);
        }

        /// <summary>
        /// Callback function to receive kItems.
        /// </summary>
        /// <param name="path">Full path to the content directory.</param>
        public void LoadItems(object path) {
            XDocument document = this.LoadXDocument(path);
            if (CurrentDispatcher != null) {
                DispatcherOperation dispatcherOp = CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ParseDeclarationFile), document);
                if (dispatcherOp != null) {
                    dispatcherOp.Completed += ItemCollectionCreated;
                }
            }
        }

        /// <summary>
        /// Parses the previously generated document in order to create a collection of kItems.
        /// </summary>
        /// <param name="document">XDocument to parse.</param>
        /// <returns>Null.</returns>
        public object ParseDeclarationFile(object document) {
            XElement root = ((XDocument)document).Root;
            XNamespace ns = root.Name.NamespaceName;
            string rootDirectory = ns.ToString();
            ICollection<XElement> elements = root.Elements(ns + "kItem").Where(e => SupportedExtensions.XPS.Equals((string)e.Attribute("Type"))).ToList();
            foreach (XElement element in elements) {
                kItem item = new kItem {
                    Id = (int)element.Attribute("Id"),
                    Directory = (string)element.Attribute("Directory"),
                    File = (string)element.Attribute("File"),
                    Type = (string)element.Attribute("Type"),
                    CoverFile = (string)element.Attribute("Thumbnail"),
                    Description = (string)element.Element(ns + "Description"),
                    Title = (string)element.Attribute("Title"),
                };
                _itemList.Add(item);
                if (ItemReceived != null) {
                    ItemReceived(item);
                }
            }
            if (ItemCollectionCreated != null) {
                ItemCollectionCreated(this, new EventArgs());
            }
            return null;
        }

        /// <summary>
        /// Creates and XDocument (XML) declaration file from the content of a directory.
        /// </summary>
        /// <param name="path">Full path of the content directory.</param>
        /// <returns>Brand new XDocument with file references.</returns>
        public XDocument LoadXDocument(object path) {
            string directory = (string)path;
            Dictionary<string, string> dicoFileThumbnail = new Dictionary<string, string>();
            List<string> fileList = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories)
                    .Where(f => SupportedExtensions.SupportedExtensionList.Contains(Path.GetExtension(f)))
                    .ToList();
            foreach (string file in fileList) {
                string thumbnail = string.Empty;
                string parentDirectory = Directory.GetParent(file).FullName;
                ICollection<string> existingThumbs = Directory.GetFiles(parentDirectory, "*.*")
                    .Where(f => SupportedExtensions.SupportedThumbnailExtensionList.Contains(Path.GetExtension(f)))
                    .ToList();
                if (existingThumbs.Any()) {
                    thumbnail = existingThumbs.FirstOrDefault();
                } else {
                    thumbnail = ThumbnailCreator.CreateThumbnail(file);
                }
                dicoFileThumbnail.Add(file, thumbnail);
            }
            int iterator = 0;
            XNamespace dir = directory;
            XDocument document =
                new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("kTouch content declarations file."),
                    new XElement(dir + "kContent",
                //new XAttribute(XNamespace.Xmlns + "dir", dir),
                        from f in fileList
                        select new XElement(dir + "kItem",
                                    new XAttribute("Id", (++iterator).ToString()),
                                    new XAttribute("Directory", Path.GetDirectoryName(f)),
                                    new XAttribute("File", f),
                                    new XAttribute("Type", Path.GetExtension(f)),
                                    new XAttribute("Thumbnail", dicoFileThumbnail[f]),
                                    new XAttribute("Title", Path.GetFileNameWithoutExtension(f)),
                                    new XElement(dir + "Description", "Add me!")
                        )
                    )
            );
            if (DocumentLoaded != null) {
                DocumentLoaded(this, new EventArgs());
            }
            return document;
        }
    }
}
