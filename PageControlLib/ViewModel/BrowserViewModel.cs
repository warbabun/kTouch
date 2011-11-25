using System.Collections.ObjectModel;
using System.Linq;
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

        //public Item Collection {
        //    get { return (Item)GetValue(CollectionProperty); }
        //    set { SetValue(CollectionProperty, value); }
        //}

        //public static readonly DependencyProperty CollectionProperty =
        //    DependencyProperty.Register("Collection", typeof(Item), typeof(BrowserViewModel), new FrameworkPropertyMetadata(CollectionChangedCallback));

        public Item Collection { get; set; }

        //public static void CollectionChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        //    if(e.NewValue != null && e.NewValue is Item) {
        //        Item item = (Item)e.NewValue;
        //        BrowserViewModel vm = (BrowserViewModel)d;
        //        if(vm.Collection != null) {
        //            vm._collectionList.Clear();
        //        } else {
        //            vm._loader.StartLoad(ref vm._collectionList, item, vm._loader.LoadCollectionSiblingsListByType);
        //            if(Navigate != null) {
        //                Navigate(item);
        //            }
        //        }
        //    }

        //}

        public bool NavigationVisible {
            get { return (bool)GetValue(NavigationVisibleProperty); }
            set { SetValue(NavigationVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NavigationVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavigationVisibleProperty =
            DependencyProperty.Register("NavigationVisible", typeof(bool), typeof(BrowserViewModel), new UIPropertyMetadata(false));


        public void ListBoxSelectionChanged(object i) {
            if(i != null && i is Item) {
                Item item = (Item)i;
                _collectionList.Clear();
                _loader.StartLoad(ref _collectionList, item, _loader.LoadSiblingsListByType);
                if(Navigate != null) {
                    Navigate(item);
                }
            }
        }

        public void GoHome() {
            _collectionList.Clear();
            _loader.StartLoad(ref _collectionList, "MainContent", _loader.LoadCollectionListByCollectionName);
            if(Navigate != null) {
                Navigate(null);
            }
        }

        public void Close() {   
            Item item = Collection;
                Item navigateTo = _loader.LoadParentCollectionByCollection(item);
                if("MainContent".Equals(navigateTo.Name) ||
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
                if(Navigate != null) {
                    Navigate(navigateTo);
                }
        }

        //else if("KleePres".Equals(navigateTo.Name)) {
        //            navigateTo = null;
        //        } else if("Success Story".Equals(navigateTo.Name)) {
        //            navigateTo = null;
        //        } else if("FrontContent".Equals(navigateTo.Name)) {
        //            navigateTo = null;
        //        }

        private ObservableCollection<Item> _collectionList;
        public ObservableCollection<Item> CollectionList {
            get {
                return _collectionList;
            }
        }



        public BrowserViewModel(NavigationService nsvc) {
            _nsvc = nsvc;
            _loader = new Loader<Item>();
            _collectionList = new ObservableCollection<Item>();
            _loader.StartLoad(ref _collectionList, "Content", _loader.LoadCollectionListByCollectionName);
        }
    }
}
