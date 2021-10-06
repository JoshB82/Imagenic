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

using _3D_Engine.Entities.SceneObjects;
using _3D_Engine.Enums;
using System;
using System.Diagnostics;

namespace _3D_Engine.Utilities
{
    internal static class DisplayMessage<T> where T : IVerbose, new()
    {
        private const string projectName = "3D-Engine";
        private static string GetTime() => DateTime.Now.ToString("HH:mm:ss");

        internal static void WithParameters(params string[] parameters)
        {
            T message = new();
            string content = Properties.Settings.Default.Verbosity switch
            {
                Verbosity.None => string.Empty,
                Verbosity.Brief => message.BriefVerbosityText,
                Verbosity.Detailed => message.DetailedVerbosityText,
                Verbosity.All => message.AllVerbosityText,
                _ => throw new Exception("Cannot handle setting.")
            };

            Trace.WriteLine($"[{GetTime()}] [{projectName}] {string.Format(content, parameters)}");
        }

        internal static void WithTypeAndParameters<U>(params string[] parameters)
        {
            T message = new();
            string content = Properties.Settings.Default.Verbosity switch
            {
                Verbosity.None => string.Empty,
                Verbosity.Brief => message.BriefVerbosityText,
                Verbosity.Detailed => message.DetailedVerbosityText,
                Verbosity.All => message.AllVerbosityText,
                _ => throw new Exception("Cannot handle setting.")
            };

            Trace.WriteLine($"[{GetTime()}] [{projectName}] [{typeof(U)}] {string.Format(content, parameters)}");
        }
    }

    internal static class ConsoleOutput
    {
        private const string projectName = "3D-Engine";
        private static string GetTime() => DateTime.Now.ToString("HH:mm:ss");

        // Display message
        internal static void DisplayMessage(string message) => Trace.WriteLine($"[{projectName}] [{GetTime()}] {message}");
        internal static void DisplayMessageFromObject(object @object, string message) => DisplayMessage($"[{@object.GetType().Name}] {message}");

        // From specific methods
        internal static void DisplayOutputDirectionMessage(SceneObject sceneObject, Verbosity verbosity)
        {
            switch (verbosity)
            {
                case Verbosity.Brief:
                    DisplayMessageFromObject(sceneObject, "Changed direction.");
                    break;
                case Verbosity.Detailed:
                    DisplayMessageFromObject(sceneObject, "Changed direction vectors.");
                    break;
                case Verbosity.All:
                    DisplayMessageFromObject(sceneObject,
                        "Changed direction to:\n" +
                        $"Forward: {sceneObject.WorldDirectionForward}\n" +
                        $"Up: {sceneObject.WorldDirectionUp}\n" +
                        $"Right: {sceneObject.WorldDirectionRight}");
                    break;
            }
        }

        internal static void DisplayMessageFrom<T>(string message)
        {

        }
    }
}