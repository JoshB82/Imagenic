using _3D_Engine.SceneObjects.Groups;
using _3D_Engine.SceneObjects.Meshes.OneDimension;
using _3D_Engine.SceneObjects.Meshes.ThreeDimensions;
using System.Windows.Forms;

namespace Interactive_Demo
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


        }
    }
}