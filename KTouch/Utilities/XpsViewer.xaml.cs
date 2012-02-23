//-----------------------------------------------------------------------
// <copyright file="XpsViewer.xaml.cs" company="Klee Group">
//     Copyright (c) Klee Group. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Xps.Packaging;
using System.Xml.Linq;
using KTouch.Properties;
using Microsoft.Surface.Presentation.Input;

namespace KTouch.Utilities {

    /// <summary>
    /// Interaction logic for XpsViewer.xaml
    /// </summary>
    public partial class XpsViewer : UserControl {

        /// <summary>
        /// Source DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(XElement), typeof(XpsViewer), new FrameworkPropertyMetadata(new PropertyChangedCallback(SourceChangedCallback)));

        /// <summary>
        /// Initializes a new instance of the XpsViewer class.
        /// </summary>
        public XpsViewer() {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(XpsViewer_Loaded);
        }

        /// <summary>
        /// An event that clients can use to be notified whenever the content changes.
        /// </summary>
        public event ChangedEventHandler SourceChanged;

        /// <summary>
        /// Exposes a FixedDocumentSequence for XpsViewer's source.
        /// </summary>
        public XElement Source {
            get { return (XElement)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// DP callback on Source changed.
        /// </summary>
        /// <param name="sender">DependencyObject sender.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs argument.</param>
        public static void SourceChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            XElement item = (XElement)e.NewValue;
            XpsViewer viewer = (XpsViewer)sender;
            viewer.xpsViewer.Document = (new XpsDocument((string)item.Attribute(Tags.FullName), FileAccess.Read)).GetFixedDocumentSequence();
            viewer.OnSourceChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Invoke the SourceChanged event; called whenever list changes.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected virtual void OnSourceChanged(EventArgs e) {
            if (SourceChanged != null) {
                SourceChanged(this, e);
            }
        }

        /// <summary>
        /// Handles Loaded event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        private void XpsViewer_Loaded(object sender, RoutedEventArgs e) {
            this.xpsViewer.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(manipulationDelta);
            this.xpsViewer.ManipulationStarting += new EventHandler<ManipulationStartingEventArgs>(manipulationStarting);
            this.xpsViewer.ManipulationInertiaStarting += new EventHandler<ManipulationInertiaStartingEventArgs>(inertiaStarting);
            TouchExtensions.AddTapGestureHandler(this.xpsViewer, new EventHandler<TouchEventArgs>(OnTapGesture));
            Mouse.AddPreviewMouseUpHandler(this.xpsViewer, new MouseButtonEventHandler(xpsViewer_MouseLeftButtonUp));
        }

        /// <summary>
        /// Handles PreviewMouseLeftButtonUp event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        private void xpsViewer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            this.xpsViewer.Zoom = 66;
            e.Handled = true;
        }

        /// <summary>
        /// Handles Tap event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        private void OnTapGesture(object sender, TouchEventArgs e) {
            this.xpsViewer.Zoom = 66;
            e.Handled = true;
        }

        #region Manipulation

        /// <summary>
        /// Handles ManipulationDelta event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        private void manipulationDelta(object sender, ManipulationDeltaEventArgs e) {
            try {
                if (e.DeltaManipulation.Translation != null) {
                    double verticalOffset = xpsViewer.VerticalOffset - (e.DeltaManipulation.Translation.Y * 3);
                    double horizontalOffset = xpsViewer.HorizontalOffset - (e.DeltaManipulation.Translation.X * 3);
                    if (verticalOffset > 0) {
                        xpsViewer.VerticalOffset = verticalOffset;
                    }
                    if (horizontalOffset > 0) {
                        xpsViewer.HorizontalOffset = horizontalOffset;
                    }
                }
                if (e.DeltaManipulation.Scale.X != 1) {
                    if (xpsViewer.Zoom > 400) {
                        xpsViewer.Zoom = 400;
                    } else if (xpsViewer.Zoom < 65) {
                        xpsViewer.Zoom = 65;
                    } else {
                        xpsViewer.Zoom *= e.DeltaManipulation.Scale.X;
                    }
                }
            } catch {
            } finally {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles ManipulationStarting event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        private void manipulationStarting(object sender, ManipulationStartingEventArgs e) {
            try {
                e.ManipulationContainer = this;
            } catch {
            } finally {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles InerniaStarting event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        private void inertiaStarting(object sender, ManipulationInertiaStartingEventArgs e) {
            e.TranslationBehavior.DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0);
            e.Handled = true;
        }

        #endregion //Manipulation
    }
}
