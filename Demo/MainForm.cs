using _3D_Engine;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.SceneObjects.Cameras;
using System.Windows.Forms;

namespace Demo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Scene scene = new();

            scene.CreateOrigin();

            float cameraWidth = pictureBox.Width / 10, cameraHeight = pictureBox.Height / 10;
            PerspectiveCamera renderCamera = new PerspectiveCamera(new Vector3D(0, 0, -100), scene.SceneObjects[0], Vector3D.UnitY, cameraWidth, cameraHeight, 10, 750);
            

            
        }
    }
}