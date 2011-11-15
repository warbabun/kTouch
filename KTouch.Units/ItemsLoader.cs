using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;

namespace KTouch.Units {

    /// <summary>
    /// Classe dédiée au chargement des éléments du CoverFlow.
    /// </summary>
    public static class ItemsLoader {
        static Dispatcher _currentDispatcher = null;

        public static Dictionary<string, ObservableCollection<Item>> InputCollections = null;

        public delegate void EventHandler(Item obj);
        public delegate void PackEventHandler(List<Item> pack);

        public static event EventHandler FileArrived;
        public static event PackEventHandler FilePackArrived;

        /// <summary>
        /// Analyse des fichiers de déclaration.
        /// </summary>
        /// <returns>L'ensemble des éléments déclarés.</returns>
        public static IEnumerable<Item> LoadItems(string path) {
            List<Item> items = new List<Item>();
            foreach(string xmlFile in Directory.GetFiles(path, "*.ktouch.xml", SearchOption.AllDirectories)) {
                items.AddRange(LoadXmlDeclarationFile(xmlFile));
            }
            return items;
        }

        /// <summary>
        /// Charge unitairement un fichier de déclaration.
        /// </summary>
        /// <param name="fileName">Chemin du fichier XML de déclaration.</param>
        /// <returns>Liste des éléments déclarés.</returns>
        public static List<Item> LoadXmlDeclarationFile(string fileName) {

            List<Item> items = new List<Item>();
            XmlDocument document = new XmlDocument();
            document.Load(fileName);

            XmlNode root = document.DocumentElement;
            foreach(XmlNode node in root.ChildNodes) {
                string directory = (Directory.GetParent(fileName)).FullName + @"\" + node.Attributes["Directory"].Value;
                Item item = new Item {
                    Directory = directory,
                    CoverFile = directory + @"\" + node.Attributes["CoverFile"].Value,
                    FullName = directory + @"\" + node.Attributes["FullName"].Value,
                    Name = node.Attributes["Name"].Value,
                    Type = node.Attributes["Type"].Value
                };
                items.Add(item);
            }
            return items;
        }

        public static List<KTouchUniverse> LoadUniverses(string fileName) {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            List<KTouchUniverse> universes = new List<KTouchUniverse>();

            XmlNode root = doc.DocumentElement;
            foreach(XmlNode node in root.ChildNodes) {
                string directory = (Directory.GetParent(fileName)).FullName + @"\" + node.Attributes["Directory"].Value;
                KTouchUniverse universe = new KTouchUniverse {
                    Directory = directory,
                    File = directory + @"\" + node.Attributes["FullName"].Value,
                    Name = node.Attributes["Name"].Value,
                };
                universes.Add(universe);
            }
            return universes;
        }

        public static void LoadFileAsync(string fileName) {
            LoadFileAsync(fileName, 9);
        }

        public static void LoadFileAsync(string fileName, int packcount) {

            XmlDocument document = new XmlDocument();
            document.Load(fileName);

            XmlNode root = document.DocumentElement;
            var tempList = new List<Item>();
            foreach(XmlNode node in root.ChildNodes) {
                string directory = (Directory.GetParent(fileName)).FullName + @"\" + node.Attributes["Directory"].Value;
                Item item = new Item {
                    Directory = directory,
                    CoverFile = directory + @"\" + node.Attributes["CoverFile"].Value,
                    FullName = directory + @"\" + node.Attributes["FullName"].Value,
                    Name = node.Attributes["Name"].Value,
                    Type = node.Attributes["Type"].Value
                };
                if(tempList.Count < packcount && FilePackArrived != null)
                    tempList.Add(item);
                else if(FilePackArrived != null)
                    FilePackArrived(tempList);
                else if(FileArrived != null)
                    FileArrived(item);
            }
        }

        public static void LoadCollections(string path) {
            _currentDispatcher = Dispatcher.CurrentDispatcher;
            InputCollections = new Dictionary<string, ObservableCollection<Item>>();
            foreach(var universe in ItemsLoader.LoadUniverses(path)) {
                List<Item> items = ItemsLoader.LoadXmlDeclarationFile(universe.File);
                if(items != null) {
                    InputCollections[universe.Name] = new ObservableCollection<Item>(items);
                }
            }
        }
    }
    public class ResultEventArgs : EventArgs {
        public List<Item> Items { get; set; }

        public ResultEventArgs(IEnumerable<Item> items) {
            this.Items = new List<Item>(items);
        }
    }


    public class XMLParser {
        public delegate void EventHandler(object sender, ResultEventArgs e);

        public event EventHandler Completed;

        string directory;

        public void LoadXMLFile(string xmlURI) {
            WebClient xmlClient = new WebClient();
            directory = (Directory.GetParent(xmlURI)).FullName + @"\";
            xmlClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(XMLFileLoaded);
            xmlClient.DownloadStringAsync(new Uri(xmlURI, UriKind.RelativeOrAbsolute));
        }

        public void XMLFileLoaded(object sender, DownloadStringCompletedEventArgs e) {
            if(e.Error == null) {
                string xmlData = e.Result;
                if(xmlData != null) {
                    XDocument xdoc = XDocument.Parse(xmlData);
                    var items = from item in xdoc.Descendants("KTouchItem")
                                select new Item {
                                    Directory = directory + item.Attribute("Directory").Value,
                                    CoverFile = directory + item.Attribute("Directory").Value + @"/" + item.Attribute("CoverFile").Value,
                                    FullName = directory + item.Attribute("Directory").Value + @"/" + item.Attribute("FullName").Value,
                                    Name = item.Attribute("Name").Value,
                                    Type = item.Attribute("Type").Value,
                                };
                    if(Completed != null) {
                        Completed(this, new ResultEventArgs(items));
                    }
                }
            }
        }
    }
}
