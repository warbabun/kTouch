using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;
using System.Windows.Xps.Packaging;
using KTouch.Units;

namespace KTouch.Controls.ViewModel {
    public class PresentationPageViewModel : DependencyObject {

        /// <summary>
        /// Timer.
        /// </summary>
        private readonly DispatcherTimer _dispatcherTimer;
        private readonly ObservableCollection<Item> _itemList;
        public ObservableCollection<Item> ItemList {
            get {
                return _itemList;
            }
        }

        public Item Item {
            get { return (Item)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(Item), typeof(PresentationPageViewModel), new FrameworkPropertyMetadata(new PropertyChangedCallback(ItemChangedCallback)));


        public static void ItemChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            PresentationPageViewModel vm = (PresentationPageViewModel)d;
            vm.LastItem = (Item)e.NewValue;
            vm.Document = (new XpsDocument(((Item)e.NewValue).FullName, FileAccess.Read)).GetFixedDocumentSequence();
        }



        public FixedDocumentSequence Document {
            get { return (FixedDocumentSequence)GetValue(DocumentProperty); }
            set { SetValue(DocumentProperty, value); }
        }

        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.Register("Document", typeof(FixedDocumentSequence), typeof(PresentationPageViewModel), new UIPropertyMetadata(null));

        //public FixedDocumentSequence Document {
        //    get;
        //    set;
        //}

        public Item LastItem {
            get { return (Item)GetValue(LastItemProperty); }
            set { SetValue(LastItemProperty, value); }
        }

        public static readonly DependencyProperty LastItemProperty =
            DependencyProperty.Register("LastItem", typeof(Item), typeof(PresentationPageViewModel), new FrameworkPropertyMetadata(new PropertyChangedCallback(LastItemChangedCallback)));

        public static void LastItemChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            PresentationPageViewModel vm = (PresentationPageViewModel)d;
            vm.Item = (Item)e.NewValue;
        }

        //public static readonly DependencyProperty ItemProperty =
        //     DependencyProperty.Register("Item", typeof(string), typeof(PresentationPage), new FrameworkPropertyMetadata(OnItemPropertyChanged));

        //public string Item {
        //    get { return (string)GetValue(ItemProperty); }
        //    set { SetValue(ItemProperty, value); }
        //}

        //private static void OnItemPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e) {
        //    try {
        //        PresentationPage page = (PresentationPage)source;
        //        page.xpsViewer.Document = (new XpsDocument((string)e.NewValue, FileAccess.Read)).GetFixedDocumentSequence();
        //        page.xpsViewer.FitToHeight();
        //    } catch {
        //    }
        //}

        public void Next() {
            int currentIndex = ItemList.IndexOf(Item);
            if (currentIndex == ItemList.Count - 1) {
                currentIndex = 0;
            } else {
                currentIndex++;
            }
            Item = ItemList.ElementAt(currentIndex);
        }

        /// <summary>
        /// Returns 'true' if the object should be visible.
        /// </summary>
        public Visibility OverlayVisibility {
            get { return (Visibility)GetValue(OverlayVisibilityProperty); }
            set { SetValue(OverlayVisibilityProperty, value); }
        }

        public static readonly DependencyProperty OverlayVisibilityProperty =
            DependencyProperty.Register("OverlayVisibility", typeof(Visibility), typeof(PresentationPageViewModel), new UIPropertyMetadata(Visibility.Visible));

        public PresentationPageViewModel(Item item) {
            this.Item = item;
            _itemList = new ObservableCollection<Item>();
            Loader<Item> kItemloader = new Loader<Item>();
            kItemloader.StartLoad(ref _itemList, item, kItemloader.LoadSiblingsListByType);

            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += new EventHandler(_dispatcherTimer_Tick);
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(3);
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
        /// <param i="sender">Event sender.</param>
        /// <param i="e">Event argument.</param>
        private void _dispatcherTimer_Tick(object sender, EventArgs e) {
            this.OverlayVisibility = Visibility.Hidden;
            _dispatcherTimer.Stop();
        }
    }
}
