using System;
using System.Windows.Threading;

namespace KTouch.Units {
    public class VisibilityTimer {

        /// <summary>
        /// Timer.
        /// </summary>
        private readonly DispatcherTimer _dispatcherTimer;

        /// <summary>
        /// Returns 'true' if the object should be visible.
        /// </summary>
        public bool IsVisible {
            get;
            set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public VisibilityTimer() {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += new EventHandler(_dispatcherTimer_Tick);
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
            _dispatcherTimer.Start();
            this.IsVisible = true;
        }

        /// <summary>
        /// Restarts the timer.
        /// </summary>
        public void Restart() {
            _dispatcherTimer.Stop();
            this.IsVisible = true;
            _dispatcherTimer.Start();
        }

        /// <summary>
        /// If no activity is registered withing the next interval timer is stopped, element is hid.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        private void _dispatcherTimer_Tick(object sender, EventArgs e) {
            this.IsVisible = false;
            _dispatcherTimer.Stop();
        }
    }
}
