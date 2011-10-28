using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Collections.Specialized;

namespace CoverFlowBase {
    /// <summary>
    /// This panel extends ConceptualPanel by ensuring that its conceptual children are also "logical" children.  
    /// Because certain things like property inheritance and resource resolution work through the logical
    /// tree, this allows the disconnected visuals to be connected to the panel's ancestor tree
    /// in a logical manner without being part of it's visual tree. 
    /// </summary>
    public abstract class LogicalPanel : ConceptualPanel {
        protected sealed override void OnChildAdded(UIElement child) {
            // if the child does not have a logical parent, assume the role
            if (LogicalTreeHelper.GetParent(child) == null) {
                AddLogicalChild(child);
            }
            OnLogicalChildrenChanged(child, null);
        }

        protected sealed override void OnChildRemoved(UIElement child) {
            // if this panel is the logical parent, remove that relationship
            if (LogicalTreeHelper.GetParent(child) == this) {
                RemoveLogicalChild(child);
            }
            OnLogicalChildrenChanged(null, child);
        }

        /// <summary>
        /// This class uses the OnLogicalChildrenChanged method to provide notification to descendants 
        /// when its logical children change.  Note that this is intentionally
        /// similar to the OnVisualChildrenChanged approach supported by all visuals.
        /// </summary>
        /// <param name="childAdded"></param>
        /// <param name="childRemoved"></param>
        protected virtual void OnLogicalChildrenChanged(UIElement childAdded, UIElement childRemoved) {
        }
    }
}