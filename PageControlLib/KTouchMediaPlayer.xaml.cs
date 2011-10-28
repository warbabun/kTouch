using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Windows.Input;
using Blake.NUI.WPF.Gestures;

namespace KTouch.Controls {
    /// <summary>
    /// Interaction logic for KTouchMediaPlayer.xaml
    /// </summary>
    public partial class KTouchMediaPlayer : UserControl {

        private bool isDragging = false;
        private DispatcherTimer _timer = null; 

        /// <summary>
        /// Source dependency property
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
                    DependencyProperty.Register("Source",
                                    typeof(string),
                                    typeof(KTouchMediaPlayer),
                                    new FrameworkPropertyMetadata(OnSourcePropertyChanged));

        /// <summary>
        /// Source property
        /// </summary>
        public string Source {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void OnSourcePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e) {
            KTouchMediaPlayer mediaPlayer = (KTouchMediaPlayer)source;
            mediaPlayer.mediaPlayerMain.Source = new Uri((string)e.NewValue);
        }

        /// <summary>
        /// Public constructor with a dispatcher timer initialisation
        /// </summary>
        public KTouchMediaPlayer() {
            InitializeComponent();
            Events.RegisterGestureEventSupport(this);
            this.CloseBtn.Click += (sender, e) => { StopPlayback(); this.Visibility = Visibility.Collapsed; e.Handled = true; };
            this.mediaPlayerMain.PreviewTouchUp += (sender, e) => { StopPlayback(); this.Visibility = Visibility.Collapsed; e.Handled = true; };
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += new EventHandler(_timer_Tick);
        }
        
        /// <summary>
        /// Dispatcher timer tick routine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _timer_Tick(object sender, EventArgs e) {   
            if(!isDragging && mediaPlayerMain.NaturalDuration.HasTimeSpan)
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
            sliderVolume.IsEnabled = mediaPlayerMain.IsLoaded;
            PlayMedia();
        }

        /// <summary>
        /// Time slider's thumb drag started event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderTime_DragStarted(object sender, DragStartedEventArgs e) {
            isDragging = true;
        }

        /// <summary>
        /// Time slider's thumb drag completed event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderTime_DragCompleted(object sender, DragCompletedEventArgs e) {
            isDragging = false;           
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

        //private void OnTapGesture(object sender, GestureEventArgs e) {
        //    var element = (MediaElement)KTouchPage.FindAncestor(typeof(MediaElement), e.OriginalSource);
        //    if (element != null){
        //        StopPlayback();
        //        this.Visibility = Visibility.Collapsed;
               
        //    }
        //    e.Handled = true;
        //}
    }
}
