using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Collections.Specialized;
using System.Windows.Media;

namespace CoverFlowBase {
    /// <summary>
    /// This panel maintains a collection of conceptual children that are neither logical
    /// children nor visual children of the panel.  This allows those visuals to be connected 
    /// to other parts of the UI, if necessary, or even to remain disconnected. 
    /// </summary>
    public abstract class ConceptualPanel : Panel {
        public ConceptualPanel() {
            Loaded += OnLoaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e) {
            Loaded -= OnLoaded;
            (Children as DisconnectedUIElementCollection).Initialize();
        }

        protected override sealed UIElementCollection CreateUIElementCollection(FrameworkElement logicalParent) {
            DisconnectedUIElementCollection children = new DisconnectedUIElementCollection(this);
            children.CollectionChanged += new NotifyCollectionChangedEventHandler(OnChildrenCollectionChanged);
            return children;
        }

        protected virtual void OnChildAdded(UIElement child) {
        }

        protected virtual void OnChildRemoved(UIElement child) {
        }

        /// <summary>
        /// For simplicity, this class will listen to change notifications on the DisconnectedUIElementCollection
        /// and provide them to descendants through the OnChildAdded and OnChildRemoved members.  
        /// </summary>
        private void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    OnChildAdded(e.NewItems[0] as UIElement);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    OnChildRemoved(e.OldItems[0] as UIElement);
                    break;
            }
        }

        protected override int VisualChildrenCount {
            get { return _visualChildren.Count; }
        }

        protected override Visual GetVisualChild(int index) {
            if (index < 0 || index >= _visualChildren.Count)
                throw new ArgumentOutOfRangeException();
            return _visualChildren[index];
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved) {
            if (visualAdded is Visual) {
                _visualChildren.Add(visualAdded as Visual);
            }

            if (visualRemoved is Visual) {
                _visualChildren.Remove(visualRemoved as Visual);
            }

            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }

        private readonly List<Visual> _visualChildren = new List<Visual>();
    }
}
