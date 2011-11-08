using System;
using System.Windows;
using Microsoft.Surface.Presentation.Controls;

namespace UniversCreator {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private readonly MainWindowViewModel _viewModel = null;

        public MainWindow() {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            this.DataContext = _viewModel;
        }

        /// <summary>
        /// Handles button click event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arg.</param>
        private void BtnAction_Click(object sender, RoutedEventArgs e) {
            if(sender as SurfaceButton == null) {
                throw new ArgumentNullException("sender");
            }
            SurfaceButton button = (SurfaceButton)sender;
            if(UniversCreator.Properties.MainWindow.LblDirectory.Equals(button.Content)) {
                _viewModel.GetDirectory();
            } else if(UniversCreator.Properties.MainWindow.LblTransfer.Equals(button.Content)) {
                _viewModel.TransferAll();
            }
        }
    }
}
