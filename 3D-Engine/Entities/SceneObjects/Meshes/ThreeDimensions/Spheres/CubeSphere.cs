using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions.Spheres
{
    public sealed class CubeSphere : Mesh
    {
        #region Fields and Properties

        public override MeshContent Content { get; set; } = new MeshContent();

        #endregion

        #region Constructors

        public CubeSphere(Vector3D worldOrigin, Orientation worldOrientation) : base(worldOrigin, worldOrientation, 3)
        {

        }

        #endregion

        #region Methods

        #endregion
    }
}