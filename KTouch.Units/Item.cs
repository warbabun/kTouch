
using System.ComponentModel;
namespace KTouch.Units {

    /// <summary>
    /// Classe représentant un élément présenté dans l'application.
    /// </summary>
    public class Item : INotifyPropertyChanged {

        private string _tag = string.Empty;
        private string _directory = string.Empty;
        private string _coverFile = string.Empty;
        private string _name = string.Empty;
        private string _description = string.Empty;
        private string _type = string.Empty;
        private string _fullName = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
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
                if (value != this._tag) {
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
                if (value != this._directory) {
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
                if (value != this._coverFile) {
                    this._coverFile = value;
                    NotifyPropertyChanged("CoverFile");
                }
            }
        }

        /// <summary>
        /// Titre de l'élément.
        /// </summary>
        public string Name {
            get {
                return this._name;
            }
            set {
                if (value != this._name) {
                    this._name = value;
                    NotifyPropertyChanged("Name");
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
                if (value != this._description) {
                    this._description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        /// <summary>
        /// Fichier principal de présentation.
        /// </summary>
        public string FullName {
            get {
                return this._fullName;
            }
            set {
                if (value != this._fullName) {
                    this._fullName = value;
                    NotifyPropertyChanged("FullName");
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
                if (value != this._type) {
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
            return _name;
        }

        public override bool Equals(object obj) {
            return obj is Item ? this._fullName == ((Item)obj)._fullName : false;
        }
    }
}
