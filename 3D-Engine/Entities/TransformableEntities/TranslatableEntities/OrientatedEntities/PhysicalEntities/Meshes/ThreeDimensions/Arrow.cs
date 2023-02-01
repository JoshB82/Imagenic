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

using Imagenic.Core.Enums;
using Imagenic.Core.Utilities;
using Imagenic.Core.Utilities.Node;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;

namespace Imagenic.Core.Entities;

[Serializable]
public sealed class Arrow : Mesh
{
    #region Fields and Properties

    // Axes
    public static readonly Arrow ZAxis = new Arrow(Vector3D.Zero, Orientation.OrientationZY, Defaults.Default.AxisArrowBodyLength, Defaults.Default.AxisArrowTipLength, Defaults.Default.AxisArrowBodyRadius, Defaults.Default.AxisArrowTipRadius, Defaults.Default.AxisArrowResolution).ColourAllSolidFaces(Color.Blue);
    public static readonly Arrow YAxis = new Arrow(Vector3D.Zero, Orientation.OrientationYNegativeZ, Defaults.Default.AxisArrowBodyLength, Defaults.Default.AxisArrowTipLength, Defaults.Default.AxisArrowBodyRadius, Defaults.Default.AxisArrowTipRadius, Defaults.Default.AxisArrowResolution).ColourAllSolidFaces(Color.Green);
    public static readonly Arrow XAxis = new Arrow(Vector3D.Zero, Orientation.OrientationXY, Defaults.Default.AxisArrowBodyLength, Defaults.Default.AxisArrowTipLength, Defaults.Default.AxisArrowBodyRadius, Defaults.Default.AxisArrowTipRadius, Defaults.Default.AxisArrowResolution).ColourAllSolidFaces(Color.Red);

    public static readonly Node<WorldPoint> Axes = GenerateAxes();

    private static Node<WorldPoint> GenerateAxes()
    {
        var axes = new WorldPoint(Vector3D.Zero);
        var XAxisNode = new Node<Arrow>(XAxis);
        var YAxisNode = new Node<Arrow>(YAxis);
        var ZAxisNode = new Node<Arrow>(ZAxis);
        return new Node<WorldPoint>(axes, XAxisNode, YAxisNode, ZAxisNode);
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
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);

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
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);

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
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);

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
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);

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
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);

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
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);

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
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);

            Structure = GenerateStructure(resolution, bodyLength, tipLength, bodyRadius, tipRadius);
        }
    }

    #endregion

    #region Constructors

    public Arrow(Vector3D worldOrigin,
                 [DisallowNull] Orientation worldOrientation,
                 float bodyLength,
                 float tipLength,
                 float bodyRadius,
                 float tipRadius,
                 int resolution,
                 [DisallowNull] EdgeStyle edgeStyle,
                 [DisallowNull] FaceStyle exteriorFaceStyle)
        : base(worldOrigin, worldOrientation, GenerateStructure(resolution, bodyLength, tipLength, bodyRadius, tipRadius, edgeStyle, exteriorFaceStyle)
              #if DEBUG
              , MessageBuilder<ArrowCreatedMessage>.Instance()
              #endif
              )
    { }

    public Arrow(Vector3D worldOrigin,
                 Vector3D tipPosition,
                 Vector3D directionUp,
                 float bodyLength,
                 float tipLength,
                 float bodyRadius,
                 float tipRadius,
                 int resolution,
                 [DisallowNull] EdgeStyle edgeStyle,
                 [DisallowNull] FaceStyle exteriorFaceStyle)
        : this(worldOrigin, Orientation.CreateOrientationForwardUp(tipPosition - worldOrigin, directionUp), bodyLength, tipLength, bodyRadius, tipRadius, resolution, edgeStyle, exteriorFaceStyle)
    { }

    /*
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
    */

    #endregion

    #region Methods

    private static MeshStructure GenerateStructure(int resolution, float bodyLength, float tipLength, float bodyRadius, float tipRadius, EdgeStyle edgeStyle, FaceStyle exteriorFaceStyle)
    {
        EventList<Vertex> vertices = GenerateVertices(resolution, bodyLength, tipLength, bodyRadius, tipRadius);
        EventList<Edge> edges = GenerateEdges(vertices, resolution, edgeStyle);
        EventList<Triangle> triangles = GenerateTriangles(vertices, resolution, exteriorFaceStyle);
        EventList<Face> faces = GenerateFaces(triangles, resolution, exteriorFaceStyle);

        return new MeshStructure(Dimension.Three, vertices, edges, triangles, faces);
    }

    private static EventList<Vertex> GenerateVertices(int resolution, float bodyLength, float tipLength, float bodyRadius, float tipRadius)
    {
        IList<Vertex> vertices = new Vertex[3 * resolution + 3];
        vertices[0] = new(Vector3D.Zero);
        vertices[1] = new(Vector3D.UnitZ * bodyLength);
        vertices[2] = new(Vector3D.UnitZ * (bodyLength + tipLength));

        float angle = Tau / resolution;
        for (int i = 0; i < resolution; i++)
        {
            float sin = Sin(angle * i), cos = Cos(angle * i);
            vertices[i + 3] = new(new Vector3D(cos * bodyRadius, sin * bodyRadius, 0));
            vertices[i + resolution + 3] = new(new Vector3D(cos * bodyRadius, sin * bodyRadius, bodyLength));
            vertices[i + 2 * resolution + 3] = new(new Vector3D(cos * tipRadius, sin * tipRadius, bodyLength));
        }

        return new EventList<Vertex>(vertices);
    }

    private static EventList<Edge> GenerateEdges(IList<Vertex> vertices, int resolution, EdgeStyle edgeStyle)
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

        //DrawEdges = false;

        return new EventList<Edge>(edges);
    }

    private static EventList<Triangle> GenerateTriangles(IList<Vertex> vertices, int resolution, FaceStyle exteriorFaceStyle)
    {
        IList<Triangle> triangles = new Triangle[6 * resolution];
        var interiorFaceStyle = SolidStyle.Black;

        for (int i = 0; i < resolution - 1; i++)
        {
            triangles[i] = new Triangle(interiorFaceStyle, exteriorFaceStyle, vertices[i + 3], vertices[0], vertices[i + 4]);
            triangles[i + resolution] = new Triangle(interiorFaceStyle, exteriorFaceStyle, vertices[i + 3], vertices[i + resolution + 3], vertices[i + resolution + 4]);
            triangles[i + 2 * resolution] = new Triangle(interiorFaceStyle, exteriorFaceStyle, vertices[i + 3], vertices[i + resolution + 4], vertices[i + 4]);
            triangles[i + 3 * resolution] = new Triangle(interiorFaceStyle, exteriorFaceStyle, vertices[i + resolution + 3], vertices[i + 2 * resolution + 4], vertices[i + 2 * resolution + 3]);
            triangles[i + 4 * resolution] = new Triangle(interiorFaceStyle, exteriorFaceStyle, vertices[i + resolution + 3], vertices[i + resolution + 4], vertices[i + 2 * resolution + 4]);
            triangles[i + 5 * resolution] = new Triangle(interiorFaceStyle, exteriorFaceStyle, vertices[i + 2 * resolution + 3], vertices[i + 2 * resolution + 4], vertices[2]);
        }
        triangles[resolution - 1] = new Triangle(interiorFaceStyle, exteriorFaceStyle, vertices[resolution + 2], vertices[0], vertices[3]);
        triangles[2 * resolution - 1] = new Triangle(interiorFaceStyle, exteriorFaceStyle, vertices[resolution + 2], vertices[2 * resolution + 2], vertices[resolution + 3]);
        triangles[3 * resolution - 1] = new Triangle(interiorFaceStyle, exteriorFaceStyle, vertices[resolution + 2], vertices[resolution + 3], vertices[3]);
        triangles[4 * resolution - 1] = new Triangle(interiorFaceStyle, exteriorFaceStyle, vertices[2 * resolution + 2], vertices[2 * resolution + 3], vertices[3 * resolution + 2]);
        triangles[5 * resolution - 1] = new Triangle(interiorFaceStyle, exteriorFaceStyle, vertices[2 * resolution + 2], vertices[resolution + 3], vertices[2 * resolution + 3]);
        triangles[6 * resolution - 1] = new Triangle(interiorFaceStyle, exteriorFaceStyle, vertices[3 * resolution + 2], vertices[2 * resolution + 3], vertices[2]);

        return new EventList<Triangle>(triangles);
    }

    private static EventList<Face> GenerateFaces(IList<Triangle> triangles, int resolution, FaceStyle exteriorFaceStyle)
    {
        IList<Face> faces = new Face[2 * resolution + 2];
        var interiorFaceStyle = SolidStyle.Black;

        faces[0] = new Face(interiorFaceStyle, exteriorFaceStyle, triangles.Take(resolution));
        faces[1] = new Face(interiorFaceStyle, exteriorFaceStyle, triangles.Skip(3 * resolution).Take(2 * resolution));
        for (int i = 0; i < resolution; i++)
        {
            faces[i + 2] = new Face(interiorFaceStyle, exteriorFaceStyle, triangles[i + resolution], triangles[i + 2 * resolution]);
            faces[i + resolution + 2] = new Face(interiorFaceStyle, exteriorFaceStyle, triangles[i + 5 * resolution]);
        }

        return new EventList<Face>(faces);
    }

    public override Arrow ShallowCopy() => (Arrow)MemberwiseClone();
    public override Arrow DeepCopy()
    {
        var arrow = (Arrow)base.DeepCopy();
        arrow.tipPosition = tipPosition;
        arrow.length = length;
        arrow.bodyLength = bodyLength;
        arrow.tipLength = tipLength;
        arrow.bodyRadius = bodyRadius;
        arrow.tipRadius = tipRadius;
        arrow.resolution = resolution;
        return arrow;
    }

    #endregion
}