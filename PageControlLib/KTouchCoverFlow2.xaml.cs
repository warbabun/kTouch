using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CoverFlowBase;
using Microsoft.Surface.Presentation.Controls;
using Blake.NUI.WPF.Gestures;
using System.Windows.Media.Animation;
using KTouch.Units;
using KTouch.Controls.Core;

namespace KTouch.Controls {
    /// <summary>
    /// Interaction logic for KTouchCoverFlow2.xaml
    /// </summary>
    public partial class KTouchCoverFlow2 : KTouchActiveControl {

        UIFlow3D _itemsPanel {
            get;
            set;
        }

        public Point AnimationStartPoint {
            get { return (Point)GetValue(AnimationStartPointProperty); }
            set { SetValue(AnimationStartPointProperty, value); }
        }

        public static readonly DependencyProperty AnimationStartPointProperty =
            DependencyProperty.Register("AnimationStartPoint", typeof(Point), typeof(KTouchCoverFlow2), new FrameworkPropertyMetadata(new Point(0, 0)));

        public KTouchCoverFlow2() {
            InitializeComponent();
            Events.RegisterGestureEventSupport(this);
            Events.AddTapGestureHandler(this, new GestureEventHandler(OnTapGesture));
            this.Loaded += OnLoaded;
            this.IsMovementBigEnough = StaticAccessors.IsVerticalMovementBigEnough;
        }

        private void OnLoaded(object sender, RoutedEventArgs e) {
            _itemsPanel = FindItemsPanel<UIFlow3D>(coverFlow);
            if (_itemsPanel == null) {
                throw new ArgumentNullException("_itemsPanel");
            }

            AnimationStartPoint = new Point(Page.ActualWidth / 2.0, Page.ActualHeight);

        }

        private double func(double x, double deltaY = 0) {
            return -2 * Math.Abs(x - Application.Current.MainWindow.ActualWidth / 2.0) + (Application.Current.MainWindow.ActualHeight - deltaY);
        }

        private void OnTapGesture(object sender, RoutedEventArgs e) {
            try {
                var kTouchScatterView = (KTouchScatterView)Page.FindName("scatterView");
                var dropTarget = (ScatterView)kTouchScatterView.FindName("dropTarget");

                ObservableCollection<KTouchItem> dropTargetCollection = (ObservableCollection<KTouchItem>)dropTarget.ItemsSource;
                ObservableCollection<KTouchItem> sourceCollection = (ObservableCollection<KTouchItem>)this.ItemsSource;

                if (sourceCollection.Count < 3)
                    return;

                int centerIndx = this.SelectedIndex;
                if (centerIndx == -1)
                    centerIndx++;

                int rightIndx = centerIndx + 1;
                if (rightIndx > sourceCollection.Count - 1)
                    rightIndx = 0;

                int leftIndx = centerIndx - 1;
                if (leftIndx < 0)
                    leftIndx = sourceCollection.Count - 1;

                dropTargetCollection.Add(sourceCollection[rightIndx]);
                dropTargetCollection.Add(sourceCollection[leftIndx]);
                dropTargetCollection.Add(sourceCollection[centerIndx]);

                double x0 = Application.Current.MainWindow.ActualWidth / 2.0;
                double y0 = Application.Current.MainWindow.ActualHeight;
                double deltaY = 400;

                List<Point> pointList = new List<Point> {
                    new Point(x0 + 100, func(x0 + 100, deltaY)),
                    new Point(x0 - 100, func(x0 - 100, deltaY)),
                    new Point(x0, y0 - 500),
                };

                var board = ((Storyboard)this.FindResource("PopInScatterViewItems")).Clone();

                List<ScatterViewItem> sviList = new List<ScatterViewItem>{
                    (ScatterViewItem)dropTarget.ItemContainerGenerator.ContainerFromIndex(dropTargetCollection.Count - 3),
                    (ScatterViewItem)dropTarget.ItemContainerGenerator.ContainerFromIndex(dropTargetCollection.Count - 2),
                    (ScatterViewItem)dropTarget.ItemContainerGenerator.ContainerFromIndex(dropTargetCollection.Count - 1),
                };

                for (int i = 0; i < sviList.Count; i++) {
                    sourceCollection.Remove(dropTargetCollection[dropTargetCollection.Count - 1 - i]);

                    var movement = (PointAnimationUsingPath)board.Children[3 * i];
                    var activation = (BooleanAnimationUsingKeyFrames)board.Children[3 * i + 1];
                    var rotation = (DoubleAnimationUsingKeyFrames)board.Children[3 * i + 2];

                    PathFigure pFigure = new PathFigure();
                    pFigure.StartPoint = new Point(x0, y0);
                    pFigure.Segments.Add(new LineSegment(pointList[i], false));

                    PathGeometry pathGeometry = new PathGeometry();
                    pathGeometry.Figures.Add(pFigure);
                    movement.PathGeometry = pathGeometry;


                    Storyboard.SetTarget(movement, sviList[i]);
                    Storyboard.SetTarget(activation, sviList[i]);
                    Storyboard.SetTarget(rotation, sviList[i]);
                }

                board.Completed += (_sender, _e) => {
                    for (int i = 0; i < sviList.Count; i++) {
                        //  double orientation = -20 + 20 * i;
                        var item = sviList[i];
                        if (((KTouchItem)item.DataContext).Type.Equals("xps")) {
                            item.Height = 400;
                            item.Width = 300;
                            item.MinHeight = 200;
                            item.MinWidth = 150;
                            item.MaxHeight = dropTarget.ActualHeight * 1.2;
                            item.MaxWidth = dropTarget.ActualHeight * 1.2 * 0.75;
                        } else {
                            item.Height = 300;
                            item.Width = 300;
                            item.MinHeight = 150;
                            item.MinWidth = 150;
                            item.MaxHeight = dropTarget.ActualHeight;
                            item.MaxWidth = dropTarget.ActualHeight;

                        }
                        //  item.Center = pointList[i];
                        //  item.Orientation = orientation;
                    }
                    var videoItem = sviList[sviList.Count-1];
                    if (((KTouchItem)videoItem.DataContext).Type.Equals("vid")) {
                        var contentPresenter = ((Border)videoItem.Template.FindName("contentBorder", videoItem)).Child;
                        var video = ((Grid)VisualTreeHelper.GetChild(contentPresenter, 0)).Children[0] as MediaElement;
                        if (video != null)
                            video.Play();
                    }
                };

                board.Begin();

            } catch { }
            e.Handled = true;

        }

        
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="oldValue"></param>
        ///// <param name="newValue"></param>
        //protected override void OnItemsSourceChanged(ObservableCollection<KTouchItem> oldValue, ObservableCollection<KTouchItem> newValue) {
        //    if (newValue != null) {
        //        this.coverFlow.ItemsSource = newValue;
        //        this.SelectedIndex = (int)(newValue.Count / 2.0);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        ///  <param name="e"></param>
        protected override void OnPreviewTouchMove(TouchEventArgs e) {
            var currentPosition = e.GetTouchPoint(this).Position;
            int selection = _itemsPanel.TryViewportHitTest(currentPosition);
            if (selection != -1) {
                SelectedIndex = selection;
                if (IsMovementBigEnough(InitialPosition, currentPosition, 20)) {
                    DraggedElement = (ContentPresenter)coverFlow.ItemContainerGenerator.ContainerFromIndex(selection); //Determine which element was touched 
                    if ((((FrameworkElement)e.OriginalSource).TouchesCaptured as List<TouchDevice>).Count == 0)
                        ((FrameworkElement)e.OriginalSource).CaptureTouch(e.TouchDevice);
                    base.OnPreviewTouchMove(e);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="visual"></param>
        /// <returns></returns>
        internal T FindItemsPanel<T>(Visual visual) {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++) {
                Visual child = VisualTreeHelper.GetChild(visual, i) as Visual;
                if (child != null) {
                    if (child is T && VisualTreeHelper.GetParent(child) is ItemsPresenter) {
                        object temp = child;
                        return (T)temp;
                    }
                    T panel = FindItemsPanel<T>(child);
                    if (panel != null) {
                        object temp = panel;
                        return (T)temp; // return the panel up the call stack
                    }
                }
            }
            return default(T);
        }
    }
}
