using System.Collections.ObjectModel;
using System.Windows;
using KTouch.Units;

namespace KTouch.Controls.ViewModel {

    public class FrontPageViewModel : DependencyObject {

        private ObservableCollection<Item> _successStoryList;
        public ObservableCollection<Item> SuccessStoryList {
            get {
                return _successStoryList;
            }
        }

        private ObservableCollection<Item> _presentationList;
        public ObservableCollection<Item> PresentationList {
            get {
                return _presentationList;
            }
        }

        //private readonly ObservableCollection<Item> _collectionList;
        //public ObservableCollection<Item> CollectionList {
        //    get {
        //        return _collectionList;
        //    }
        //}

        public FrontPageViewModel() {
            Loader<Item> loader = new Loader<Item>();
            _successStoryList = new ObservableCollection<Item>();
            _presentationList = new ObservableCollection<Item>();
            //   _collectionList = new ObservableCollection<Item>();
            //   loader.StartLoad(ref _collectionList, "MainContent", loader.LoadCollectionListByCollection);
            loader.StartLoad(ref _successStoryList, "Klee Group", loader.LoadItemListByCollectionName);
            loader.StartLoad(ref _presentationList, "Success Story", loader.LoadItemListByCollectionName);
        }
    }
}
