using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using KTouch.Controls.Core;
using KTouch.Units;

namespace KTouch.Controls {
    /// <summary>
    /// Interaction logic for KTouchMenuControl.xaml
    /// </summary>
    public partial class KTouchMenuControl : UserControl {
        kPage _page;

        public KTouchMenuControl ( ) {
            this.InitializeComponent ( );
            this.pathListBox.AddHandler ( ButtonBase.ClickEvent, new RoutedEventHandler ( ( sender, e ) => {
                Uri newPage = new Uri ( PageControl.PageDictionnary [ ( ( Button ) e.OriginalSource ).Tag.ToString ( ) ], UriKind.Relative );
                try {
                    if ( _page.NavigationService.CurrentSource != newPage )
                        _page.NavigationService.Navigate ( newPage );
                    else
                        _page.NavigationService.Refresh ( );
                } catch {

                }

            } ) );

            this.pathListBox.AddHandler ( ButtonBase.PreviewMouseRightButtonDownEvent, new RoutedEventHandler ( ( sender, e ) => {
                e.Handled = true;
            } ) );
            this.Loaded += ( sender, e ) => {
                _page = ( kPage ) StaticAccessors.FindAncestor ( typeof ( kPage ), this );
                switch ( _page.NavigationService.CurrentSource.ToString ( ) ) {
                    case "Views/FrontView.xaml":
                        pathListBox.SelectedIndex = 3;
                        break;
                    case "Views/SolutionsView.xaml":
                        pathListBox.SelectedIndex = 2;
                        break;
                    case "Views/RealisationsView.xaml":
                        pathListBox.SelectedIndex = 1;
                        break;
                    case "Views/StagesView.xaml":
                        pathListBox.SelectedIndex = 0;
                        break;
                    default:
                        throw new UriFormatException ( "Navigation failed. Universe not found" );
                }
            };
        }
    }
}