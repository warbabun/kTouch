using System.Windows.Controls;
using System.Xml.Linq;
using KTouch.ViewModel;

namespace KTouch {

    /// <summary>
    /// Encapsulates a main page of content that can be navigated to and hosted in kBrowser.
    /// </summary>
    public partial class ListPage : Page {

        private BaseViewModel _vm;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ListPage(XElement item) {
            InitializeComponent();
            _vm = new BaseViewModel(item);
            DataContext = _vm;
        }
    }
}
