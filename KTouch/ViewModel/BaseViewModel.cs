﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Xml.Linq;
using KTouch.Properties;
using KTouch.Utilities;

namespace KTouch.ViewModel {

    /// <summary>
    /// Class in charge of supplying ListPage with information.
    /// Also serves as a base class for other VM.
    /// </summary>
    public class BaseViewModel : DependencyObject {

        #region DependencyProperties

        /// <summary>
        /// XElement object.
        /// </summary>
        public XElement Item {
            get { return (XElement)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        /// <summary>
        /// Item DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(XElement), typeof(BaseViewModel), new FrameworkPropertyMetadata(new PropertyChangedCallback(ItemChangedCallback)));

        /// <summary>
        /// DP callback on Item changed.
        /// </summary>
        /// <param name="sender">DependencyObject sender.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs argument.</param>
        public static void ItemChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            ((BaseViewModel)sender).OnItemChanged((XElement)e.NewValue);
        }

        /// <summary>
        /// VM callback on Item changed.
        /// </summary>
        /// <param name="value">String new value.</param>
        protected virtual void OnItemChanged(XElement item) {
            if (item != null) {
                this.CurrentTitle = (string)item.Attribute(Tags.FullName);
            }
        }

        /// <summary>
        /// Current title of the page.
        /// </summary>
        public string CurrentTitle {
            get { return (string)GetValue(CurrentTitleProperty); }
            set { SetValue(CurrentTitleProperty, value); }
        }

        /// <summary>
        /// CurrentTitle DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty CurrentTitleProperty =
            DependencyProperty.Register("CurrentTitle", typeof(string), typeof(BaseViewModel), new FrameworkPropertyMetadata("", new PropertyChangedCallback(CurrentTitleChangedCallBack)));

        /// <summary>
        /// DP callback on CurrentTitle changed.
        /// </summary>
        /// <param name="sender">DependencyObject sender.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs argument.</param>
        public static void CurrentTitleChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            ((BaseViewModel)sender).OnCurrentTitleChanged((string)e.NewValue);
        }

        /// <summary>
        /// VM callback on Item changed.
        /// </summary>
        /// <param name="value">String new value.</param>
        protected virtual void OnCurrentTitleChanged(string value) {
        }

        #endregion //DependencyProperties

        #region Properties

        protected ObservableCollection<XElement> _itemList;

        /// <summary>
        /// ObservableCollection of XElement objects.
        /// </summary>
        public ObservableCollection<XElement> ItemList {
            get {
                return _itemList;
            }
        }

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public BaseViewModel() {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="item">XElement object.</param>
        public BaseViewModel(XElement item) {
            Item = item;
            ObjectDataProvider provider = Application.Current.FindResource("loader") as ObjectDataProvider;
            if (provider != null) {
                Loader loader = (Loader)provider.ObjectInstance;
                _itemList = new ObservableCollection<XElement>(item.Elements());
            }
        }

        #endregion //Constructors

        #region Functions

        /// <summary>
        /// Proceeds to the next element in the ItemList.
        /// </summary>
        public void Next() {
            int currentIndex = ItemList.IndexOf(Item);
            if (currentIndex == ItemList.Count - 1) {
                currentIndex = 0;
            } else {
                currentIndex++;
            }
            Item = ItemList.ElementAt(currentIndex);
        }

        #endregion //Functions
    }
}
