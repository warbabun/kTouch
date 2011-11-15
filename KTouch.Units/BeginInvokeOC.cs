
using System.Collections.ObjectModel;
using System.Windows.Threading;
namespace KTouch.Units {

    public class BeginInvokeOC<T> : ObservableCollection<T> {
        private Dispatcher dispatcherUIThread;

        private delegate void SetItemCallback(int index, T item);
        private delegate void RemoveItemCallback(int index);
        private delegate void ClearItemsCallback();
        private delegate void InsertItemCallback(int index, T item);
        private delegate void MoveItemCallback(int oldIndex, int newIndex);

        public BeginInvokeOC(Dispatcher dispatcher) {
            this.dispatcherUIThread = dispatcher;
        }

        protected override void SetItem(int index, T item) {
            if (dispatcherUIThread.CheckAccess()) {
                base.SetItem(index, item);
            } else {
                dispatcherUIThread.BeginInvoke(DispatcherPriority.Send, new SetItemCallback(SetItem), index, new object[] { item });
            }
        }

        /// <summary>
        /// Override ToString() method.
        /// </summary>
        /// <returns>Titre de l'élément</returns>
        //public override string ToString() {
        //    return _name;
        //}

        //public override bool Equals(object obj) {
        //    return obj is Item ? this._id == ((Item)obj)._id : false;
        //}
        // Similar code for RemoveItem, ClearItems, InsertItem and MoveItem

    }
}
