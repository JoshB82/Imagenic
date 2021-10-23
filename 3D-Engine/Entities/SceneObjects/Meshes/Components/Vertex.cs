/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 *
 */

using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.Entities.SceneObjects.Meshes.Components
{
    /// <summary>
    /// Encapsulates creation of an <see cref="Vertex"/>.
    /// </summary>
    public sealed class Vertex
    {
        #region Fields and Properties

        public Vector3D? Normal { get; set; }
        public Vector4D Point { get; set; }

        #endregion

        #region Constructors

        public Vertex(Vector4D point, Vector3D? normal = null)
        {
            Point = point;
            Normal = normal;
        }

        #endregion
    }
}