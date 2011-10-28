using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Xml;
using System.Xml.Serialization;
using CoverFlowBase;
using KTouch.Controls.Core;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Win32;

namespace KTouch.Controls {
    /// <summary>
    /// Interaction logic for KTouchParametersListBox.xaml
    /// </summary>
    public partial class KTouchParametersListBox : UserControl {

        /// <summary>
        /// ElementFlow that has to be ajusted
        /// TODO: REMOVE
        /// </summary>
        private UIFlow3D _elementFlow;
        public UIFlow3D elementFlow {
            get { return _elementFlow; }
            set { _elementFlow = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public KTouchParametersListBox ( ) {
            InitializeComponent ( );
        }

        /// <summary>
        /// RegulationSlider value changed event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Values_SnappedValueChanged ( object sender, RoutedPropertyChangedEventArgs<double> e ) {
            if ( IsLoaded ) {
                SurfaceSlider current = ( SurfaceSlider ) e.OriginalSource;
                Label currentlabel = ( Label ) ( ( Grid ) VisualTreeHelper.GetParent ( current ) ).Children [ 0 ];
                currentlabel.Content = current.Name + ": " + e.NewValue.ToString ( );
                switch ( current.Name ) {
                    case "TiltAngle":
                        elementFlow.TiltAngle = e.NewValue;
                        break;
                    case "ItemGap":
                        elementFlow.ItemGap = e.NewValue;
                        break;
                    case "PopoutDistance":
                        elementFlow.PopoutDistance = e.NewValue;
                        break;
                    case "FrontItemGap":
                        elementFlow.FrontItemGap = e.NewValue;
                        break;
                    case "FieldOfView":
                        elementFlow.Camera.FieldOfView = e.NewValue;
                        break;
                    case "LookDirectionX":
                        elementFlow.Camera.LookDirection = new Vector3D ( e.NewValue, elementFlow.Camera.LookDirection.Y, elementFlow.Camera.LookDirection.Z );
                        break;
                    case "LookDirectionY":
                        elementFlow.Camera.LookDirection = new Vector3D ( elementFlow.Camera.LookDirection.X, e.NewValue, elementFlow.Camera.LookDirection.Z );
                        break;
                    case "LookDirectionZ":
                        elementFlow.Camera.LookDirection = new Vector3D ( elementFlow.Camera.LookDirection.X, elementFlow.Camera.LookDirection.Y, e.NewValue );
                        break;
                    case "PositionX":
                        elementFlow.Camera.Position = new Point3D ( e.NewValue, elementFlow.Camera.Position.Y, elementFlow.Camera.Position.Z );
                        break;
                    case "PositionY":
                        elementFlow.Camera.Position = new Point3D ( elementFlow.Camera.Position.X, e.NewValue, elementFlow.Camera.Position.Z );
                        break;
                    case "PositionZ":
                        elementFlow.Camera.Position = new Point3D ( elementFlow.Camera.Position.X, elementFlow.Camera.Position.Y, e.NewValue );
                        break;
                }
            }
        }

        /// <summary>
        /// Set current parameters of KTouchCoverFlow to a respective regulator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded ( object sender, RoutedEventArgs e ) {
            if ( _elementFlow != null ) {
                FieldOfView.Value = elementFlow.Camera.FieldOfView;
                lblFieldOfView.Content = FieldOfView.Name + ": " + FieldOfView.Value;

                FrontItemGap.Value = elementFlow.FrontItemGap;
                lblFrontItemGap.Content = FrontItemGap.Name + ": " + FrontItemGap.Value;

                ItemGap.Value = elementFlow.ItemGap;
                lblItemGap.Content = ItemGap.Name + ": " + ItemGap.Value;

                LookDirectionX.Value = elementFlow.Camera.LookDirection.X;
                lblLookDirectionX.Content = LookDirectionX.Name + ": " + LookDirectionX.Value;

                LookDirectionY.Value = elementFlow.Camera.LookDirection.Y;
                lblLookDirectionY.Content = LookDirectionY.Name + ": " + LookDirectionY.Value;

                LookDirectionZ.Value = elementFlow.Camera.LookDirection.Z;
                lblLookDirectionZ.Content = LookDirectionZ.Name + ": " + LookDirectionZ.Value;

                PopoutDistance.Value = elementFlow.PopoutDistance;
                lblPopoutDistance.Content = PopoutDistance.Name + ": " + PopoutDistance.Value;

                PositionX.Value = elementFlow.Camera.Position.X;
                lblPositionX.Content = PositionX.Name + ": " + PositionX.Value;

                PositionY.Value = elementFlow.Camera.Position.Y;
                lblPositionY.Content = PositionY.Name + ": " + PositionY.Value;

                PositionZ.Value = elementFlow.Camera.Position.Z;
                lblPositionZ.Content = PositionZ.Name + ": " + PositionZ.Value;

                TiltAngle.Value = elementFlow.TiltAngle;
                lblTiltAngle.Content = TiltAngle.Name + ": " + TiltAngle.Value;
            }
        }

        /// <summary>
        /// SaveButton click handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click ( object sender, RoutedEventArgs e ) {
            SaveFileDialog saveDialog = new SaveFileDialog ( );
            saveDialog.DefaultExt = ".xml";
            saveDialog.Filter = "Xml documents (.xml)|*.xml";
            if ( saveDialog.ShowDialog ( ) == true ) {
                KTouchCoverFlowProperties properties = new KTouchCoverFlowProperties ( );
                properties.FieldOfView = FieldOfView.Value.ToString ( );
                properties.FrontItemGap = FrontItemGap.Value.ToString ( );
                properties.ItemGap = ItemGap.Value.ToString ( );
                properties.LookDirectionX = LookDirectionX.Value.ToString ( );
                properties.LookDirectionY = LookDirectionY.Value.ToString ( );
                properties.LookDirectionZ = LookDirectionZ.Value.ToString ( );
                properties.PopoutDistance = PopoutDistance.Value.ToString ( );
                properties.PositionX = PositionX.Value.ToString ( );
                properties.PositionY = PositionY.Value.ToString ( );
                properties.PositionZ = PositionZ.Value.ToString ( );
                properties.TiltAngle = TiltAngle.Value.ToString ( );
                SaveProperties ( properties, saveDialog.FileName );
            }
        }

        /// <summary>
        /// LoadButton click handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Load_Click ( object sender, RoutedEventArgs e ) {
            OpenFileDialog openDialog = new OpenFileDialog ( );
            openDialog.DefaultExt = ".xml";
            openDialog.Filter = "Xml documents (.xml)|*.xml";
            Nullable<bool> result = openDialog.ShowDialog ( );
            if ( result == true ) {
                KTouchCoverFlowProperties properties = LoadProperties ( openDialog.FileName );
                if ( properties != null ) {
                    FieldOfView.Value = Convert.ToDouble ( properties.FieldOfView );
                    FrontItemGap.Value = Convert.ToDouble ( properties.FrontItemGap );
                    ItemGap.Value = Convert.ToDouble ( properties.ItemGap );
                    LookDirectionX.Value = Convert.ToDouble ( properties.LookDirectionX );
                    LookDirectionY.Value = Convert.ToDouble ( properties.LookDirectionY );
                    LookDirectionZ.Value = Convert.ToDouble ( properties.LookDirectionZ );
                    PopoutDistance.Value = Convert.ToDouble ( properties.PopoutDistance );
                    PositionX.Value = Convert.ToDouble ( properties.PositionX );
                    PositionY.Value = Convert.ToDouble ( properties.PositionY );
                    PositionZ.Value = Convert.ToDouble ( properties.PositionZ );
                    TiltAngle.Value = Convert.ToDouble ( properties.TiltAngle );
                }
            }
        }

        /// <summary>
        /// Parse parameters from an existing *.xml file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private KTouchCoverFlowProperties LoadProperties ( string path ) {
            XmlDocument document = new XmlDocument ( );
            document.Load ( path );
            XmlNode rootNode = document [ "KTouchCoverFlowProperties" ];
            KTouchCoverFlowProperties item = new KTouchCoverFlowProperties ( ) {
                FieldOfView = rootNode [ "FieldOfView" ].InnerText,
                FrontItemGap = rootNode [ "FrontItemGap" ].InnerText,
                ItemGap = rootNode [ "ItemGap" ].InnerText,
                LookDirectionX = rootNode [ "LookDirectionX" ].InnerText,
                LookDirectionY = rootNode [ "LookDirectionY" ].InnerText,
                LookDirectionZ = rootNode [ "LookDirectionZ" ].InnerText,
                PopoutDistance = rootNode [ "PopoutDistance" ].InnerText,
                PositionX = rootNode [ "PositionX" ].InnerText,
                PositionY = rootNode [ "PositionY" ].InnerText,
                PositionZ = rootNode [ "PositionZ" ].InnerText,
                TiltAngle = rootNode [ "TiltAngle" ].InnerText
            };
            return item;
        }

        /// <summary>
        /// Write parameters to a *.xml file
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="path"></param>
        public void SaveProperties ( KTouchCoverFlowProperties properties, string path ) {
            XmlSerializer serializer = new XmlSerializer ( typeof ( KTouchCoverFlowProperties ) );
            TextWriter textWriter = new StreamWriter ( path, false );
            serializer.Serialize ( textWriter, properties );
            textWriter.Close ( );
        }
    }
}
