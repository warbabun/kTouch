﻿//-----------------------------------------------------------------------
// <copyright file="BrowserViewModel.cs" company="Klee Group">
//     Copyright (c) Klee Group. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Xml.Linq;
using KTouch.Properties;
using KTouch.Utilities;

namespace KTouch.ViewModel {

    /// <summary>
    /// Class in charge of supplying Browser with the information.
    /// </summary>
    public class BrowserViewModel : BaseViewModel {

        private readonly Loader _currentLoader;

        /// <summary>
        /// Initializes a new instance of the BrowserViewModel class.
        /// </summary>
        public BrowserViewModel() {
            ObjectDataProvider provider = Application.Current.FindResource("loader") as ObjectDataProvider;
            if (provider != null) {
                _currentLoader = (Loader)provider.ObjectInstance;
                ItemList = new ObservableCollection<XElement>(_currentLoader.Root.Elements());
                _currentLoader.SetItemTag();
            }
        }

        /// <summary>
        /// Callback on CurrentTitle DP changed.
        /// </summary>
        /// <param name="value">String new value.</param>
        protected override void OnCurrentTitleChanged(string value) {
            if (string.IsNullOrEmpty(value)) {
                return;
            }
            XElement currentElement = this._currentLoader.LoadItemByFullName(value);
            foreach (XElement e in this.ItemList) {
                if (string.Equals((string)currentElement.Attribute(Tags.Tag), (string)e.Attribute(Tags.Name))) {
                    this.Item = e;
                    return;
                }
            }
        }
    }
}
