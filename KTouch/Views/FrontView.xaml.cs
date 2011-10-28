using System;
using System.Windows;
using Blake.NUI.WPF.Gestures;
using KTouch.Controls.Core;
using KTouch.Controls.ViewModel;
using KTouch.Units;
using Microsoft.Surface.Presentation.Controls;

namespace KTouch {
    /// <summary>
    /// Interaction logic for NewFrontPage.xaml
    /// </summary>
    public partial class NewFrontPage : KTouchPage {

        public NewFrontPage ( ) {
            InitializeComponent ( );

            Events.RegisterGestureEventSupport ( this );
            AddHandler ( Events.TapGestureEvent, new GestureEventHandler ( OnTapGesture ) );
            DataContext = new KTouchFrontViewModel ( );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTapGesture ( object sender, GestureEventArgs e ) {
            this.Play ( sender, e );
            e.Handled = true;
        }

        private void Play ( object sender, RoutedEventArgs e ) {
            if ( !KTouchPage.XpsViewer.IsVisible && !KTouchPage.MediaViewer.IsVisible ) {
                var item = ( SurfaceListBoxItem ) StaticAccessors.FindAncestor ( typeof ( SurfaceListBoxItem ), e.OriginalSource );
                if ( item != null )
                    KTouchPage.ShowInViewer ( ( KTouchItem ) item.DataContext );
            }
            e.Handled = true;
        }

        private void Navigate ( object sender, RoutedEventArgs e ) {
            Uri newPage = new Uri ( PageControl.PageDictionnary [ ( ( SurfaceButton ) e.OriginalSource ).Tag.ToString ( ) ], UriKind.Relative );
            try {
                if ( NavigationService.CurrentSource != newPage )
                    NavigationService.Navigate ( newPage );
                else
                    NavigationService.Refresh ( );
            } catch {

            }
            e.Handled = true;
        }
    }
}
