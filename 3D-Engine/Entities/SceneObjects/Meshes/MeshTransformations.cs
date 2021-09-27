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
using System;

namespace _3D_Engine.Entities.SceneObjects.Meshes
{
    public static class MeshExtensions
    {
        #region Scaling

        /// <summary>
        /// Scales a <see cref="Mesh"/> in the x-direction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mesh"></param>
        /// <param name="scaleFactor">Factor to scale by.</param>
        /// <returns></returns>
        public static T ScaleX<T>(this T mesh, float scaleFactor, Predicate<Mesh> predicate = null) where T : Mesh
        {
            foreach (Mesh child in mesh.GetAllChildrenAndSelf(x => x is Mesh mesh && predicate(mesh)))
            {
                child.Scaling = new Vector3D(child.Scaling.x * scaleFactor, child.Scaling.y, child.Scaling.z);
            }
            return mesh;
        }

        /// <summary>
        /// Scales a <see cref="Mesh"/> in the y-direction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mesh"></param>
        /// <param name="scaleFactor">Factor to scale by.</param>
        /// <returns></returns>
        public static T ScaleY<T>(this T mesh, float scaleFactor, Predicate<Mesh> predicate = null) where T : Mesh
        {
            foreach (Mesh child in mesh.GetAllChildrenAndSelf(x => x is Mesh mesh && predicate(mesh)))
            {
                child.Scaling = new Vector3D(child.Scaling.x, child.Scaling.y * scaleFactor, child.Scaling.z);
            }
            return mesh;
        }

        /// <summary>
        /// Scales a <see cref="Mesh"/> in the z-direction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mesh"></param>
        /// <param name="scaleFactor">Factor to scale by.</param>
        /// <returns></returns>
        public static T ScaleZ<T>(this T mesh, float scaleFactor, Predicate<Mesh> predicate = null) where T : Mesh
        {
            foreach (Mesh child in mesh.GetAllChildrenAndSelf(x => x is Mesh mesh && predicate(mesh)))
            {
                child.Scaling = new Vector3D(child.Scaling.x, child.Scaling.y, child.Scaling.z * scaleFactor);
            }
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
        public static T Scale<T>(this T mesh, float scaleFactorX, float scaleFactorY, float scaleFactorZ, Predicate<Mesh> predicate = null) where T : Mesh
        {
            foreach (Mesh child in mesh.GetAllChildrenAndSelf(x => x is Mesh mesh && predicate(mesh)))
            {
                child.Scaling = new Vector3D(child.Scaling.x * scaleFactorX, child.Scaling.y * scaleFactorY, child.Scaling.z * scaleFactorZ);
            }
            return mesh;
        }

        /// <summary>
        /// Scales a <see cref="Mesh"/> in all directions by the same scale factor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mesh"></param>
        /// <param name="scaleFactor">Factor to scale by.</param>
        /// <returns></returns>
        public static T Scale<T>(this T mesh, float scaleFactor, Predicate<Mesh> predicate = null) where T : Mesh
        {
            foreach (Mesh child in mesh.GetAllChildrenAndSelf(x => x is Mesh mesh && predicate(mesh)))
            {
                child.Scaling = new Vector3D(child.Scaling.x * scaleFactor, child.Scaling.y * scaleFactor, child.Scaling.z * scaleFactor);
            }
            return mesh;
        }

        /// <summary>
        /// Scales a <see cref="Mesh"/> in all directions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mesh"></param>
        /// <param name="scaleFactor">Vector representing factors to scale by.</param>
        /// <returns></returns>
        public static T Scale<T>(this T mesh, Vector3D scaleFactor, Predicate<Mesh> predicate = null) where T : Mesh
        {
            foreach (Mesh child in mesh.GetAllChildrenAndSelf(x => x is Mesh mesh && predicate(mesh)))
            {
                child.Scaling = scaleFactor;
            }
            return mesh;
        }

        #endregion
    }
}