using System;
using System.Windows;
using System.Windows.Threading;

namespace KTouch.Units {
    public class VisibilityTimer : DependencyObject {


        /// <summary>
        /// Timer
        /// </summary>
        private readonly DispatcherTimer _dispatcherTimer;

        /// <summary>
        /// Inner part of TouchesCaptured property
        /// </summary>
        private bool _touchesCaptured = true;

        /// <summary>
        /// Property recieves or provides current activity
        /// </summary>
        public bool TouchesCaptured {
            get { return _touchesCaptured; }
            set {
                _touchesCaptured = value;
                if ( value ) {
                    _dispatcherTimer.Start ( );
                    PanelVisible = true;
                }
            }
        }


        /// <summary>
        /// Exposes visibility state regarding current activity withing the programm
        /// </summary>
        public bool PanelVisible {
            get { return ( bool ) GetValue ( PanelVisibleProperty ); }
            set { SetValue ( PanelVisibleProperty, value ); }
        }

        private static readonly DependencyProperty PanelVisibleProperty =
            DependencyProperty.Register ( "PanelVisible",
                            typeof ( bool ),
                            typeof ( VisibilityTimer ),
                            new UIPropertyMetadata ( true ) );

        /// <summary>
        /// Public constructor for VisibilityTimer class
        /// </summary>
        public VisibilityTimer ( ) {
            _dispatcherTimer = new DispatcherTimer ( );
            _dispatcherTimer.Tick += _dispatcherTimer_Tick;
            _dispatcherTimer.Interval = TimeSpan.FromSeconds ( 2 );
            _dispatcherTimer.Start ( );
        }

        /// <summary>
        /// If no activity within a defined interval the timer changes TouchesCaptured property (activity state) to false.
        /// If no activity is registered withing the next interval timer is stopped, element is hid.
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
        private void _dispatcherTimer_Tick ( object sender, EventArgs e ) {
            if ( TouchesCaptured )
                TouchesCaptured = false;
            else {
                _dispatcherTimer.Stop ( );
                PanelVisible = false;
            }
        }
    }
}
