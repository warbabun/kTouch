using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using KTouch.Units;

namespace KTouch.Controls.Core {
    public abstract class KTouchPage : Page {

        public static Process KBoard;


        private KTouchMenuControl _kTouchMenuControl;
        private Dictionary<KTouchItem, DragInfo> _draggedItemsCollection;

        private static KTouchMessagePopup _messagePopup;
        public static KTouchMessagePopup MessagePopup {
            get {
                if ( _messagePopup == null ) {
                    _messagePopup = new KTouchMessagePopup ( );
                }
                return _messagePopup;
            }
        }

        public static KTouchXpsViewer XpsViewer {
            get;
            private set;
        }

        public static KTouchMediaPlayer MediaViewer {
            get;
            private set;
        }

        public bool MenuVisible {
            get { return ( bool ) GetValue ( MenuVisibleProperty ); }
            set { SetValue ( MenuVisibleProperty, value ); }
        }

        public static readonly DependencyProperty MenuVisibleProperty =
            DependencyProperty.Register ( "MenuVisible", typeof ( bool ), typeof ( KTouchPage ), new UIPropertyMetadata ( true ) );


        public VisibilityTimer Timer { get; private set; }
        public Dictionary<KTouchItem, DragInfo> DraggedItemsCollection {
            get { return _draggedItemsCollection; }
        }

        public KTouchPage ( ) {
            _draggedItemsCollection = new Dictionary<KTouchItem, DragInfo> ( );

            _kTouchMenuControl = new KTouchMenuControl ( ) { Name = "Menu" };

            XpsViewer = new KTouchXpsViewer ( );
            MediaViewer = new KTouchMediaPlayer ( );
            Timer = new VisibilityTimer ( );

        }

        protected override void OnInitialized ( EventArgs e ) {
            base.OnInitialized ( e );
            if ( MenuVisible )
                ( ( Grid ) Content ).Children.Add ( _kTouchMenuControl );
            ( ( Grid ) Content ).Children.Add ( XpsViewer );
            ( ( Grid ) Content ).Children.Add ( MediaViewer );
        }

        protected override void OnPreviewTouchDown ( TouchEventArgs e ) {
            base.OnPreviewTouchDown ( e );
            Timer.TouchesCaptured = true;
        }

        public static void ShowInViewer ( KTouchItem item ) {
            switch ( item.Type ) {
                case "vid":
                    MediaViewer.Source = item.File;
                    MediaViewer.Visibility = Visibility.Visible;
                    break;
                default:
                    KTouchPage.XpsViewer.Document = item.File;
                    XpsViewer.IsOpen = true;
                    // XpsViewer.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
