using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Lights;
using _3D_Engine.Entities.Groups;
using _3D_Engine.Entities.SceneObjects;
using Imagenic.Core.Entities.SceneObjects.Meshes.ThreeDimensions;
using Imagenic.Core.Entities.SceneObjects.Meshes.TwoDimensions.Ellipses;
using Imagenic.Core.Entities.SceneObjects.Meshes.ZeroDimensions;
using Imagenic.Core.Maths.Vectors;

namespace Statistics_Demo
{
    public partial class MainForm : Form
    {
        private readonly OrthogonalCamera camera;
        private Group sceneToRender;
        private List<Keys> keysPressed = new();
        private int noSceneObjects = 0;
        private AllStatisticsForm allStatisticsForm;

        public MainForm()
        {
            InitializeComponent();
            this.KeyPreview = true; //?

            Group scene = new();

            WorldPoint origin = WorldPoint.ZeroOrigin;
            scene.Add(origin);
            Group axes = Arrow.Axes;
            scene.Add(axes);

            Circle circle = new(new Vector3D(50, 50, 50), Vector3D.UnitX, Vector3D.UnitY, 25, 50);
            scene.Add(circle);

            DistantLight light = new(new Vector3D(0, 100, 0), scene.Meshes[0], Vector3D.UnitZ);
            scene.Add(light);

            camera = new OrthogonalCamera(new Vector3D(500, 50, 200), scene.SceneObjects[0], Vector3D.UnitY, pictureBox.Width / 10f, pictureBox.Height / 10f, 50, 750, pictureBox.Width, pictureBox.Height);
            scene.Add(camera);

            noSceneObjects = 4;
            noSceneObjectsValueLabel.Text = noSceneObjects.ToString();

            sceneToRender = scene;

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
                    this.Invoke((MethodInvoker)(() => pictureBox.Image = camera.Render(sceneToRender)));
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
            const float cameraPanDampener = 0.01f, cameraTiltDampener = 0.0000001f;

            for (int i = 0; i < keysPressed.Count; i++)
            {
                switch (keysPressed[i])
                {
                    case Keys.W:
                        // Pan forward
                        camera.PanForward(cameraPanDampener * updateTime);
                        KeyboardUpdateListView();
                        break;
                    case Keys.A:
                        // Pan left
                        camera.PanLeft(cameraPanDampener * updateTime);
                        KeyboardUpdateListView();
                        break;
                    case Keys.D:
                        // Pan right
                        camera.PanRight(cameraPanDampener * updateTime);
                        KeyboardUpdateListView();
                        break;
                    case Keys.S:
                        // Pan backward
                        camera.PanBackward(cameraPanDampener * updateTime);
                        KeyboardUpdateListView();
                        break;
                    case Keys.Q:
                        // Pan up
                        camera.PanUp(cameraPanDampener * updateTime);
                        KeyboardUpdateListView();
                        break;
                    case Keys.E:
                        // Pan down
                        camera.PanDown(cameraPanDampener * updateTime);
                        KeyboardUpdateListView();
                        break;
                    case Keys.I:
                        // Rotate up
                        camera.RotateUp(cameraTiltDampener * updateTime);
                        KeyboardUpdateListView();
                        break;
                    case Keys.J:
                        // Rotate left
                        camera.RotateLeft(cameraTiltDampener * updateTime);
                        KeyboardUpdateListView();
                        break;
                    case Keys.L:
                        // Rotate right
                        camera.RotateRight(cameraTiltDampener * updateTime);
                        KeyboardUpdateListView();
                        break;
                    case Keys.K:
                        // Rotate down
                        camera.RotateDown(cameraTiltDampener * updateTime);
                        KeyboardUpdateListView();
                        break;
                    case Keys.U:
                        // Roll left
                        camera.RollLeft(cameraTiltDampener * updateTime);
                        KeyboardUpdateListView();
                        break;
                    case Keys.O:
                        // Roll right
                        camera.RollRight(cameraTiltDampener * updateTime);
                        KeyboardUpdateListView();
                        break;
                }
            }
        }

        private void KeyboardUpdateListView()
        {
            if (allStatisticsForm is not null) this.Invoke((MethodInvoker)(() => UpdateListView()));
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            keysPressed.Add(e.KeyCode);
            keysPressed = keysPressed.Distinct().ToList();
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e) => keysPressed.Remove(e.KeyCode);

        private void viewAllButton_Click(object sender, System.EventArgs e)
        {
            allStatisticsForm = new();
            UpdateListView();
            allStatisticsForm.Show();
        }

        private void UpdateListView()
        {
            allStatisticsForm.listView.Items.Clear();

            foreach (SceneEntity sceneObject in sceneToRender.SceneObjects)
            {
                var sceneObjectData = new string[]
                {
                    sceneObject.Id.ToString(),
                    sceneObject.GetType().Name,
                    sceneObject.WorldOrigin.ToString(),
                    sceneObject.WorldDirectionForward.ToString(),
                    sceneObject.WorldDirectionUp.ToString(),
                    sceneObject.WorldDirectionRight.ToString()
                };
                allStatisticsForm.listView.Items.Add(new ListViewItem(sceneObjectData));
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            allStatisticsForm.Close();
        }

        private void addCubeButton_Click(object sender, System.EventArgs e)
        {
            byte[] rndNums = new byte[4];
            new Random().NextBytes(rndNums);
            Cube cube = new(new Vector3D(rndNums[0], rndNums[1], rndNums[2]), Vector3D.UnitX, Vector3D.UnitY, rndNums[3]);
            sceneToRender.Add(cube);
            IncreaseSceneObjectCount();
        }

        private void addConeButton_Click(object sender, System.EventArgs e)
        {

        }

        private void addCylinderButton_Click(object sender, System.EventArgs e)
        {

        }

        private void IncreaseSceneObjectCount() => noSceneObjectsValueLabel.Text = (++noSceneObjects).ToString();
    }
}