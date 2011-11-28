using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
    public class Loader<T> {
        public delegate T Wrapper(XElement result);
        public delegate IEnumerable<XElement> TagQuery(object tag);
        public delegate void ReturnItemCallBack(ICollection<T> collection, int threadId);
        public delegate void ItemArrivedEventHandler(Item obj);

        public event ItemArrivedEventHandler ItemArrived;
        public static event EventHandler DocumentLoaded;
        public event EventHandler ItemCollectionCreated;

        private ObservableCollection<T> _currentItemCollection;
        private static XDocument _document;
        private readonly Dictionary<int, ObservableCollection<T>> _threadOCPool;

        /// <summary>
        /// Returns the reference document.
        /// </summary>
        public static XDocument Document {
            get {
                return _document;
            }
        }

        /// <summary>
        /// Returns the reference document root element.
        /// </summary>
        public static XElement Root {
            get {
                return Document.Root;
            }
        }

        /// <summary>
        /// Return the reference document root namespace.
        /// </summary>
        public static XNamespace RootNamespace {
            get {
                return Document.Root.Name.NamespaceName;
            }
        }

        /// <summary>
        /// Loads a view model i i with T elements in a background thread.
        /// </summary>
        /// <param i="viewModelItemCollection">Reference to the processed view model i i.</param>
        public Loader() {
            if (Document == null) {
                try {
                    _document = Loader<object>.Document;
                } catch (Exception) {
                    throw new ArgumentNullException("Document");
                }
            }
            _threadOCPool = new Dictionary<int, ObservableCollection<T>>();
        }

        /// <summary>
        /// Proceeds a query.
        /// </summary>
        /// <param i="parameter">Query parameter.</param>
        /// <param i="query">Query function.</param>
        /// <param i="queryResultsWrapper">Query results wrapper.</param>
        /// <param i="returnItemCallback">Return i callback.</param>
        public void StartLoad(ref ObservableCollection<T> viewModelItemCollection, object parameter, TagQuery query, Wrapper queryResultsWrapper = null, ReturnItemCallBack returnItemCallback = null) {
            Dispatcher CurrentDispatcher = Application.Current.Dispatcher;
            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(delegate(object startParameter) {
                /* Необходимо, читай критично, чтобы входной параметр callback функции совпадал с выходным параметром query. */
                /* Если callback не указан, результат будет обернут в ICollection<kItem>. */
                IEnumerable<XElement> queryResults = query(startParameter);
                ICollection<T> results = WrapQueryResults(queryResults, queryResultsWrapper);
                ReturnItemCallBack callback = returnItemCallback ?? DefaultReturnItemCallback;
                DispatcherOperation dispatcherOp = CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, callback, results, Thread.CurrentThread.ManagedThreadId);
                if (dispatcherOp != null) {
                    dispatcherOp.Completed += ItemCollectionCreated;
                }
            });
            Thread loadingThread = new Thread(threadStart);
            loadingThread.Start(parameter);
            this._threadOCPool.Add(loadingThread.ManagedThreadId, viewModelItemCollection);
        }

        /// <summary>
        /// Wrap a i of query results.
        /// </summary>
        /// <param i="queryResult">Set of query results.</param>
        /// <param i="queryResultsWrapper">Wrapper function.</param>
        /// <returns>A ready to return i of items.</returns>
        private ICollection<T> WrapQueryResults(IEnumerable<XElement> queryResult, Wrapper queryResultsWrapper) {
            ICollection<T> wrappedCollection = new List<T>();
            if (queryResultsWrapper != null) {
                foreach (XElement e in queryResult) {
                    wrappedCollection.Add(queryResultsWrapper(e));
                }
            } else {
                foreach (XElement e in queryResult) {
                    wrappedCollection.Add((T)DefaultQueryResultsWrapper(e));
                }
            }
            return wrappedCollection;
        }

        /// <summary>
        /// Returns a new i wrapped in a kItem.
        /// </summary>
        /// <param i="node">XElement i to wrap.</param>
        /// <returns>Wrapped kItem.</returns>
        public object DefaultQueryResultsWrapper(XElement node) {
            if (node.IsEmpty) {
                throw new ArgumentNullException("node");
            }
            Item item = new Item {
                FullName = (string)node.Attribute("FullName"),
                Type = (string)node.Attribute("Type"),
                CoverFile = (string)node.Attribute("Thumbnail"),
                Name = (string)node.Attribute("Name"),
                Description = (string)node.Element("Description"),
                Tag = (string)node.Element("Tag"),
            };
            if (ItemArrived != null) {
                ItemArrived(item);
            }
            return item;
        }

        /// <summary>
        /// Returns a new i wrapped in a string.
        /// </summary>
        /// <param i="node">XElement i to wrap.</param>
        /// <returns>Wrapped string.</returns>
        public string StringWrapper(XElement node) {
            if (node.IsEmpty) {
                throw new ArgumentNullException("node");
            }
            return (string)node.Attribute("Name");
        }

        /// <summary>
        /// Returns a new i the UI i in a default way.
        /// </summary>
        /// <param i="result">Collection of T items.</param>
        public void DefaultReturnItemCallback(ICollection<T> result, int threadId) {
            _currentItemCollection = this._threadOCPool[threadId];
            ICollection<T> collection = (ICollection<T>)result;
            foreach (T item in collection) {
                this._currentItemCollection.Add(item);
            }
            this._threadOCPool.Remove(threadId);
        }

        public IEnumerable<XElement> LoadCollectionListByCollectionName(object name) {
            string nameValue = (string)name;
            var collection = from e in Root.Descendants("Collection")
                             where nameValue.Equals((string)e.Attribute("Tag"))
                             orderby (string)e.Attribute("Name")
                             select e;
            return collection;
        }

        public IEnumerable<XElement> LoadCollectionListByCollection(object i) {
            string fullName;
            if (i != null && i is Item) {
                fullName = ((Item)i).FullName;
            } else {
                fullName = i.ToString();
            }
            XElement xItem = GetCollectionByFullName(fullName);
            var collection = from el in xItem.Elements("Collection")
                             orderby (string)el.Attribute("Name")
                             select el;
            return collection;
        }

        /* TODO: Remove this shitty nullObject. */
        public IEnumerable<XElement> LoadPageCollectionList(object nullObject) {
            var collection = from e in Root.Elements("Collection")
                             select e;
            return collection;
        }

        /// <summary>
        /// Returns all items within the same directory.
        /// </summary>
        /// <param name="i">Processed item.</param>
        /// <returns>Sieblings of the item.</returns>
        public IEnumerable<XElement> LoadSiblingsList(object i) {
            Item item = (Item)i;
            XElement xItem = GetCollectionByFullName(item.FullName);
            IOrderedEnumerable<XElement> siblingsList = from e in xItem.Parent.Elements()
                                                        orderby (string)e.Attribute("Name")
                                                        select e;
            return siblingsList;
        }

        /// <summary>
        /// Returns all items of the same type within the directory.
        /// </summary>
        /// <param name="i">Processed item.</param>
        /// <returns>Sieblings of the item.</returns>
        public IEnumerable<XElement> LoadCollectionSiblingsListByType(object i) {
            Item item = (Item)i;
            IEnumerable<XElement> collectionSiblingsList = LoadSiblingsList(item)
                .Where(it => item.Type.Equals((string)it.Attribute("Type")));
            return collectionSiblingsList;
        }

        public IEnumerable<XElement> LoadSiblingsListByType(object i) {
            Item item = (Item)i;
            XElement xItem = GetXElementByFullName(item.FullName);
            IOrderedEnumerable<XElement> siblingsList = from e in xItem.Parent.Elements()
                                                        where item.Type.Equals((string)e.Attribute("Type"))
                                                        orderby (string)e.Attribute("Name")
                                                        select e;
            return siblingsList;
        }

        private XElement GetXElementByFullName(string fullName) {
            XElement xItem = Root
              .Descendants()
              .Where(e => fullName.Equals((string)e.Attribute("FullName")))
              .FirstOrDefault();

            if (xItem == null) {
                throw new KeyNotFoundException(string.Format(CultureInfo.CurrentCulture, "{0}", fullName));
            }
            return xItem;
        }

        /// <summary>
        /// Returns a node represention the collection in the file tree.
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        private XElement GetCollectionByFullName(string fullName) {
            /* Id. */
            XElement xItem = Root
                .Descendants("Collection")
                .Where(e => fullName.Equals((string)e.Attribute("FullName")))
                .FirstOrDefault();

            if (xItem == null) {
                throw new KeyNotFoundException(string.Format(CultureInfo.CurrentCulture, "{0}", fullName));
            }
            return xItem;
        }

        private XElement GetItemByFullName(string fullName) {
            /* Id. */
            XElement xItem = Root
                .Descendants("Item")
                .Where(e => fullName.Equals((string)e.Attribute("FullName")))
                .FirstOrDefault();

            if (xItem == null) {
                throw new KeyNotFoundException(string.Format(CultureInfo.CurrentCulture, "{0}", fullName));
            }
            return xItem;
        }

        public IEnumerable<XElement> LoadItemListDescendantsByCollectionName(object name) {
            string nameValue = (string)name;
            var collection = from e in Root.Descendants("Collection")
                             where nameValue.Equals((string)e.Attribute("Name"))
                             select e;
            var itemList = from e in collection.Descendants("Item")
                           select e;
            return itemList;
        }

        public IEnumerable<XElement> LoadItemListByCollection(object collection) {
            XElement xItem = GetCollectionByFullName(((Item)collection).FullName);
            IOrderedEnumerable<XElement> collectionList = from e in xItem.Elements()
                                                          orderby (string)e.Attribute("Name")
                                                          select e;
            return collectionList;
        }

        public IEnumerable<XElement> LoadItemListByCollectionName(object name) {
            string nameValue = (string)name;
            var collection = from e in Root.Descendants("Item")
                             where nameValue.Equals((string)e.Element("Tag"))
                             orderby (string)e.Attribute("FullName")
                             select e;
            return collection;
        }

        public Item LoadParentCollectionByCollection(object collection) {
            if (collection != null) {
                XElement xItem = GetXElementByFullName(((Item)collection).FullName);
                if (xItem != null) {
                    XElement xItemParent = xItem.Parent;
                    Item item = new Item {
                        FullName = (string)xItemParent.Attribute("FullName"),
                        Type = (string)xItemParent.Attribute("Type"),
                        CoverFile = (string)xItemParent.Attribute("Thumbnail"),
                        Name = (string)xItemParent.Attribute("Name"),
                        Description = (string)xItemParent.Element("Description"),
                        Tag = (string)xItemParent.Element("Tag"),
                    };
                    return item;
                } else {
                    return null;
                }

            } else {
                return null;
            }

        }

        public IEnumerable<XElement> LoadParentCollectionListByCollection(object collection) {
            XElement xItem = GetXElementByFullName(((Item)collection).FullName).Parent;
            IOrderedEnumerable<XElement> collectionList = from e in xItem.Parent.Elements("Collection")
                                                          orderby (string)e.Attribute("Name")
                                                          select e;
            return collectionList;
        }

        public static void LoadFileTree(string rootDirectory) {
            DirectoryInfo root = new DirectoryInfo(rootDirectory);
            XDocument document = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("kTouch content declarations file."),
                WalkDirectoryTree(root));
            if (DocumentLoaded != null) {
                DocumentLoaded(null, new EventArgs());
            }
            _document = document;
        }

        private static XElement WalkDirectoryTree(DirectoryInfo root) {
            XElement current =
                new XElement("Collection",
                    new XAttribute("Name", root.Name),
                    new XAttribute("FullName", root.FullName),
                    new XAttribute("Type", "dir"),
                    new XAttribute("Tag", root.Parent.Name));
            // First find all the subdirectories (exculing file only directories) under this directory.
            List<DirectoryInfo> directoryList = root
                .GetDirectories()
                .Where(d => d.GetDirectories().Any())
                .ToList();
            // Resursive call for each subdirectory.
            foreach (DirectoryInfo directory in directoryList) {
                current.Add(WalkDirectoryTree(directory));
            }
            // Now, process all the file directories directly under this folder.
            List<DirectoryInfo> fileFolderList = root.GetDirectories()
                .Except(directoryList)
                .ToList();
            foreach (DirectoryInfo fileDirectory in fileFolderList) {
                // Now process all the files directly under this folder.
                List<FileInfo> fileList = null;
                try {
                    fileList = fileDirectory
                        .GetFiles("*.*")
                        .Where(f => SupportedExtensions.SupportedExtensionList.Contains(f.Extension))
                        .ToList();
                } catch (UnauthorizedAccessException e) {
                    Console.WriteLine(e.Message);
                } catch (DirectoryNotFoundException e) {
                    Console.WriteLine(e.Message);
                }
                foreach (FileInfo file in fileList) {
                    current.Add(new XElement("Item",
                            new XAttribute("FullName", file.FullName),
                            new XAttribute("Type", file.Extension),
                            new XAttribute("Thumbnail", GetFileThumbnail(file)),
                            new XAttribute("Name", file.Directory.Name),
                                new XElement("Tag", root.Name),
                                new XElement("Description", "Add me!")
                        ));
                }
            }
            return current;
        }

        private static string GetFileThumbnail(FileInfo file) {
            string thumbnail = string.Empty;
            ICollection<FileInfo> existingThumbs = file.Directory.GetFiles("*.*")
                .Where(f => SupportedExtensions.SupportedThumbnailExtensionList.Contains(f.Extension))
                .ToList();
            if (existingThumbs.Any()) {
                thumbnail = existingThumbs.First().FullName;
            } else {
                thumbnail = ThumbnailCreator.CreateThumbnail(file.FullName);
            }
            return thumbnail;
        }

        /// <summary>
        /// Creates and XDocument (XML) declaration file from the content of a directory.
        /// </summary>
        /// <param i="path">Full path of the content directory.</param>
        /// <returns> Creates brand new XDocument with file references.</returns>
        public static void LoadXDocument(string directory) {
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
                                    new XAttribute("FullName", f),
                                    new XAttribute("Type", Path.GetExtension(f)),
                                    new XAttribute("Thumbnail", dicoFileThumbnail[f]),
                                    new XAttribute("Name", Path.GetFileNameWithoutExtension(f)),
                                    new XElement(dir + "Tag", Directory.GetParent(f).Parent.Name),
                                    new XElement(dir + "Description", "Add me!")
                        )
                    )
            );
            if (DocumentLoaded != null) {
                DocumentLoaded(null, new EventArgs());
            }
            _document = document;
        }

        /* Old Stuff
         * 
        /// <summary>
        /// Starts a new thread with a parameter and a delegate.
        /// </summary>
        /// <param i="parameter">Parameter.</param>
        /// <param i="start">ParameterizedThreadStart delegate.</param>
        /// <returns>Thread's ManagedThreadId.</returns>
        private static int kLoaderParameterizedThreadStart(object parameter, ParameterizedThreadStart start) {

            return loadingThread.ManagedThreadId;
        }

        /// <summary>
        /// Exposes an exotic public method for loading a i i by i.
        /// </summary>
        /// <param i="itemCollection">Item i.</param>
        /// <param i="i">Tag of the i i.</param>
        public void LoadItemCollectionByTag(object i) {
            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(LoadItemCollectionByTagAsynch);
            Thread loadingThread = new Thread(LoadItemCollectionByTagAsynch);
            loadingThread.Start(i);
            //ThreadItemCollectionPool.Add(loadingThread.ManagedThreadId, itemCollection);
        }


        /// <summary>
        /// Loads an i i by i in a background thread.
        /// </summary>
        /// <param i="path">Full path to the content directory.</param>
        private static void LoadItemCollectionByTagAsynch(object i) {
            Dispatcher CurrentDispatcher = Application.Current.Dispatcher;
            if(CurrentDispatcher != null) {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                KeyValuePair<int, string> pair = new KeyValuePair<int, string>(threadId, (string)i);
                DispatcherOperation dispatcherOp = CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ParseDeclarationFile), pair);
                if(dispatcherOp != null) {
                    dispatcherOp.Completed += ItemCollectionCreated;
                }
            }
        }

        /// <summary>
        /// Parses the kDocument in order to create a i of kItems.
        /// </summary>
        /// <param i="pairThreadIdTag">Key-value pair of the i and processing thread id.</param>
        /// <returns>Null.</returns>
        private static object ParseDeclarationFile(object pairThreadIdTag) {
            KeyValuePair<int, string> pair = (KeyValuePair<int, string>)pairThreadIdTag;
            string fullName = pair.Value;
            XElement root = _document.Root;
            XNamespace ns = root.Name.NamespaceName;
            string rootDirectory = ns.ToString();
            var i = from e in root.Elements(ns + "kItem")
                             where !string.IsNullOrEmpty(fullName) ? string.Equals((string)e.Element(ns + "Tag"), fullName) : true
                             orderby (string)e.Attribute("Name")
                             select e;
            ObservableCollection<kItem> threadItemCollection = ThreadItemCollectionPool[pair.Key];
            foreach(XElement e in i) {
                threadItemCollection.Add(CreateItemFromXElement(e, ns));
            }
            return null;
        }

        /// <summary>                                                                                                                                                  
        /// Creates a kItem from the processed XML node.
        /// </summary>
        /// <param i="element">XElement node.</param>
        /// <param i="ns">XNamespace namespace.</param>
        /// <returns>kItem object.</returns>
        private static kItem CreateItemFromXElement(XElement element, XNamespace ns) {
            if(element.IsEmpty) {
                throw new ArgumentNullException("element");
            }
            kItem i = new kItem {
                Id = (int)element.Attribute("Id"),
                Directory = (string)element.Attribute("Directory"),
                FullName = (string)element.Attribute("FullName"),
                Type = (string)element.Attribute("Type"),
                CoverFile = (string)element.Attribute("Thumbnail"),
                Description = (string)element.Element(ns + "Description"),
                Tag = (string)element.Element(ns + "Tag"),
                Name = (string)element.Attribute("Name"),
            };
            if(ItemArrived != null) {
                ItemArrived(i);
            }
            return i;
        }
        */
    }
}