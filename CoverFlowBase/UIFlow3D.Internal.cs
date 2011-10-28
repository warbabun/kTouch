using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CoverFlowBase {
    public partial class UIFlow3D {

        private Viewport2DVisual3D CreateMeshModel(Visual visualElement) {
            var model = new Viewport2DVisual3D {
                Geometry = new MeshGeometry3D {
                    TriangleIndices = new Int32Collection(
                        new int[] { 0, 1, 2, 2, 3, 0 }),
                    TextureCoordinates = new PointCollection(
                        new Point[] 
                            { 
                                new Point(0, 1), 
                                new Point(1, 1), 
                                new Point(1, 0), 
                                new Point(0, 0) 
                            }),
                    Positions = CreateMeshPositions(visualElement as FrameworkElement)
                },
                Material = new DiffuseMaterial(),

                // Host the 2D element in the 3D model.
                Visual = visualElement
            };

            model.Transform = (InternalResources["Transfrom3DGroup"] as Transform3DGroup).Clone();

            Viewport2DVisual3D.SetIsVisualHostMaterial(model.Material, true);

            return model;
        }

        private Point3DCollection CreateMeshPositions(FrameworkElement visualElement) {
            double aspect;

            if (visualElement == null || double.IsNaN(visualElement.Width) || double.IsNaN(visualElement.Height)) {
                aspect = ElementWidth / ElementHeight;
            } else
                aspect = visualElement.Width / visualElement.Height;

            double factor = 0.5;


            Point3DCollection positions = new Point3DCollection();
            positions.Add(new Point3D(-aspect / 2, -1 * factor, 0));
            positions.Add(new Point3D(aspect / 2, -1 * factor, 0));
            positions.Add(new Point3D(aspect / 2, 1 * factor, 0));
            positions.Add(new Point3D(-aspect / 2, 1 * factor, 0));

            return positions;
        }
    }
}