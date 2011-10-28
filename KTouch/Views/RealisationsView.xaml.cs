using KTouch.Controls.Core;
using KTouch.Controls.ViewModel;

namespace KTouch {
    /// <summary>
    /// Interaction logic for ClientsTESTPage.xaml
    /// </summary>
    public partial class RealisationsPage : KTouchPage {

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
