using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using KTouch.Units;

namespace KTouch.Controls.Core {

    /// <summary>
    /// Encapsulates a template for a page of content that can be navigated to and hosted 
    /// by System.Windows.Navigation.NavigationWindow.
    /// </summary>
    public class kPage : Page {

        public static Process KBoard;

        //private KTouchMenuControl _kTouchMenuControl;
        private Dictionary<kItem, DragInfo> _draggedItemsCollection;

        private static KTouchMessagePopup _messagePopup;
        public static KTouchMessagePopup MessagePopup {
            get {
                if(_messagePopup == null) {
                    _messagePopup = new KTouchMessagePopup();
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
            get { return (bool)GetValue(MenuVisibleProperty); }
            set { SetValue(MenuVisibleProperty, value); }
        }

        public static readonly DependencyProperty MenuVisibleProperty =
            DependencyProperty.Register("MenuVisible", typeof(bool), typeof(kPage), new UIPropertyMetadata(true));


        public VisibilityTimer Timer { get; private set; }
        public Dictionary<kItem, DragInfo> DraggedItemsCollection {
            get { return _draggedItemsCollection; }
        }

        public kPage() {
            _draggedItemsCollection = new Dictionary<kItem, DragInfo>();

            //_kTouchMenuControl = new KTouchMenuControl ( ) { Name = "Menu" };

            XpsViewer = new KTouchXpsViewer();
            MediaViewer = new KTouchMediaPlayer();
            Timer = new VisibilityTimer();

        }

        protected override void OnInitialized(EventArgs e) {
            base.OnInitialized(e);
            //if ( MenuVisible )
            //    ( ( Grid ) Content ).Children.Add ( _kTouchMenuControl );
            ((Grid)Content).Children.Add(XpsViewer);
            ((Grid)Content).Children.Add(MediaViewer);
        }

        protected override void OnPreviewTouchDown(TouchEventArgs e) {
            base.OnPreviewTouchDown(e);
        }

        public static void ShowInViewer(kItem item) {
            switch(item.Type) {
                case "vid":
                    MediaViewer.Source = item.File;
                    MediaViewer.Visibility = Visibility.Visible;
                    break;
                default:
                    kPage.XpsViewer.Document = item.File;
                    XpsViewer.IsOpen = true;
                    // XpsViewer.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
