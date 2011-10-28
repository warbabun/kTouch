using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using KTouch.Units;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;

namespace KTouch.Controls.Core {

    public delegate bool MovementBigEnough ( Point initialPosition, Point currentPosition, int m );

    public class KTouchActiveControl : UserControl {

        public ObservableCollection<KTouchItem> ItemsSource {
            get { return ( ObservableCollection<KTouchItem> ) GetValue ( ItemsSourceProperty ); }
            set { SetValue ( ItemsSourceProperty, value ); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register ( "ItemsSource",
                typeof ( ObservableCollection<KTouchItem> ),
                typeof ( KTouchActiveControl ),
                new FrameworkPropertyMetadata ( null ) );

        public int SelectedIndex {
            get { return ( int ) GetValue ( SelectedIndexProperty ); }
            set { SetValue ( SelectedIndexProperty, value ); }
        }

        public static readonly DependencyProperty SelectedIndexProperty =
           DependencyProperty.Register ( "SelectedIndex",
               typeof ( int ),
               typeof ( KTouchActiveControl ),
               new UIPropertyMetadata ( -1 ) );

        private bool _isManipulated = false;
        protected MovementBigEnough IsMovementBigEnough = StaticAccessors.IsMovementBigEnough;
        protected FrameworkElement DraggedElement;

        private Point _initialPosition;
        protected Point InitialPosition {
            get {
                return this._initialPosition;
            }
            set {
                if ( value != this._initialPosition ) {
                    this._initialPosition = value;
                }
            }
        }

        private KTouchPage _page;
        protected KTouchPage Page {
            get {
                if ( _page == null ) {
                    _page = ( KTouchPage ) StaticAccessors.FindAncestor ( typeof ( KTouchPage ), this );
                }
                return _page;
            }
        }
        /// <summary>
        /// Constructeur.
        /// </summary>
        public KTouchActiveControl ( )
            : base ( ) {
            AddHandler ( SurfaceDragDrop.PreviewDragCompletedEvent, new EventHandler<SurfaceDragCompletedEventArgs> ( this.OnDragCompleted ) );
            AddHandler ( SurfaceDragDrop.PreviewDragCanceledEvent, new EventHandler<SurfaceDragDropEventArgs> ( this.OnDragCanceled ) );
            AddHandler ( SurfaceDragDrop.PreviewDropEvent, new EventHandler<SurfaceDragDropEventArgs> ( this.OnDrop ) );

        }

        protected void CreateCursorForDragging ( DragInfo item ) {
            VisualBrush brush = new VisualBrush { TileMode = TileMode.None };

            bool scale = false;

            switch ( this.Name ) {
                case "coverFlow":
                    brush.Visual = ( ( Grid ) VisualTreeHelper.GetChild ( DraggedElement, 0 ) ).Children [ 0 ];
                    scale = true;
                    break;
                case "scatterView":
                    brush.Visual = ( FrameworkElement ) VisualTreeHelper.GetChild ( DraggedElement, 0 );
                    break;
                default:
                    brush.Visual = ( FrameworkElement ) VisualTreeHelper.GetChild ( DraggedElement, 0 );
                    break;
            }
            brush.AlignmentX = AlignmentX.Center;
            brush.AlignmentY = AlignmentY.Center;
            brush.Stretch = Stretch.Fill;

            item.Cursor = new ContentPresenter ( ) {          //Creating new Cursor for drag visualisation 
                Content = new DragDataContext ( ) {
                    Context = ( KTouchItem ) this.DraggedElement.DataContext,
                    Adorner = brush,
                },
                Style = Application.Current.FindResource ( "CursorStyle2" ) as Style,
            };

            if ( scale ) {

                item.Cursor.Height = 1.5 * this.ActualHeight;
                if ( ( ( KTouchItem ) item.DraggedElement.DataContext ).Type == "xps" )
                    item.Cursor.Width = 1.5 * this.ActualHeight / Math.Sqrt ( 2.0 );
                else
                    item.Cursor.Width = 1.5 * this.ActualHeight;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="item"></param>
        protected void StartDragging ( DragInfo item ) {

            if ( item.DraggedElement != null &&
                item.DraggedElement.DataContext.ToString ( ) != "{DisconnectedItem}" && //TODO: Узнать почему иногда элементы бывают "отсоедины"
                item.Source != null &&
                item.OriginalSource != null ) {

                List<TouchDevice> devices = new List<TouchDevice> ( item.OriginalSource.TouchesCaptured );
                if ( devices.Count < 2 ) {
                    CreateCursorForDragging ( item );

                    var startDragOkay =
                        SurfaceDragDrop.BeginDragDrop (
                            item.Source,                        // The FrameworkElement object that the cursor is dragged out from.
                            item.DraggedElement,                // The FrameworkElement object that is dragged from the drag source.
                            item.Cursor,                        // The visual element of the cursor.
                            item.DraggedElement.DataContext,    // The data associated with the cursor.
                            devices,                            // The input devices that start dragging the cursor.
                            DragDropEffects.Move );              // The allowed drag-and-drop effects of the operation.

                    if ( startDragOkay != null &&
                        !Page.DraggedItemsCollection.ContainsKey ( ( KTouchItem ) item.DraggedElement.DataContext ) ) {

                        item.DraggedElement.Visibility = Visibility.Hidden;
                        Page.DraggedItemsCollection.Add ( ( KTouchItem ) item.DraggedElement.DataContext, item ); //Add the Item to the collection of other items being dragged
                    }
                } else {
                    _isManipulated = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewTouchMove ( TouchEventArgs e ) {
            base.OnPreviewTouchMove ( e );

            StartDragging ( new DragInfo {
                Source = ( FrameworkElement ) e.Source,
                OriginalSource = ( FrameworkElement ) e.OriginalSource,
                DraggedElement = DraggedElement,

            } );
            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewTouchDown ( TouchEventArgs e ) {
            base.OnPreviewTouchDown ( e );
            //    Console.WriteLine("KTouchActiveControl_OnPreviewTouchDown");

            _initialPosition = e.GetTouchPoint ( this ).Position;
            DraggedElement = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewTouchUp ( TouchEventArgs e ) {
            base.OnPreviewTouchUp ( e );
            //   Console.WriteLine("KTouchActiveControl_OnPreviewTouchUp");

            _isManipulated = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnDragCompleted ( object sender, SurfaceDragCompletedEventArgs e ) {
            //   Console.WriteLine("KTouchActiveControl_DragCompleted");

            if ( e.Cursor.Effects == DragDropEffects.Move && Page.DraggedItemsCollection.ContainsKey ( ( KTouchItem ) e.Cursor.Data ) ) {
                if ( SelectedIndex > 0 ) {
                    SelectedIndex--;
                } else {
                    SelectedIndex = 0;
                }

                ItemsSource.Remove ( ( KTouchItem ) e.Cursor.Data );
                Page.DraggedItemsCollection.Remove ( ( KTouchItem ) e.Cursor.Data );      //Removing the item from drag collection
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnDragCanceled ( object sender, SurfaceDragDropEventArgs e ) {
            //    Console.WriteLine("KTouchActiveControl_OnDragCanceled");

            Page.DraggedItemsCollection [ ( KTouchItem ) ( e.Cursor.Data ) ].DraggedElement.Visibility = Visibility.Visible; //Recovering the hidden element
            Page.DraggedItemsCollection.Remove ( ( KTouchItem ) e.Cursor.Data );      //Removing the item from drag collection

            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnDrop ( object sender, SurfaceDragDropEventArgs e ) {
            //    Console.WriteLine("KTouchActiveControl_OnDrop");

            var element = ( KTouchItem ) e.Cursor.Data;
            if ( !ItemsSource.Contains ( element ) ) {
                ItemsSource.Add ( element );
                SetElement ( element, e.Cursor.GetPosition ( this ) );
            } else if ( Page.DraggedItemsCollection.ContainsKey ( element ) ) {
                var draggedElement = ( FrameworkElement ) Page.DraggedItemsCollection [ element ].DraggedElement; //Recovering the hidden element
                draggedElement.Visibility = Visibility.Visible;
                if ( draggedElement is ScatterViewItem )
                    ( ( ScatterViewItem ) draggedElement ).Center = e.Cursor.GetPosition ( this );
                Page.DraggedItemsCollection.Remove ( element );      //Removing the item from drag collection
            }
            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        protected virtual void SetElement ( KTouchItem element, Point p ) {
            //   Console.WriteLine("KTouchActiveControl_ChangeElementForDrop");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        protected void PopIn ( FrameworkElement element, string name, double from = 0, double to = 0 ) {
            //   Console.WriteLine("KTouchActiveControl_PopIn");

            var storyboard = ( ( Storyboard ) element.FindResource ( name ) ).Clone ( );
            var animation = ( DoubleAnimation ) storyboard.Children [ 0 ];

            animation.From = from;
            animation.To = to;

            storyboard.Begin ( element );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        protected void PopOut ( FrameworkElement element, string name, double from = 0, double to = 0 ) {
            //   Console.WriteLine("KTouchActiveControl_PopOut");

            var storyboard = ( ( Storyboard ) element.FindResource ( name ) ).Clone ( );
            var animation = ( DoubleAnimation ) storyboard.Children [ 0 ];

            animation.From = from;
            animation.To = to;

            storyboard.Begin ( element );
        }
    }
}
