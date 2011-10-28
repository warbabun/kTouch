using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Blake.NUI.WPF.Gestures;
using System.Configuration;

namespace PageControlLib {
    /// <summary>
    /// Interaction logic for KTouchBande.xaml
    /// </summary>
    public partial class KTouchBande : KTouchStaticControl {

        /// <summary>
        /// Registring 'Header' property of KTouchBande
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
          DependencyProperty.Register("Header",
                                      typeof(string),
                                      typeof(KTouchBande),
                                      new PropertyMetadata("Header"));

        /// <summary>
        /// 'Header' property of KTouchBande
        /// </summary>
        public string Header {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected override void OnItemsSourceChanged(ObservableCollection<KTouchItem> oldValue, ObservableCollection<KTouchItem> newValue) {          
            this.listBox.ItemsSource = newValue;
        }

        /// <summary>
        /// Public constructor
        /// </summary>
        public KTouchBande() : base(){
            InitializeComponent();
            Events.RegisterGestureEventSupport(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTapGesture(object sender, GestureEventArgs e) {
            var item = (ListBoxItem)KTouchPage.FindAncestor(typeof(ListBoxItem), e.OriginalSource);
            if (item != null) {
                KTouchPage.ShowInViewer((KTouchItem)item.DataContext);
            } else {
                try {
                    Page.NavigationService.Navigate(new Uri(PageControl.PageDictionnary[Header], UriKind.Relative));
                } catch {
                    MessageBox.Show("Navigation failed");
                }
            }
            e.Handled = true;
        }
    }
}
