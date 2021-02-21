/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Provides methods for outputting console messages.
 */

using System;
using System.Diagnostics;

namespace _3D_Engine.Miscellaneous
{
    internal static class ConsoleOutput
    {
        private static string GetTime() => DateTime.Now.ToString("HH:mm:ss");
        
        // Display message
        internal static void DisplayMessage(string message) => Trace.WriteLine($"[{GetTime()}] {message}");
        internal static void DisplayMessageFromObject(object @object, string message) => DisplayMessage($"[{@object.GetType().Name}] {message}");
    }
}