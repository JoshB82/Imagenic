using _3D_Engine.Maths.Vectors;
using System.Windows.Forms;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Lights;
using _3D_Engine.Entities.Groups;
using Imagenic.Core.Entities.SceneObjects.Meshes.ThreeDimensions;
using Imagenic.Core.Entities.SceneObjects.Meshes.ZeroDimensions;

namespace Simple_Demo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // Create a new scene
            Group scene = new();

            // Create some meshes
            WorldPoint origin = WorldPoint.ZeroOrigin;
            scene.Add(origin);
            Group axes = Arrow.Axes;
            scene.Add(axes);

            Cube cube = new(
                origin: new Vector3D(10, 10, 10),
                directionForward: Vector3D.UnitZ,
                directionUp: Vector3D.UnitY,
                sideLength: 30
            );
            Cone cone = new(
                origin: new Vector3D(70, 10, 10),
                directionForward: Vector3D.UnitZ,
                directionUp: Vector3D.UnitY,
                height: 30,
                radius: 20,
                resolution: 50
            );
            scene.Add(cube, cone);

            // Create a light
            DistantLight light = new(
                origin: new Vector3D(0, 100, 0),
                pointedAt: scene.Meshes[0],
                directionUp: Vector3D.UnitZ
            );
            scene.Add(light);

            // Create a camera
            OrthogonalCamera renderCamera = new(
                origin: new Vector3D(500, 0, 100),
                pointedAt: scene.SceneObjects[0],
                directionUp: Vector3D.UnitY,
                viewWidth: pictureBox.Width / 10f,
                viewHeight: pictureBox.Height / 10f,
                zNear: 50,
                zFar: 750,
                renderWidth: pictureBox.Width,
                renderHeight: pictureBox.Height
            );

            // Render the scene and display the output in the picture box
            pictureBox.Image = renderCamera.Render(scene);
        }
    }
}