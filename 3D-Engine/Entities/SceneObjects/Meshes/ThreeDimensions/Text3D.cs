using _3D_Engine.Maths.Vectors;
using _3D_Engine.SceneObjects.Meshes;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions
{
    public sealed class Text3D : Mesh
    {
        #region Fields and Properties

        public string[] Fonts { get; set; }
        public float Size { get; set; }
        public char Style { get; set; }
        public float Depth { get; set; }

        #endregion

        #region Constructors
        
        public Text3D(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, string[] fonts, float size, char style, float depth) : base(origin, direction_forward, direction_up)
        {
            Dimension = 3;


        }

        #endregion
    }
}