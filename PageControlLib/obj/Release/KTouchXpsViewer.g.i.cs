﻿#pragma checksum "..\..\KTouchXpsViewer.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B69D7E7CB2FC0EA8D92C55A658D39309"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using KTouch.Controls;
using KTouch.Units;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Controls.Primitives;
using Microsoft.Surface.Presentation.Controls.TouchVisualizations;
using Microsoft.Surface.Presentation.Input;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace KTouch.Controls {
    
    
    /// <summary>
    /// KTouchXpsViewer
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class KTouchXpsViewer : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\KTouchXpsViewer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal KTouch.Controls.KTouchXpsViewer kTouchXpsViewer;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\KTouchXpsViewer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\KTouchXpsViewer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DocumentViewer xpsViewer;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\KTouchXpsViewer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Surface.Presentation.Controls.SurfaceButton CloseBtn;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/KTouch.Controls;component/ktouchxpsviewer.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\KTouchXpsViewer.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.kTouchXpsViewer = ((KTouch.Controls.KTouchXpsViewer)(target));
            
            #line 15 "..\..\KTouchXpsViewer.xaml"
            this.kTouchXpsViewer.ManipulationDelta += new System.EventHandler<System.Windows.Input.ManipulationDeltaEventArgs>(this.manipulationDelta);
            
            #line default
            #line hidden
            
            #line 16 "..\..\KTouchXpsViewer.xaml"
            this.kTouchXpsViewer.ManipulationStarting += new System.EventHandler<System.Windows.Input.ManipulationStartingEventArgs>(this.manipulationStarting);
            
            #line default
            #line hidden
            
            #line 17 "..\..\KTouchXpsViewer.xaml"
            this.kTouchXpsViewer.ManipulationInertiaStarting += new System.EventHandler<System.Windows.Input.ManipulationInertiaStartingEventArgs>(this.inertiaStarting);
            
            #line default
            #line hidden
            return;
            case 2:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.xpsViewer = ((System.Windows.Controls.DocumentViewer)(target));
            return;
            case 4:
            this.CloseBtn = ((Microsoft.Surface.Presentation.Controls.SurfaceButton)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

