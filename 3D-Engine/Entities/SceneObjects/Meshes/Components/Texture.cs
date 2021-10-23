/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 *
 */

using _3D_Engine.Maths.Vectors;
using System;
using System.Drawing;

namespace _3D_Engine.Entities.SceneObjects.Meshes.Components
{
    public sealed class Texture : IDisposable
    {
        #region Fields and Properties

        /// <summary>
        /// The <see cref="Bitmap"/> containing the <see cref="Texture"/> data.
        /// </summary>
        public Bitmap File { get; set; }
        /// <summary>
        /// Defines how the outside of a <see cref="Texture"/> file should be drawn.
        /// </summary>
        public OutsideTextureBehaviour Outside_Behaviour { get; set; } = OutsideTextureBehaviour.Repeat;
        /// <summary>
        /// The <see cref="Color"/> used to fill outside of <see cref="Texture"/> should <see cref="OutsideTextureBehaviour.Colour_Fill"/> be selected for Outside_Behaviour.
        /// </summary>
        public Color OutsideColour { get; set; } = Color.Black;
        public Vector3D[] Vertices { get; set; }

        #endregion

        #region Constructors
        
        public Texture(Bitmap file, Vector3D[] vertices)
        {
            File = file;
            Vertices = vertices;
        }

        #endregion

        #region Methods

        public void Dispose() => throw new NotImplementedException();

        public static Vector3D[] Generate_Vertices(string type) =>
            type switch
            {
                "Plane" or "Square" => new Vector3D[4] // WHY z=1?
                {
                    new Vector3D(0, 0, 1), // 0
                    new Vector3D(1, 0, 1), // 1
                    new Vector3D(1, 1, 1), // 2
                    new Vector3D(0, 1, 1) // 3
                },
                _ => throw new ArgumentException($"Could not generate texture vertices; unknown type: {type}", type), //discard?
            };

        #endregion

    }

    public enum OutsideTextureBehaviour : byte
    {
        Colour_Fill,
        Repeat,
        Edge_Stretch
    }
}