//-----------------------------------------------------------------------
// <copyright file="FrontPageViewModel.cs" company="Klee Group">
//     Copyright (c) Klee Group. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using System.Windows.Data;
using System.Xml.Linq;
using KTouch.Utilities;

namespace KTouch.ViewModel {

    /// <summary>
    /// Class in charge of supplying FrontPage with information.
    /// </summary>
    public class FrontPageViewModel : DependencyObject {

        /// <summary>
        /// Initializes a new instance of the FrontPageViewModel class.
        /// </summary>
        public FrontPageViewModel() {
            ObjectDataProvider provider = Application.Current.FindResource("loader") as ObjectDataProvider;
            if (provider != null) {
                Loader loader = (Loader)provider.ObjectInstance;
                FirstList = new ObservableCollection<XElement>(loader.LoadItemDescendantListByFullNameAndType(this.FirstRowHeader, SupportedExtensions.XPS));
                SecondList = new ObservableCollection<XElement>(loader.LoadItemDescendantListByFullNameAndType(this.SecondRowHeader, SupportedExtensions.XPS));
                ThirdList = new ObservableCollection<XElement>(loader.LoadItemDescendantListByFullNameAndType(this.ThirdRowHeader, SupportedExtensions.XPS));
            }
        }

        /// <summary>
        /// Header of the first row.
        /// </summary>
        public string FirstRowHeader {
            get {
                return ConfigurationManager.AppSettings.Get("FirstRowHeader").ToString();
            }
        }

        /// <summary>
        /// Header of the second row.
        /// </summary>
        public string SecondRowHeader {
            get {
                return ConfigurationManager.AppSettings.Get("SecondRowHeader").ToString();
            }
        }

        /// <summary>
        /// Header of the third row.
        /// </summary>
        public string ThirdRowHeader {
            get {
                return ConfigurationManager.AppSettings.Get("ThirdRowHeader").ToString();
            }
        }

        /// <summary>
        /// ObservableCollection of XElement objects of the first row.
        /// </summary>
        public ObservableCollection<XElement> FirstList {
            get;
            set;
        }

        /// <summary>
        /// ObservableCollection of XElement objects of the second row.
        /// </summary>
        public ObservableCollection<XElement> SecondList {
            get;
            set;
        }

        /// <summary>
        /// ObservableCollection of XElement objects of the third row.
        /// </summary>
        public ObservableCollection<XElement> ThirdList {
            get;
            set;
        }
    }
}
