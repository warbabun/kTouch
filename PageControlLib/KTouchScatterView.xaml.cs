using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Blake.NUI.WPF.Gestures;
using KTouch.Controls.Core;
using KTouch.Units;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;

namespace KTouch.Controls {
    /// <summary>
    /// Interaction logic for ScatterView.xaml
    /// </summary>
    public partial class KTouchScatterView : KTouchActiveControl {
        private List<MediaElement> _mediaPlayers = new List<MediaElement> ( );

        public KTouchScatterView ( ) {
            this.InitializeComponent ( );
            Events.RegisterGestureEventSupport ( this );
            IsMovementBigEnough = StaticAccessors.IsMovementBigEnough;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTapGesture ( object sender, GestureEventArgs e ) {
            foreach ( var player in _mediaPlayers ) {
                player.Pause ( );
            }
            var svi = ( ScatterViewItem ) StaticAccessors.FindAncestor ( typeof ( ScatterViewItem ), e.OriginalSource );
            if ( svi != null )
                KTouchPage.ShowInViewer ( ( KTouchItem ) svi.DataContext );
            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnPreviewTouchDown ( TouchEventArgs e ) {
            base.OnPreviewTouchDown ( e );
            DraggedElement = ( ScatterViewItem ) StaticAccessors.FindAncestor ( typeof ( ScatterViewItem ), e.OriginalSource );
        }

        protected override void OnPreviewTouchMove ( TouchEventArgs e ) {
            if ( IsMovementBigEnough ( InitialPosition, e.GetTouchPoint ( this ).Position, 5 ) ) {
                base.OnPreviewTouchMove ( e );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void SetElement ( KTouchItem element, Point position ) {
            base.SetElement ( element, position );

            var dragItem = ( ScatterViewItem ) dropTarget.ItemContainerGenerator.ContainerFromItem ( element );
            dragItem.Center = position;
            dragItem.Orientation = 0;

            if ( ( ( KTouchItem ) dragItem.DataContext ).Type.Equals ( "xps" ) ) {
                dragItem.Height = 400;
                dragItem.Width = 300;
                dragItem.MinHeight = 200;
                dragItem.MinWidth = 150;
                dragItem.MaxHeight = dropTarget.ActualHeight * 1.2;
                dragItem.MaxWidth = dropTarget.ActualHeight * 1.2 * 0.75;
            } else {
                dragItem.Height = 300;
                dragItem.Width = 300;
                dragItem.MinHeight = 150;
                dragItem.MinWidth = 150;
                dragItem.MaxHeight = dropTarget.ActualHeight;
                dragItem.MaxWidth = dropTarget.ActualHeight;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnDragCompleted ( object sender, SurfaceDragCompletedEventArgs e ) {
            base.OnDragCompleted ( sender, e );

            var element = ( KTouchItem ) e.Cursor.Data;
            var coverFlow = ( KTouchCoverFlow2 ) Page.FindName ( "coverFlow" );
            if ( coverFlow != null && e.Cursor.GetPosition ( this ).X > dropTarget.ActualWidth ||
                e.Cursor.GetPosition ( this ).X < 0 ||
                e.Cursor.GetPosition ( this ).Y < 0 ||
                e.Cursor.GetPosition ( this ).Y > ( dropTarget.ActualHeight - coverFlow.ActualHeight ) ) {
                try {
                    ObservableCollection<KTouchItem> collection = ( ObservableCollection<KTouchItem> ) coverFlow.ItemsSource;
                    var board = ( ( Storyboard ) this.FindResource ( "PopOutScatterViewItems" ) ).Clone ( );
                    var svItem = ( ScatterViewItem ) dropTarget.ItemContainerGenerator.ContainerFromItem ( element );

                    svItem.Visibility = Visibility.Visible;

                    var movement = ( PointAnimationUsingPath ) board.Children [ 0 ];

                    double x0 = Application.Current.MainWindow.ActualWidth / 2.0;
                    double y0 = Application.Current.MainWindow.ActualHeight;

                    PathFigure pFigure = new PathFigure ( );
                    pFigure.StartPoint = e.Cursor.GetPosition ( this );
                    pFigure.Segments.Add ( new LineSegment ( new Point ( x0, y0 ), false ) );

                    PathGeometry pathGeometry = new PathGeometry ( );
                    pathGeometry.Figures.Add ( pFigure );
                    movement.PathGeometry = pathGeometry;

                    foreach ( var animation in board.Children )
                        Storyboard.SetTarget ( animation, svItem );

                    board.Completed += ( _sender, _e ) => {
                        ItemsSource.Remove ( element );
                        if ( !collection.Contains ( element ) ) {
                            collection.Add ( element );
                            coverFlow.UpdateLayout ( );
                        }
                    };

                    board.Begin ( );
                } catch { }
            }
            e.Handled = true;
        }

        private void Play ( object sender, RoutedEventArgs e ) {
            var btn = ( SurfaceButton ) e.OriginalSource;
            var video = ( ( Grid ) VisualTreeHelper.GetParent ( btn ) ).Children [ 0 ] as MediaElement;
            if ( btn != null && video != null ) {
                switch ( btn.Name ) {
                    case "Play":
                        video.Play ( );
                        break;
                    case "Pause":
                        video.Pause ( );
                        break;
                }
            } else {
                var svi = sender as ScatterViewItem;
                if ( svi != null )
                    KTouchPage.ShowInViewer ( ( KTouchItem ) svi.DataContext );
            }
            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaElement_Loaded ( object sender, RoutedEventArgs e ) {
            var media = ( MediaElement ) sender;
            _mediaPlayers.Add ( media );
            media.Pause ( );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaElement_Unloaded ( object sender, RoutedEventArgs e ) {
            var media = ( MediaElement ) sender;
            media.Stop ( );
            _mediaPlayers.Remove ( media );
        }
    }
}