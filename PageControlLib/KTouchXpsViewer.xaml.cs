using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Xps.Packaging;
using System;

namespace KTouch.Controls {

    /// <summary>
    /// Interaction logic for KTouchXpsViewer.xaml
    /// </summary>
    public partial class KTouchXpsViewer : UserControl {

        public static readonly DependencyProperty DocumentProperty =
               DependencyProperty.Register("Document", typeof(string), typeof(KTouchXpsViewer), new FrameworkPropertyMetadata(OnDocumentPropertyChanged));

        public string Document {
            get { return (string)GetValue(DocumentProperty); }
            set { SetValue(DocumentProperty, value); }
        }

        public KTouchXpsViewer() {
            InitializeComponent();
        }

        private static void OnDocumentPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e) {
            try {
                KTouchXpsViewer docviewer = (KTouchXpsViewer)source;
                docviewer.xpsViewer.Document = (new XpsDocument((string)e.NewValue, FileAccess.Read)).GetFixedDocumentSequence();
                docviewer.xpsViewer.FitToHeight();
            } catch {
            } 
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
                if (e.DeltaManipulation.Scale.X != 1) {
                    if (xpsViewer.Zoom > 400)
                        xpsViewer.Zoom = 400;
                    else if (xpsViewer.Zoom < 20)
                        xpsViewer.Zoom = 20;
                    else
                        xpsViewer.Zoom *= e.DeltaManipulation.Scale.X;
                }
            } catch {
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
