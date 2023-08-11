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

using Imagenic.Core.Enums;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;

namespace Imagenic.Core.Entities;

/// <summary>
/// An abstract base class that defines objects of type <see cref="Light"/>. Any object which inherits from this class provides lighting functionality.
/// </summary>
public abstract partial class Light : RenderingEntity
{
    #region Fields and Properties

    // Appearance
    private Color colour = Color.White;
    /// <summary>
    /// The colour of the <see cref="Light"/>.
    /// </summary>
    public Color Colour
    {
        get => colour;
        set
        {
            if (value == colour) return;
            colour = value;
            InvokeRenderEvent(RenderUpdate.NewRender);
        }
    }

    private float strength;
    public float Strength
    {
        get => strength;
        set
        {
            if (value < 0)
            {
                // throw exception
            }
            if (value == strength) return;
            strength = value;
            InvokeRenderEvent(RenderUpdate.NewRender);
        }
    }

    #endregion

    #region Constructors

    protected Light(Vector3D worldOrigin,
                    [DisallowNull] Orientation worldOrientation,
                    float viewWidth,
                    float viewHeight,
                    float zNear,
                    float zFar) : base(worldOrigin, worldOrientation, viewWidth, viewHeight, zNear, zFar) { }

    #endregion

    #region Methods

    /*
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
    */
    #endregion
}