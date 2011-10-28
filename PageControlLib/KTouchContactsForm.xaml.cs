using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using KTouch.Controls.Core;
using KTouch.Controls.ViewModel;
namespace KTouch.Controls {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class KTouchContactsForm : Popup {




        //public string Preferences {
        //    get { return (string)GetValue(PreferencesProperty); }
        //    set { SetValue(PreferencesProperty, value); }
        //}

        //public static readonly DependencyProperty PreferencesProperty =
        //    DependencyProperty.Register("Preferences", typeof(string), typeof(KTouchContactsForm), new FrameworkPropertyMetadata("", OnPreferencesChanged));

        //public static void OnPreferencesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        //    var form = (KTouchContactsForm)d;
        //    if (form.viewModel != null) {
        //        form.viewModel.Preferences = (string)e.NewValue;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        public KTouchContactsForm ( ) {
            InitializeComponent ( );
            EventManager.RegisterClassHandler ( typeof ( TextBox ),
                   TextBox.GotFocusEvent,
                   new RoutedEventHandler ( ( sender, args ) => {
                       ( ( TextBox ) sender ).SelectAll ( );
                   } ) );
            //this.CloseBtn.PreviewTouchUp += (sender, e) => {
            //    this.IsOpen = false;
            //    e.Handled = true;
            //};
            this.DataContextChanged += new DependencyPropertyChangedEventHandler ( KTouchContactsForm_DataContextChanged );
        }

        void KTouchContactsForm_DataContextChanged ( object sender, DependencyPropertyChangedEventArgs e ) {
            ( ( ContactViewModel ) this.DataContext ).IsOpen = true;
            this.IsOpen = true;
            Console.Write ( "aaa" );
        }



        public CustomPopupPlacement [ ] CustomPlacementCallback ( Size popupSize, Size targetSize, Point offset ) {
            CustomPopupPlacement placement1 = new CustomPopupPlacement ( new Point ( targetSize.Width / 2.0, targetSize.Height - popupSize.Height ), PopupPrimaryAxis.Vertical );
            CustomPopupPlacement placement2 = new CustomPopupPlacement ( new Point ( 10, 20 ), PopupPrimaryAxis.Horizontal );
            return new CustomPopupPlacement [ ] { placement1, placement2 };
        }

        protected override void OnClosed ( EventArgs e ) {
            var storyClose = ( Storyboard ) this.FindResource ( "CloseContacts" );
            if ( storyClose != null )
                storyClose.Begin ( this.LayoutRoot );
            if ( !KTouchPage.KBoard.HasExited )
                KTouchPage.KBoard.Kill ( );
            base.OnClosed ( e );
        }

        protected override void OnOpened ( EventArgs e ) {
            var storyOpen = ( Storyboard ) this.FindResource ( "OpenContacts" );
            if ( storyOpen != null )
                storyOpen.Begin ( this.LayoutRoot );
            KTouchPage.KBoard = Process.Start ( "osk" );
            //      this.Preferences = SetPreferences((KTouchStack)this.PlacementTarget);
            base.OnOpened ( e );
        }

        //private string SetPreferences(KTouchStack stack) {
        //    var collection = (ObservableCollection<KTouchItem>)stack.ItemsSource;
        //    string arguments = "";
        //    foreach (KTouchItem item in collection) {
        //        arguments += item.ToString() + ";";
        //    }
        //    return arguments;
        //}

        public void TextBoxSetFocus ( object sender, System.Windows.Input.TouchEventArgs e ) {
            bool b = ( ( TextBox ) sender ).Focus ( );
            if ( !b )
                throw new InvalidOperationException ( "Unable to set focus to this texbox" );
        }
    }
}
