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


        private ObservableCollection<Item> _itemList;
        public ObservableCollection<Item> ItemList {
            get {
                return _itemList;
            }
        }

        public MainPageViewModel(Item item) {
            Collection = item;
            _itemList = new ObservableCollection<Item>();
            Loader<Item> loader = new Loader<Item>();
            loader.StartLoad(ref _itemList, Collection, loader.LoadItemListByCollection);
        }
    }
}
