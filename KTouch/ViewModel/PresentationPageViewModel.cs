using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using System.Xml.Linq;
using KTouch.Properties;
using KTouch.Utilities;

namespace KTouch.ViewModel {

    /// <summary>
    /// Class in charge of supplying PresentationPage with the information.
    /// </summary>
    public class PresentationPageViewModel : BaseViewModel {

     
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="item">Current item.</param>
        public PresentationPageViewModel(XElement item) {
            this.Item = item;
            ObjectDataProvider provider = Application.Current.FindResource("loader") as ObjectDataProvider;
            if (provider != null) {
                Loader loader = (Loader)provider.ObjectInstance;
                this._itemList = new ObservableCollection<XElement>(item.Parent.Elements().Where(e => !string.Equals(SupportedExtensions.DIR, (string)e.Attribute(Tags.Type))));
            }
        }
    }
}
