//-----------------------------------------------------------------------
// <copyright file="VideoPage.xaml.cs" company="Klee Group">
//     Copyright (c) Klee Group. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Windows.Controls;
using System.Xml.Linq;
using KTouch.ViewModel;

namespace KTouch.Views {

    /// <summary>
    /// Interaction logic for PresentationPage.xaml
    /// </summary>
    public partial class VideoPage : Page {

        private VideoPageViewModel _vm;

        /// <summary>
        /// Initializes a new instance of the VideoPage class.
        /// </summary>
        /// <param name="item">XElement object.</param>
        public VideoPage(XElement item) {
            InitializeComponent();
            _vm = new VideoPageViewModel(item);
            this.DataContext = _vm;
        }
    }
}
