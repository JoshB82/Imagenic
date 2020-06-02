namespace _3D_Graphics
{
    /// <summary>
    /// Class that defines planes that clip shapes.
    /// </summary>
    public sealed class Clipping_Plane
    {
        /// <summary>
        /// A point on the clipping plane.
        /// </summary>
        public Vector3D Point { get; set; }
        /// <summary>
        /// Normal vector pointing towards the volume to keep.
        /// </summary>
        public Vector3D Normal { get; set; }

        public Clipping_Plane(Vector3D point, Vector3D normal)
        {
            Point = point;
            Normal = normal;
        }
    }
}