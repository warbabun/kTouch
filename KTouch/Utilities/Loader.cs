using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace KTouch.Utilities {

    /// <summary>
    /// Classe dédiée au chargement des éléments .
    /// </summary>
    public class Loader {

        private readonly XElement _root;

        /// <summary>
        /// Returns the root element of the tree.
        /// </summary>
        public XElement Root {
            get {
                return _root;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="source">Source directory path.</param>
        public Loader(string source) {
            _root = this.WalkDirectoryTree(new DirectoryInfo(source));
        }

        /// <summary>
        /// Walks the root directory.
        /// </summary>
        /// <param name="root">DirectoryInfo object of the root folder.</param>
        /// <returns>Tree of XElement objects.</returns>
        private XElement WalkDirectoryTree(DirectoryInfo root) {
            XElement current =
                new XElement("Item",
                    new XAttribute("Name", root.Name),
                    new XAttribute("FullName", root.FullName),
                    new XAttribute("Type", "dir"),
                    new XAttribute("Parent", root.Parent.FullName));
            // First find all the subdirectories (exculing file only directories) under this directory.
            List<DirectoryInfo> directoryList = root
                .GetDirectories()
                .Where(d => d.GetDirectories().Any())
                .ToList();
            // Resursive call for each subdirectory.
            foreach(DirectoryInfo directory in directoryList) {
                current.Add(WalkDirectoryTree(directory));
            }
            // Now, process all the file directories directly under this folder.
            List<DirectoryInfo> fileFolderList = root.GetDirectories()
                .Except(directoryList)
                .ToList();
            foreach(DirectoryInfo fileDirectory in fileFolderList) {
                // Now process all the files directly under this folder.
                List<FileInfo> fileList = null;
                try {
                    fileList = fileDirectory
                        .GetFiles("*.*")
                        .Where(f => SupportedExtensions.SupportedExtensionList.Contains(f.Extension))
                        .ToList();
                } catch(UnauthorizedAccessException e) {
                    Console.WriteLine(e.Message);
                } catch(DirectoryNotFoundException e) {
                    Console.WriteLine(e.Message);
                }
                foreach(FileInfo file in fileList) {
                    current.Add(
                        new XElement("Item",
                            new XAttribute("Name", file.Directory.Name),
                            new XAttribute("FullName", file.FullName),
                            new XAttribute("Type", file.Extension),
                            new XAttribute("Thumbnail", GetFileThumbnail(file)),
                            new XAttribute("Parent", root.Parent.FullName),
                            new XAttribute("Description", "Add me!")
                        ));
                }
            }
            return current;
        }

        /// <summary>
        /// Looks for an existing thumbnail for a file and if missing/if necessary can create one.
        /// </summary>
        /// <param name="file">File that is processed.</param>
        /// <returns>Path of the thumbnail image.</returns>
        private string GetFileThumbnail(FileInfo file) {
            string thumbnail = string.Empty;
            ICollection<FileInfo> existingThumbs = file.Directory.GetFiles("*.*")
                .Where(f => SupportedExtensions.SupportedThumbnailExtensionList.Contains(f.Extension))
                .ToList();
            if(existingThumbs.Any()) {
                thumbnail = existingThumbs.First().FullName;
            } else {
                thumbnail = "/Resources/noimage.jpg";
                //thumbnail = ThumbnailCreator.CreateThumbnail(file.FullName);
            }
            return thumbnail;
        }

        #region Functions

        /// <summary>
        /// Loads the item by its full name.
        /// </summary>
        /// <param name="fullName">Attribute FullName.</param>
        /// <returns>XElement object.</returns>
        public XElement LoadItemByFullName(object fullName) {
            string value = (string)fullName;
            var collection = from e in _root.Descendants("Item")
                             where value.Equals((string)e.Attribute("FullName"))
                             select e;
            return collection.FirstOrDefault();
        }

        /// <summary>
        /// Loads the item by its name.
        /// </summary>
        /// <param name="name">Attribute Name.</param>
        /// <returns>XElement object.</returns>
        public XElement LoadItemByName(object name) {
            string value = (string)name;
            var collection = from e in _root.DescendantsAndSelf("Item")
                             where value.Equals((string)e.Attribute("Name"))
                             select e;
            return collection.FirstOrDefault();
        }

        /// <summary>
        /// Loads the list of item's descendants by item full name and of item type.
        /// </summary>
        /// <param name="fullName">Attribute FullName.</param>
        /// <param name="type">Attribute Type.</param>
        /// <returns>List of XElement objects.</returns>
        public IEnumerable<XElement> LoadItemDescendantListByFullNameAndType(object fullName, object type) {
            var collection = from e in this.LoadItemByName(fullName).Descendants()
                             where !string.Equals("Supprimer", (string)e.Parent.Attribute("Name"))
                                && string.Equals((string)type, (string)e.Attribute("Type"))
                             select e;
            return collection;
        }

        /// <summary>
        /// Sets the item Tag attibute value by the top folder name.
        /// </summary>
        public void SetItemTag() {
            var rootElements = this._root.Elements();
            foreach(var item in rootElements) {
                foreach(var descendant in item.DescendantsAndSelf()) {
                    descendant.SetAttributeValue("Tag", (string)item.Attribute("Name"));
                }
            }
        }

        #endregion //Functions
    }
}