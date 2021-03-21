/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines an enum for options describing how view volume outlines are drawn.
 */

using System;

namespace _3D_Engine.Enums
{
    /// <summary>
    /// Options describing how view volume outlines are drawn.
    /// </summary>
    [Flags]
    public enum VolumeOutline : byte
    {
        /// <summary>
        /// Indicates that no view volume outline should be drawn.
        /// </summary>
        None = 0,
        /// <summary>
        /// Indicates that a view volume outline should be drawn from the origin to the near plane.
        /// </summary>
        Near = 1,
        /// <summary>
        /// Indicates that a view volume outline should be drawn from the origin to the far plane.
        /// </summary>
        Far = 2
    }
}
