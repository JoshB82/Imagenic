namespace _3D_Engine
{
    public class Shape : Scene_Object
    {
        #region Fields and Properties

        // ID
        /// <summary>
        /// Unique identification number for this shape.
        /// </summary>
        public int ID { get; private set; }
        private static int next_id = -1;

        // Meshes
        public Mesh Collision_Mesh { get; set; }
        public Mesh Render_Mesh { get; set; }

        // Appearance
        /// <summary>
        /// Determines if the shape is selected or not.
        /// </summary>
        public bool Selected { get; set; } = false;
        /// <summary>
        /// Determines if the shape is visible or not.
        /// </summary>
        public bool Visible { get; set; } = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a shape with a collision mesh and a render mesh.
        /// </summary>
        /// <param name="collision_mesh">The mesh that will be used when determing collisions.</param>
        /// <param name="render_mesh">The mesh that will be rendered to the screen.</param>
        public Shape(Mesh collision_mesh, Mesh render_mesh)
        {
            ID = ++next_id;

            Collision_Mesh = collision_mesh;
            Render_Mesh = render_mesh;
        }

        /// <summary>
        /// Create a shape with a render mesh that is also a collision mesh.
        /// </summary>
        /// <param name="render_mesh">The mesh that will be rendered and used for determining collisions.</param>
        public Shape(Mesh render_mesh) : this(render_mesh, render_mesh) { }

        #endregion
    }
}