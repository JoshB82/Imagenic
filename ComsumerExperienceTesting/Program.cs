using Imagenic.Core.Entities;
using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities;
using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.PhysicalEntities.Meshes.ThreeDimensions.Cuboids;
using Imagenic.Core.Maths;
using Imagenic.Core.Maths.Vectors;

namespace Imagenic.ComsumerExperienceTesting;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        Vector3D test = new(1, 2, 3);

        var cube = new Cube(worldOrigin: test,
                            worldOrientation: Orientation.OrientationNegativeYZ,
                            sideLength: 100);

        cube.Orientate(Orientation.OrientationNegativeYX);

        cube.Transform(e => e.DrawEdges = false)
            .Transform(e => e.SideLength = 4)
            .Transform((e, i) => e.Visible = i, true);

        cube.Transform((e, i) => e.Id + i, 1)
            .Transform((e, i) => { e.Opacity = i; });
    }
}