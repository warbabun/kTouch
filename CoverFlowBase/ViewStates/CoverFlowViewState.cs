using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System;

namespace CoverFlowBase {
    internal class CoverFlowViewState : ViewStateBase {
        protected override Storyboard PrepareItemAnimation(UIFlow3D owner, int index, UIFlow3D.ElementAnimationType type) {
            // Initialize storyboard
            Storyboard sb = (owner.InternalResources["ItemAnimator2"] as Storyboard).Clone();
            owner.PrepareTemplateStoryboard(sb, index);

            // Child animations
            Rotation3DAnimationUsingKeyFrames rotAnim = sb.Children[0] as Rotation3DAnimationUsingKeyFrames;
            DoubleAnimationUsingKeyFrames xAnim = sb.Children[1] as DoubleAnimationUsingKeyFrames;
            DoubleAnimationUsingKeyFrames yAnim = sb.Children[2] as DoubleAnimationUsingKeyFrames;
            DoubleAnimationUsingKeyFrames zAnim = sb.Children[3] as DoubleAnimationUsingKeyFrames;

            SplineRotation3DKeyFrame rotKf = rotAnim.KeyFrames[0] as SplineRotation3DKeyFrame;

            switch (type) {
                case UIFlow3D.ElementAnimationType.Left:
                    (rotKf.Value as AxisAngleRotation3D).Angle = owner.TiltAngle;
                    xAnim.KeyFrames[0].Value = -1 * owner.ItemGap * (owner.SelectedIndex - index) - owner.FrontItemGap;
                    zAnim.KeyFrames[0].Value = -1 * owner.PopoutDistance * (owner.SelectedIndex - index) * (owner.SelectedIndex - index) / 24;

                    break;
                case UIFlow3D.ElementAnimationType.Right:
                    (rotKf.Value as AxisAngleRotation3D).Angle = -1 * owner.TiltAngle;
                    xAnim.KeyFrames[0].Value = owner.ItemGap * (index - owner.SelectedIndex) + owner.FrontItemGap;
                    zAnim.KeyFrames[0].Value = -1 * owner.PopoutDistance * (index - owner.SelectedIndex) * (index - owner.SelectedIndex) / 24;

                    break;
                case UIFlow3D.ElementAnimationType.Selection:
                    (rotKf.Value as AxisAngleRotation3D).Angle = 0;
                    xAnim.KeyFrames[0].Value = 0;
                    zAnim.KeyFrames[0].Value = owner.PopoutDistance;

                    break;
            }

            return sb;
        }

        private void SetAnimation() {

        }
    }
}