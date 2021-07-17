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

using System.Collections.Generic;
using _3D_Engine.Entities.SceneObjects.Meshes;
using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Entities.SceneObjects;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Lights;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;

namespace _3D_Engine.Entities.Groups
{
    //^^ encapsulates above? And everywhere else?

    /// <summary>
    /// Encapsulates creation of a <see cref="Group"/>.
    /// </summary>
    public partial class Group
    {
        #region Fields and Properties

        // Contents
        public List<SceneObject> SceneObjects { get; set; } = new();
        public List<Camera> Cameras { get; set; } = new();
        public List<Light> Lights { get; set; } = new();
        public List<Mesh> Meshes { get; set; } = new();

        // Render
        private Camera renderCamera;
        internal Camera RenderCamera
        {
            get => renderCamera;
            set
            {
                renderCamera = value;
                foreach (SceneObject sceneObject in SceneObjects)
                {
                    sceneObject.RenderCameras.Add(renderCamera);
                }
            }
        }

        #endregion

        #region Constructors

        public Group() { }
        public Group(SceneObject sceneObject) => Add(sceneObject);
        public Group(IEnumerable<SceneObject> sceneObjects) => Add(sceneObjects);
        public Group(params SceneObject[] sceneObjects) => Add(sceneObjects);
        public Group(Group group) => Add(group);
        public Group(IEnumerable<Group> groups) => Add(groups);
        public Group(params Group[] groups) => Add(groups);

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
                sceneObject.RenderCameras.Add(RenderCamera);
            }
        }
        public void Add(IEnumerable<SceneObject> sceneObjects)
        {
            foreach (SceneObject sceneObject in sceneObjects)
            {
                Add(sceneObject);
            }
        }
        public void Add(params SceneObject[] sceneObjects) => Add((IEnumerable<SceneObject>)sceneObjects);
        public void Add(Group group) => Add(group.SceneObjects);
        public void Add(IEnumerable<Group> groups)
        {
            foreach (Group group in groups)
            {
                Add(group);
            }
        }
        public void Add(params Group[] groups) => Add((IEnumerable<Group>)groups);

        // Remove
        //public void RemoveAll(Predicate<SceneObject> predicate) => ;

        //??
        public void Remove(int id)
        {
            SceneObjects.RemoveAll(x => x.Id == id);
            switch (SceneObjects.Find(x => x.Id == id))
            {
                case Camera camera:
                    Cameras.Remove(camera);
                    break;
                case Light light:
                    Lights.Remove(light);
                    break;
                case Mesh mesh:
                    Meshes.Remove(mesh);
                    break;
            }
        }
        public void Remove(IEnumerable<int> ids)
        {
            foreach (int id in ids)
            {
                Remove(id);
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
            List<Vertex> vertices = new();
            List<Edge> edges = new();
            List<Face> faces = new();
            List<Texture> textures = new();

            foreach (Mesh mesh in group.Meshes)
            {
                vertices.AddRange(mesh.Vertices);
                edges.AddRange(mesh.Edges);
                //faces.AddRange(mesh.Triangles);
                textures.AddRange(mesh.Textures);
                faces.AddRange(mesh.Faces);
            }

            Custom customCast = new(group.Meshes[0].WorldOrigin, group.Meshes[0].WorldDirectionForward, group.Meshes[0].WorldDirectionUp, vertices.ToArray(), edges.ToArray(), faces.ToArray(), textures.ToArray());

            return customCast;
        }

        #endregion
    }
}