using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Threading;
using System.Xml.Linq;

namespace KTouch.Utilities {

    /// <summary>
    /// Provides a threadsafe ObservableCollection of Item objects.
    /// </summary>
    public class ItemCollection : ObservableCollection<XElement> {

        private Dispatcher _dispatcher;
        private ReaderWriterLockSlim _lock;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ItemCollection() {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _lock = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// Clear all items
        /// </summary>
        protected override void ClearItems() {
            _dispatcher.InvokeIfRequired(() => {
                _lock.EnterWriteLock();
                try {
                    base.ClearItems();
                } finally {
                    _lock.ExitWriteLock();
                }
            }, DispatcherPriority.DataBind);
        }

        /// <summary>
        /// Inserts an item
        /// </summary>
        protected override void InsertItem(int index, XElement item) {
            _dispatcher.InvokeIfRequired(() => {
                if(index > this.Count)
                    return;

                _lock.EnterWriteLock();
                try {
                    base.InsertItem(index, item);
                } finally {
                    _lock.ExitWriteLock();
                }
            }, DispatcherPriority.DataBind);

        }

        /// <summary>
        /// Moves an item
        /// </summary>
        protected override void MoveItem(int oldIndex, int newIndex) {
            _dispatcher.InvokeIfRequired(() => {
                _lock.EnterReadLock();
                Int32 itemCount = this.Count;
                _lock.ExitReadLock();

                if(oldIndex >= itemCount |
                    newIndex >= itemCount |
                    oldIndex == newIndex)
                    return;

                _lock.EnterWriteLock();
                try {
                    base.MoveItem(oldIndex, newIndex);
                } finally {
                    _lock.ExitWriteLock();
                }
            }, DispatcherPriority.DataBind);
        }

        /// <summary>
        /// Removes an item
        /// </summary>
        protected override void RemoveItem(int index) {
            _dispatcher.InvokeIfRequired(() => {
                if(index >= this.Count)
                    return;

                _lock.EnterWriteLock();
                try {
                    base.RemoveItem(index);
                } finally {
                    _lock.ExitWriteLock();
                }
            }, DispatcherPriority.DataBind);
        }

        /// <summary>
        /// Sets an item
        /// </summary>
        protected override void SetItem(int index, XElement item) {
            _dispatcher.InvokeIfRequired(() => {
                _lock.EnterWriteLock();
                try {
                    base.SetItem(index, item);
                } finally {
                    _lock.ExitWriteLock();
                }
            }, DispatcherPriority.DataBind);
        }

        /// <summary>
        /// Return as a cloned copy of this Collection
        /// </summary>
        public XElement[] ToSyncArray() {
            _lock.EnterReadLock();
            try {
                XElement[] _sync = new XElement[this.Count];
                this.CopyTo(_sync, 0);
                return _sync;
            } finally {
                _lock.ExitReadLock();
            }
        }
    }

    /// <summary>
    /// WPF Threading extension methods
    /// </summary>
    public static class WPFControlThreadingExtensions {

        /// <summary>
        /// A simple WPF threading extension method, to invoke a delegate
        /// on the correct thread if it is not currently on the correct thread
        /// Which can be used with DispatcherObject types
        /// </summary>
        /// <param name="disp">The Dispatcher object on which to do the Invoke</param>
        /// <param name="dotIt">The delegate to run</param>
        /// <param name="priority">The DispatcherPriority</param>
        public static void InvokeIfRequired(this Dispatcher disp, Action dotIt, DispatcherPriority priority) {
            if(disp.Thread != Thread.CurrentThread) {
                disp.Invoke(priority, dotIt);
            } else
                dotIt();
        }
    }
}

