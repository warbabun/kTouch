
using System.ComponentModel;
namespace KTouch.Units {

    /// <summary>
    /// Classe représentant un élément présenté dans l'application.
    /// </summary>
    public class kItem : INotifyPropertyChanged {

        private int _id;
        private string _tag = string.Empty;
        private string _directory = string.Empty;
        private string _coverFile = string.Empty;
        private string _title = string.Empty;
        private string _description = string.Empty;
        private string _type = string.Empty;
        private string _file = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info) {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        /// <summary>
        /// Technical identifier of the object.
        /// </summary>
        public int Id {
            get {
                return this._id;
            }
            set {
                if(value != this._id) {
                    this._id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }


        /// <summary>
        /// Tag of the collection.
        /// </summary>
        public string Tag {
            get {
                return this._tag;
            }
            set {
                if(value != this._tag) {
                    this._tag = value;
                    NotifyPropertyChanged("Tag");
                }
            }
        }

        /// <summary>
        /// Chemin vers le dossier de contenu.
        /// </summary>
        public string Directory {
            get {
                return this._directory;
            }
            set {
                if(value != this._directory) {
                    this._directory = value;
                    NotifyPropertyChanged("Directory");
                }
            }
        }

        /// <summary>
        /// Chemin vers l'image de présentation.
        /// </summary>
        public string CoverFile {
            get {
                return this._coverFile;
            }
            set {
                if(value != this._coverFile) {
                    this._coverFile = value;
                    NotifyPropertyChanged("CoverFile");
                }
            }
        }

        /// <summary>
        /// Titre de l'élément.
        /// </summary>
        public string Title {
            get {
                return this._title;
            }
            set {
                if(value != this._title) {
                    this._title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        /// <summary>
        /// Description de l'élément.
        /// </summary>
        public string Description {
            get {
                return this._description;
            }
            set {
                if(value != this._description) {
                    this._description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        /// <summary>
        /// Fichier principal de présentation.
        /// </summary>
        public string File {
            get {
                return this._file;
            }
            set {
                if(value != this._file) {
                    this._file = value;
                    NotifyPropertyChanged("File");
                }
            }
        }

        /// <summary>
        /// Type du contenu.
        /// </summary>
        public string Type {
            get {
                return this._type;
            }
            set {
                if(value != this._type) {
                    this._type = value;
                    NotifyPropertyChanged("Type");
                }
            }
        }

        /// <summary>
        /// Override ToString() method.
        /// </summary>
        /// <returns>Titre de l'élément</returns>
        public override string ToString() {
            return _title;
        }

        public override bool Equals(object obj) {
            return obj is kItem ? this._id == ((kItem)obj)._id : false;
        }
    }
}
