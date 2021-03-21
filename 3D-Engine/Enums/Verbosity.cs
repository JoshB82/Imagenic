/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines an enum for options describing the verbosity of output.
 */

namespace _3D_Engine.Enums
{
    /// <summary>
    /// Options describing the verbosity of output.
    /// </summary>
    public enum Verbosity
    {
        /// <summary>
        /// Indicates that output should be empty.
        /// </summary>
        None,
        /// <summary>
        /// Indicates that output should be short and concise.
        /// </summary>
        Brief,
        /// <summary>
        /// Indicates that output should be comprehensive and thorough.
        /// </summary>
        Detailed,
        /// <summary>
        /// Indicates that output should contain all necessary information.
        /// </summary>
        All
    }
}