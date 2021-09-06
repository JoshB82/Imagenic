/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines methods for scaling Meshes.
 */

using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Transformations;
using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.Entities.SceneObjects.Meshes
{
    public static class MeshExtensions
    {
        #region Rotations

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mesh"></param>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static T Rotate<T>(this T mesh, Vector3D axis, float angle) where T : Mesh
        {
            ((SceneObject)mesh).Rotate(axis, angle);

            Matrix4x4 rotation = Transform.Rotate(axis, angle);
            foreach (Vertex vertex in mesh.Vertices)
            {
                if (vertex.Normal.HasValue)
                {
                    vertex.Normal = (Vector3D)(rotation * new Vector4D(vertex.Normal.Value, 1));
                }
            }

            return mesh;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mesh"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static T RotateBetweenVectors<T>(this T mesh, Vector3D v1, Vector3D v2, Vector3D? axis = null) where T : Mesh
        {
            ((SceneObject)mesh).RotateBetweenVectors(v1, v2, axis);

            Matrix4x4 rotation = Transform.RotateBetweenVectors(v1, v2, axis);
            foreach (Vertex vertex in mesh.Vertices)
            {
                if (vertex.Normal.HasValue)
                {
                    vertex.Normal = (Vector3D)(rotation * new Vector4D(vertex.Normal.Value, 1));
                }
            }

            return mesh;
        }

        #endregion

        #region Scaling

        /// <summary>
        /// Scales a <see cref="Mesh"/> in the x-direction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mesh"></param>
        /// <param name="scaleFactor">Factor to scale by.</param>
        /// <returns></returns>
        public static T ScaleX<T>(this T mesh, float scaleFactor) where T : Mesh
        {
            mesh.Scaling = new Vector3D(mesh.Scaling.x * scaleFactor, mesh.Scaling.y, mesh.Scaling.z);
            return mesh;
        }

        /// <summary>
        /// Scales a <see cref="Mesh"/> in the y-direction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mesh"></param>
        /// <param name="scaleFactor">Factor to scale by.</param>
        /// <returns></returns>
        public static T ScaleY<T>(this T mesh, float scaleFactor) where T : Mesh
        {
            mesh.Scaling = new Vector3D(mesh.Scaling.x, mesh.Scaling.y * scaleFactor, mesh.Scaling.z);
            return mesh;
        }

        /// <summary>
        /// Scales a <see cref="Mesh"/> in the z-direction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mesh"></param>
        /// <param name="scaleFactor">Factor to scale by.</param>
        /// <returns></returns>
        public static T ScaleZ<T>(this T mesh, float scaleFactor) where T : Mesh
        {
            mesh.Scaling = new Vector3D(mesh.Scaling.x, mesh.Scaling.y, mesh.Scaling.z * scaleFactor);
            return mesh;
        }

        /// <summary>
        /// Scales a <see cref="Mesh"/> in all directions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mesh"></param>
        /// <param name="scaleFactorX">Factor to scale by in the x-direction.</param>
        /// <param name="scaleFactorY">Factor to scale by in the y-direction.</param>
        /// <param name="scaleFactorZ">Factor to scale by in the z-direction.</param>
        /// <returns></returns>
        public static T Scale<T>(this T mesh, float scaleFactorX, float scaleFactorY, float scaleFactorZ) where T : Mesh
        {
            mesh.Scaling = new Vector3D(mesh.Scaling.x * scaleFactorX, mesh.Scaling.y * scaleFactorY, mesh.Scaling.z * scaleFactorZ);
            return mesh;
        }

        /// <summary>
        /// Scales a <see cref="Mesh"/> in all directions by the same scale factor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mesh"></param>
        /// <param name="scaleFactor">Factor to scale by.</param>
        /// <returns></returns>
        public static T Scale<T>(this T mesh, float scaleFactor) where T : Mesh
        {
            mesh.Scaling = new Vector3D(mesh.Scaling.x * scaleFactor, mesh.Scaling.y * scaleFactor, mesh.Scaling.z * scaleFactor);
            return mesh;
        }

        /// <summary>
        /// Scales a <see cref="Mesh"/> in all directions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mesh"></param>
        /// <param name="scaleFactor">Vector representing factors to scale by.</param>
        /// <returns></returns>
        public static T Scale<T>(this T mesh, Vector3D scaleFactor) where T : Mesh
        {
            mesh.Scaling = scaleFactor;
            return mesh;
        }

        #endregion
    }
}