using _3D_Engine.Enums;
using _3D_Engine.Maths.Vectors;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Lights;
using _3D_Engine.Entities.Groups;
using Imagenic.Core.Entities.SceneObjects.Meshes.ThreeDimensions;
using Imagenic.Core.Entities.SceneObjects.Meshes.ZeroDimensions;

namespace Interactive_Demo
{
    public partial class MainForm : Form
    {
        private readonly PerspectiveCamera camera;
        private List<Keys> keysPressed = new();

        public MainForm()
        {
            InitializeComponent();

            MessageBox.Show("Use QEWASD to move camera, UOIJKL to rotate camera.", "Controls");

            // Create a new scene
            Group scene = new();

            // Create some meshes
            WorldPoint origin = WorldPoint.ZeroOrigin;
            scene.Add(origin);
            Group axes = Arrow.Axes;
            scene.Add(axes);

            Cube cube = new(new Vector3D(10, 10, 10), Vector3D.UnitZ, Vector3D.UnitY, 30);
            Cone cone = new(new Vector3D(70, 10, 10), Vector3D.UnitZ, Vector3D.UnitY, 30, 20, 50);
            scene.Add(cube, cone);

            // Create a light
            DistantLight light = new(new Vector3D(0, 100, 0), scene.Meshes[0], Vector3D.UnitZ);
            scene.Add(light);

            // Create a camera
            float cameraViewWidth = pictureBox.Width / 10f, cameraViewHeight = pictureBox.Height / 10f;
            camera = new(new Vector3D(100, 0, 100), scene.SceneObjects[0], Vector3D.UnitY, cameraViewWidth, cameraViewHeight, 10, 750, pictureBox.Width, pictureBox.Height);
            scene.Add(camera);

            // Adjust render settings
            //camera.MakeRenderSizeOfControl(pictureBox);

            _3D_Engine.Properties.Settings.Default.Verbosity = Verbosity.None;

            camera.SceneToRender = scene;

            Thread thread = new(Loop) { IsBackground = true };
            thread.Start();
        }

        private void Loop()
        {
            bool running = true;

            const int maxFramePerSecond = 60, maxUpdatesPerSecond = 60;
            const long frameMinimumTime = 1000 / maxFramePerSecond; //(?)
            const long updateMinimumTime = 1000 / maxUpdatesPerSecond; // ?

            int noFrames = 0, noUpdates = 0, timer = 1;

            long nowTime, deltaTime, frameTime = 0, updateTime = 0;

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
                    this.Invoke((MethodInvoker)(() => pictureBox.Image = camera.Render()));
                    noFrames++; //?
                    frameTime -= frameMinimumTime;
                }

                if (updateTime >= updateMinimumTime)
                {
                    //Update_Position(); uncomment
                    noUpdates++; //?
                    updateTime -= updateMinimumTime;
                }

                if (nowTime >= 1000 * timer)
                {
                    this.Invoke((MethodInvoker)(() => this.Text = $"Interactive Demo - FPS: {noFrames}, UPS: {noUpdates}"));
                    noFrames = 0; noUpdates = 0;
                    timer += 1;
                }

                CheckKeyboard(updateTime);
            }
        }

        private void CheckKeyboard(long updateTime)
        {
            const float cameraPanDampener = 0.0001f, cameraTiltDampener = 0.00001f;

            for (int i = 0; i < keysPressed.Count; i++)
            {
                switch (keysPressed[i])
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
                        // Pan backward
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

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            keysPressed.Add(e.KeyCode);
            keysPressed = keysPressed.Distinct().ToList();
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e) => keysPressed.Remove(e.KeyCode);

        private void MainForm_Resize(object sender, System.EventArgs e)
        {
            //camera.ViewWidth = pictureBox.Width / 10f;
            //camera.ViewHeight = pictureBox.Height / 10f;
            //camera.RenderWidth = pictureBox.Width;
            //camera.RenderHeight = pictureBox.Height;
        }
    }
}