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
using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Enums;
using Imagenic.Core.Loaders.Meshes;
using Imagenic.Core.Loaders.Readers;
using Imagenic.Core.Utilities;
using Imagenic.Core.Utilities.Messages;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Imagenic.Core.Entities;

/// <summary>
/// Encapsulates creation of a <see cref="Mesh"/>.
/// </summary>
[Serializable]
public partial class Mesh : PhysicalEntity
{
    #region Fields and Properties

    #if DEBUG
    
    //[NonSerialized] - not available for properties? :(
    private protected override IMessageBuilder<MeshCreatedMessage>? MessageBuilder => (IMessageBuilder<MeshCreatedMessage>?)base.MessageBuilder;

    #endif

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
            InvokeRenderEvent(RenderUpdate.NewRender);
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
            InvokeRenderEvent();
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
    //public bool CastsShadows { get; set; } = true;
    /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Draw_Outline']/*"/>
    public bool DrawOutline { get; set; } = false;
    //public float Opacity { get; set; } = 1f;

    // Matrices and Vectors
    internal override void CalculateModelToWorldMatrix()
    {
        base.CalculateModelToWorldMatrix();
        ModelToWorld *= Transform.Scale(Scaling);
    }



    #endregion

    #region Constructors

    #if DEBUG

    private protected Mesh(Vector3D worldOrigin,
                           Orientation worldOrientation,
                           MeshStructure structure,
                           IMessageBuilder<MeshCreatedMessage> mb) : base(worldOrigin, worldOrientation, mb)
    {
        MessageBuilder!.AddParameter(DrawEdges)
                       .AddParameter(DrawFaces)
                       .AddParameter(DrawOutline)
                       .AddParameter(HasTexture);

        NonDebugConstructorBody(structure);
    }

    #else

    protected Mesh(Vector3D worldOrigin,
                   Orientation worldOrientation,
                   MeshStructure structure) : base(worldOrigin, worldOrientation)
    {
        DrawEdges = Structure.Edges is not null;
        DrawFaces = Structure.Faces is not null;
    }

    /// <summary>
    /// Creates a <see cref="Custom"/> mesh from joining two other meshes.
    /// </summary>
    /// <param name="worldOrigin">The position of the resultant <see cref="Custom"/> mesh.</param>
    /// <param name="worldOrientation">The direction the resultant <see cref="Custom"/> mesh faces.</param>
    /// <param name="m1">The first <see cref="Mesh"/> to be joined.</param>
    /// <param name="m2">The second <see cref="Mesh"/> to be joined.</param>
    public Mesh(Vector3D worldOrigin,
                [DisallowNull] Orientation worldOrientation,
                Mesh m1,
                Mesh m2) : base(worldOrigin, worldOrientation)
    {
        Structure = new MeshStructure();
        Structure.Vertices = m1.Structure.Vertices.Concat(m2.Structure.Vertices).ToEventList();
        Structure.Edges = m1.Structure.Edges.Concat(m2.Structure.Edges).ToEventList();
        Structure.Triangles = m1.Structure.Triangles.Concat(m2.Structure.Triangles).ToEventList();
        Structure.Faces = m1.Structure.Faces.Concat(m2.Structure.Faces).ToEventList();
        Textures = m1.Textures.Concat(m2.Textures).ToArray();
    }

    #endif

    private void NonDebugConstructorBody(MeshStructure structure)
    {
        Structure = structure;
    }

    public static async Task<Mesh> CreateMeshWithReader(Vector3D worldOrigin,
                                                        [DisallowNull] Orientation worldOrientation,
                                                        [DisallowNull] MeshReader reader,
                                                        CancellationToken ct = default)
    {
        ThrowIfNull(worldOrientation, reader);
        var structure = new MeshStructure(await reader.GetVerticesAsync(ct),
                                          await reader.GetEdgesAsync(ct),
                                          await reader.GetFacesAsync(ct));

        #if DEBUG

        return new Mesh(worldOrigin, worldOrientation, structure, MessageBuilder<MeshCreatedMessage>.Instance()); //?

        #else

        return new Mesh(worldOrigin, worldOrientation, structure);

        #endif
    }

    #endregion

    #region Methods

    public Mesh Join(Mesh mesh)
    {
        Structure.Vertices.AddRange(mesh.Structure.Vertices);
        Structure.Edges.AddRange(mesh.Structure.Edges);
        Structure.Triangles.AddRange(mesh.Structure.Triangles);
        Structure.Faces.AddRange(mesh.Structure.Faces);
        return this;
    }

    #endregion
}