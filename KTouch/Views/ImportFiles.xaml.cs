//-----------------------------------------------------------------------
// <copyright file="ImportFiles.xaml.cs" company="Klee Group">
//     Copyright (c) Klee Group. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using KTouch.ViewModel;
using Microsoft.Surface.Presentation.Controls;

namespace KTouch {
    /// <summary>
    /// Interaction logic for ImportFiles.xaml
    /// </summary>
    public partial class ImportFiles : Page {

        private readonly ImportFilesViewModel _viewModel = null;

        /// <summary>
        /// Initializes a new instance of the ImportFiles class.
        /// </summary>
        public ImportFiles() {
            InitializeComponent();
            _viewModel = new ImportFilesViewModel();
            this.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(BtnAction_Click));
            this.DataContext = _viewModel;
        }

        /// <summary>
        /// Handles button click event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arg.</param>
        private void BtnAction_Click(object sender, RoutedEventArgs e) {
            if (sender as SurfaceButton == null) {
                throw new ArgumentNullException("sender");
            }
            SurfaceButton button = (SurfaceButton)sender;
            if (KTouch.Properties.Resources.LblDirectory.Equals(button.Content)) {
                _viewModel.GetDirectory();
            } else if (KTouch.Properties.Resources.LblTransfer.Equals(button.Content)) {
                _viewModel.TransferAll();
            }
        }
    }
}
