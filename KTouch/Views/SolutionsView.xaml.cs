using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using KTouch.Controls;
using KTouch.Controls.ViewModel;
using KTouch.Controls.Core;



namespace KTouch {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SolutionsPage : kPage {

        /// <summary>
        /// Constructeur.
        /// </summary>
        public SolutionsPage(){
            InitializeComponent();
            this.DataContext = new KTouchMainViewModel("Solutions"); 
            
        }
    } 
}

