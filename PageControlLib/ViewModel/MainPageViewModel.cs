using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using KTouch.Units;

namespace KTouch.Controls.ViewModel {

    public class MainPageViewModel {

        private readonly string _currentTag;

        public string CurrentTag {
            get {
                return _currentTag;
            }
        }

        private readonly ObservableCollection<string> _tagList;
        public ObservableCollection<string> TagList {
            get {
                return _tagList;
            }
        }

        private ObservableCollection<kItem> _firstRowItemList;
        public ObservableCollection<kItem> FirstRowItemList {
            get {
                return _firstRowItemList;
            }
        }

        private ObservableCollection<kItem> _secondRowItemList;
        public ObservableCollection<kItem> SecondRowItemList {
            get {
                return _secondRowItemList;
            }
        }

        public IEnumerable<XElement> LoadMainPageCollectionByTag(object tag) {
            string tagValue = (string)tag;
            XElement root = Loader<kItem>.Root;
            XNamespace ns = Loader<kItem>.RootNamespace;
            string rootDirectory = ns.ToString();
            IOrderedEnumerable<XElement> collection = from e in root.Elements(ns + "kItem")
                                                      where !string.IsNullOrEmpty(tagValue) ? string.Equals((string)e.Element(ns + "Tag"), tagValue) : true
                                                      orderby (string)e.Attribute("Title")
                                                      select e;
            return collection;
        }

        public IEnumerable<XElement> LoadTagCollection(object tag) {
            XElement root = Loader<string>.Root;
            XNamespace ns = Loader<string>.RootNamespace;
            List<XElement> nodeList = new List<XElement>();
            //root.Descendants(ns + "Tag")
            //    .ToList()
            //    .ForEach(rootEl => { if(!nodeList.Select(presEl => presEl.Value).Contains(rootEl.Value)) nodeList.Add(rootEl); });
            nodeList.Add(root.Descendants(ns + "Tag").FirstOrDefault());
            nodeList.AddRange(from e in root.Descendants(ns + "Tag")
                              where nodeList.Any() && !nodeList.Select(t => t.Value).Contains(e.Value)
                              select e);
            return nodeList;
        }

        public string StringWrapper(XElement node) {
            if(node.IsEmpty) {
                throw new ArgumentNullException("node");
            }
            return node.Value;
        }

        public MainPageViewModel(string tag) {
            _currentTag = tag;
            _tagList = new ObservableCollection<string>();
            _firstRowItemList = new ObservableCollection<kItem>();
            _secondRowItemList = new ObservableCollection<kItem>();
            Loader<string> tagLoader = new Loader<string>();
            tagLoader.ItemCollectionCreated += new EventHandler(MainPageViewModel_ItemCollectionCreated);
            tagLoader.StartLoad(ref _tagList, null, LoadTagCollection, StringWrapper);
        }

        void MainPageViewModel_ItemCollectionCreated(object sender, EventArgs e) {
            Loader<kItem> kItemLoader = new Loader<kItem>();
            string firstTag = CurrentTag;
            if(string.IsNullOrEmpty(firstTag)) {
                firstTag = this.TagList.First();
                string secondTag = this.TagList.Last();
                kItemLoader.StartLoad(ref _secondRowItemList, secondTag, LoadMainPageCollectionByTag);
            } 
            kItemLoader.StartLoad(ref _firstRowItemList, firstTag, LoadMainPageCollectionByTag);
        }

        //void kLoader_ItemReceived(kItem obj) {
        //    this.FirstRowItemList.Add(obj);
        //}
    }
}
