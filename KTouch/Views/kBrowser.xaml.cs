using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using KTouch.Controls.ViewModel;
using KTouch.Units;
using KTouch.Views;
using Microsoft.Surface.Presentation.Controls;
using Blake.NUI.WPF.Gestures;
using System.Windows.Navigation;
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
            Events.RegisterGestureEventSupport(this);
            Events.AddTapGestureHandler(_mainFrame, new GestureEventHandler(OnTapGesture));
            Events.AddTapGestureHandler(navigationListBox, new GestureEventHandler(OnTapGesture));
            _vm = new BrowserViewModel(_mainFrame.NavigationService);
            BrowserViewModel.Navigate += new BrowserViewModel.NavigateEventHandler(BrowserViewModel_Navigate);
            this.DataContext = _vm;
        }

        void BrowserViewModel_Navigate(Item item) {
            if(item == null) {
                _mainFrame.NavigationService.Navigate(new FrontPage());
            } else {
                if("dir".Equals(item.Type)) {
                    _mainFrame.NavigationService.Navigate(new ListPage((Item)item));
                } else {
                    _mainFrame.NavigationService.Navigate(new PresentationPage((Item)item));
                }
                this.navigationListBox.SelectedItem = item;
                _mainFrame.NavigationUIVisibility = NavigationUIVisibility.Visible;
            }
        }

        protected void OnTapGesture(object sender, GestureEventArgs e) {
            if(e.OriginalSource is Canvas) {
                navigationListBox.SelectedIndex++;
                int next = navigationListBox.SelectedIndex;
                _mainFrame.NavigationService.Navigate(new PresentationPage((Item)navigationListBox.Items[next]));
                e.Handled = false;
                return;
            }
            object source = ((FrameworkElement)e.OriginalSource).DataContext;
            _vm.ListBoxSelectionChanged(source);
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
            _mainFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

        }

        private void CloseClick(object sender, RoutedEventArgs e) {
            _vm.Close();
        }

        private void HomeClick(object sender, RoutedEventArgs e) {
            _vm.GoHome();
        }

    }
}
