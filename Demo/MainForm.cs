using _3D_Engine;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.SceneObjects.Cameras;
using _3D_Engine.SceneObjects.Lights;
using _3D_Engine.SceneObjects.Meshes.ThreeDimensions;
using System.Windows.Forms;

namespace Demo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // Create a new scene
            Scene scene = new();

            // Create some meshes
            scene.CreateOrigin();
            Cube cube = new(new(10, 10, 10), Vector3D.UnitZ, Vector3D.UnitY, 30);
            scene.Add(cube);
            Cone cone = new(new(70, 10, 10), Vector3D.UnitZ, Vector3D.UnitY, 30, 20, 50);
            scene.Add(cone);

            // Create a light
            //DistantLight light = new(new(0, 100, 0), scene.Meshes[0], Vector3D.UnitZ);
            //scene.Add(light);

            // Create a camera
            float cameraWidth = pictureBox.Width / 10f, cameraHeight = pictureBox.Height / 10f;
            PerspectiveCamera camera = new(new(0, 0, -100), scene.Meshes[0], Vector3D.UnitY, cameraWidth, cameraHeight, 10, 750);
            scene.Add(camera);

            // Adjust render settings
            camera.MakeRenderSizeOfControl(pictureBox);

            // Render the scene and display the output in the picture box
            pictureBox.Image = camera.Render();
        }
    }
}