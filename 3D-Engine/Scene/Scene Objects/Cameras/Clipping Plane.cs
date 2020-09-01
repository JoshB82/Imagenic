﻿namespace _3D_Engine
{
    internal sealed class Clipping_Plane
    {
        /// <summary>
        /// A point on the clipping plane.
        /// </summary>
        internal Vector3D Point { get; set; }
        /// <summary>
        /// Normal vector pointing towards the volume to keep. (on the boundary?, clipping in non-linear space?)
        /// </summary>
        internal Vector3D Normal { get; set; }

        internal Clipping_Plane(Vector3D point, Vector3D normal)
        {
            Point = point;
            Normal = normal;
        }
    }
}