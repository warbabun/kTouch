using System.Windows;
using System.Windows.Input;
using KTouch.Units;
using KTouch.Views;

namespace KTouch {

    /// <summary>
    /// Represents a window that supports content navigation.
    /// </summary>
    public partial class kBrowser : Window {

        /// <summary>
        /// Constructor.
        /// </summary>
        public kBrowser() {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(kBrowser_Loaded);
        }

        /// <summary>
        /// Navigates the 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Navigate(object sender, TouchEventArgs e) {
            object source = ((FrameworkElement)e.OriginalSource).DataContext;
            if (source is Item) {
                Item item = (Item)source;
                if ("dir".Equals(item.Type)) {
                    _mainFrame.NavigationService.Navigate(new MainPage((Item)item));
                } else {
                    _mainFrame.NavigationService.Navigate(new PresentationPage((Item)item));
                }
            }
            e.Handled = true;
        }

        /// <summary>
        /// Handles Loaded event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        void kBrowser_Loaded(object sender, RoutedEventArgs e) {
            _mainFrame.NavigationService.Navigate(new MainPage());
        }

        private void SurfaceButton_Click(object sender, RoutedEventArgs e) {
            _mainFrame.NavigationService.Navigate(new MainPage());
        }
    }
}
