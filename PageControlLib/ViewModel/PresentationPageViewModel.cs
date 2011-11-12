using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using KTouch.Units;

namespace KTouch.Controls.ViewModel {
    public class PresentationPageViewModel {

        private readonly ObservableCollection<kItem> _itemList;

        public ObservableCollection<kItem> ItemList {
            get {
                return _itemList;
            }
        }

        public kItem Item {
            get;
            private set;
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

        public PresentationPageViewModel(kItem item) {
            this.Item = item;
            _itemList = new ObservableCollection<kItem>();
            Loader<kItem> loader = new Loader<kItem>(ref _itemList);
            loader.StartLoad(item.Tag, this.LoadMainPageCollectionByTag);

        }
    }
}
