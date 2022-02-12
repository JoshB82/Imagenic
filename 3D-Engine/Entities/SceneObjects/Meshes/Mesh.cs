/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a Mesh, which represents any one, two or three-dimensional mesh.
 */

using _3D_Engine.Constants;
using _3D_Engine.Entities.SceneObjects.RenderingObjects;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Transformations;
using _3D_Engine.Maths.Vectors;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Triangles;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes;

/// <summary>
/// Encapsulates creation of a <see cref="Mesh"/>.
/// </summary>
public abstract partial class Mesh : SceneObject
{
    #region Fields and Properties

    // Structure
    private MeshStructure structure;
    public MeshStructure Structure
    {
        get => structure;
        set
        {
            structure = value ?? throw new ParameterCannotBeNullException();
        }
    }
    
    /// <summary>
    /// The <see cref="Vertex">vertices</see> in the <see cref="Mesh"/>.
    /// </summary>
    //public Vertex[] Vertices { get; protected set; }
    /// <summary>
    /// The <see cref="Edge">edges</see> in the <see cref="Mesh"/>.
    /// </summary>
    //public Edge[] Edges { get; protected set; }
    /// <summary>
    /// The <see cref="Triangle">triangles</see> in the <see cref="Mesh"/>.
    /// </summary>
    //public Triangle[] Triangles { get; internal set; }

    // Appearance
    private bool drawEdges;
    /// <summary>
    /// Determines if the <see cref="Mesh"> Mesh's</see> <see cref="Edge">Edges</see> are drawn.
    /// </summary>
    public bool DrawEdges
    {
        get => drawEdges;
        set
        {
            if (value == drawEdges) return;
            drawEdges = value;
            RequestNewRenders();
        }
    }

    private bool drawFaces;
    /// <summary>
    /// Determines if the<see cref="Mesh"> Mesh's</see> <see cref="Triangle">Faces</see> are drawn.
    /// </summary>
    public bool DrawFaces
    {
        get => drawFaces;
        set
        {
            if (value == drawFaces) return;
            drawFaces = value;
            RequestNewRenders();
        }
    }

    // Headed Rendering Object
    internal List<RenderingObject> HeadedRenderingObjects { get; set; } = new();
    internal override void RequestNewRenders()
    {
        base.RequestNewRenders();

        foreach (RenderingObject renderingObject in HeadedRenderingObjects)
        {
            foreach (Camera camera in renderingObject.Renderers)
            {
                camera.NewRenderNeeded = true;
            }
        }
    }

    // Textures
    /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Textures']/*"/>
    public Texture[] Textures { get; internal set; }
    /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Has_Texture']/*"/>
    public bool HasTexture { get; protected set; } = false;

    // Miscellaneous
    /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Dimension']/*"/>
    //public int Dimension { get; }
    /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Casts_Shadows']/*"/>
    public bool CastsShadows { get; set; } = true;
    /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Draw_Outline']/*"/>
    public bool DrawOutline { get; set; } = false;
    public float Opacity { get; set; } = 1f;

    // Matrices and Vectors
    internal override void CalculateModelToWorldMatrix()
    {
        base.CalculateModelToWorldMatrix();
        ModelToWorld *= Transform.Scale(Scaling);
    }

    private Vector3D scaling = Vector3D.One;
    public Vector3D Scaling
    {
        get => scaling;
        set
        {
            if (value == scaling) return;
            scaling = value;
            CalculateModelToWorldMatrix();
            RequestNewRenders();
        }
    }

    #endregion

    #region Constructors

    protected Mesh(Vector3D worldOrigin,
                   Orientation worldOrientation,
                   MeshStructure structure,
                   bool hasDirectionArrows = true) : base(worldOrigin, worldOrientation, hasDirectionArrows)
    {
        if (worldOrientation is null)
        {
            // throw exception
        }

        Structure = structure;

        DrawEdges = Structure.Edges is not null;
        DrawFaces = Structure.Faces is not null;
    }

    #endregion
}