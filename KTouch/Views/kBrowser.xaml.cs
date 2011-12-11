using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Xml.Linq;
using KTouch.ViewModel;
using KTouch.Views;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
namespace KTouch {

    /// <summary>
    /// Represents a window that supports content navigation.
    /// </summary>
    public partial class kBrowser : SurfaceWindow {

        private BrowserViewModel _vm;

        /// <summary>
        /// Constructor.
        /// </summary>
        public kBrowser() {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(kBrowser_Loaded);
            _vm = new BrowserViewModel();
            this.DataContext = _vm;
        }

        /// <summary>
        /// Handles Loaded event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        void kBrowser_Loaded(object sender, RoutedEventArgs e) {
            _mainFrame.Navigated += new NavigatedEventHandler(_mainFrame_Navigated);
            _mainFrame.NavigationService.Navigate(new FrontPage());
            TouchExtensions.AddTapGestureHandler(_mainFrame, new EventHandler<TouchEventArgs>(OnTapGesture));
            TouchExtensions.AddTapGestureHandler(navigationListBox, new EventHandler<TouchEventArgs>(OnTapGesture));
        }

        /// <summary>
        /// Handles Navigated event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        void _mainFrame_Navigated(object sender, NavigationEventArgs e) {
            if(e.Content is FrontPage) {
                _mainFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
                this.navigationListBox.SelectedIndex = -1;
            } else {
                _mainFrame.NavigationUIVisibility = NavigationUIVisibility.Visible;
                _vm.CurrentTitle = ((Page)e.Content).Title;
            }
        }

        /// <summary>
        /// Handles tap event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        protected void OnTapGesture(object sender, TouchEventArgs e) {
            object source = ((FrameworkElement)e.OriginalSource).DataContext;
            if(source != null) {
                XElement item = source as XElement;
                if(item == null) {
                    _mainFrame.NavigationService.Navigate(new FrontPage());
                } else {
                    string currentSelection = (string)item.Attribute("FullName");
                    if(_vm.CurrentTitle != currentSelection) {
                        if(string.Equals("dir", (string)item.Attribute("Type"))) {
                            _mainFrame.NavigationService.Navigate(new ListPage(item));
                        } else {
                            _mainFrame.NavigationService.Navigate(new PresentationPage(item));
                        }
                        _vm.CurrentTitle = currentSelection;
                    }
                }
            }
        }
    }
}
