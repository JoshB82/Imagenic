/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a torus mesh.
 */

using _3D_Engine.Enums;
using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.PhysicalEntities.Edges;
using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.PhysicalEntities.Faces;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using System.Collections.Generic;
using static System.MathF;

namespace Imagenic.Core.Entities.SceneObjects.Meshes.ThreeDimensions;

/// <summary>
/// A sealed class representing a three-dimensional torus mesh. It inherits from the abstract <see cref="Mesh"/> class.
/// </summary>
/// <remarks>
/// Composition:<br/>
/// <list type="bullet">
/// <item><description>A number of vertices equal to the product of the <see cref="InnerResolution">inner resolution</see> and the <see cref="OuterResolution">outer resolution</see>.</description></item>
/// <item><description></description></item>
/// <item><description></description></item>
/// </list>
/// </remarks>
public sealed class Torus : Mesh
{
    #region Fields and Properties

    private float innerRadius, outerRadius;
    private int innerResolution, outerResolution;

    /// <summary>
    /// The radius of the empty inner circle.
    /// </summary>
    public float InnerRadius
    {
        get => innerRadius;
        set
        {
            if (value == innerRadius) return;
            innerRadius = value;
            RequestNewRenders();

            Structure.Vertices = GenerateVertices(innerResolution, outerResolution, innerRadius, outerRadius);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public float OuterRadius
    {
        get => outerRadius;
        set
        {
            if (value == outerRadius) return;
            outerRadius = value;
            RequestNewRenders();

            Structure.Vertices = GenerateVertices(innerResolution, outerResolution, innerRadius, outerRadius);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public int InnerResolution
    {
        get => innerResolution;
        set
        {
            if (value == innerResolution) return;
            innerResolution = value;
            RequestNewRenders();

            Structure.Vertices = GenerateVertices(innerResolution, outerResolution, innerRadius, outerRadius);
            Structure.Edges = GenerateEdges(innerResolution, outerResolution);
            Structure.Faces = GenerateFaces(innerResolution, outerResolution);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public int OuterResolution
    {
        get => outerResolution;
        set
        {
            if (value == outerResolution) return;
            outerResolution = value;
            RequestNewRenders();

            Structure.Vertices = GenerateVertices(innerResolution, outerResolution, innerRadius, outerRadius);
            Structure.Edges = GenerateEdges(innerResolution, outerResolution);
            Structure.Faces = GenerateFaces(innerResolution, outerResolution);
        }
    }

    #endregion

    #region Constructors

    public Torus(Vector3D worldOrigin,
                 Orientation worldOrientation,
                 float innerRadius,
                 float outerRadius,
                 int innerResolution,
                 int outerResolution) : base(worldOrigin, worldOrientation, GenerateStructure(innerResolution, outerResolution, innerRadius, outerRadius))
    {
        this.innerRadius = innerRadius;
        this.outerRadius = outerRadius;
        this.innerResolution = innerResolution;
        this.outerResolution = outerResolution;
    }

    #endregion

    #region Methods

    private static MeshStructure GenerateStructure(int innerResolution, int outerResolution, float innerRadius, float outerRadius)
    {
        IList<Vertex> vertices = GenerateVertices(innerResolution, outerResolution, innerRadius, outerRadius);
        IList<Edge> edges = GenerateEdges(innerResolution, outerResolution);
        IList<Face> faces = GenerateFaces(innerResolution, outerResolution);

        return new MeshStructure(Dimension.Three, vertices, edges, faces);
    }

    private static IList<Vertex> GenerateVertices(int innerResolution, int outerResolution, float innerRadius, float outerRadius)
    {
        IList<Vertex> vertices = new Vertex[innerResolution * outerResolution];
        vertices[0] = new Vertex(Vector3D.Zero);

        float interiorRadius = (outerRadius - innerRadius) / 2, exteriorRadius = innerRadius + interiorRadius;
        float innerAngle = Tau / innerResolution, outerAngle = Tau / outerResolution;
        for (int i = 0; i < outerResolution; i++)
        {
            for (int j = 0; j < innerResolution; j++)
            {
                vertices[innerResolution * i + j + 1] = new Vertex(new Vector3D(Cos(innerAngle * i) * interiorRadius * Cos(outerAngle * i) * exteriorRadius,
                                                                                        Sin(innerAngle * i) * interiorRadius,
                                                                                        Sin(outerAngle * i) * exteriorRadius));
            }
        }

        return vertices;
    }

    private static IList<Edge> GenerateEdges(int innerResolution, int outerResolution)
    {
        IList<Edge> edges = new Edge[innerResolution * outerResolution * 2];

        return edges;
    }

    private static IList<Face> GenerateFaces(int innerResolution, int outerResolution)
    {
        IList<Face> faces = new Face[innerResolution * outerResolution * 2];

        return faces;
    }

    #endregion
}