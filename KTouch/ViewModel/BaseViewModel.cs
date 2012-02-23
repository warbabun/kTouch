//-----------------------------------------------------------------------
// <copyright file="BaseViewModel.cs" company="Klee Group">
//     Copyright (c) Klee Group. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.ObjectModel;
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

        #region Fields

        /// <summary>
        /// Item DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(XElement), typeof(BaseViewModel), new FrameworkPropertyMetadata(new PropertyChangedCallback(ItemChangedCallback)));

        /// <summary>
        /// CurrentTitle DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty CurrentTitleProperty =
            DependencyProperty.Register("CurrentTitle", typeof(string), typeof(BaseViewModel), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(CurrentTitleChangedCallBack)));

        #endregion //Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BaseViewModel class.
        /// </summary>
        public BaseViewModel() {
        }

        /// <summary>
        /// Initializes a new instance of the BaseViewModel class.
        /// </summary>
        /// <param name="item">XElement object.</param>
        public BaseViewModel(XElement item) {
            Item = item;
            ObjectDataProvider provider = Application.Current.FindResource("loader") as ObjectDataProvider;
            if (provider != null) {
                Loader loader = (Loader)provider.ObjectInstance;
                this.ItemList = new ObservableCollection<XElement>(item.Elements());
            }
        }

        #endregion //Constructors

        #region Properties

        /// <summary>
        /// XElement object.
        /// </summary>
        public XElement Item {
            get { return (XElement)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        /// <summary>
        /// Current title of the page.
        /// </summary>
        public string CurrentTitle {
            get { return (string)GetValue(CurrentTitleProperty); }
            set { SetValue(CurrentTitleProperty, value); }
        }

        /// <summary>
        /// Gets an ObservableCollection of XElement objects.
        /// </summary>
        public ObservableCollection<XElement> ItemList {
            get;
            set;
        }

        #endregion //Properties

        #region Methods

        /// <summary>
        /// DP callback on CurrentTitle changed.
        /// </summary>
        /// <param name="sender">DependencyObject sender.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs argument.</param>
        public static void CurrentTitleChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            ((BaseViewModel)sender).OnCurrentTitleChanged((string)e.NewValue);
        }

        /// <summary>
        /// DP callback on Item changed.
        /// </summary>
        /// <param name="sender">DependencyObject sender.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs argument.</param>
        public static void ItemChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            ((BaseViewModel)sender).OnItemChanged((XElement)e.NewValue);
        }

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

        /// <summary>
        /// VM callback on Item changed.
        /// </summary>
        /// <param name="item">Source item.</param>
        protected virtual void OnItemChanged(XElement item) {
            if (item != null) {
                this.CurrentTitle = (string)item.Attribute(Tags.FullName);
            }
        }

        /// <summary>
        /// VM callback on Item changed.
        /// </summary>
        /// <param name="value">String new value.</param>
        protected virtual void OnCurrentTitleChanged(string value) {
        }
        #endregion // Methods
    }
}
