using System;
using System.Collections.ObjectModel;
using System.Windows;
using KTouch.Units;

namespace KTouch.Controls.ViewModel {

    public class MainPageViewModel : DependencyObject {

        public Item Collection {
            get { return (Item)GetValue(CollectionProperty); }
            set { SetValue(CollectionProperty, value); }
        }

        public static readonly DependencyProperty CollectionProperty =
            DependencyProperty.Register("Collection", typeof(Item), typeof(MainPageViewModel), new UIPropertyMetadata(null));

        public Item Item {
            get { return (Item)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(Item), typeof(MainPageViewModel), new UIPropertyMetadata(null));

        private ObservableCollection<Item> _collectionList;
        public ObservableCollection<Item> CollectionList {
            get {
                return _collectionList;
            }
        }

        private ObservableCollection<Item> _itemList;
        public ObservableCollection<Item> ItemList {
            get {
                return _itemList;
            }
        }

        public MainPageViewModel() {
            Loader<Item> collectionListLoader = new Loader<Item>();
            _collectionList = new ObservableCollection<Item>();
            collectionListLoader.StartLoad(ref _collectionList, null, collectionListLoader.LoadPageCollectionList);
        }

        public MainPageViewModel(Item item) {
            Collection = item;
            _collectionList = new ObservableCollection<Item>();
            _itemList = new ObservableCollection<Item>();
            Loader<Item> collectionListLoader = new Loader<Item>();
            collectionListLoader.ItemCollectionCreated += new EventHandler(MainPageViewModel_ItemCollectionCreated);
            collectionListLoader.StartLoad(ref _collectionList, item, collectionListLoader.LoadPageCollectionListByName);
        }

        void MainPageViewModel_ItemCollectionCreated(object sender, EventArgs e) {
            Loader<Item> itemLoader = new Loader<Item>();
            itemLoader.StartLoad(ref _itemList, Collection, itemLoader.LoadCollectionItemListByName);
        }
    }
}
