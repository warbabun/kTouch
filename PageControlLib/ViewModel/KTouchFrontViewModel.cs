using System.Collections.ObjectModel;
using KTouch.Units;

namespace KTouch.Controls.ViewModel {

    public class KTouchFrontViewModel : ViewModelBase {

        private ObservableCollection<kItem> _itemList;

        public ObservableCollection<kItem> ItemList {
            get {
                if (_itemList == null) {
                    _itemList = new ObservableCollection<kItem>();
                }
                return _itemList;
            }
        }

        public KTouchFrontViewModel() {
            kLoader.ItemReceived += new kLoader.MikeEventHandler(kLoader_ItemReceived);
        }

        void kLoader_ItemReceived(kItem obj) {
            this.ItemList.Add(obj);
        }
    }
}
