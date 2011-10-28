using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;

namespace KTouchScreenSaver {
  public static class KTouchScreenSaverSettings {

        const string _settingsFile = "KTouchScreenSaverSettings.xml";

        static string _currentPath = null;

        public static string Path {
            get {
                if (_currentPath == null) {
                    return ConfigurationManager.AppSettings["VideoFile"];
                } else {
                    return _currentPath;
                }
            }
            set {
                if (value != null) {
                    _currentPath = value;
                }
            }
        }

        public static void SaveSettings(string path) {
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(string));

                FileStream fs = new FileStream(_settingsFile, FileMode.Create);
                TextWriter writer = new StreamWriter(fs, new UTF8Encoding());
                serializer.Serialize(writer, path);
                writer.Close();
            } catch { }
        }

        public static void LoadSettings() {
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(string));

                FileStream fs = new FileStream(_settingsFile, FileMode.Open);
                TextReader reader = new StreamReader(fs);
                Path = (string)serializer.Deserialize(reader);
            } catch (Exception ex){
                Console.WriteLine("Settings loading failed");
            }
        }

    }
}
