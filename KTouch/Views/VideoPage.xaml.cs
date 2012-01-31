using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Linq;
using KTouch.ViewModel;

namespace KTouch.Views {

    /// <summary>
    /// Interaction logic for PresentationPage.xaml
    /// </summary>
    public partial class VideoPage : Page {

        private bool fullScreen = false;
        private VideoPageViewModel _vm;
        private bool _isDragging = false;
        private DispatcherTimer _timer = null;
        object _winContext = null;
        object _pageContext = null;
        private XElement _currentItem;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="item">XElement object.</param>
        public VideoPage(XElement item) {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(VideoPage_Loaded);
            _vm = new VideoPageViewModel(item);
            this.DataContext = _vm;
        }

        /// <summary>
        /// Handles Loaded event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        void VideoPage_Loaded(object sender, RoutedEventArgs e) {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += new EventHandler(_timer_Tick);
            PlayMedia();
        }

        /// <summary>
        /// Handles Tap event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        protected void OnTapGesture(object sender, TouchEventArgs e) {
            e.Handled = true;
        }

        private void ExpandButton_Click(object sender, RoutedEventArgs e) {
            Window win = App.Current.MainWindow;
            if (!fullScreen) {
                _winContext = win.Content;
                _pageContext = this;
                win.Content = _pageContext;
            } else if (_pageContext != null) {
                win.Content = _winContext;
                ((Frame)((Grid)win.Content).Children[0]).Content = (VideoPage)_pageContext;
                //((Frame)((Grid)_winContext).Children[0]).Content = _pageContext;
            } else {
                win.Content = new Browser();
            }
            fullScreen = !fullScreen;
        }

        #region Playback navigation

        /// <summary>
        /// Dispatcher timer tick routine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _timer_Tick(object sender, EventArgs e) {
            if (!_isDragging && mediaPlayerMain.NaturalDuration.HasTimeSpan)
                sliderTime.Value = mediaPlayerMain.Position.TotalMilliseconds / mediaPlayerMain.NaturalDuration.TimeSpan.TotalMilliseconds;
        }

        /// <summary>
        /// Stop media when ended
        /// </summary>
        private void mediaPlayerMain_MediaEnded(object sender, RoutedEventArgs e) {
            StopPlayback();
        }

        /// <summary>
        /// Initialise UI elements based on current media item
        /// </summary>
        private void mediaPlayerMain_MediaOpened(object sender, RoutedEventArgs e) {
            sliderTime.IsEnabled = mediaPlayerMain.IsLoaded;
            PlayMedia();
        }

        /// <summary>
        /// Time slider's thumb drag started event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderTime_DragStarted(object sender, DragStartedEventArgs e) {
            _isDragging = true;
        }

        /// <summary>
        /// Time slider's thumb drag completed event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderTime_DragCompleted(object sender, DragCompletedEventArgs e) {
            _isDragging = false;
            mediaPlayerMain.Position = TimeSpan.FromMilliseconds(sliderTime.Value * mediaPlayerMain.NaturalDuration.TimeSpan.TotalMilliseconds);
        }

        /// <summary>
        /// On kTouchMediaPlayer loaded behaviour
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kTouchMediaPlayer_Loaded(object sender, RoutedEventArgs e) {
            PlayMedia();
        }

        /// <summary>
        /// The Play method will begin the media if it is not currently active or 
        /// resume media if it is paused. This has no effect if the media is already running.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            PlayMedia();
        }

        /// <summary>
        /// The Pause method pauses the media if it is currently running.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PauseCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            PausePlayback();
        }

        /// <summary>
        /// The Stop method stops and resets the media to be played from the beginning.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            StopPlayback();
        }

        /// <summary>
        /// Commands can be executed if mediaPlayerMain is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = mediaPlayerMain.IsLoaded;
        }

        /// <summary>
        /// Play method for KTouchMediaPlayer
        /// </summary>
        private void PlayMedia() {
            mediaPlayerMain.Play();
            _timer.Start();
        }

        /// <summary>
        /// Pause method for KTouchMediaPlayer
        /// </summary>
        private void PausePlayback() {
            mediaPlayerMain.Pause();
            _timer.Stop();
        }

        /// <summary>
        /// Stop method for KTouchMediaPlayer
        /// </summary>
        private void StopPlayback() {
            mediaPlayerMain.Stop();
            _timer.Stop();
        }

        #endregion //Playback navigation
    }
}
