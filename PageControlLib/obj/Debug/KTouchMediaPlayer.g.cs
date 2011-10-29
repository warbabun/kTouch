﻿#pragma checksum "..\..\KTouchMediaPlayer.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "BC5985653E60870CECEBDB9B0B2D83EF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Blake.NUI.WPF.Gestures;
using CoverFlowBase;
using KTouch.Controls;
using KTouch.Controls.Core;
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
    /// KTouchMediaPlayer
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class KTouchMediaPlayer : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 6 "..\..\KTouchMediaPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal KTouch.Controls.KTouchMediaPlayer kTouchMediaPlayer;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\KTouchMediaPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\KTouchMediaPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid videoLayot;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\KTouchMediaPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MediaElement mediaPlayerMain;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\KTouchMediaPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Surface.Presentation.Controls.SurfaceButton Play;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\KTouchMediaPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Surface.Presentation.Controls.SurfaceButton Pause;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\KTouchMediaPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Surface.Presentation.Controls.SurfaceButton Stop;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\KTouchMediaPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Surface.Presentation.Controls.SurfaceSlider sliderTime;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\KTouchMediaPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Surface.Presentation.Controls.SurfaceSlider sliderVolume;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\KTouchMediaPlayer.xaml"
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
            System.Uri resourceLocater = new System.Uri("/KTouch.Controls;component/ktouchmediaplayer.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\KTouchMediaPlayer.xaml"
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
            this.kTouchMediaPlayer = ((KTouch.Controls.KTouchMediaPlayer)(target));
            
            #line 11 "..\..\KTouchMediaPlayer.xaml"
            this.kTouchMediaPlayer.Loaded += new System.Windows.RoutedEventHandler(this.kTouchMediaPlayer_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 14 "..\..\KTouchMediaPlayer.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.PlayCommand_Executed);
            
            #line default
            #line hidden
            
            #line 14 "..\..\KTouchMediaPlayer.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.Command_CanExecute);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 15 "..\..\KTouchMediaPlayer.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.PauseCommand_Executed);
            
            #line default
            #line hidden
            
            #line 15 "..\..\KTouchMediaPlayer.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.Command_CanExecute);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 16 "..\..\KTouchMediaPlayer.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.StopCommand_Executed);
            
            #line default
            #line hidden
            
            #line 16 "..\..\KTouchMediaPlayer.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.Command_CanExecute);
            
            #line default
            #line hidden
            return;
            case 5:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            this.videoLayot = ((System.Windows.Controls.Grid)(target));
            return;
            case 7:
            this.mediaPlayerMain = ((System.Windows.Controls.MediaElement)(target));
            
            #line 52 "..\..\KTouchMediaPlayer.xaml"
            this.mediaPlayerMain.MediaEnded += new System.Windows.RoutedEventHandler(this.mediaPlayerMain_MediaEnded);
            
            #line default
            #line hidden
            
            #line 53 "..\..\KTouchMediaPlayer.xaml"
            this.mediaPlayerMain.MediaOpened += new System.Windows.RoutedEventHandler(this.mediaPlayerMain_MediaOpened);
            
            #line default
            #line hidden
            return;
            case 8:
            this.Play = ((Microsoft.Surface.Presentation.Controls.SurfaceButton)(target));
            return;
            case 9:
            this.Pause = ((Microsoft.Surface.Presentation.Controls.SurfaceButton)(target));
            return;
            case 10:
            this.Stop = ((Microsoft.Surface.Presentation.Controls.SurfaceButton)(target));
            return;
            case 11:
            this.sliderTime = ((Microsoft.Surface.Presentation.Controls.SurfaceSlider)(target));
            
            #line 91 "..\..\KTouchMediaPlayer.xaml"
            this.sliderTime.AddHandler(System.Windows.Controls.Primitives.Thumb.DragStartedEvent, new System.Windows.Controls.Primitives.DragStartedEventHandler(this.sliderTime_DragStarted));
            
            #line default
            #line hidden
            
            #line 92 "..\..\KTouchMediaPlayer.xaml"
            this.sliderTime.AddHandler(System.Windows.Controls.Primitives.Thumb.DragCompletedEvent, new System.Windows.Controls.Primitives.DragCompletedEventHandler(this.sliderTime_DragCompleted));
            
            #line default
            #line hidden
            return;
            case 12:
            this.sliderVolume = ((Microsoft.Surface.Presentation.Controls.SurfaceSlider)(target));
            return;
            case 13:
            this.CloseBtn = ((Microsoft.Surface.Presentation.Controls.SurfaceButton)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

