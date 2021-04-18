using _3D_Engine.Groups;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.SceneObjects.Meshes.ThreeDimensions;

namespace Benchmarking.Profiles
{
    internal static partial class ProfileCollection
    {
        private static void CubeRender()
        {
            Group scene = new();

            Cube cube = new(Vector3D.Zero, Vector3D.One, Vector3D.UnitY, 30);

            scene.Add(cube);
        }
    }
}
