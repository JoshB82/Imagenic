﻿using Imagenic.Core.CascadeBuffers;
using Imagenic.Core.Entities;
using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities;
using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.PhysicalEntities.Meshes.ThreeDimensions.Cuboids;
using Imagenic.Core.Entities.TransformableEntities;
using Imagenic.Core.Maths;
using Imagenic.Core.Maths.Vectors;
using Imagenic.Core.Transitions;

namespace Imagenic.ComsumerExperienceTesting;

internal class Program
{
    public static void Main(string[] args)
    {
        Cube cubeTest = new Cube(Vector3D.Zero, Orientation.OrientationXY, 100, SolidEdgeStyle.Black, SolidStyle.Red);

        cubeTest.Transform(e => e.SideLength++)
                .Transform((e, i) => e.SideLength += i);

        float i = 0;
        cubeTest.Transform(e => { i = e.SideLength++; })
                .Transform(e => e.SideLength += i);


        Cube cube = new Cube(Vector3D.Zero);

        cube.Start(0, 1.5f, out Transition t1)
            .Translate(new Vector3D(50, 100, 150))
            .Start(2, 2.5f, out Transition t2)
            .Scale(new Vector3D(3, 3, 3))
            .End(t1)
            .End(t2);

        // ----

        Vector3D test = new(1, 2, 3);

        var cube = new Cube(worldOrigin: test,
                            worldOrientation: Orientation.OrientationNegativeYZ,
                            sideLength: 100);

        // ----

        cube.Start(0, out Core.Transitions.Transition t)
            .Transform(e => { e.SideLength = 4; })
            .Translate(new Vector3D(10, 40, 50))
            .Orientate(Orientation.OrientationNegativeXY)
            .End(t);

        cube.TranslateX(4)
            .Start(0, out var t1)
            .TranslateY(5)
            .Start(2, out var t2)
            .TranslateZ(6)
            .End(t1)
            .Translate(new Vector3D(7, 8, 9))
            .End(t2);


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

        cube.TranslateX(3)
            .TranslateXCascade(2);

        cube.TranslateXCascade(42)
            .Translate();

        cube.TranslateXCascade(2)
            .TranslateX();

        cube.TranslateXCascade(4)
            .TranslateYC(5)
            .TranslateY();

        Console.ReadLine();

        cube = cube.Transform((e, i) => e.CastsShadows = i, i == 3);
        cube.Transform((e, i) => { e.CastsShadows = i; });

        cube.Transform((e, i) => e.CastsShadows = i, Console.ReadLine() == "yes")
            .Transform((e, i) => e.DrawFaces = i)
            .Transform((e, i) => { e.DrawEdges = i; })
            .TranslateX(3);

        

        cubes.Transform((e, i) => { e.Opacity = i; }, float.Parse(Console.ReadLine() ?? string.Empty))
             .Transform((e, i) => e.DrawOutline = i, bool.Parse(Console.ReadLine() ?? string.Empty))
             .Transform((e, i) => { e.DrawEdges = i; });

        cubes.Transform(x => x.Opacity = 0.5f, x => x.Opacity > 0.5)
             .Transform((e, v) => e.SideLength = v)
             .Transform((e, v) => { e.DrawEdges = true; return v; })
             .Sum(e => e.Value)
             .TranslateX(10);
    }
}