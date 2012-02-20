using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private void PresentationPage_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            TouchExtensions.AddTapGestureHandler(this.player, new EventHandler<TouchEventArgs>(OnTapGesture));
            this.player.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(player_MouseLeftButtonUp);
            //this.player.AddHandler(ButtonBase.ClickEvent, new MouseButtonEventHandler(player_MouseLeftButtonUp));
            this.player.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(OnMouseClick));
        }

        /// <summary>
        /// Handles PreviewMouseLeftButtonUp event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        private void player_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            _vm.Next();
            e.Handled = true;
        }

        /// <summary>
        /// Handles Tap event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        private void OnTapGesture(object sender, TouchEventArgs e) {
            _vm.Next();
            e.Handled = true;
        }

        /// <summary>
        /// Handles mouse click event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        private void OnMouseClick(object sender, RoutedEventArgs e) {
            _vm.Next();
            e.Handled = true;
        }
    }
}
