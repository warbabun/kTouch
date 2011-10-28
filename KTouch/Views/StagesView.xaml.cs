using System.Windows;
using KTouch.Controls;
using KTouch.Controls.ViewModel;

namespace KTouch {
    /// <summary>
    /// Interaction logic for InternshipPage.xaml
    /// </summary>
    public partial class StagesPage : KTouchPage {

        KTouchExtendedViewModel vm = null;

        public StagesPage ( ) {
            InitializeComponent ( );

        }

        private void KTouchPage_Loaded ( object sender, RoutedEventArgs e ) {
            vm = new KTouchExtendedViewModel ( "Stages" );
            this.DataContext = vm;
        }

        private void KTouchPage_Unloaded ( object sender, RoutedEventArgs e ) {
            vm = null;
        }
    }
}
