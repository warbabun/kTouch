using System.Windows.Controls;
using System.Xml.Linq;
using KTouch.ViewModel;

namespace KTouch.Views {

    /// <summary>
    /// Interaction logic for PresentationPage.xaml
    /// </summary>
    public partial class VideoPage : Page {

        private VideoPageViewModel _vm;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="item">XElement object.</param>
        public VideoPage(XElement item) {
            InitializeComponent();
            _vm = new VideoPageViewModel(item);
            this.DataContext = _vm;
        }
    }
}
