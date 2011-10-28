using System.Windows.Media.Animation;

namespace CoverFlowBase {
    internal abstract class ViewStateBase {
        public void SelectElement(UIFlow3D owner, int index) {
            Storyboard anim;
            for (int leftItem = 0; leftItem < index; leftItem++) {
                anim = PrepareItemAnimation(owner, leftItem, UIFlow3D.ElementAnimationType.Left);
                owner.AnimateElement(anim);
            }

            anim = PrepareItemAnimation(owner, index, UIFlow3D.ElementAnimationType.Selection);
            owner.AnimateElement(anim);

            for (int rightItem = index + 1; rightItem < owner.VisibleChildrenCount; rightItem++) {
                anim = PrepareItemAnimation(owner, rightItem, UIFlow3D.ElementAnimationType.Right);
                owner.AnimateElement(anim);
            }
        }

        protected virtual Storyboard PrepareItemAnimation(UIFlow3D owner, int index, UIFlow3D.ElementAnimationType type) {
            return null;
        }
    }
}