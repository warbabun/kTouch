using System;
using System.Windows;

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
        /// Handles Loaded event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void kBrowser_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            _mainFrame.NavigationService.Navigate(new Uri("/Views/MainPage.xaml", UriKind.Relative));
        }
    }
}
