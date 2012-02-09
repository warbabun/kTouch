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

        protected ObservableCollection<XElement> _firstList;
        protected ObservableCollection<XElement> _secondList;
        protected ObservableCollection<XElement> _thirdList;

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
            get {
                return _firstList;
            }
        }

        /// <summary>
        /// ObservableCollection of XElement objects of the second row.
        /// </summary>
        public ObservableCollection<XElement> SecondList {
            get {
                return _secondList;
            }
        }

        /// <summary>
        /// ObservableCollection of XElement objects of the third row.
        /// </summary>
        public ObservableCollection<XElement> ThirdList {
            get {
                return _thirdList;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FrontPageViewModel() {
            ObjectDataProvider provider = Application.Current.FindResource("loader") as ObjectDataProvider;
            if (provider != null) {
                Loader loader = (Loader)provider.ObjectInstance;
                _firstList = new ObservableCollection<XElement>(loader.LoadItemDescendantListByFullNameAndType(this.FirstRowHeader, SupportedExtensions.XPS));
                _secondList = new ObservableCollection<XElement>(loader.LoadItemDescendantListByFullNameAndType(this.SecondRowHeader, SupportedExtensions.XPS));
                _thirdList = new ObservableCollection<XElement>(loader.LoadItemDescendantListByFullNameAndType(this.ThirdRowHeader, SupportedExtensions.XPS));
            }
        }
    }
}
