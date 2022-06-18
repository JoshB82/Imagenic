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

namespace Imagenic.Core.Enums;

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

/// <summary>
/// Interface ensuring all implementing types contain strings describing text for all verbosities.
/// </summary>
public interface IVerbose
{
    //public string BriefVerbosityText { get; }
    //public string DetailedVerbosityText { get; }
    //public string AllVerbosityText { get; }

    //static abstract string NoneVerbosityText { get; } = string.Empty;
    static abstract string BriefVerbosityText { get; }
    static abstract string DetailedVerbosityText { get; }
    static abstract string AllVerbosityText { get; }
}