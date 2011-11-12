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
    public class Loader<T> {
        public delegate T Wrapper(XElement result);
        public delegate IEnumerable<XElement> TagQuery(object tag);
        public delegate void ReturnItemCallBack(ICollection<T> collection);
        public delegate void ItemArrivedEventHandler(kItem obj);

        public static event ItemArrivedEventHandler ItemArrived;
        public static event EventHandler DocumentLoaded;
        public static event EventHandler ItemCollectionCreated;

        public readonly ObservableCollection<T> _currentItemCollection;
        private static XDocument _document;

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
        /// Loads a view model item collection with T elements in a background thread.
        /// </summary>
        /// <param name="viewModelItemCollection">Reference to the processed view model item collection.</param>
        public Loader(ref ObservableCollection<T> viewModelItemCollection) {
            if(Document == null) {
                try {
                    _document = Loader<object>.Document;
                } catch(Exception) {
                    throw new ArgumentNullException("Document");
                }
            }
            _currentItemCollection = viewModelItemCollection;

        }

        /// <summary>
        /// Proceeds a query.
        /// </summary>
        /// <param name="parameter">Query parameter.</param>
        /// <param name="query">Query function.</param>
        /// <param name="queryResultsWrapper">Query results wrapper.</param>
        /// <param name="returnItemCallback">Return item callback.</param>
        public void StartLoad(object parameter, TagQuery query, Wrapper queryResultsWrapper = null, ReturnItemCallBack returnItemCallback = null) {
            Dispatcher CurrentDispatcher = Application.Current.Dispatcher;
            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(delegate(object startParameter) {
                /* Необходимо, читай критично, чтобы входной параметр callback функции совпадал с выходным параметром query. */
                /* Если callback не указан, результат будет обернут в ICollection<kItem>. */
                IEnumerable<XElement> queryResults = query(startParameter);
                ICollection<T> results = WrapQueryResults(queryResults, queryResultsWrapper);
                ReturnItemCallBack callback = returnItemCallback ?? DefaultReturnItemCallback;
                DispatcherOperation dispatcherOp = CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, callback, results);
                if(dispatcherOp != null) {
                    dispatcherOp.Completed += ItemCollectionCreated;
                }
            });
            Thread loadingThread = new Thread(threadStart);
            loadingThread.Start(parameter);
        }

        /// <summary>
        /// Wrap a collection of query results.
        /// </summary>
        /// <param name="queryResult">Set of query results.</param>
        /// <param name="queryResultsWrapper">Wrapper function.</param>
        /// <returns>A ready to return collection of items.</returns>
        private ICollection<T> WrapQueryResults(IEnumerable<XElement> queryResult, Wrapper queryResultsWrapper) {
            ICollection<T> wrappedCollection = new List<T>();
            if(queryResultsWrapper != null) {
                foreach(XElement e in queryResult) {
                    wrappedCollection.Add(queryResultsWrapper(e));
                }
            } else {
                foreach(XElement e in queryResult) {
                    wrappedCollection.Add((T)DefaultQueryResultsWrapper(e));
                }
            }
            return wrappedCollection;
        }

        /// <summary>
        /// Returns a new item wrapped in a kItem.
        /// </summary>
        /// <param name="node">XElement item to wrap.</param>
        /// <returns>Wrapped kItem.</returns>
        public object DefaultQueryResultsWrapper(XElement node) {
            if(node.IsEmpty) {
                throw new ArgumentNullException("node");
            }
            XNamespace ns = Loader<T>.RootNamespace;
            kItem item = new kItem {
                Id = (int)node.Attribute("Id"),
                Directory = (string)node.Attribute("Directory"),
                File = (string)node.Attribute("File"),
                Type = (string)node.Attribute("Type"),
                CoverFile = (string)node.Attribute("Thumbnail"),
                Description = (string)node.Element(ns + "Description"),
                Tag = (string)node.Element(ns + "Tag"),
                Title = (string)node.Attribute("Title"),
            };
            if(ItemArrived != null) {
                ItemArrived(item);
            }
            return item;
        }

        /// <summary>
        /// Returns a new item the UI collection in a default way.
        /// </summary>
        /// <param name="result">Collection of T items.</param>
        public void DefaultReturnItemCallback(ICollection<T> result) {
            ICollection<T> collection = (ICollection<T>)result;
            foreach(T item in collection) {
                this._currentItemCollection.Add(item);
            }
        }

        /// <summary>
        /// Creates and XDocument (XML) declaration file from the content of a directory.
        /// </summary>
        /// <param name="path">Full path of the content directory.</param>
        /// <returns> Creates brand new XDocument with file references.</returns>
        public static void LoadXDocument(string directory) {
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
                                    new XElement(dir + "Tag", Directory.GetParent(f).Parent.Name),
                                    new XElement(dir + "Description", "Add me!")
                        )
                    )
            );
            if(DocumentLoaded != null) {
                DocumentLoaded(null, new EventArgs());
            }
            _document = document;
        }

        /* Old Stuff
         * 
        /// <summary>
        /// Starts a new thread with a parameter and a delegate.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        /// <param name="start">ParameterizedThreadStart delegate.</param>
        /// <returns>Thread's ManagedThreadId.</returns>
        private static int kLoaderParameterizedThreadStart(object parameter, ParameterizedThreadStart start) {

            return loadingThread.ManagedThreadId;
        }

        /// <summary>
        /// Exposes an exotic public method for loading a item collection by tag.
        /// </summary>
        /// <param name="itemCollection">Item collection.</param>
        /// <param name="tag">Tag of the item collection.</param>
        public void LoadItemCollectionByTag(object tag) {
            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(LoadItemCollectionByTagAsynch);
            Thread loadingThread = new Thread(LoadItemCollectionByTagAsynch);
            loadingThread.Start(tag);
            //ThreadItemCollectionPool.Add(loadingThread.ManagedThreadId, itemCollection);
        }


        /// <summary>
        /// Loads an item collection by tag in a background thread.
        /// </summary>
        /// <param name="path">Full path to the content directory.</param>
        private static void LoadItemCollectionByTagAsynch(object tag) {
            Dispatcher CurrentDispatcher = Application.Current.Dispatcher;
            if(CurrentDispatcher != null) {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                KeyValuePair<int, string> pair = new KeyValuePair<int, string>(threadId, (string)tag);
                DispatcherOperation dispatcherOp = CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ParseDeclarationFile), pair);
                if(dispatcherOp != null) {
                    dispatcherOp.Completed += ItemCollectionCreated;
                }
            }
        }

        /// <summary>
        /// Parses the kDocument in order to create a collection of kItems.
        /// </summary>
        /// <param name="pairThreadIdTag">Key-value pair of the tag and processing thread id.</param>
        /// <returns>Null.</returns>
        private static object ParseDeclarationFile(object pairThreadIdTag) {
            KeyValuePair<int, string> pair = (KeyValuePair<int, string>)pairThreadIdTag;
            string tagValue = pair.Value;
            XElement root = _document.Root;
            XNamespace ns = root.Name.NamespaceName;
            string rootDirectory = ns.ToString();
            var collection = from e in root.Elements(ns + "kItem")
                             where !string.IsNullOrEmpty(tagValue) ? string.Equals((string)e.Element(ns + "Tag"), tagValue) : true
                             orderby (string)e.Attribute("Title")
                             select e;
            ObservableCollection<kItem> threadItemCollection = ThreadItemCollectionPool[pair.Key];
            foreach(XElement e in collection) {
                threadItemCollection.Add(CreateItemFromXElement(e, ns));
            }
            return null;
        }

        /// <summary>                                                                                                                                                  
        /// Creates a kItem from the processed XML node.
        /// </summary>
        /// <param name="element">XElement node.</param>
        /// <param name="ns">XNamespace namespace.</param>
        /// <returns>kItem object.</returns>
        private static kItem CreateItemFromXElement(XElement element, XNamespace ns) {
            if(element.IsEmpty) {
                throw new ArgumentNullException("element");
            }
            kItem item = new kItem {
                Id = (int)element.Attribute("Id"),
                Directory = (string)element.Attribute("Directory"),
                File = (string)element.Attribute("File"),
                Type = (string)element.Attribute("Type"),
                CoverFile = (string)element.Attribute("Thumbnail"),
                Description = (string)element.Element(ns + "Description"),
                Tag = (string)element.Element(ns + "Tag"),
                Title = (string)element.Attribute("Title"),
            };
            if(ItemArrived != null) {
                ItemArrived(item);
            }
            return item;
        }
        */
    }
}