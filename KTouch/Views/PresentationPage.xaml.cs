//-----------------------------------------------------------------------
// <copyright file="PresentationPage.xaml.cs" company="Klee Group">
//     Copyright (c) Klee Group. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
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
        /// Initializes a new instance of the PresentationPage class.
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
            Mouse.AddPreviewMouseUpHandler(this.player, new MouseButtonEventHandler(player_MouseLeftButtonUp));
        }

        /// <summary>
        /// Handles PreviewMouseLeftButtonUp event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        private void player_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            _vm.Next();
        }

        /// <summary>
        /// Handles Tap event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        private void OnTapGesture(object sender, TouchEventArgs e) {
            _vm.Next();
        }
    }
}
