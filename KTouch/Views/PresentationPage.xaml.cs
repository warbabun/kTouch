using System;
using System.Windows.Controls;
using System.Windows.Input;
using KTouch.Controls.ViewModel;
using KTouch.Units;

namespace KTouch.Views {
    /// <summary>
    /// Interaction logic for PresentationPage.xaml
    /// </summary>
    public partial class PresentationPage : Page {

        private PresentationPageViewModel _vm;

        public PresentationPage(kItem item) {
            InitializeComponent();
            _vm = new PresentationPageViewModel(item);
            this.DataContext = _vm;
            this.PreviewTouchDown += new EventHandler<TouchEventArgs>(kBrowser_PreviewTouchDown);
        }

        /// <summary>
        /// Handles the PreviewTouchDown event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kBrowser_PreviewTouchDown(object sender, TouchEventArgs e) {
            _vm.RestartTimer();
        }
    }
}
