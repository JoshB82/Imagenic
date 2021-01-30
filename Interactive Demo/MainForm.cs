using _3D_Engine.Maths.Vectors;
using _3D_Engine.SceneObjects.Cameras;
using _3D_Engine.SceneObjects.Groups;
using _3D_Engine.SceneObjects.Lights;
using _3D_Engine.SceneObjects.Meshes.OneDimension;
using _3D_Engine.SceneObjects.Meshes.ThreeDimensions;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Interactive_Demo
{
    public partial class MainForm : Form
    {
        private PerspectiveCamera camera;
        private long updateTime;

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

            Cube cube = new(new Vector3D(10, 10, 10), Vector3D.UnitZ, Vector3D.UnitY, 30);
            scene.Add(cube);
            Cone cone = new(new Vector3D(70, 10, 10), Vector3D.UnitZ, Vector3D.UnitY, 30, 20, 50);
            scene.Add(cone);

            // Create a light
            DistantLight light = new(new Vector3D(0, 100, 0), scene.Meshes[0], Vector3D.UnitZ);
            scene.Add(light);

            // Create a camera
            float cameraWidth = pictureBox.Width / 10f, cameraHeight = pictureBox.Height / 10f;
            camera = new(new Vector3D(100, 0, 100), scene.SceneObjects[0], Vector3D.UnitY, cameraWidth, cameraHeight, 10, 750);
            scene.Add(camera);

            // Adjust render settings
            camera.MakeRenderSizeOfControl(pictureBox);

            camera.Scene = scene;

            Thread thread = new Thread(Loop) { IsBackground = true };
            thread.Start();
        }

        private void Loop()
        {
            bool running = true;

            const int maxFramePerSecond = 60, maxUpdatesPerSecond = 60;
            const long frameMinimumTime = 1000 / maxFramePerSecond; //(?)
            const long updateMinimumTime = 1000 / maxUpdatesPerSecond; // ?

            int noFrames = 0, noUpdates = 0, timer = 1;

            long nowTime, deltaTime, frameTime = 0;

            Stopwatch sw = Stopwatch.StartNew();
            long startTime = sw.ElapsedMilliseconds;

            while (running)
            {
                nowTime = sw.ElapsedMilliseconds;
                deltaTime = nowTime - startTime;
                startTime = nowTime;

                frameTime += deltaTime;
                updateTime += deltaTime;

                if (frameTime >= frameMinimumTime)
                {
                    pictureBox.Image = camera.Render();
                    noFrames++; //?
                    frameTime -= frameMinimumTime;
                }

                if (updateTime >= updateMinimumTime)
                {
                    //Update_Position(); uncomment
                    noUpdates++;
                    updateTime -= updateMinimumTime;
                }

                if (nowTime >= 1000 * timer)
                {
                    //Invoke((MethodInvoker)delegate { Text = $"Interactive Demo - FPS: {noFrames}, UPS: {noUpdates}"; }); // ?
                    noFrames = 0; noUpdates = 0;
                    timer += 1;
                }
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            const float cameraPanDampener = 0.0008f;
            const float cameraTiltDampener = 0.000001f;

            switch (e.KeyCode)
            {
                case Keys.W:
                    // Pan forward
                    camera.PanForward(cameraPanDampener * updateTime);
                    break;
                case Keys.A:
                    // Pan left
                    camera.PanLeft(cameraPanDampener * updateTime);
                    break;
                case Keys.D:
                    // Pan right
                    camera.PanRight(cameraPanDampener * updateTime);
                    break;
                case Keys.S:
                    // Pan back
                    camera.PanBackward(cameraPanDampener * updateTime);
                    break;
                case Keys.Q:
                    // Pan up
                    camera.PanUp(cameraPanDampener * updateTime);
                    break;
                case Keys.E:
                    // Pan down
                    camera.PanDown(cameraPanDampener * updateTime);
                    break;
                case Keys.I:
                    // Rotate up
                    camera.RotateUp(cameraTiltDampener * updateTime);
                    break;
                case Keys.J:
                    // Rotate left
                    camera.RotateLeft(cameraTiltDampener * updateTime);
                    break;
                case Keys.L:
                    // Rotate right
                    camera.RotateRight(cameraTiltDampener * updateTime);
                    break;
                case Keys.K:
                    // Rotate down
                    camera.RotateDown(cameraTiltDampener * updateTime);
                    break;
                case Keys.U:
                    // Roll left
                    camera.RollLeft(cameraTiltDampener * updateTime);
                    break;
                case Keys.O:
                    // Roll right
                    camera.RollRight(cameraTiltDampener * updateTime);
                    break;
            }
        }
    }
}