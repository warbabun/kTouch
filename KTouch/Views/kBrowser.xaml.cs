using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Blake.NUI.WPF.Gestures;
using KTouch.Controls.ViewModel;
using KTouch.Units;
using KTouch.Views;
using Microsoft.Surface.Presentation.Controls;
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

            _mainFrame.Navigated += new NavigatedEventHandler(_mainFrame_Navigated);

            this.Loaded += new RoutedEventHandler(kBrowser_Loaded);
            Events.RegisterGestureEventSupport(this);
            Events.AddTapGestureHandler(_mainFrame, new GestureEventHandler(OnTapGesture));
            Events.AddTapGestureHandler(navigationListBox, new GestureEventHandler(OnTapGesture));
            _vm = new BrowserViewModel(_mainFrame.NavigationService);
            BrowserViewModel.Navigate += new BrowserViewModel.NavigateEventHandler(BrowserViewModel_Navigate);
            this.DataContext = _vm;
        }

        void _mainFrame_Navigated(object sender, NavigationEventArgs e) {
            if (e.Content is FrontPage) {
                _mainFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
                this.navigationListBox.SelectedIndex = -1;
            } else {
                _mainFrame.NavigationUIVisibility = NavigationUIVisibility.Visible;
            }
        }

        void BrowserViewModel_Navigate(Item item) {
            if (item == null) {
                _mainFrame.NavigationService.Navigate(new FrontPage());
            } else {
                if ("dir".Equals(item.Type)) {
                    _mainFrame.NavigationService.Navigate(new ListPage((Item)item));
                } else {
                    _mainFrame.NavigationService.Navigate(new PresentationPage((Item)item));
                }
                this.navigationListBox.SelectedItem = item;
            }
        }

        protected void OnTapGesture(object sender, GestureEventArgs e) {
            object source = ((FrameworkElement)e.OriginalSource).DataContext;
            if (source != null) {
                this.BrowserViewModel_Navigate(source as Item);
            }
        }

        /// <summary>
        /// Navigates the 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Navigate(object sender, TouchEventArgs e) {

        }

        /// <summary>
        /// Handles Loaded event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        void kBrowser_Loaded(object sender, RoutedEventArgs e) {
            _mainFrame.NavigationService.Navigate(new FrontPage());
        }
    }
}
