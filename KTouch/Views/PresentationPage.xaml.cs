using System;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KTouch.Views {
    /// <summary>
    /// Interaction logic for PresentationPage.xaml
    /// </summary>
    public partial class PresentationPage : Page {
        public PresentationPage() {
            InitializeComponent();
            this.NavigationService.Navigated += new NavigatedEventHandler(NavigationService_Navigated);
        }

        void NavigationService_Navigated(object sender, NavigationEventArgs e) {
            Console.WriteLine("hello page!");
            //e.Uri.Query.T
            //if(e.Uri.TryGetValue("msg", out msg))

            //    textBlock1.Text = msg;

        }
    }
}
