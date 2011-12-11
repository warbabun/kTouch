using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml.Linq;

namespace KTouch.Utilities {

    /// <summary>
    /// Class designed to return an item template based on its type.
    /// </summary>
    public partial class ItemTemplateSelector : DataTemplateSelector {

        private static DataTemplate _folderTemplate;
        private DataTemplate _itemTemplate;

        /// <summary>
        /// Lazy load of folder template.
        /// </summary>
        protected DataTemplate FolderTemplate {
            get {
                if(_folderTemplate == null) {
                    _folderTemplate = MakeFolderTemplate();
                }
                return _folderTemplate;
            }
        }

        /// <summary>
        /// Lazy load of item template.
        /// </summary>
        protected DataTemplate ItemTemplate {
            get {
                if(_itemTemplate == null) {
                    _itemTemplate = MakeItemTemplate();
                }
                return _itemTemplate;
            }
        }

        /// <summary>
        /// Returns a System.Windows.DataTemplate based on custom logic.
        /// </summary>
        /// <param name="data">The data object for which to select the templat</param>
        /// <param name="container">The data-bound object.</param>
        /// <returns>Returns a System.Windows.DataTemplate or null. The default value is null.</returns>
        public override DataTemplate SelectTemplate(object data, DependencyObject container) {
            XElement item = data as XElement;
            if(item == null) {
                return null;
            } else if("dir".Equals((string)item.Attribute("Type"))) {
                return FolderTemplate;
            } else {
                return ItemTemplate;
            }
        }

        #region Templates

        /// <summary>
        /// Generates a folder template.
        /// </summary>
        /// <returns>DataTemplate for a folder.</returns>
        private DataTemplate MakeFolderTemplate() {
            XNamespace _xmlns = @"http://schemas.microsoft.com/winfx/2006/xaml/presentation";
            XElement templ =
                new XElement(_xmlns + "DataTemplate",
                    new XAttribute("xmlns", _xmlns),
                        new XElement(_xmlns + "TextBlock",
                            new XAttribute("TextAlignment", "Center"),
                            new XAttribute("TextWrapping", "WrapWithOverflow"),
                            new XAttribute("Text", "{Binding Path=Attribute[Name].Value}")));
            DataTemplate dt = (DataTemplate)XamlReader.Load(templ.CreateReader());
            return dt;
        }

        /// <summary>
        /// Generates an item template.
        /// </summary>
        /// <returns>DataTemplate for an item.</returns>
        private DataTemplate MakeItemTemplate() {
            XNamespace _xmlns = @"http://schemas.microsoft.com/winfx/2006/xaml/presentation";
            XElement templ =
                new XElement(_xmlns + "DataTemplate", new XAttribute("xmlns", _xmlns),
                    new XElement(_xmlns + "Image", new XAttribute("Stretch", "UniformToFill"), new XAttribute("Source", "{Binding Path=Attribute[Thumbnail].Value}")));
            DataTemplate dt = (DataTemplate)XamlReader.Load(templ.CreateReader());
            return dt;
        }

        #endregion //Templates
    }
}