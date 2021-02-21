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

using _3D_Engine.SceneObjects.Meshes;
using _3D_Engine.SceneObjects.Meshes.Components;
using _3D_Engine.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.SceneObjects.RenderingObjects.Lights;
using System.Collections.Generic;

namespace _3D_Engine.SceneObjects.Groups
{
    //^^ encapsulates above? And everywhere else?

    /// <summary>
    /// Encapsulates creation of a <see cref="Group"/>.
    /// </summary>
    public partial class Group
    {
        #region Fields and Properties

        public List<SceneObject> SceneObjects { get; set; } = new();
        public List<Camera> Cameras { get; set; } = new();
        public List<Light> Lights { get; set; } = new();
        public List<Mesh> Meshes { get; set; } = new();

        internal Camera RenderCamera { get; set; }

        #endregion

        #region Constructors

        public Group() { }

        public Group(IEnumerable<SceneObject> sceneObjects) => Add(sceneObjects);

        #endregion

        #region Methods

        // Add
        public void Add(SceneObject sceneObject)
        {
            SceneObjects.Add(sceneObject);
            switch (sceneObject)
            {
                case Camera camera:
                    Cameras.Add(camera);
                    break;
                case Light light:
                    Lights.Add(light);
                    break;
                case Mesh mesh:
                    Meshes.Add(mesh);
                    break;
            }

            if (RenderCamera is not null)
            {
                RenderCamera.NewRenderNeeded = true;
                sceneObject.RenderCamera = RenderCamera;
            }
        }
        public void Add(Group group) => Add(group.SceneObjects);
        public void Add(IEnumerable<SceneObject> sceneObjects)
        {
            foreach(SceneObject sceneObject in sceneObjects)
            {
                Add(sceneObject);
            }
        }

        // Remove
        public void Remove() { }

        #endregion

        #region Casting

        /// <summary>
        /// Casts a <see cref="Group"/> into a <see cref="Custom"/>.
        /// </summary>
        /// <param name="group"><see cref="Group"/> to cast.</param>
        public static explicit operator Custom(Group group)
        {
            List<Vertex> vertices = new();
            List<Edge> edges = new();
            List<Face> faces = new();
            List<Texture> textures = new();

            foreach (Mesh mesh in group.Meshes)
            {
                vertices.AddRange(mesh.Vertices);
                edges.AddRange(mesh.Edges);
                faces.AddRange(mesh.Faces);
                textures.AddRange(mesh.Textures);
            }

            Custom customCast = new Custom(group.Meshes[0].WorldOrigin, group.Meshes[0].WorldDirectionForward, group.Meshes[0].WorldDirectionUp, vertices.ToArray(), edges.ToArray(), faces.ToArray(), textures.ToArray());

            return customCast;
        }

        #endregion
    }
}