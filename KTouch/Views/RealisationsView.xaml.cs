using KTouch.Controls.Core;
using KTouch.Controls.ViewModel;

namespace KTouch {
    /// <summary>
    /// Interaction logic for ClientsTESTPage.xaml
    /// </summary>
    public partial class RealisationsPage : kPage {

        /// <summary>
        /// Constructeur.
        /// </summary>
        public RealisationsPage ( )
            : base ( ) {
            InitializeComponent ( );
            this.DataContext = new KTouchMainViewModel ( "Realisations" );
        }
    }
}
