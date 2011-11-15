using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using KTouch.Units;

namespace KTouch.Controls.ViewModel {
    public class KTouchExtendedViewModel : ViewModelBase {

        private readonly Dispatcher _currentDispatcher;
        private readonly string _universeName;
        bool disposed = false;
        RelayCommand _mailCommand;

        private ObservableCollection<Item> _initialCollection = null;
        private ObservableCollection<Item> _scatterViewCollection = null;
        private ObservableCollection<Item> _usedItemsCollection = null;
        //private bool _contactsFormIsOpen = false;

        //public bool ContactsFormIsOpen {
        //    get { return _contactsFormIsOpen; }
        //    set {
        //        _contactsFormIsOpen = value;
        //        base.OnPropertyChanged("ContactsFormIsOpen");
        //    }
        //}

        //private ContactViewModel _contactDataContext = null;
        //public ContactViewModel ContactDataContext {
        //    get {
        //        if ( _contactDataContext == null ) {
        //            _contactDataContext = new ContactViewModel ( );
        //            EventHandler handler = null;
        //            handler = delegate {
        //                _contactDataContext.IsOpen = false;
        //                _contactDataContext.RequestClose -= handler;
        //            };
        //            _contactDataContext.RequestClose += handler;
        //        }
        //        return _contactDataContext;
        //    }
        //    set {
        //        if ( value != null ) {
        //            _contactDataContext = value;
        //            base.OnPropertyChanged ( "ContactDataContext" );
        //        }
        //    }
        //}

        /// <summary>
        /// Public command for Mail button
        /// </summary>
        public ICommand MailCommand {
            get {
                if ( _mailCommand == null )
                    _mailCommand = new RelayCommand ( p => this.OnMail ( ), p => this.CanMail );
                return _mailCommand;
            }
        }

        public bool CanMail {
            get {
                return ( UsedItemsCollection.Count > 0 );
            }
        }

        private void OnMail ( ) {
            string arguments = "";
            foreach ( Item item in _usedItemsCollection ) {
                arguments += item.ToString ( ) + ";";
            }
            //ContactDataContext.Preferences = arguments;
            //ContactDataContext.IsOpen = true;
        }

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

        public ObservableCollection<Item> UsedItemsCollection {
            get {
                if ( _usedItemsCollection == null ) {
                    _usedItemsCollection = new ObservableCollection<Item> ( );
                };
                return _usedItemsCollection;

            }
            set {
                _usedItemsCollection = value;
                base.OnPropertyChanged ( "UsedItemsCollection" );
            }
        }

        public KTouchExtendedViewModel ( string universeName ) {

            this._universeName = universeName;



        }

        void ItemsLoader_FilePackArrived ( System.Collections.Generic.List<Item> pack ) {
            Action dispatchAction = ( ) => {
                foreach ( var item in pack )
                    InitialCollection.Add ( item );
            };
            DispatcherOperation op = _currentDispatcher.BeginInvoke ( dispatchAction, DispatcherPriority.Send );
            ItemsLoader.FilePackArrived -= ItemsLoader_FilePackArrived;
        }

        void loader_FileArrived ( Item obj ) {
            Action dispatchAction = ( ) => { InitialCollection.Add ( obj ); };
            _currentDispatcher.BeginInvoke ( dispatchAction, DispatcherPriority.Background );
        }
    }
}
