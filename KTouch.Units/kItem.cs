
namespace KTouch.Units {

    /// <summary>
    /// Classe représentant un élément présenté dans l'application.
    /// </summary>
    public class kItem {

        /// <summary>
        /// Technical identifier of the object.
        /// </summary>
        public int Id {
            get;
            set;
        }

        /// <summary>
        /// Chemin vers le dossier de contenu.
        /// </summary>
        public string Directory {
            get;
            set;
        }

        /// <summary>
        /// Chemin vers l'image de présentation.
        /// </summary>
        public string CoverFile {
            get;
            set;
        }

        /// <summary>
        /// Titre de l'élément.
        /// </summary>
        public string Title {
            get;
            set;
        }

        /// <summary>
        /// Description de l'élément.
        /// </summary>
        public string Description {
            get;
            set;
        }

        /// <summary>
        /// Fichier principal de présentation.
        /// </summary>
        public string File {
            get;
            set;
        }

        /// <summary>
        /// Type du contenu.
        /// </summary>
        public string Type {
            get;
            set;
        }

        /// <summary>
        /// Override ToString() method.
        /// </summary>
        /// <returns>Titre de l'élément</returns>
        public override string ToString() {
            return Title;
        }
    }
}
