using System;
using System.Windows;
using System.Windows.Controls;
using KTouch.Controls.ViewModel;
using Microsoft.Surface.Presentation.Controls;

namespace KTouch {
    /// <summary>
    /// Interaction logic for ImportFiles.xaml
    /// </summary>
    public partial class ImportFiles : Page {

        private readonly ImportFilesViewModel _viewModel = null;

        public ImportFiles() {
            InitializeComponent();
            _viewModel = new ImportFilesViewModel();
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
            if (KTouch.Properties.kTouchResources.LblDirectory.Equals(button.Content)) {
                _viewModel.GetDirectory();
            } else if (KTouch.Properties.kTouchResources.LblTransfer.Equals(button.Content)) {
                _viewModel.TransferAll();
            }
        }
    }
}
