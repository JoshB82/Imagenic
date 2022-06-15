/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines methods for scaling Meshes and other functionality.
 */

using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.PhysicalEntities.Faces;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Triangles;
using Imagenic.Core.Entities.TransformableEntities.TranslatableEntities.OrientatedEntities.PhysicalEntities;
using System;
using System.Drawing;

namespace Imagenic.Core.Entities.SceneObjects.Meshes
{
    public static class MeshExtensions
    {
        #region Miscellaneous

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mesh"></param>
        /// <param name="colour"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T ColourAllSolidFaces<T>(this T mesh, Color colour, Predicate<Mesh> predicate = null) where T : Mesh
        {
            foreach (Mesh child in mesh.GetAllChildrenAndSelf<Mesh>(predicate))
            {
                foreach (Face face in child.Structure.Faces)
                {
                    foreach (Triangle triangle in face.Triangles)
                    {
                        if (triangle is SolidTriangle solidTriangle)
                        {
                            solidTriangle.Colour = colour;
                        }
                    }
                }
            }
            return mesh;
        }

        #endregion

        #region Scaling

        public static T ScaleX<T>(this T physicalEntity, float scaleFactor) where T : PhysicalEntity
        {

        }

        public static IEnumerable<T> ScaleX<T>(this IEnumerable<T> physicalEntities, float scaleFactor) where T : PhysicalEntity
        {

        }

        public static IEnumerable<T> ScaleX<T>(this IEnumerable<T> physicalEntities, float scaleFactor, Func<T, bool> predicate) where T : PhysicalEntity
        {

        }

        public static Node<T> ScaleX<T>(this Node<T> physicalEntityNode, float scaleFactor) where T : PhysicalEntity
        {

        }

        public static Node<T> ScaleX<T>(this Node<T> physicalEntityNode, float scaleFactor, Func<T, bool> predicate) where T : PhysicalEntity
        {

        }

        public static IEnumerable<Node<T>> ScaleX<T>(this IEnumerable<Node<T>> physicalEntityNodes, float scaleFactor) where T : PhysicalEntity
        {

        }

        public static IEnumerable<Node<T>> ScaleX<T>(this IEnumerable<Node<T>> physicalEntityNodes, float scaleFactor, Func<Node<T>, bool> predicate) where T : PhysicalEntity
        {

        }








        /// <summary>
        /// Scales a <see cref="Mesh"/> in the x-direction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mesh"></param>
        /// <param name="scaleFactor">Factor to scale by.</param>
        /// <returns></returns>
        public static T ScaleX<T>(this T mesh, float scaleFactor, Predicate<Mesh> predicate = null) where T : Mesh
        {
            foreach (Mesh child in mesh.GetAllChildrenAndSelf<Mesh>(predicate))
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
            foreach (Mesh child in mesh.GetAllChildrenAndSelf<Mesh>(predicate))
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
            foreach (Mesh child in mesh.GetAllChildrenAndSelf<Mesh>(predicate))
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
            foreach (Mesh child in mesh.GetAllChildrenAndSelf<Mesh>(predicate))
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
            foreach (Mesh child in mesh.GetAllChildrenAndSelf<Mesh>(predicate))
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
            foreach (Mesh child in mesh.GetAllChildrenAndSelf<Mesh>(predicate))
            {
                child.Scaling = scaleFactor;
            }
            return mesh;
        }

        #endregion
    }
}