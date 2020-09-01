﻿using System.Diagnostics;
using System.Drawing;

namespace _3D_Engine
{
    public sealed class Texture
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
        /// The <see cref="Color"/> used to fill outside of <see cref="Texture"/> should <see cref="Outside_Texture_Behaviour.Colour_Fill"/> be selected for Outside_Behaviour.
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
            switch (type)
            {
                case "Plane":
                case "Square":
                    return new Vector3D[4] // WHY Z=1?
                    {
                    new Vector3D(0, 0, 1), // 0
                    new Vector3D(1, 0, 1), // 1
                    new Vector3D(1, 1, 1), // 2
                    new Vector3D(0, 1, 1) // 3
                    };
                default:
                    Trace.WriteLine("Could not generate texture vertices; unknown type: " + type);
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