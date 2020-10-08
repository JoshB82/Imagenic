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

namespace _3D_Engine
{
    /// <summary>
    /// Encapsulates creation of a <see cref="World_Point"/> mesh.
    /// </summary>
    public sealed class World_Point : Mesh
    {
        #region Constructors

        /// <summary>
        /// Creates a <see cref="World_Point"/> mesh.
        /// </summary>
        /// <param name="origin">The position of the <see cref="World_Point"/>.</param>
        public World_Point(Vector3D origin) : base(origin, Vector3D.Unit_Z, Vector3D.Unit_Y)
        {
            Dimension = 1;

            Vertices = new Vertex[1] { new Vertex(new Vector4D(0, 0, 0, 1)) };

            Draw_Edges = false;
            
            Draw_Faces = false;
        }

        #endregion
    }
}