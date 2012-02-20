﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Linq;
using KTouch.Properties;
using KTouch.Views;
using Microsoft.Surface.Presentation.Input;

namespace KTouch.Utilities {

    /// <summary>
    /// Interaction logic for VideoElement.xaml
    /// </summary>
    public partial class VideoElement : UserControl {

        private bool fullScreen = false;
        private bool _isPause = false;
        private bool _isDragging = false;
        private DispatcherTimer _timer = null;
        private object _winContext = null;
        private TimeSpan? _position;

        // An event that clients can use to be notified whenever the
        // content changes.
        public event ChangedEventHandler SourceChanged;

        // Invoke the SourceChanged event; called whenever list changes
        protected virtual void OnSourceChanged(EventArgs e) {
            if (SourceChanged != null)
                SourceChanged(this, e);
        }

        /// <summary>
        /// Exposes an Uri object for VideoPlayer's source.
        /// </summary>
        public XElement Source {
            get { return (XElement)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// Source DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(XElement), typeof(VideoElement), new FrameworkPropertyMetadata(new PropertyChangedCallback(SourceChangedCallback)));

        /// <summary>
        /// DP callback on Source changed.
        /// </summary>
        /// <param name="sender">DependencyObject sender.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs argument.</param>
        public static void SourceChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            XElement item = (XElement)e.NewValue;
            VideoElement player = (VideoElement)sender;
            player.mediaPlayerMain.Source = new Uri((string)item.Attribute(Tags.FullName), UriKind.Absolute);
            player.OnSourceChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public VideoElement() {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(VideoElement_Loaded);
        }

        /// <summary>
        /// Handles event when page is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideoElement_Loaded(object sender, RoutedEventArgs e) {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += new EventHandler(_timer_Tick);
            TouchExtensions.AddTapGestureHandler(mediaPlayerMain, new EventHandler<TouchEventArgs>(OnTapGesture));
            mediaPlayerMain.MouseLeftButtonUp += new MouseButtonEventHandler(mediaPlayerMain_MouseLeftButtonUp);
            PlayMedia();
        }

        #region Playback navigation

        /// <summary>
        /// Handles mouse click event on the media element.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        private void mediaPlayerMain_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            if (_isPause) {
                this.PlayMedia();
            } else {
                this.PausePlayback();
            }
            _isPause = !_isPause;
            e.Handled = true;
        }

        /// <summary>
        /// Handles Tap event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        protected void OnTapGesture(object sender, TouchEventArgs e) {
            if (_isPause) {
                this.PlayMedia();
            } else {
                this.PausePlayback();
            }
            _isPause = !_isPause;
            e.Handled = true;
        }

        /// <summary>
        /// Handles switch between fullscreen and regular mode.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        private void ExpandButton_Click(object sender, RoutedEventArgs e) {
            Window win = App.Current.MainWindow;
            if (!fullScreen) {
                _winContext = win.Content;
                win.Content = this;
            } else if (_winContext != null) {
                VideoPage p = (VideoPage)win.Content;
                _position = p.player.mediaPlayerMain.Position;
                win.Content = _winContext;
                Frame f = (Frame)((Grid)win.Content).Children[0];
                f.NavigationService.Refresh();
            }
            fullScreen = !fullScreen;
        }

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
            if (_position.HasValue) {
                mediaPlayerMain.Position = _position.Value;
                _position = null;
            }
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
