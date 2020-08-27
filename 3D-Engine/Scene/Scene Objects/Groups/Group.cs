using System.Collections.Generic;
using System.Linq;

namespace _3D_Engine
{
    public partial class Group : Scene_Object
    {
        #region Fields and Properties

        public List<Scene_Object> Scene_Objects { get; set; }

        public override Vector3D World_Origin
        {
            get => base.World_Origin;
            set
            {
                base.World_Origin = value;
                if (Scene_Objects != null) foreach (Scene_Object scene_object in Scene_Objects) scene_object.World_Origin += value - base.World_Origin;
            }
        }

        #endregion

        #region Constructors

        public Group(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, bool has_direction_arrows = true) : base(origin, direction_forward, direction_up, has_direction_arrows)
        {
            Scene_Objects = new List<Scene_Object>();
        }

        public Group(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, List<Scene_Object> scene_objects, bool has_direction_arrows = true) : base(origin, direction_forward, direction_up, has_direction_arrows)
        {
            Scene_Objects = scene_objects;
        }

        #endregion

        #region Casting

        /// <summary>
        /// Casts the <see cref="Group"/> into a <see cref="Custom"/>.
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

            Custom custom_cast = new Custom(group.World_Origin, group.World_Direction_Forward, group.World_Direction_Up, vertices, edges, faces, textures);

            return custom_cast;
        }

        #endregion
    }
}