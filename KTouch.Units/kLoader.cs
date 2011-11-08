using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace KTouch.Units {

    /// <summary>
    /// Classe dédiée au chargement des éléments .
    /// </summary>
    public class kLoader {

        /// <summary>
        /// Creates and XDocument (XML) declaration file from the content of a directory.
        /// </summary>
        /// <param name="directory">Full path of the directory.</param>
        /// <returns>XDocument (XML) declaration file.</returns>
        public XDocument LoadXDocument(string directory) {
            Dictionary<string, string> dicoFileThumbnail = new Dictionary<string, string>();
            List<string> fileList = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories)
                    .Where(f => SupportedExtensions.SupportedExtensionList.Contains(Path.GetExtension(f)))
                    .ToList();
            foreach (string file in fileList) {
                string thumbnail = string.Empty;
                string parentDirectory = Directory.GetParent(file).FullName;
                ICollection<string> existingThumbs = Directory.GetFiles(parentDirectory, "*.*")
                    .Where(f => SupportedExtensions.SupportedThumbnailExtensionList.Contains(Path.GetExtension(f)))
                    .ToList();
                if (existingThumbs.Any()) {
                    thumbnail = Path.GetFileName(existingThumbs.FirstOrDefault());
                } else {
                    thumbnail = ThumbnailCreator.CreateThumbnail(file);
                }
                dicoFileThumbnail.Add(file, thumbnail);
            }
            XNamespace dir = new Uri(directory).ToString();
            XDocument kContent =
                new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("kTouch content declarations file."),
                    new XElement(dir + "kContent",
                        new XAttribute(XNamespace.Xmlns + "dir", dir),
                        from f in fileList
                        select new XElement(dir + "kItem",
                                    new XAttribute("FileName", Path.GetFileNameWithoutExtension(f)),
                                    new XAttribute("FileType", Path.GetExtension(f)),
                                    new XAttribute("Thumbnail", dicoFileThumbnail[f])
                        )
                    )
            );
            return kContent;
        }
    }
}
