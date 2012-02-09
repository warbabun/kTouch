using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using KTouch.Properties;
using KTouch.Utilities;

namespace KTouch.ViewModel {

    /// <summary>
    /// Class in charge of supplying VideoPage with the information.
    /// </summary>
    public class VideoPageViewModel : BaseViewModel {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="item">Current item.</param>
        public VideoPageViewModel(XElement item) {
            this.Item = item;
            this._itemList = new ObservableCollection<XElement>(item.Parent.Elements().Where(e => !string.Equals(SupportedExtensions.DIR, (string)e.Attribute(Tags.Type))));
        }
    }
}
