/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines an enum for options describing how the viewport is constructed.
 */

namespace _3D_Engine.Enums;

/// <summary>
/// Options describing how the viewport is constructed.
/// </summary>
public enum Viewport
{
    /// <summary>
    /// Indicates that the viewport should consist of a single view covering all available space.
    /// </summary>
    Single,
    /// <summary>
    /// Indicates that the viewport should consist of two identical views but separated vertically down the middle of the available space and each taking up half of the available space.
    /// </summary>
    LeftAndRight,
    /// <summary>
    /// Indicates that the viewport should consist of two identical views but separated horizontally across the middle of the available space and each taking up half of the available space.
    /// </summary>
    TopAndBottom
}