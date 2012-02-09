using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using KTouch.ViewModel;
using Microsoft.Surface.Presentation.Input;

namespace KTouch.Views {

    /// <summary>
    /// Interaction logic for PresentationPage.xaml
    /// </summary>
    public partial class PresentationPage : Page {

        private PresentationPageViewModel _vm;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="item">XElement object.</param>
        public PresentationPage(XElement item) {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PresentationPage_Loaded);
            _vm = new PresentationPageViewModel(item);
            this.DataContext = _vm;
        }

        /// <summary>
        /// Handles Loaded event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        void PresentationPage_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            TouchExtensions.AddTapGestureHandler(player, new EventHandler<TouchEventArgs>(OnTapGesture));
        }

        /// <summary>
        /// Handles Tap event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        protected void OnTapGesture(object sender, TouchEventArgs e) {
            _vm.Next();
            player.xpsViewer.Zoom = 66;
            e.Handled = true;
        }
    }
}
