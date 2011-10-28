using System;
using System.Configuration;
using System.Windows;
using System.Windows.Input;

namespace KTouchScreenSaver {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window {

        public Window1() {
            InitializeComponent();

            // Read in settings from .xml file
            KTouchScreenSaverSettings.LoadSettings();
            
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            browser.Stop();
            Application.Current.Shutdown();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            browser.Stop();
            Application.Current.Shutdown();
        }

        private void browser_Loaded(object sender, RoutedEventArgs e) {
            try {
                browser.Source = new Uri(KTouchScreenSaverSettings.Path, UriKind.Relative);
                browser.Play();
            } catch (Exception ex) {
                Console.WriteLine("UNHANDLED EXCEPTION: {0}", ex.Message);
                browser.Source = new Uri(ConfigurationManager.AppSettings["VideoFile"], UriKind.Relative);
            }
        }

        private void browser_MediaEnded(object sender, RoutedEventArgs e) {
            browser.Stop();
            browser.Play();
        }

        private void browser_MediaOpened(object sender, RoutedEventArgs e) {
            browser.Play();
        }
    }
}
