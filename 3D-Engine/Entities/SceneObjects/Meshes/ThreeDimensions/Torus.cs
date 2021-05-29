using _3D_Engine.Maths.Vectors;
using _3D_Engine.SceneObjects.Meshes;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Torus"/> mesh.
    /// </summary>
    public sealed class Torus : Mesh
    {
        #region Fields and Properties

        public float InnerRadius { get; set; }
        public float OuterRadius { get; set; }

        #endregion

        #region Constructors

        public Torus(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float radius, float innerRadius, float outerRadius) : base(origin, directionForward, directionUp)
        {
            Dimension = 3;
        }

        #endregion
    }
}