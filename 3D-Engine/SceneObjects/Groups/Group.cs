﻿/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a group.
 */

using _3D_Engine.Maths.Vectors;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Engine.SceneObjects.Groups
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Group"/>.
    /// </summary>
    public partial class Group : SceneObject
    {
        #region Fields and Properties

        public List<SceneObject> Scene_Objects { get; set; }

        public override Vector3D WorldOrigin
        {
            get => base.WorldOrigin;
            set
            {
                base.WorldOrigin = value;
                if (Scene_Objects is not null) foreach (SceneObject scene_object in Scene_Objects) scene_object.WorldOrigin += value - base.WorldOrigin;
            }
        }

        #endregion

        #region Constructors

        public Group(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, bool has_direction_arrows = true) : base(origin, direction_forward, direction_up, has_direction_arrows)
        {
            Scene_Objects = new List<SceneObject>();
        }

        public Group(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, List<SceneObject> scene_objects, bool has_direction_arrows = true) : base(origin, direction_forward, direction_up, has_direction_arrows)
        {
            Scene_Objects = scene_objects;
        }

        #endregion

        #region Casting

        /// <summary>
        /// Casts a <see cref="Group"/> into a <see cref="Custom"/>.
        /// </summary>
        /// <param name="group"><see cref="Group"/> to cast.</param>
        public static explicit operator Custom(Group group)
        {
            Vertex[] vertices = new Vertex[0];
            Edge[] edges = new Edge[0];
            Face[] faces = new Face[0];
            Texture[] textures = new Texture[0];

            foreach (Mesh mesh in group.Scene_Objects)
            {
                vertices = vertices.Concat(mesh.Vertices).ToArray();
                edges = edges.Concat(mesh.Edges).ToArray();
                faces = faces.Concat(mesh.Faces).ToArray();
                textures = textures.Concat(mesh.Textures).ToArray();
            }

            Custom custom_cast = new Custom(group.WorldOrigin, group.WorldDirectionForward, group.WorldDirectionUp, vertices, edges, faces, textures);

            return custom_cast;
        }

        #endregion
    }
}