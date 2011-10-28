using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Diagnostics;
using Microsoft.Surface.Presentation.Controls;

namespace KTouch.Controls {
    /// <summary>
    /// Interaction logic for KTouchStack.xaml
    /// </summary>
    public partial class KTouchStack : LibraryStack {

        public KTouchStack() {
            InitializeComponent();
        }

        void KTouchStack_Loaded(object sender, RoutedEventArgs e) {
            var template = (ControlTemplate)this.Template;

            var popup = (KTouchContactsForm)template.FindName("MailPopup", this);
            if (popup != null) {
                //  popup.Preferences = arguments;
                popup.IsOpen = true;
            }
        }

        //public void MailButtonClick(object sender, RoutedEventArgs e) {

        //    if ((((SurfaceButton)e.OriginalSource)).Name != "MailButton" &&  ! (this.Items.Count > 0))
        //        return;
        //    string arguments = "";
        //    foreach (KTouchItem item in ItemsSource) {
        //        arguments += item.ToString() + ";";
        //    }


        //}
    }
}
