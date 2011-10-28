using System;
using System.Windows.Input;
using Microsoft.Win32;

namespace KTouchScreenSaver {
    class KTouchScreenSaverSettingsModelView : ViewModelBase{

        string _filePath = null; 

        RelayCommand _saveCommand;
        RelayCommand _closeCommand;
        RelayCommand _browseCommand;

        public event EventHandler RequestClose;

        public ICommand SaveCommand {
            get {
                if (_saveCommand == null) {
                    _saveCommand = new RelayCommand(p => this.Save(), p => this.CanSave);
                }
                return _saveCommand;
            }
        }

        public ICommand CloseCommand {
            get {
                if (_closeCommand == null) {
                    _closeCommand = new RelayCommand(p => this.OnRequestClose());
                }
                return _closeCommand;
            }
        }

        public ICommand BrowseCommand {
            get {
                if (_browseCommand == null) {
                    _browseCommand = new RelayCommand(p => this.Browse());
                }
                return _browseCommand;
            }
        }

        public string FilePath {
            get {
                return _filePath;
            }
            set {
                _filePath = value;
                base.OnPropertyChanged("FilePath");
            }
        }

        void Save() {
            KTouchScreenSaverSettings.SaveSettings(FilePath);
            FilePath = null; 
        }

        bool CanSave {
            get {
                return (!String.IsNullOrEmpty(FilePath) &&
                        !String.IsNullOrWhiteSpace(FilePath) &&
                        (FilePath.EndsWith(".avi") || 
                         FilePath.EndsWith(".wmv") || 
                         FilePath.EndsWith(".mpg") ||
                         FilePath.EndsWith(".mp4")));
            }
        }

        /// <summary>
        /// Close operation handler
        /// </summary>
        void OnRequestClose() {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        void Browse() {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.InitialDirectory = @"C:\";
            openDialog.DefaultExt = ".mp4";
            openDialog.Filter = "Video Files(*.avi; *.mp4; *.wmv; *.mpg)|*.avi; *.mp4; *.wmv; *.mpg|All files (*.*)|*.*";
            Nullable<bool> result = openDialog.ShowDialog();
            if (result == true) {
                FilePath = openDialog.FileName;
            }
        }
    }
}
