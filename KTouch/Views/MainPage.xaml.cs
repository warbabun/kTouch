using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Blake.NUI.WPF.Gestures;
using KTouch.Controls.Core;
using KTouch.Controls.ViewModel;
using KTouch.Units;
using Microsoft.Surface.Presentation.Controls;

namespace KTouch {
    /// <summary>
    /// Encapsulates a main page of content that can be navigated to and hosted in kBrowser.
    /// </summary>
    public partial class MainPage : kPage {

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainPage() {
            InitializeComponent();

            Events.RegisterGestureEventSupport(this);
            this.AddHandler(Button.ClickEvent, new RoutedEventHandler(Navigate));
            this.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.Play));
            AddHandler(Events.TapGestureEvent, new GestureEventHandler(OnTapGesture));
            DataContext = new KTouchFrontViewModel();
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
                    kPage.ShowInViewer((KTouchItem)item.DataContext);
            }
            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Navigate(object sender, RoutedEventArgs e) {
            Uri newPage = new Uri(PageControl.PageDictionnary[((SurfaceButton)e.OriginalSource).Tag.ToString()], UriKind.Relative);
            try {
                if(NavigationService.CurrentSource != newPage)
                    NavigationService.Navigate(newPage);
                else
                    NavigationService.Refresh();
            } catch {

            }
            e.Handled = true;
        }
    }
}
