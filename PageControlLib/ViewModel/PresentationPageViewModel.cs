using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;
using KTouch.Units;

namespace KTouch.Controls.ViewModel {
    public class PresentationPageViewModel : DependencyObject {

        private readonly ObservableCollection<kItem> _itemList;

        public ObservableCollection<kItem> ItemList {
            get {
                return _itemList;
            }
        }

        public kItem Item {
            get { return (kItem)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(kItem), typeof(PresentationPageViewModel), new FrameworkPropertyMetadata(new PropertyChangedCallback(ItemChangedCallback)));


        public static void ItemChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            PresentationPageViewModel vm = (PresentationPageViewModel)d;
            vm.LastItem = (kItem)e.NewValue;
        }

        public kItem LastItem {
            get { return (kItem)GetValue(LastItemProperty); }
            set { SetValue(LastItemProperty, value); }
        }

        public static readonly DependencyProperty LastItemProperty =
            DependencyProperty.Register("LastItem", typeof(kItem), typeof(PresentationPageViewModel), new FrameworkPropertyMetadata(new PropertyChangedCallback(LastItemChangedCallback)));

        public static void LastItemChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            PresentationPageViewModel vm = (PresentationPageViewModel)d;
            vm.Item = (kItem)e.NewValue;
        }

        /// <summary>
        /// Timer.
        /// </summary>
        private readonly DispatcherTimer _dispatcherTimer;

        /// <summary>
        /// Returns 'true' if the object should be visible.
        /// </summary>
        public Visibility OverlayVisibility {
            get { return (Visibility)GetValue(OverlayVisibilityProperty); }
            set { SetValue(OverlayVisibilityProperty, value); }
        }

        public static readonly DependencyProperty OverlayVisibilityProperty =
            DependencyProperty.Register("OverlayVisibility", typeof(Visibility), typeof(PresentationPageViewModel), new UIPropertyMetadata(Visibility.Visible));


        public IEnumerable<XElement> LoadMainPageCollectionByTag(object tag) {
            string tagValue = (string)tag;
            XElement root = Loader<kItem>.Root;
            XNamespace ns = Loader<kItem>.RootNamespace;
            string rootDirectory = ns.ToString();
            IOrderedEnumerable<XElement> collection = from e in root.Elements(ns + "kItem")
                                                      where !string.IsNullOrEmpty(tagValue) ? string.Equals((string)e.Element(ns + "Tag"), tagValue) : true
                                                      orderby (string)e.Attribute("Title")
                                                      select e;
            return collection;
        }

        public PresentationPageViewModel(kItem item) {
            this.Item = item;
            _itemList = new ObservableCollection<kItem>();
            Loader<kItem> kItemloader = new Loader<kItem>();
            kItemloader.StartLoad(ref _itemList, item.Tag, this.LoadMainPageCollectionByTag);

            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += new EventHandler(_dispatcherTimer_Tick);
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
            _dispatcherTimer.Start();
            this.OverlayVisibility = Visibility.Visible;
        }

        public void RestartTimer() {
            _dispatcherTimer.Stop();
            this.OverlayVisibility = Visibility.Visible;
            _dispatcherTimer.Start();
        }

        /// <summary>
        /// If no activity is registered withing the next interval timer is stopped, element is hid.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        private void _dispatcherTimer_Tick(object sender, EventArgs e) {
            this.OverlayVisibility = Visibility.Hidden;
            _dispatcherTimer.Stop();
        }
    }
}
