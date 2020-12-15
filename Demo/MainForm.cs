using _3D_Engine;
using _3D_Engine.Maths.Vectors;
using System.Windows.Forms;

namespace Demo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Scene scene = new Scene(pictureBox, pictureBox.Width, pictureBox.Height);

            scene.CreateOrigin();

            float cameraWidth = pictureBox.Width / 10, cameraHeight = pictureBox.Height / 10;
            Perspective_Camera renderCamera = new Perspective_Camera(new Vector3D(0, 0, -100), scene.SceneObjects[0], Vector3D.UnitY, cameraWidth, cameraHeight, 10, 750);
            

            
        }
    }
}