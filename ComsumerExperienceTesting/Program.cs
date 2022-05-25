﻿using Imagenic.Core.Entities;
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

        // ---

        // Actions => indicate not passing on value
        // Funcs => indicate passing on value

        cube.Transform(e => { e.DrawEdges = true; }); // action

        float outsideValue = 0.5f;
        cube.Transform(e => e.Opacity = outsideValue); // func


        cube.Transform(e => e.Opacity = outsideValue + new Random().Next()) // func
            .Transform((e, i) => { e.SideLength = i; }); // action

        var cube2 = new Cube(Vector3D.Zero, Orientation.OrientationNegativeXY, 4);
        var cubes = new[] { cube, cube2 };

        // both cubes have their side lengths increased by one and their opacities set to 3.5
        cubes.Transform(e => { e.SideLength++; })
             .Transform((e, i) => { e.Opacity = 0.5f + i; }, new int[] { 3, 3 });
    }
}