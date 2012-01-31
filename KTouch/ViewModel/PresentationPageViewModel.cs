using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using System.Xml.Linq;
using KTouch.Utilities;

namespace KTouch.ViewModel {

    /// <summary>
    /// Class in charge of supplying PresentationPage with the information.
    /// </summary>
    public class PresentationPageViewModel : BaseViewModel {

        /// <summary>
        /// Callback on Item DP changed.
        /// </summary>
        /// <param name="item">XElement new value.</param>
        protected override void OnItemChanged(XElement item) {
            base.OnItemChanged(item);
            this.Document = (new XpsDocument((string)item.Attribute("FullName"), FileAccess.Read)).GetFixedDocumentSequence();
        }

        /// <summary>
        /// Exposes a FixedDocumentSequence for XPS viewer's source.
        /// </summary>
        public FixedDocumentSequence Document {
            get { return (FixedDocumentSequence)GetValue(DocumentProperty); }
            set { SetValue(DocumentProperty, value); }
        }

        /// <summary>
        /// Document DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.Register("Document", typeof(FixedDocumentSequence), typeof(PresentationPageViewModel), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="item">Current item.</param>
        public PresentationPageViewModel(XElement item) {
            this.Item = item;
            ObjectDataProvider provider = Application.Current.FindResource("loader") as ObjectDataProvider;
            if (provider != null) {
                Loader loader = (Loader)provider.ObjectInstance;
                this._itemList = new ObservableCollection<XElement>(item.Parent.Elements().Where(e => !string.Equals("dir", (string)e.Attribute("Type"))));
            }
        }
    }
}
