using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.SceneObjects.RenderingObjects
{
    internal sealed class ClippingPlane
    {
        /// <summary>
        /// A point on the clipping plane.
        /// </summary>
        internal Vector3D Point;

        /// <summary>
        /// Normal vector pointing towards the volume to keep. (on the boundary?, clipping in non-linear space?)
        /// </summary>
        internal Vector3D Normal;

        internal ClippingPlane(Vector3D point, Vector3D normal)
        {
            Point = point;
            Normal = normal;
        }
    }
}