using System.Windows.Controls;
using KTouch.ViewModel;

namespace KTouch {

    /// <summary>
    /// Encapsulates a front page of content that can be navigated to and hosted in kBrowser.
    /// </summary>
    public partial class FrontPage : Page {

        private readonly FrontPageViewModel _vm;

        /// <summary>
        /// Constructor.
        /// </summary>
        public FrontPage() {
            InitializeComponent();
            _vm = new FrontPageViewModel();
            this.DataContext = _vm;
        }
    }
}
