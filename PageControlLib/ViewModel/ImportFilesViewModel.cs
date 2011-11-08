using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;

namespace KTouch.Controls.ViewModel {

    /// <summary>
    /// Supplies data to the MainWindow.
    /// </summary>
    class MainWindowViewModel {

        private ObservableCollection<string> _sourceCollection;
        private ObservableCollection<string> _resultCollection;
        private string _sourceDirectory;
        private string _contentDirectoryName;
        private XmlWriter _writer;
        private const string _defaultDirectoryName = "_kTouchUnivers";
        private const string _referenceFile = "/kTouchItems.xml";
        private DirectoryInfo _contentDirectoryInfo;
        private ICommand _chooseDirectory;

        /// <summary>
        /// Public command for Directory button.
        /// </summary>
        public ICommand GetDirectoryCommand {
            get {
                if(_chooseDirectory == null)
                    _chooseDirectory = new RelayCommand(x => this.GetDirectory());
                return _chooseDirectory;
            }
        }

        /// <summary>
        /// Result collection of something.
        /// </summary>
        public ObservableCollection<string> ResultCollection {
            get {
                if(_resultCollection == null) {
                    _resultCollection = new ObservableCollection<string>();
                }
                return _resultCollection;
            }
        }

        /// <summary>
        /// Source collection of filenames.
        /// </summary>
        public ObservableCollection<string> SourceCollection {
            get {
                if(_sourceCollection == null) {
                    _sourceCollection = new ObservableCollection<string>();
                };
                return _sourceCollection;
            }
        }

        /// <summary>
        /// Returns reference file directory information.
        /// </summary>
        private DirectoryInfo ContentDirectoryInfo {
            get {
                if(_contentDirectoryInfo == null) {
                    if(!string.IsNullOrEmpty(_contentDirectoryName)) {
                        _contentDirectoryInfo = new DirectoryInfo(_contentDirectoryName);
                    } else {
                        string subDirectory = Path.Combine(Directory.GetParent(_sourceDirectory).FullName, _defaultDirectoryName);
                        _contentDirectoryInfo = Directory.CreateDirectory(subDirectory);
                    }
                }
                return _contentDirectoryInfo;
            }
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        public MainWindowViewModel() {
        }

        /// <summary>
        /// Directory button click handler.
        /// </summary>
        public void GetDirectory() {
            FolderBrowserDialog openDialog = new FolderBrowserDialog();
            DialogResult result = openDialog.ShowDialog();
            if(result == DialogResult.OK) {
                _sourceDirectory = openDialog.SelectedPath;
                List<string> files =
                    Directory
                    .GetFiles(_sourceDirectory, "*.*", SearchOption.AllDirectories)
                    .Where(file => SupportedExtensionList.Contains(Path.GetExtension(file)))
                    .ToList();
                files.ForEach(file => SourceCollection.Add(file));
            }
        }

        /// <summary>
        /// Prepares all the files for processing.
        /// </summary>
        public void TransferAll() {
            FolderBrowserDialog openDialog = new FolderBrowserDialog();
            DialogResult result = openDialog.ShowDialog();
            if(result == DialogResult.OK) {
                _contentDirectoryName = openDialog.SelectedPath;
                foreach(string file in SourceCollection) {
                    string fileCopy = CopyFile(file);
                    /* TODO: Change to asynch. */
                    ResultCollection.Add(fileCopy);
                }
                SourceCollection.Clear();
            }
        }

        /// <summary>
        /// Copies the file creating a directory to stock the related information.
        /// </summary>
        /// <param name="file">File to copy.</param>
        /// <returns>Destination file path.</returns>
        private string CopyFile(string file) {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
            DirectoryInfo fileDirectoryInfo = ContentDirectoryInfo.CreateSubdirectory(fileNameWithoutExtension);
            string newFilePath = Path.Combine(fileDirectoryInfo.FullName, Path.GetFileName(file));
            if(!File.Exists(newFilePath)) {
                File.Copy(file, newFilePath);
            }
            return newFilePath;
        }
    }
}