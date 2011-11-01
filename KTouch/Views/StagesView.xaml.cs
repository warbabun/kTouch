using System.Windows;
using KTouch.Controls;
using KTouch.Controls.ViewModel;
using KTouch.Controls.Core;

namespace KTouch {
    /// <summary>
    /// Interaction logic for InternshipPage.xaml
    /// </summary>
    public partial class StagesPage : kPage {

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
