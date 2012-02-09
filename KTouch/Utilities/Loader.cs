using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using KTouch.Properties;

namespace KTouch.Utilities {

    /// <summary>
    /// Classe dédiée au chargement des éléments .
    /// </summary>
    public class Loader {

        private readonly XElement _root;

        /// <summary>
        /// Gets the default source directory to take information from.
        /// </summary>
        protected string SourceDirectory {
            get {
                return ConfigurationManager.AppSettings["SourceDirectory"].ToString();
            }
        }

        /// <summary>
        /// Returns true if needed to create thumbs for the content.
        /// </summary>
        protected bool IsCreateMissingThumbs {
            get {
                return string.Equals("true", ConfigurationManager.AppSettings["IsCreateMissingThumbs"].ToString());
            }
        }

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
        public Loader() {
            _root = this.WalkDirectoryTree(new DirectoryInfo(SourceDirectory));
        }

        /// <summary>
        /// Walks the root directory.
        /// </summary>
        /// <param name="root">DirectoryInfo object of the root folder.</param>
        /// <returns>Tree of XElement objects.</returns>
        private XElement WalkDirectoryTree(DirectoryInfo root) {
            XElement current =
                new XElement(Tags.Item,
                    new XAttribute(Tags.Name, root.Name),
                    new XAttribute(Tags.FullName, root.FullName),
                    new XAttribute(Tags.Type, SupportedExtensions.DIR),
                    new XAttribute(Tags.Parent, root.Parent.FullName));

            // First find all the subdirectories (exculing file only directories) under this directory.
            List<DirectoryInfo> directoryList = root
                .GetDirectories()
                .Where(d => d.GetDirectories().Any())
                .ToList();

            // Resursive call for each subdirectory.
            foreach (DirectoryInfo directory in directoryList) {
                current.Add(WalkDirectoryTree(directory));
            }

            // Now, process all the file directories directly under this folder.
            List<DirectoryInfo> fileFolderList = root.GetDirectories()
                .Except(directoryList)
                .ToList();

            // Now process all the files directly under this folder.
            foreach (DirectoryInfo fileDirectory in fileFolderList) {
                List<FileInfo> fileList = null;
                try {
                    fileList = fileDirectory
                        .GetFiles("*.*")
                        .Where(f => SupportedExtensions.SupportedExtensionList.Contains(f.Extension))
                        .ToList();
                } catch (UnauthorizedAccessException e) {
                    Console.WriteLine(e.Message);
                } catch (DirectoryNotFoundException e) {
                    Console.WriteLine(e.Message);
                }
                foreach (FileInfo file in fileList) {
                    current.Add(
                        new XElement(Tags.Item,
                            new XAttribute(Tags.Name, file.Directory.Name),
                            new XAttribute(Tags.FullName, file.FullName),
                            new XAttribute(Tags.Type, file.Extension),
                            new XAttribute(Tags.Thumb, GetFileThumbnail(file)),
                            new XAttribute(Tags.Parent, root.Parent.FullName),
                            new XAttribute(Tags.Desc, "Add me!")
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
            if (existingThumbs.Any()) {
                thumbnail = existingThumbs.First().FullName;
            } else if (IsCreateMissingThumbs) {
                thumbnail = ThumbnailCreator.CreateThumbnail(file.FullName);
            } else {
                thumbnail = Resources.NoImage;
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
            var collection = from e in _root.Descendants(Tags.Item)
                             where value.Equals((string)e.Attribute(Tags.FullName))
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
            var collection = from e in _root.DescendantsAndSelf(Tags.Item)
                             where value.Equals((string)e.Attribute(Tags.Name))
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
                             where !string.Equals("Supprimer", (string)e.Parent.Attribute(Tags.Name))
                                && string.Equals((string)type, (string)e.Attribute(Tags.Type))
                             select e;
            return collection;
        }

        /// <summary>
        /// Sets the item Tag attibute value by the top folder name.
        /// </summary>
        public void SetItemTag() {
            var rootElements = this._root.Elements();
            foreach (var item in rootElements) {
                foreach (var descendant in item.DescendantsAndSelf()) {
                    descendant.SetAttributeValue(Tags.Tag, (string)item.Attribute(Tags.Name));
                }
            }
        }

        #endregion //Functions
    }
}