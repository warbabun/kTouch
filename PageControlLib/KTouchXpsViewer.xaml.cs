using System.Windows.Controls;
using System.Windows.Input;
using Blake.NUI.WPF.Gestures;
using System.Windows.Xps.Packaging;
using System.IO;
using System.Windows;
using System;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Media;
using System.IO.Packaging;
using System.Windows.Markup;

namespace KTouch.Controls {
    /// <summary>
    /// Interaction logic for KTouchXpsViewer.xaml
    /// </summary>
    public partial class KTouchXpsViewer : UserControl {



        public bool IsOpen {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(KTouchXpsViewer), new FrameworkPropertyMetadata(false, OnIsOpenPropertyChanged));

        public static void OnIsOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var viewer = (KTouchXpsViewer)d;
            Storyboard storyboard = null; 
            switch ((bool)e.NewValue) {
                case true:
                    viewer.Visibility = Visibility.Visible;
                    storyboard = (Storyboard)Application.Current.FindResource("OpenContacts");
                    break;
                case false:
                   // storyboard = (Storyboard)Application.Current.FindResource("CloseContacts");
                    viewer.Visibility = Visibility.Collapsed;
                    break;
            }
            if (storyboard != null)
                storyboard.Begin(viewer.LayoutRoot);
        }


        public KTouchXpsViewer() {
            InitializeComponent();
            Events.RegisterGestureEventSupport(this);
            Events.AddDoubleTapGestureHandler(this.xpsViewer, new GestureEventHandler(OnDoubleTapGesture));
            this.CloseBtn.Click += (sender, e) => { 
                this.IsOpen = false; 
                e.Handled = true; 
            };
        }

        public static readonly DependencyProperty DocumentProperty =
               DependencyProperty.Register("Document", typeof(string), typeof(KTouchXpsViewer), new FrameworkPropertyMetadata(OnDocumentPropertyChanged));

        public string Document {
            get { return (string)GetValue(DocumentProperty); }
            set { SetValue(DocumentProperty, value); }
        }

        private static void OnDocumentPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e) {
            KTouchXpsViewer docviewer = (KTouchXpsViewer)source;
            docviewer.xpsViewer.Document = (new XpsDocument((string)e.NewValue, FileAccess.Read)).GetFixedDocumentSequence();
            docviewer.xpsViewer.FitToWidth();
        }

        private void OnDoubleTapGesture(object sender, GestureEventArgs e) {
            this.IsOpen = false;
            e.Handled = true;
        }

        private void manipulationDelta(object sender, ManipulationDeltaEventArgs e) {
            
            try {
                if (e.DeltaManipulation.Translation != null) {
                    double verticalOffset = xpsViewer.VerticalOffset - e.DeltaManipulation.Translation.Y * 3;
                    double horizontalOffset = xpsViewer.HorizontalOffset - e.DeltaManipulation.Translation.X * 3;
                    if (verticalOffset > 0)
                        xpsViewer.VerticalOffset = verticalOffset;
                    if (horizontalOffset > 0)
                        xpsViewer.HorizontalOffset = horizontalOffset;
                }
                //if (e.DeltaManipulation.Scale.X != 1) {
                //    if (xpsViewer.Zoom > 400)
                //        xpsViewer.Zoom = 400;
                //    else if (xpsViewer.Zoom < 20)
                //        xpsViewer.Zoom = 20;
                //    else
                //        xpsViewer.Zoom *= e.DeltaManipulation.Scale.X;
                //}
            } catch  { 
            } finally {
                e.Handled = true;
            }
        }

        private void manipulationStarting(object sender, ManipulationStartingEventArgs e) {
            try {
                e.ManipulationContainer = this;
            } catch {
            } finally {
                e.Handled = true;
            }
        }

        void inertiaStarting(object sender, ManipulationInertiaStartingEventArgs e) {
            e.TranslationBehavior.DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0);
            e.Handled = true;
        }
    }
}
