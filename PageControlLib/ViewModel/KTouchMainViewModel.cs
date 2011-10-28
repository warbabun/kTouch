using System.Collections.ObjectModel;
using KTouch.Units;

namespace KTouch.Controls.ViewModel {

    public class KTouchMainViewModel : ViewModelBase {


        private ObservableCollection<KTouchItem> _initialCollection = null;
        private ObservableCollection<KTouchItem> _scatterViewCollection = null;
        private readonly string _universeName;

        public ObservableCollection<KTouchItem> InitialCollection {
            get {
                if ( _initialCollection == null ) {
                    _initialCollection = new ObservableCollection<KTouchItem> ( ItemsLoader.InputCollections [ _universeName ] );
                };
                return _initialCollection;
            }
            set {
                _initialCollection = value;
                base.OnPropertyChanged ( "InitialCollection" );
            }
        }

        public ObservableCollection<KTouchItem> ScatterViewCollection {
            get {
                if ( _scatterViewCollection == null ) {
                    _scatterViewCollection = new ObservableCollection<KTouchItem> ( );
                };
                return _scatterViewCollection;

            }
            set {
                _scatterViewCollection = value;
                base.OnPropertyChanged ( "ScatterViewCollection" );
            }
        }

        /// <summary>
        /// Main constructor for KTouchMainViewModel class
        /// </summary>
        /// <param name="path"></param>
        public KTouchMainViewModel ( string universeName ) {
            this._universeName = universeName;
        }

    }
}
