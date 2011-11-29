using System.Windows.Controls;
using System.Windows.Input;
using Blake.NUI.WPF.Gestures;
using KTouch.Controls.ViewModel;
using KTouch.Units;

namespace KTouch.Views {

    /// <summary>
    /// Interaction logic for PresentationPage.xaml
    /// </summary>
    public partial class PresentationPage : Page {

        private PresentationPageViewModel _vm;

        public PresentationPage(Item item) {
            InitializeComponent();
            _vm = new PresentationPageViewModel(item);
            _vm.DocumentChanged += new System.EventHandler(_vm_DocumentChanged);
            Events.RegisterGestureEventSupport(this);
            Events.AddTapGestureHandler(xpsViewer, new GestureEventHandler(OnTapGesture));
            this.DataContext = _vm;
            //    this.PreviewTouchDown += new EventHandler<TouchEventArgs>(kBrowser_PreviewTouchDown);
        }

        void _vm_DocumentChanged(object sender, System.EventArgs e) {
            xpsViewer.Zoom = 60;
        }

        protected void OnTapGesture(object sender, GestureEventArgs e) {
            _vm.Next();
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

        /// <summary>
        /// Handles the PreviewTouchDown event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void kBrowser_PreviewTouchDown(object sender, TouchEventArgs e) {
        //    _vm.RestartTimer();
        //}
    }
}
