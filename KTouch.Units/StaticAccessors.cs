using System;
using System.Windows;
using System.Windows.Media;

namespace KTouch.Units {
    public static class StaticAccessors {
        public static object FindAncestor ( Type ancestorType, object visual ) {
            DependencyObject dobj = visual as DependencyObject;
            while ( dobj != null && !ancestorType.IsInstanceOfType ( dobj ) ) {
                dobj = VisualTreeHelper.GetParent ( dobj );
            }
            return dobj;
        }

        public static bool IsMovementBigEnough ( Point initialPosition, Point currentPosition, int m ) {
            return ( Math.Abs ( currentPosition.X - initialPosition.X ) >= ( SystemParameters.MinimumHorizontalDragDistance * m ) ||
                 Math.Abs ( currentPosition.Y - initialPosition.Y ) >= ( SystemParameters.MinimumVerticalDragDistance * m ) );
        }

        public static bool IsHorizontalMovementBigEnough ( Point initialPosition, Point currentPosition, int m ) {
            return ( Math.Abs ( currentPosition.X - initialPosition.X ) >= ( SystemParameters.MinimumHorizontalDragDistance * m ) );
        }

        public static bool IsVerticalMovementBigEnough ( Point initialPosition, Point currentPosition, int m ) {
            return ( Math.Abs ( currentPosition.Y - initialPosition.Y ) >= ( SystemParameters.MinimumVerticalDragDistance * m ) );
        }

        //public static bool capturedTouch(IEnumerable<TouchDevice> list) {
        //    return (new List<TouchDevice>(list)).Count > 0;
        //}

        //public static DependencyObject FindChildDep(DependencyObject parent) {

        //    // Confirm parent and childName are valid. 
        //    if (parent == null) return null;

        //    var parentUIEl = parent as UIElement;
        //    var parentUIEl3D = parent as UIElement3D;

        //    if (parentUIEl == null && parentUIEl3D == null)
        //        return null;

        //    else if (parentUIEl != null && capturedTouch(parentUIEl.TouchesCaptured))
        //        return parent;

        //    else if (parentUIEl3D != null && capturedTouch(parentUIEl3D.TouchesCaptured))
        //        return parent;

        //    DependencyObject foundChild = null;

        //    int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
        //    for (int i = 0; i < childrenCount; i++) {
        //        var child = VisualTreeHelper.GetChild(parent, i);
        //        // If the child is not of the request child type child

        //        //if (child as UIElement == null)
        //        //    continue;

        //        // recursively drill down the tree
        //        foundChild = FindChildDep(child);

        //        // If the child is found, break so we do not overwrite the found child. 
        //        if (foundChild != null) break;
        //    }

        //    return foundChild;
        //}

        //public static UIElement FindChild(UIElement parent) {

        //    // Confirm parent and childName are valid. 
        //    if (parent == null) return null;

        //    if (capturedTouch(parent.TouchesCaptured))
        //        return parent;

        //    UIElement foundChild = null;

        //    int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
        //    for (int i = 0; i < childrenCount; i++) {
        //        var child = VisualTreeHelper.GetChild(parent, i);
        //        // If the child is not of the request child type child
        //        if (child as UIElement == null)
        //            continue;
        //        // recursively drill down the tree
        //        foundChild = FindChild((UIElement)child);

        //        // If the child is found, break so we do not overwrite the found child. 
        //        if (foundChild != null) break;
        //    }

        //    return foundChild;
        //}

        //public static UIElement3D FindChild3D(UIElement3D parent) {

        //    // Confirm parent and childName are valid. 
        //    if (parent == null) return null;

        //    if (capturedTouch(parent.TouchesCaptured))
        //        return parent;

        //    UIElement3D foundChild = null;

        //    int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
        //    for (int i = 0; i < childrenCount; i++) {
        //        var child = VisualTreeHelper.GetChild(parent, i);
        //        // If the child is not of the request child type child
        //        if (child as UIElement3D == null)
        //            continue;
        //        // recursively drill down the tree
        //        foundChild = FindChild3D((UIElement3D)child);

        //        // If the child is found, break so we do not overwrite the found child. 
        //        if (foundChild != null) break;
        //    }

        //    return foundChild;
        //}
    }
}
