//-----------------------------------------------------------------------
// <copyright file="PresentationPageViewModel.cs" company="Klee Group">
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
    /// Class in charge of supplying PresentationPage with the information.
    /// </summary>
    public class PresentationPageViewModel : BaseViewModel {

        /// <summary>
        /// Initializes a new instance of the PresentationPageViewModel class.
        /// </summary>
        /// <param name="item">Current item.</param>
        public PresentationPageViewModel(XElement item) {
            this.Item = item;
            ObjectDataProvider provider = Application.Current.FindResource("loader") as ObjectDataProvider;
            if (provider != null) {
                Loader loader = (Loader)provider.ObjectInstance;
                this.ItemList = new ObservableCollection<XElement>(item.Parent.Elements().Where(e => !string.Equals(SupportedExtensions.DIR, (string)e.Attribute(Tags.Type))));
            }
        }
    }
}
