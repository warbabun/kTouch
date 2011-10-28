using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KTouch.Units {

    /// <summary>
    /// 
    /// </summary>
    public class DragInfo {

        /// <summary>
        /// The FrameworkElement object that is dragged from the drag source.
        /// </summary>
        public FrameworkElement DraggedElement {
            get;
            set;
        }

        public DragDataContext ElementDataContext {
            get;
            set;
        }

        /// <summary>
        /// The visual element of the cursor.
        /// </summary>
        public ContentPresenter Cursor {
            get;
            set;
        }

        /// <summary>
        /// The FrameworkElement object that the item is dragged out from.
        /// </summary>
        public FrameworkElement Source {
            get;
            set;
        }

        /// <summary>
        /// The FrameworkElement object that was originally touched.
        /// </summary>
        public FrameworkElement OriginalSource {
            get;
            set;
        }
    }

    public class DragDataContext {
        public KTouchItem Context {
            get;
            set;
        }

        public VisualBrush Adorner {
            get;
            set;
        }
    }
}
