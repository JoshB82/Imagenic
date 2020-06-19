using System.Drawing;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Spot"/>.
    /// </summary>
    public struct Spot
    {
        #region Fields and Properties

        // Positions
        internal Vector4D Point { get; set; }
        public Vector3D World_Point { get; internal set; }

        // Appearance
        public Color Colour { get; set; }
        public int Diameter { get; set; }
        public bool Visible { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Spot"/>.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Spot"/>.</param>
        /// <param name="colour">The colour of the <see cref="Spot"/>.</param>
        public Spot(Vector4D origin, Color? colour = null) : this()
        {
            Point = origin;
            Colour = colour ?? Color.BlueViolet;
            Diameter = 10;
            Visible = true;
        }

        #endregion
    }

    /// <summary>
    /// Handles creation of an <see cref="Edge"/>.
    /// </summary>
    public struct Edge
    {
        #region Fields and Properties

        // Positions
        internal Vector4D P1 { get; set; }
        internal Vector4D P2 { get; set; }
        public Vector3D World_P1 { get; internal set; }
        public Vector3D World_P2 { get; internal set; }

        // Appearance
        public Color Colour { get; set; }
        public bool Visible { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an <see cref="Edge"/>
        /// </summary>
        /// <param name="p1">The position of the first point on the <see cref="Edge"/>.</param>
        /// <param name="p2">The position of the second point on the <see cref="Edge"/>.</param>
        /// <param name="colour">The colour of the <see cref="Edge"/>.</param>
        public Edge(Vector4D p1, Vector4D p2, Color? colour = null) : this()
        {
            P1 = p1; P2 = p2;
            Colour = colour ?? Color.Black;
            Visible = true;
        }

        #endregion
    }

    // Don't understand how this() works :(
    /// <summary>
    /// Handles creation of a <see cref="Face"/>.
    /// </summary>
    public struct Face
    {
        #region Fields and Properties

        // Positions
        internal Vector4D P1 { get; set; }
        internal Vector4D P2 { get; set; }
        internal Vector4D P3 { get; set; }
        public Vector3D World_P1 { get; internal set; }
        public Vector3D World_P2 { get; internal set; }
        public Vector3D World_P3 { get; internal set; }
        public Vector3D T1 { get; internal set; }
        public Vector3D T2 { get; internal set; }
        public Vector3D T3 { get; internal set; }

        // Appearance
        public Color Colour { get; set; }
        public Texture Texture_Object { get; set; }
        public bool Draw_Outline { get; set; }
        public bool Visible { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="colour"></param>
        public Face(Vector4D p1, Vector4D p2, Vector4D p3, Color? colour = null) : this()
        {
            P1 = p1; P2 = p2; P3 = p3;
            Colour = colour ?? Color.SeaGreen;
            Draw_Outline = false;
            Visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="texture_object"></param>
        public Face(Vector4D p1, Vector4D p2, Vector4D p3, Vector3D t1, Vector3D t2, Vector3D t3, Texture texture_object) : this()
        {
            P1 = p1; P2 = p2; P3 = p3;
            T1 = t1; T2 = t2; T3 = t3;
            Texture_Object = texture_object;
            Draw_Outline = false;
            Visible = true;
        }

        #endregion
    }

    public class Texture
    {
        #region Fields and Properties

        /// <summary>
        /// The <see cref="Bitmap"/> containing the texture data.
        /// </summary>
        public Bitmap File { get; set; }
        /// <summary>
        /// Defines how the outside of a <see cref="Texture"/> file should be drawn.
        /// </summary>
        public Outside_Texture_Behaviour Outside_Behaviour { get; set; } = Outside_Texture_Behaviour.Repeat;
        /// <summary>
        /// <see cref="Color"/> used to fill outside of <see cref="Texture"/> should <see cref="Outside_Texture_Behaviour.Colour_Fill"/> be selected for Outside_Behaviour.
        /// </summary>
        public Color Outside_Colour { get; set; } = Color.Black;
        public Vector3D[] Vertices { get; set; }

        #endregion

        #region Constructors

        public Texture(Bitmap file, Vector3D[] vertices)
        {
            File = file;
            Vertices = vertices;
        }

        #endregion

        public static Vector3D[] Generate_Vertices(string type)
        {
            switch(type)
            {
                case "plane":
                case "square":
                    return new Vector3D[4] // WHY Z=1?
                    {
                        new Vector3D(0, 0, 1), // 0
                        new Vector3D(1, 0, 1), // 1
                        new Vector3D(1, 1, 1), // 2
                        new Vector3D(0, 1, 1) // 3
                    };
                default:
                    return null;
            }
        }
    }

    public enum Outside_Texture_Behaviour : byte
    {
        Colour_Fill,
        Repeat,
        Edge_Stretch
    }
}