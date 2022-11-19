/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines an arrow mesh.
 */

using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Maths.Vectors;
using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.PhysicalEntities.Edges;
using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.PhysicalEntities.Faces;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Triangles;
using Imagenic.Core.Entities.SceneObjects.Meshes.ZeroDimensions;
using Imagenic.Core.Enums;
using System.Collections.Generic;
using System.Drawing;
using static _3D_Engine.Properties.Settings;
using static System.MathF;

namespace Imagenic.Core.Entities.SceneObjects.Meshes.ThreeDimensions;

public sealed class Arrow : Mesh
{
    #region Fields and Properties

    // Axes
    public static readonly Arrow ZAxis = new Arrow(Vector3D.Zero, Orientation.OrientationZY, Default.AxisArrowBodyLength, Default.AxisArrowTipLength, Default.AxisArrowBodyRadius, Default.AxisArrowTipRadius, Default.AxisArrowResolution).ColourAllSolidFaces(Color.Blue);
    public static readonly Arrow YAxis = new Arrow(Vector3D.Zero, Orientation.OrientationYNegativeZ, Default.AxisArrowBodyLength, Default.AxisArrowTipLength, Default.AxisArrowBodyRadius, Default.AxisArrowTipRadius, Default.AxisArrowResolution).ColourAllSolidFaces(Color.Green);
    public static readonly Arrow XAxis = new Arrow(Vector3D.Zero, Orientation.OrientationXY, Default.AxisArrowBodyLength, Default.AxisArrowTipLength, Default.AxisArrowBodyRadius, Default.AxisArrowTipRadius, Default.AxisArrowResolution).ColourAllSolidFaces(Color.Red);

    public static readonly WorldPoint Axes = GenerateAxes();

    private static WorldPoint GenerateAxes()
    {
        WorldPoint axes = new WorldPoint(Vector3D.Zero);
        axes.AddChildren(XAxis, YAxis, ZAxis);
        return axes;
    }

    private Vector3D tipPosition;
    private float length, bodyLength, tipLength, bodyRadius, tipRadius;
    private int resolution;

    public override Orientation WorldOrientation
    {
        get => base.WorldOrientation;
        set
        {
            base.WorldOrientation = value;

            tipPosition = base.WorldOrientation.DirectionForward * length;
        }
    }

    public Vector3D TipPosition
    {
        get => tipPosition;
        set
        {
            if (value == tipPosition) return;
            tipPosition = value;
            InvokeRenderingEvents();

            length = (tipPosition - WorldOrigin).Magnitude();
            bodyLength = length - tipLength;
        }
    }

    public float Length
    {
        get => length;
        set
        {
            if (value == length) return;
            length = value;
            InvokeRenderingEvents();

            tipPosition = WorldOrientation.DirectionForward * length;
            bodyLength = length = tipLength;
        }
    }

    public float BodyLength
    {
        get => bodyLength;
        set
        {
            if (value == bodyLength) return;
            bodyLength = value;
            InvokeRenderingEvents();

            length = bodyLength + tipLength;
            tipPosition = WorldOrigin + WorldOrientation.DirectionForward * length;

            Structure.Vertices = GenerateVertices(Resolution, bodyLength, tipLength, bodyRadius, tipRadius);
        }
    }

    public float TipLength
    {
        get => tipLength;
        set
        {
            if (value == tipLength) return;
            tipLength = value;
            InvokeRenderingEvents();

            length = bodyLength + tipLength;
            tipPosition = WorldOrigin + WorldOrientation.DirectionForward * length;

            Structure.Vertices = GenerateVertices(Resolution, bodyLength, tipLength, bodyRadius, tipRadius);
        }
    }

    public float BodyRadius
    {
        get => bodyRadius;
        set
        {
            if (value == bodyRadius) return;
            bodyRadius = value;
            InvokeRenderingEvents();

            Structure.Vertices = GenerateVertices(Resolution, bodyLength, tipLength, bodyRadius, tipRadius);
        }
    }

    public float TipRadius
    {
        get => tipRadius;
        set
        {
            if (value == tipRadius) return;
            tipRadius = value;
            InvokeRenderingEvents();

            Structure.Vertices = GenerateVertices(Resolution, bodyLength, tipLength, bodyRadius, tipRadius);
        }
    }

    public int Resolution
    {
        get => resolution;
        set
        {
            if (value == resolution) return;
            resolution = value;
            InvokeRenderingEvents();

            Structure = GenerateStructure(resolution, bodyLength, tipLength, bodyRadius, tipRadius);
        }
    }

    #endregion

    #region Constructors

    internal Arrow(Vector3D worldOrigin,
                   Orientation worldOrientation,
                   float bodyLength,
                   float tipLength,
                   float bodyRadius,
                   float tipRadius,
                   int resolution,
                   bool hasDirectionArrows) : base(worldOrigin, worldOrientation, GenerateStructure(resolution, bodyLength, tipLength, bodyRadius, tipRadius), hasDirectionArrows)
    {
        length = bodyLength + tipLength;
        tipPosition = worldOrigin + worldOrientation.DirectionForward * length;
        this.bodyLength = bodyLength;
        this.tipLength = tipLength;
        this.bodyRadius = bodyRadius;
        this.tipRadius = tipRadius;
        this.resolution = resolution;
    }

    public Arrow(Vector3D worldOrigin,
                    Orientation worldOrientation,
                    float bodyLength,
                    float tipLength,
                    float bodyRadius,
                    float tipRadius,
                    int resolution) : this(worldOrigin, worldOrientation, bodyLength, tipLength, bodyRadius, tipRadius, resolution, true) { }

    public static Arrow ArrowTipPosition(Vector3D worldOrigin,
                                            Vector3D tipPosition,
                                            Vector3D directionUp,
                                            float bodyLength,
                                            float tipLength,
                                            float bodyRadius,
                                            float tipRadius,
                                            int resolution)
    {
        return new Arrow(worldOrigin, Orientation.CreateOrientationForwardUp(tipPosition - worldOrigin, directionUp), bodyLength, tipLength, bodyRadius, tipRadius, resolution, true);
    }

    #endregion

    #region Methods

    private static MeshStructure GenerateStructure(int resolution, float bodyLength, float tipLength, float bodyRadius, float tipRadius)
    {
        IList<Vertex> vertices = GenerateVertices(resolution, bodyLength, tipLength, bodyRadius, tipRadius);
        IList<Edge> edges = GenerateEdges(vertices, resolution);
        IList<Face> faces = GenerateFaces(vertices, resolution);

        return new MeshStructure(Dimension.Three, vertices, edges, faces);
    }

    private static IList<Vertex> GenerateVertices(int resolution, float bodyLength, float tipLength, float bodyRadius, float tipRadius)
    {
        IList<Vertex> vertices = new Vertex[3 * resolution + 3];
        vertices[0] = new(new Vector4D(0, 0, 0, 1));
        vertices[1] = new(new Vector4D(Vector3D.UnitZ * bodyLength, 1));
        vertices[2] = new(new Vector4D(Vector3D.UnitZ * (bodyLength + tipLength), 1));

        float angle = Tau / resolution;
        for (int i = 0; i < resolution; i++)
        {
            float sin = Sin(angle * i), cos = Cos(angle * i);
            vertices[i + 3] = new(new Vector4D(cos * bodyRadius, sin * bodyRadius, 0, 1));
            vertices[i + resolution + 3] = new(new Vector4D(cos * bodyRadius, sin * bodyRadius, bodyLength, 1));
            vertices[i + 2 * resolution + 3] = new(new Vector4D(cos * tipRadius, sin * tipRadius, bodyLength, 1));
        }

        return vertices;
    }

    private static IList<Edge> GenerateEdges(IList<Vertex> vertices, int resolution)
    {
        IList<Edge> edges = new Edge[5 * resolution];

        for (int i = 0; i < resolution - 1; i++)
        {
            edges[i] = new SolidEdge(vertices[i + 3], vertices[i + 4]);
            edges[i + resolution] = new SolidEdge(vertices[i + resolution + 3], vertices[i + resolution + 4]);
            edges[i + 2 * resolution] = new SolidEdge(vertices[i + 2 * resolution + 3], vertices[i + 2 * resolution + 4]);
        }
        edges[resolution - 1] = new SolidEdge(vertices[resolution + 2], vertices[3]);
        edges[2 * resolution - 1] = new SolidEdge(vertices[2 * resolution + 2], vertices[resolution + 3]);
        edges[3 * resolution - 1] = new SolidEdge(vertices[3 * resolution + 2], vertices[2 * resolution + 3]);

        for (int i = 0; i < resolution; i++)
        {
            edges[i + 3 * resolution] = new SolidEdge(vertices[i + 3], vertices[i + resolution + 3]);
            edges[i + 4 * resolution] = new SolidEdge(vertices[i + 2 * resolution + 3], vertices[2]);
        }

        DrawEdges = false;

        return edges;
    }

    private static IList<Face> GenerateFaces(IList<Vertex> vertices, int resolution)
    {
        IList<Face> faces = new Face[2 * resolution + 2];

        Triangles = new SolidTriangle[6 * resolution];

        for (int i = 0; i < resolution - 1; i++)
        {
            Triangles[i] = new SolidTriangle(vertices[i + 3], vertices[0], vertices[i + 4]);
            Triangles[i + resolution] = new SolidTriangle(vertices[i + 3], vertices[i + resolution + 3], vertices[i + resolution + 4]);
            Triangles[i + 2 * resolution] = new SolidTriangle(vertices[i + 3], vertices[i + resolution + 4], vertices[i + 4]);
            Triangles[i + 3 * resolution] = new SolidTriangle(vertices[i + resolution + 3], vertices[i + 2 * resolution + 4], vertices[i + 2 * resolution + 3]);
            Triangles[i + 4 * resolution] = new SolidTriangle(vertices[i + resolution + 3], vertices[i + resolution + 4], vertices[i + 2 * resolution + 4]);
            Triangles[i + 5 * resolution] = new SolidTriangle(vertices[i + 2 * resolution + 3], vertices[i + 2 * resolution + 4], vertices[2]);
        }
        Triangles[resolution - 1] = new SolidTriangle(vertices[resolution + 2], vertices[0], vertices[3]);
        Triangles[2 * resolution - 1] = new SolidTriangle(vertices[resolution + 2], vertices[2 * resolution + 2], vertices[resolution + 3]);
        Triangles[3 * resolution - 1] = new SolidTriangle(vertices[resolution + 2], vertices[resolution + 3], vertices[3]);
        Triangles[4 * resolution - 1] = new SolidTriangle(vertices[2 * resolution + 2], vertices[2 * resolution + 3], vertices[3 * resolution + 2]);
        Triangles[5 * resolution - 1] = new SolidTriangle(vertices[2 * resolution + 2], vertices[resolution + 3], vertices[2 * resolution + 3]);
        Triangles[6 * resolution - 1] = new SolidTriangle(vertices[3 * resolution + 2], vertices[2 * resolution + 3], vertices[2]);

        return faces;
    }

    #endregion
}