using System.Windows.Controls;
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
        }
    }
}
