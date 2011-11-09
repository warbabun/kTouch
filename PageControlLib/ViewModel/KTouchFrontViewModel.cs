using System.Collections.ObjectModel;
using KTouch.Units;

namespace KTouch.Controls.ViewModel {

    public class KTouchFrontViewModel : ViewModelBase {





        private ObservableCollection<kItem> _stagesCollection = null;
        private ObservableCollection<kItem> _solutionsCollection = null;
        private ObservableCollection<kItem> _realisationsCollection = null;

        public ObservableCollection<kItem> StagesCollection {
            get {
                if(_stagesCollection == null) {
                    _stagesCollection = new ObservableCollection<kItem>(ItemsLoader.InputCollections["Stages"]);
                };
                return _stagesCollection;
            }
            set {
                _stagesCollection = value;
                base.OnPropertyChanged("StagesCollection");
            }
        }
        public ObservableCollection<kItem> SolutionsCollection {
            get {
                if(_solutionsCollection == null) {
                    _solutionsCollection = new ObservableCollection<kItem>(ItemsLoader.InputCollections["Solutions"]);
                };
                return _solutionsCollection;
            }
            set {
                _solutionsCollection = value;
                base.OnPropertyChanged("SolutionsCollection");
            }
        }

        public ObservableCollection<kItem> RealisationsCollection {
            get {
                if(_realisationsCollection == null) {
                    _realisationsCollection = new ObservableCollection<kItem>(ItemsLoader.InputCollections["Realisations"]);
                };
                return _realisationsCollection;
            }
            set {
                _realisationsCollection = value;
                base.OnPropertyChanged("RealisationsCollection");
            }
        }

        public KTouchFrontViewModel() {
            //StagesCollection = new ObservableCollection<KTouchItem>(ItemsLoader.LoadXmlDeclarationFile(@"C:\KTouchUnivers\Stages\stages.ktouch.xml"));

            //for (int i = 0; i < 3; i++)
            //    list.AddRange(ItemsLoader.LoadXmlDeclarationFile(@"C:\KTouchUnivers\Solutions2\solutions.ktouch.xml"));
            //SolutionsCollection = new ObservableCollection<KTouchItem>(list);
            //list.Clear();

            //for (int i = 0; i < 3; i++)
            //    list.AddRange(ItemsLoader.LoadXmlDeclarationFile(@"C:\KTouchUnivers\Realisations\realisations.ktouch.xml"));
            //RealisationsCollection = new ObservableCollection<KTouchItem>(list);
            //list.Clear();
        }
    }

}
