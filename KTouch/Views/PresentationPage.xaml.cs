using System.Windows.Controls;
using KTouch.Controls.ViewModel;
using KTouch.Units;

namespace KTouch.Views {
    /// <summary>
    /// Interaction logic for PresentationPage.xaml
    /// </summary>
    public partial class PresentationPage : Page {

        public PresentationPage(kItem item) {
            InitializeComponent();
            this.DataContext = new PresentationPageViewModel(item);
        }
    }
}
