using System;
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
            object item = ((FrameworkElement)e.OriginalSource).DataContext;
            //SurfaceListBox listBox = (SurfaceListBox)e.Source;
            //object item = listBox.SelectedItem;
            //kItem item = (kItem);
            if(item is kItem) {
                _mainFrame.NavigationService.Navigate(new PresentationPage((kItem)item));
            } else if(item is string) {
                _mainFrame.NavigationService.Navigate(new MainPage((string)item));
            }
            e.Handled = true;
        }

        /// <summary>
        /// Handles Loaded event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        void kBrowser_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            _mainFrame.NavigationService.Navigate(new Uri("/Views/MainPage.xaml", UriKind.Relative));
        }
    }
}
