/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a point Mesh called a WorldPoint which consists of a single Vertex.
 */

using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.Entities.SceneObjects.Meshes.OneDimension
{
    /// <summary>
    /// Encapsulates creation of a <see cref="WorldPoint"/> mesh.
    /// </summary>
    public sealed class WorldPoint : Mesh
    {
        #region Fields and Properties

        public override MeshContent Content { get; set; } = new MeshContent();

        public static readonly WorldPoint ZeroOrigin = new(Vector3D.Zero);

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="WorldPoint"/> mesh.
        /// </summary>
        /// <param name="worldOrigin">The position of the <see cref="WorldPoint"/>.</param>
        public WorldPoint(Vector3D worldOrigin) : this(worldOrigin, Orientation.ModelOrientation) { }

        public WorldPoint(Vector3D worldOrigin,
                          Orientation worldOrientation) : base(worldOrigin, worldOrientation, 0)
        {
            Vertices = new Vertex[1] { new Vertex(new Vector4D(0, 0, 0, 1)) };
            DrawEdges = false;
            DrawFaces = false;
        }

        #endregion
    }
}