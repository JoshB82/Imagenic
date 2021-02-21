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
using _3D_Engine.Miscellaneous;
using _3D_Engine.Rendering;
using _3D_Engine.Transformations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace _3D_Engine.SceneObjects.RenderingObjects.Lights
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Light"/>.
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
                UpdateRenderCamera();
            }
        }
        
        private float strength;
        public float Strength
        {
            get => strength;
            set
            {
                strength = value;
                UpdateRenderCamera();
            }
        }

        // COME BACK TO
        internal bool ShadowMapNeedsUpdating { get; set; } = true;

        // Shadow map volume
        internal Buffer2D<float> ShadowMap { get; set; }

        public override int RenderWidth
        {
            get => base.RenderWidth;
            set
            {
                base.RenderWidth = value;
                UpdateProperties();
            }
        }
        public override int RenderHeight
        {
            get => base.RenderHeight;
            set
            {
                base.RenderHeight = value;
                UpdateProperties();
            }
        }

        private void UpdateProperties()
        {
            // Set shadow map
            ShadowMap = new(RenderWidth, RenderHeight);
            
            // Set screen-to-window matrix
            ScreenToWindow = Transform.Scale(0.5f * (RenderWidth - 1), 0.5f * (RenderHeight - 1), 1) * RenderingObject.windowTranslate;
        }

        #endregion

        #region Constructors

        internal Light(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float viewWidth, float viewHeight, float zNear, float zFar) : base(origin, directionForward, directionUp, viewWidth, viewHeight, zNear, zFar) { }

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

            using (Bitmap shadowMapBitmap = new Bitmap(RenderWidth, RenderHeight))
            {
                for (int x = 0; x < RenderWidth; x++)
                {
                    for (int y = 0; y < RenderHeight; y++)
                    {
                        if (ShadowMap.Values[x][y] == 2)
                        {
                            shadowMapBitmap.SetPixel(x, y, Color.DarkMagenta);
                        }
                        else
                        {
                            int value = (255 * ((ShadowMap.Values[x][y] + 1) / 2)).RoundToInt();

                            Color greyscaleColour = Color.FromArgb(255, value, value, value);
                            shadowMapBitmap.SetPixel(x, y, greyscaleColour);
                        }
                    }
                }

                shadowMapBitmap.Save(filePath, ImageFormat.Bmp);
            }

            ConsoleOutput.DisplayMessageFromObject(this, $"Successfully saved shadow map");
        }

        #endregion
    }
}