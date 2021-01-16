/*
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
using _3D_Engine.SceneObjects.Meshes;
using _3D_Engine.SceneObjects.Meshes.Components;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Engine.SceneObjects.Groups
{
    //^^ encapsulates above? And everywhere else?

    /// <summary>
    /// Encapsulates creation of a <see cref="Group"/>.
    /// </summary>
    public partial class Group
    {
        #region Fields and Properties

        public List<SceneObject> SceneObjects { get; set; }

        public override Vector3D WorldOrigin
        {
            get => base.WorldOrigin;
            set
            {
                base.WorldOrigin = value;
                if (SceneObjects is not null)
                {
                    foreach (SceneObject sceneObject in SceneObjects)
                    {
                        sceneObject.WorldOrigin += value - base.WorldOrigin;
                    }
                }
            }
        }

        #endregion

        #region Constructors

        public Group() => SceneObjects = new();
        public Group(List<SceneObject> sceneObjects) => SceneObjects = sceneObjects;

        #endregion

        #region Methods

        public void Add(SceneObject sceneObject) => SceneObjects.Add(sceneObject);
        public void Add(Group group)
        {
            foreach(SceneObject sceneObject in group.SceneObjects)
            {
                SceneObjects.Add(sceneObject);
            }
        }
        public void Add(IEnumerable<SceneObject> sceneObjects)
        {
            foreach(SceneObject sceneObject in sceneObjects)
            {
                SceneObjects.Add(sceneObject);
            }
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

            foreach (Mesh mesh in group.SceneObjects)
            {
                vertices = vertices.Concat(mesh.Vertices).ToArray();
                edges = edges.Concat(mesh.Edges).ToArray();
                faces = faces.Concat(mesh.Faces).ToArray();
                textures = textures.Concat(mesh.Textures).ToArray();
            }

            Custom customCast = new Custom(group.WorldOrigin, group.WorldDirectionForward, group.WorldDirectionUp, vertices, edges, faces, textures);

            return customCast;
        }

        #endregion
    }
}