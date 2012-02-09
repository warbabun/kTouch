using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Xml.Linq;
using KTouch.Properties;
using KTouch.Utilities;
using KTouch.ViewModel;
using KTouch.Views;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
namespace KTouch {

    /// <summary>
    /// Represents a window that supports content navigation.
    /// </summary>
    public partial class Browser : SurfaceWindow {

        private readonly BrowserViewModel _vm;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Browser() {
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
            _mainFrame.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(_mainFrame_PreviewMouseLeftButtonUp);
            navigationListBox.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(_mainFrame_PreviewMouseLeftButtonUp);
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
            if (e.Content is FrontPage) {
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
            this.HandleEvent(((FrameworkElement)e.OriginalSource).DataContext);
        }

        /// <summary>
        /// Handles mouse click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _mainFrame_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            this.HandleEvent(((FrameworkElement)e.OriginalSource).DataContext);
        }

        /// <summary>
        /// Handles navigation.
        /// </summary>
        /// <param name="dataSource">Collection item.</param>
        private void HandleEvent(object dataSource) {
            if (dataSource != null) {
                XElement item = dataSource as XElement;
                if (item == null) {
                    return;
                } else {
                    string currentSelection = (string)item.Attribute(Tags.FullName);
                    if (_vm.CurrentTitle != currentSelection) {
                        if (string.Equals(SupportedExtensions.DIR, (string)item.Attribute(Tags.Type))) {
                            _mainFrame.NavigationService.Navigate(new ListPage(item));
                        } else if (SupportedExtensions.SupportedMediaExtensionList.Contains((string)item.Attribute(Tags.Type))) {
                            _mainFrame.NavigationService.Navigate(new VideoPage(item));
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
