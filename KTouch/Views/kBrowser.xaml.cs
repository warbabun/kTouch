using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using KTouch.Units;
using KTouch.Views;

namespace KTouch {

    /// <summary>
    /// Represents a window that supports content navigation.
    /// </summary>
    public partial class kBrowser : Window {

        /// <summary>
        /// Timer.
        /// </summary>
        private readonly DispatcherTimer _dispatcherTimer;

        /// <summary>
        /// Returns 'true' if the object should be visible.
        /// </summary>
        public Visibility OverlayVisibility {
            get;
            set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public kBrowser() {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(kBrowser_Loaded);
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += new EventHandler(_dispatcherTimer_Tick);
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
            _dispatcherTimer.Start();
            this.OverlayVisibility = Visibility.Visible;
            //this.PreviewTouchDown += new EventHandler<TouchEventArgs>(Navigate);
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
        /// Handles the PreviewTouchDown event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kBrowser_PreviewTouchDown(object sender, TouchEventArgs e) {
            _dispatcherTimer.Stop();
            this.OverlayVisibility = Visibility.Hidden;
            _dispatcherTimer.Start();
        }

        /// <summary>
        /// Handles Loaded event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        void kBrowser_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            _mainFrame.NavigationService.Navigate(new Uri("/Views/MainPage.xaml", UriKind.Relative));
            // this._mainFrame.FindName("content");
            //((FrameworkElement)_mainFrame.Content).PreviewTouchDown += new EventHandler<TouchEventArgs>(Navigate);
        }

        /// <summary>
        /// If no activity is registered withing the next interval timer is stopped, element is hid.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        private void _dispatcherTimer_Tick(object sender, EventArgs e) {
            this.OverlayVisibility = Visibility.Collapsed;
            _dispatcherTimer.Stop();
        }
    }
}
