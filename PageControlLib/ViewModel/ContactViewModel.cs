using System;
using System.ComponentModel;
using System.Windows.Input;
using KTouch.Controls.Core;
using KTouch.Controls.Model;
using KTouch.Units;


namespace KTouch.Controls.ViewModel {
    public partial class ContactViewModel : ViewModelBase, IDataErrorInfo {

        Contact _contact;
        public readonly ContactsEntities Contacts;
        //private readonly Popup _messagePopup = null; 

        RelayCommand _saveCommand;
        RelayCommand _closeCommand;
        public event EventHandler RequestClose;

        // bool _isPhoneChecked = false;
        //  Visibility _isPhoneVisible = Visibility.Hidden;

        /// <summary>
        /// Constructor
        /// </summary>
        public ContactViewModel ( ) {
            Contacts = new ContactsEntities ( );
            _contact = Contact.CreateNewContact ( );


        }

        /// <summary>
        /// Public property Name. 
        /// </summary>
        public string Name {
            get { return _contact.Name; }
            set {
                if ( value != _contact.Name ) {
                    _contact.Name = value;
                    base.OnPropertyChanged ( "Name" );
                }
            }
        }

        /// <summary>
        /// Public property Surname
        /// </summary>
        public string Surname {
            get { return _contact.Surname; }
            set {
                if ( value != _contact.Surname ) {
                    _contact.Surname = value;
                    base.OnPropertyChanged ( "Surname" );
                }
            }
        }

        /// <summary>
        /// Public property Email
        /// </summary>
        public string Email {
            get { return _contact.Email; }
            set {
                if ( value != _contact.Email ) {
                    _contact.Email = value;
                    base.OnPropertyChanged ( "Email" );
                }
            }
        }

        /// <summary>
        /// Public property Phone
        /// </summary>
        public string Phone {
            get { return _contact.Phone; }
            set {
                if ( value != _contact.Phone ) {
                    _contact.Phone = value;
                    base.OnPropertyChanged ( "Phone" );
                }
            }
        }

        /// <summary>
        /// Public property Preferences
        /// </summary>
        public string Preferences {
            get {
                return _contact.Preferences;
            }
            set {
                if ( value != _contact.Preferences ) {
                    _contact.Preferences = value;
                    base.OnPropertyChanged ( "Preferences" );
                }
            }
        }

        private bool _isOpen = false;
        public bool IsOpen {
            get { return _isOpen; }
            set {
                if ( value != _isOpen ) {
                    _isOpen = value;
                    base.OnPropertyChanged ( "IsOpen" );
                }
            }
        }

        /// <summary>
        /// Public property responsible for the visibility of Phone section
        /// </summary>
        //public Visibility IsPhoneVisible {
        //    get {return _isPhoneVisible; }
        //    set {
        //        if (value != _isPhoneVisible) {
        //            _isPhoneVisible = value;
        //        }
        //        base.OnPropertyChanged("IsPhoneVisible");
        //    }
        //}

        /// <summary>
        /// Public property used to establish a interraction logic 
        /// between IsChecked property of CheckBox and Visibility of Phone section
        /// </summary>
        //public bool IsPhoneChecked {
        //    get { return _isPhoneChecked; }
        //    set {
        //        if (value != _isPhoneChecked) {
        //            _isPhoneChecked = value;
        //            if (_isPhoneChecked) {
        //                IsPhoneVisible = Visibility.Visible;
        //            } else {
        //                IsPhoneVisible = Visibility.Hidden;
        //                Phone = null;
        //            }
        //            base.OnPropertyChanged("IsPhoneChecked");
        //        }
        //    }
        //}

        /// <summary>
        /// Public command for Save button
        /// </summary>
        public ICommand SaveCommand {
            get {
                if ( _saveCommand == null ) {
                    _saveCommand = new RelayCommand ( p => this.Save ( ), p => this.CanSave );
                }
                return _saveCommand;
            }
        }

        /// <summary>
        /// Public command for Close button
        /// </summary>
        public ICommand CloseCommand {
            get {
                if ( _closeCommand == null )
                    _closeCommand = new RelayCommand ( p => this.OnRequestClose ( ) );
                return _closeCommand;
            }
        }

        /// <summary>
        /// Close operation handler
        /// </summary>
        void OnRequestClose ( ) {
            EventHandler handler = this.RequestClose;
            this.Name = null;
            this.Surname = null;
            this.Email = null;
            this.Phone = null;

            if ( handler != null )
                handler ( this, EventArgs.Empty );
        }

        /// <summary>
        /// Save operation handler
        /// </summary>
        void Save ( ) {

            if ( !_contact.IsValid )
                throw new InvalidOperationException ( "This information is not valid" );

            ContactsInfo newInfo = new ContactsInfo ( ) {
                Name = this.Name,
                Surname = this.Surname,
                Email = this.Email,
                Phone = this.Phone,
                Preferences = this.Preferences,
            };
            try {
                Contacts.AddToContactsInfoes ( newInfo );
                Contacts.SaveChanges ( );
                kPage.MessagePopup.IsOpen = true;
            } catch ( Exception e ) {
                Console.WriteLine ( "Database update failed : ", e.Message );
            } finally {
                this.OnRequestClose ( );
            }
        }

        /// <summary>
        /// Predicate used to determine whether Save command can be executed or not
        /// </summary>
        bool CanSave {
            get { return _contact.IsValid; }
        }

        /// <summary>
        /// 
        /// </summary>
        string IDataErrorInfo.Error {
            get { return ( _contact as IDataErrorInfo ).Error; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        string IDataErrorInfo.this [ string propertyName ] {
            get {
                string error = null;
                error = ( _contact as IDataErrorInfo ) [ propertyName ];
                CommandManager.InvalidateRequerySuggested ( );
                return error;
            }
        }
    }
}
