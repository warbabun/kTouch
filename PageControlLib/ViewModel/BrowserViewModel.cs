using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Navigation;
using KTouch.Units;

namespace KTouch.Controls.ViewModel {

    public class BrowserViewModel : DependencyObject {

        private readonly Loader<Item> _loader;
        private readonly NavigationService _nsvc;

        public static PropertyChangedCallback NavigationCallBack;
        public delegate void NavigateEventHandler(Item obj);
        public static event NavigateEventHandler Navigate;

        public Item Collection { get; set; }
        private ObservableCollection<Item> _collectionList;
        public ObservableCollection<Item> CollectionList {
            get {
                if (_collectionList == null) {
                    _collectionList = new ObservableCollection<Item>();
                    _loader.StartLoad(ref _collectionList, "Content", _loader.LoadCollectionListByCollectionName);
                }
                return _collectionList;
            }
        }

        public void ListBoxSelectionChanged(object i) {
            if (i != null && i is Item) {
                Item item = (Item)i;
                _collectionList.Clear();
                _loader.StartLoad(ref _collectionList, item, _loader.LoadSiblingsListByType);
                if (Navigate != null) {
                    Navigate(item);
                }
            }
        }

        public void GoHome() {
            //_collectionList.Clear();
            //_loader.StartLoad(ref _collectionList, "MainContent", _loader.LoadCollectionListByCollectionName);
            if (Navigate != null) {
                Navigate(null);
            }
        }

        public void Close() {
            Item item = Collection;
            Item navigateTo = _loader.LoadParentCollectionByCollection(item);
            if ("MainContent".Equals(navigateTo.Name) ||
                "KleePres".Equals(navigateTo.Name) ||
                "Success Story".Equals(navigateTo.Name) ||
                "FrontContent".Equals(navigateTo.Name)
                ) {
                navigateTo = null;
                _collectionList.Clear();
                _loader.StartLoad(ref _collectionList, "MainContent", _loader.LoadCollectionListByCollectionName);
            } else {
                _collectionList.Clear();
                _loader.StartLoad(ref _collectionList, item, _loader.LoadParentCollectionListByCollection);
            }
            if (Navigate != null) {
                Navigate(navigateTo);
            }
        }

        public BrowserViewModel(NavigationService nsvc) {
            _nsvc = nsvc;
            _loader = new Loader<Item>();
        }
    }
}
