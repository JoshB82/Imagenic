/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a light.
 */

using _3D_Engine.Maths.Vectors;
using _3D_Engine.Utilities;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace _3D_Engine.Entities.SceneObjects.RenderingObjects.Lights
{
    /// <summary>
    /// An abstract base class that defines objects of type <see cref="Light"/>. Any object which inherits from this class provides lighting functionality.
    /// </summary>
    public abstract partial class Light : RenderingObject
    {
        #region Fields and Properties

        // Appearance
        private Color colour = Color.White;
        /// <summary>
        /// The <see cref="Color"/> of the <see cref="Light"/>.
        /// </summary>
        public Color Colour
        {
            get => colour;
            set
            {
                colour = value;
                RequestNewRenders();
            }
        }

        private float strength;
        public float Strength
        {
            get => strength;
            set
            {
                strength = value;
                RequestNewRenders();
            }
        }

        // COME BACK TO
        internal bool ShadowMapNeedsUpdating { get; set; } = true;

        #endregion

        #region Constructors

        internal Light(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float viewWidth, float viewHeight, float zNear, float zFar, int renderWidth, int renderHeight) : base(origin, directionForward, directionUp, viewWidth, viewHeight, zNear, zFar, renderWidth, renderHeight) { }

        #endregion

        #region Methods

        // Export
        /// <summary>
        /// Exports the shadow map to the current working directory of the application, in an "Export" folder.
        /// </summary>
        /// <remarks>The folder is created if it does not exist.</remarks>
        public void ExportShadowMap() => ExportShadowMap($"{Directory.GetCurrentDirectory()}\\Export\\{GetType().Name}_{Id}_Export_Map.bmp");

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Light.Export_Shadow_Map(System.String)']/*"/>
        public void ExportShadowMap(string filePath)
        {
            ConsoleOutput.DisplayMessageFromObject(this, "Generating shadow map...");

            string fileDirectory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(fileDirectory)) Directory.CreateDirectory(fileDirectory);

            using (Bitmap shadowMapBitmap = new(RenderWidth, RenderHeight))
            {
                for (int x = 0; x < RenderWidth; x++)
                {
                    for (int y = 0; y < RenderHeight; y++)
                    {
                        if (zBuffer.Values[x][y] == 2)
                        {
                            shadowMapBitmap.SetPixel(x, y, Color.DarkMagenta);
                        }
                        else
                        {
                            int value = (255 * ((zBuffer.Values[x][y] + 1) / 2)).RoundToInt();

                            Color greyscaleColour = Color.FromArgb(255, value, value, value);
                            shadowMapBitmap.SetPixel(x, y, greyscaleColour);
                        }
                    }
                }

                shadowMapBitmap.Save(filePath, ImageFormat.Bmp);
            }

            ConsoleOutput.DisplayMessageFromObject(this, $"Successfully saved shadow map.");
        }

        internal override void ResetBuffers()
        {
            zBuffer.SetAllToValue(outOfBoundsValue);
        }

        internal override void AddPointToBuffers<T>(T data, int x, int y, float z)
        {
            #if DEBUG

            if (x >= 0 && y >= 0 && x < RenderWidth && y < RenderHeight)
            {
                if (z.ApproxLessThan(zBuffer.Values[x][y], 1E-4f))
                {
                    zBuffer.Values[x][y] = z;
                }
            }
            else
            {
                throw new InvalidOperationException($"Attempted to add a point outside buffer range at ({x}, {y}, {z}).");
            }

            #else

            if (z.ApproxLessThan(zBuffer.Values[x][y], 1E-4f))
            {
                zBuffer.Values[x][y] = z;
            }

            #endif
        }

        #endregion
    }
}