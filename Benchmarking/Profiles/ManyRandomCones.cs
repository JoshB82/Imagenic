using _3D_Engine.Maths.Vectors;
using System;
using _3D_Engine.Entities.SceneObjects.Meshes.OneDimension;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Entities.Groups;
using Imagenic.Core.Entities.SceneObjects.Meshes.ThreeDimensions;

namespace Benchmarking.Profiles
{
    internal static partial class ProfileCollection
    {
        private static void ManyRandomCones()
        {
            Group scene = new();

            WorldPoint origin = WorldPoint.ZeroOrigin;
            scene.Add(origin);

            int noCones = 100;
            Random rnd = new();
            byte[] origins = new byte[noCones * 3], heights = new byte[noCones], radii = new byte[noCones];
            rnd.NextBytes(origins); rnd.NextBytes(heights); rnd.NextBytes(radii);

            Cone[] cones = new Cone[noCones];

            for (int i = 0; i <= noCones - 1; i++)
            {
                cones[i] = new Cone(new Vector3D(origins[i], origins[i + 100], origins[i + 200]), Vector3D.UnitZ, Vector3D.UnitY, heights[i], radii[i], 50);
                scene.Add(cones[i]);
            }

            PerspectiveCamera renderCamera = new(new Vector3D(0, 0, 100), origin, Vector3D.UnitY);

            renderCamera.Render(scene);
        }
    }
}