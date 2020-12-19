/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a world point mesh.
 */

using _3D_Engine.Maths.Vectors;
using _3D_Engine.SceneObjects.Meshes.Components;

namespace _3D_Engine.SceneObjects.Meshes.OneDimension
{
    /// <summary>
    /// Encapsulates creation of a <see cref="WorldPoint"/> mesh.
    /// </summary>
    public sealed class WorldPoint : Mesh
    {
        #region Constructors

        /// <summary>
        /// Creates a <see cref="WorldPoint"/> mesh.
        /// </summary>
        /// <param name="origin">The position of the <see cref="WorldPoint"/>.</param>
        public WorldPoint(Vector3D origin) : this(origin, Vector3D.UnitZ, Vector3D.UnitY) { }

        public WorldPoint(Vector3D origin, Vector3D directionForward, Vector3D directionUp) : base(origin, directionForward, directionUp)
        {
            Dimension = 1;
            
            Vertices = new Vertex[1] { new Vertex(new Vector4D(0, 0, 0, 1)) };

            Draw_Edges = false;
            
            Draw_Faces = false;
        }

        #endregion
    }
}