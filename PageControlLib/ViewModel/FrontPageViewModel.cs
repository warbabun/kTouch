using System.Collections.ObjectModel;
using System.Windows;
using KTouch.Units;

namespace KTouch.Controls.ViewModel {

    public class FrontPageViewModel : DependencyObject {

        private ObservableCollection<Item> _stageList;
        public ObservableCollection<Item> StageList {
            get {
                return _stageList;
            }
        }

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

        public FrontPageViewModel() {
            Loader<Item> loader = new Loader<Item>();
            _successStoryList = new ObservableCollection<Item>();
            _presentationList = new ObservableCollection<Item>();
            _stageList = new ObservableCollection<Item>();
            loader.StartLoad(ref _successStoryList, "Klee Group", loader.LoadItemListByCollectionName);
            loader.StartLoad(ref _presentationList, "Success Story", loader.LoadItemListByCollectionName);
            loader.StartLoad(ref _stageList, "Offres de stages", loader.LoadItemListDescendantsByCollectionName);
        }
    }
}
