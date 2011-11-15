using System.Collections.ObjectModel;
using KTouch.Units;

namespace KTouch.Controls.ViewModel {

    public class KTouchMainViewModel : ViewModelBase {


        private ObservableCollection<Item> _initialCollection = null;
        private ObservableCollection<Item> _scatterViewCollection = null;
        private readonly string _universeName;

        public ObservableCollection<Item> InitialCollection {
            get {
                if ( _initialCollection == null ) {
                    _initialCollection = new ObservableCollection<Item> ( ItemsLoader.InputCollections [ _universeName ] );
                };
                return _initialCollection;
            }
            set {
                _initialCollection = value;
                base.OnPropertyChanged ( "InitialCollection" );
            }
        }

        public ObservableCollection<Item> ScatterViewCollection {
            get {
                if ( _scatterViewCollection == null ) {
                    _scatterViewCollection = new ObservableCollection<Item> ( );
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
