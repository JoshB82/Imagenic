using _3D_Engine.Entities.Groups;
using _3D_Engine.Entities.SceneObjects.Meshes.OneDimension;
using _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Maths.Vectors;

namespace Benchmarking.Profiles
{
    internal static partial class ProfileCollection
    {
        private static void CubeRender()
        {
            Group scene = new();

            WorldPoint origin = WorldPoint.ZeroOrigin;
            Cube cube = new(Vector3D.Zero, Vector3D.One, Vector3D.UnitY, 30);

            scene.Add(origin, cube);

            PerspectiveCamera renderCamera = new(new Vector3D(0, 0, 100), origin, Vector3D.UnitY);

            renderCamera.Render(scene);
        }
    }
}