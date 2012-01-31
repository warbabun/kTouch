using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace KTouch.ViewModel {

    /// <summary>
    /// Class in charge of supplying VideoPage with the information.
    /// </summary>
    public class VideoPageViewModel : BaseViewModel {

        /// <summary>
        /// Callback on Item DP changed.
        /// </summary>
        /// <param name="item">XElement new value.</param>
        protected override void OnItemChanged(XElement item) {
            base.OnItemChanged(item);
            this.Source = new Uri((string)item.Attribute("FullName"), UriKind.Absolute);
        }

        /// <summary>
        /// Exposes an Uri object for VideoPlayer's source.
        /// </summary>
        public Uri Source {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// Source DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(Uri), typeof(VideoPageViewModel), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="item">Current item.</param>
        public VideoPageViewModel(XElement item) {
            this.Item = item;
            this._itemList = new ObservableCollection<XElement>(item.Parent.Elements().Where(e => !string.Equals("dir", (string)e.Attribute("Type"))));
        }
    }
}
