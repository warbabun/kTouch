using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml.Linq;
using Microsoft.Surface.Presentation.Controls;


namespace KTouch.Units {

    public partial class ItemTemplateSelector : DataTemplateSelector {

        public override DataTemplate SelectTemplate(object data, DependencyObject container) {
            if(data != null && data is Item) {
                Item item = data as Item;
                if("dir".Equals(item.Type)) {
                    return CollectionTemplate;
                } else {
                    return ItemTemplate;
                }
            } else {
                return null;
            }
        }

        private static DataTemplate _collectionTemplate;
        protected DataTemplate CollectionTemplate {
            get {
                if(_collectionTemplate == null) {
                    _collectionTemplate = MakeCollectionTemplate();
                }
                return _collectionTemplate;
            }
        }

        private DataTemplate _itemTemplate;
        protected DataTemplate ItemTemplate {
            get {
                if(_itemTemplate == null) {
                    _itemTemplate = MakeItemTemplate();
                }
                return _itemTemplate;
            }
        }

        private DataTemplate MakeCollectionTemplate() {
            XNamespace _xmlns = @"http://schemas.microsoft.com/winfx/2006/xaml/presentation";
            XElement templ =
                new XElement(_xmlns + "DataTemplate", new XAttribute("xmlns", _xmlns),
                    new XElement(_xmlns + "TextBlock", new XAttribute("Text", "{Binding Name}")));
            DataTemplate dt = (DataTemplate)XamlReader.Load(templ.CreateReader());
            return dt;
        }

        private DataTemplate MakeItemTemplate() {
            XNamespace _xmlns = @"http://schemas.microsoft.com/winfx/2006/xaml/presentation";
            XElement templ =
                new XElement(_xmlns + "DataTemplate", new XAttribute("xmlns", _xmlns),
                    new XElement(_xmlns + "Image", new XAttribute("Source", "{Binding CoverFile}")));
            DataTemplate dt = (DataTemplate)XamlReader.Load(templ.CreateReader());
            return dt;
        }
    }

    public partial class KTouchScatterViewItemStyleSelector : StyleSelector {

        public override Style SelectStyle(object item, DependencyObject container) {
            base.SelectStyle(item, container);

            if(container == null || item == null)
                return null;
            else {
                ScatterView scatterView = StaticAccessors.FindAncestor(typeof(ScatterView), container) as ScatterView;
                try {
                    if(((Item)item).Type.Equals("vid"))
                        return (Style)scatterView.FindResource("VidScatterViewItemStyle");
                    else
                        return (Style)scatterView.FindResource("XpsScatterViewItemStyle");
                } catch {
                    return null;
                }
            }
        }
    }

    public partial class KTouchBandeStyleSelector : StyleSelector {
        public override Style SelectStyle(object item, DependencyObject container) {
            base.SelectStyle(item, container);

            if(container == null || item == null)
                return null;
            else {
                var page = (Page)StaticAccessors.FindAncestor(typeof(Page), container);
                try {
                    if(((Item)item).Type.Equals("xps")) {
                        return (Style)page.FindResource("XpsListBoxItemStyle");
                    } else if(((Item)item).Type.Equals("vid")) {
                        return (Style)page.FindResource("VidListBoxItemStyle");
                    } else if(((Item)item).Type.Equals("pic")) {
                        return (Style)page.FindResource("PicListBoxItemStyle");
                    } else
                        return null;
                } catch {
                    return null;
                }
            }
        }
    }

}