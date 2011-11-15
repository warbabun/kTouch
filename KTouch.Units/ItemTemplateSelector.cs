using System.Windows;
using System.Windows.Controls;
using Microsoft.Surface.Presentation.Controls;


namespace KTouch.Units {

    public partial class ItemTemplateSelector : DataTemplateSelector {

        public override DataTemplate SelectTemplate(object item, DependencyObject container) {
            base.SelectTemplate(item, container);

            if(container == null || item == null)
                return null;
            else {
                ScatterView scatterView = StaticAccessors.FindAncestor(typeof(ScatterView), container) as ScatterView;
                try {
                    if(((Item)item).Type.Equals("vid"))
                        return scatterView.FindResource("scatterViewVideoItemTemplate") as DataTemplate;
                    else
                        return scatterView.FindResource("scatterViewItemTemplate") as DataTemplate;
                } catch {
                    return null;
                }
            }
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