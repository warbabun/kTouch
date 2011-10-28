using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace KTouch.Controls {
    /// <summary>
    /// Interaction logic for KTouchMessagePopup.xaml
    /// </summary>
    public partial class KTouchMessagePopup : Popup {

        private readonly DispatcherTimer _messagePopupTimer;

        public KTouchMessagePopup() {
            InitializeComponent();
            VerticalOffset = Application.Current.MainWindow.ActualHeight;
            _messagePopupTimer = new DispatcherTimer();
            _messagePopupTimer.Interval = TimeSpan.FromSeconds(3);
            _messagePopupTimer.Tick += (sender, e) => {
                if (KTouchPage.MessagePopup.IsOpen)
                    KTouchPage.MessagePopup.IsOpen = false;
            };
        }

        protected override void OnOpened(EventArgs e) {
            base.OnOpened(e);
            _messagePopupTimer.Start();
        }
        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            _messagePopupTimer.Stop();
        }
    }
}
