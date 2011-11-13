using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Blake.NUI.WPF.Gestures;
using KTouch.Controls.Core;
using KTouch.Controls.ViewModel;
using KTouch.Units;
using KTouch.Views;
using Microsoft.Surface.Presentation.Controls;

namespace KTouch {

    /// <summary>
    /// Encapsulates a main page of content that can be navigated to and hosted in kBrowser.
    /// </summary>
    public partial class MainPage : Page {

        private readonly string _tag;
        protected string CurrentTag {
            get {
                return !string.IsNullOrEmpty(_tag) ? _tag : "kTouchContent";
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainPage() {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        public MainPage(string tag) {
            InitializeComponent();
            this._tag = tag;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e) {
            //Events.RegisterGestureEventSupport(this);
            //this.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(Navigate));
            //this.AddHandler(Events.TapGestureEvent, new GestureEventHandler(Navigate));
            DataContext = new MainPageViewModel(_tag);
        }

        /// <summary>
        /// Represents the method that will handle the GetureEvent raised by the sender.
        /// </summary>
        /// <param name="sender">The source of the GetureEvent.</param>
        /// <param name="e">An GestureEventArgs that contains the event data.</param>
        private void OnTapGesture(object sender, GestureEventArgs e) {
            this.Play(sender, e);
            e.Handled = true;
        }

        /// <summary>
        /// Plays the content element (kItem) in the PresentationPage.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An RoutedEventArgs that contains the event data.</param>
        private void Play(object sender, RoutedEventArgs e) {
            if(!kPage.XpsViewer.IsVisible && !kPage.MediaViewer.IsVisible) {
                var item = (SurfaceListBoxItem)StaticAccessors.FindAncestor(typeof(SurfaceListBoxItem), e.OriginalSource);
                if(item != null)
                    kPage.ShowInViewer((kItem)item.DataContext);
            }
            e.Handled = true;
        }

       
    }
}
