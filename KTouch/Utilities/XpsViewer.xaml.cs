using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace KTouch.Utilities {
    /// <summary>
    /// Interaction logic for XpsViewer.xaml
    /// </summary>
    public partial class XpsViewer : UserControl {

        public XpsViewer() {
            InitializeComponent();
            this.xpsViewer.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(manipulationDelta);
            this.xpsViewer.ManipulationStarting += new EventHandler<ManipulationStartingEventArgs>(manipulationStarting);
            this.xpsViewer.ManipulationInertiaStarting += new EventHandler<ManipulationInertiaStartingEventArgs>(inertiaStarting);
        }

        #region Manipulation

        /// <summary>
        /// Handles ManipulationDelta event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
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
                    else if (xpsViewer.Zoom < 65)
                        xpsViewer.Zoom = 65;
                    else
                        xpsViewer.Zoom *= e.DeltaManipulation.Scale.X;
                }
            } catch {
            } finally {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles ManipulationStarting event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        private void manipulationStarting(object sender, ManipulationStartingEventArgs e) {
            try {
                e.ManipulationContainer = this;
            } catch {
            } finally {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles InerniaStarting event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        void inertiaStarting(object sender, ManipulationInertiaStartingEventArgs e) {
            e.TranslationBehavior.DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0);
            e.Handled = true;
        }

        #endregion //Manipulation
    }
}
