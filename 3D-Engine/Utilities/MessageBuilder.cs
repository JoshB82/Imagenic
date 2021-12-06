/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Provides methods for displaying messages.
 */

using _3D_Engine.Entities.SceneObjects;
using _3D_Engine.Enums;
using System;
using System.Diagnostics;

namespace _3D_Engine.Utilities
{
    internal class MessageBuilder<T> where T : IVerbose, new()
    {
        private const string projectName = "3D-Engine";
        private static string GetTime() => DateTime.Now.ToString("HH:mm:ss");

        private string message;

        private string[] parameters;

        private string AddToMessage(string value)
        {
            if (message is not null)
            {
                message += " ";
            }
            return message += value;
        }

        internal MessageBuilder(bool includeTime = true, bool includeProjectName = true)
        {
            if (includeTime)
            {
                AddToMessage($"[{GetTime()}]");
            }
            if (includeProjectName)
            {
                AddToMessage($"[{projectName}]");
            }
        }

        internal MessageBuilder<T> AddType(Type type)
        {
            AddToMessage($"[{type}]");
            return this;
        }

        internal MessageBuilder<T> AddType<U>()
        {
            Message = $"[{type}] {Message}";

            Trace.WriteLine($"[{GetTime()}] [{projectName}] [{typeof(U)}] {GetMessage()}");

            return this;
        }

        internal MessageBuilder<T> AddParameters(params string[] parameters)
        {
            this.parameters = parameters;

            Trace.WriteLine($"[{GetTime()}] [{projectName}] {GetMessageWithParameters(parameters)}");

            return this;
        }

        

        internal static void WithTypeAndParameters<U>(params string[] parameters)
        {
            if (Properties.Settings.Default.Verbosity == Verbosity.None)
            {
                return;
            }

            Trace.WriteLine($"[{GetTime()}] [{projectName}] [{typeof(U)}] {GetMessageWithParameters(parameters)}");
        }

        private static string GetMessage()
        {
            T text = new();
            return Properties.Settings.Default.Verbosity switch
            {
                Verbosity.Brief => text.BriefVerbosityText,
                Verbosity.Detailed => text.DetailedVerbosityText,
                Verbosity.All => text.AllVerbosityText,
                _ => throw new Exception("Cannot handle setting.")
            };
        }

        internal string Build()
        {
            if (Properties.Settings.Default.Verbosity == Verbosity.None)
            {
                return string.Empty;
            }
            else if (parameters is null)
            {
                return AddToMessage(GetMessage());
            }
            else
            {
                return AddToMessage(string.Format(GetMessage(), parameters));
            }
        }
    }

    internal static class MessageHelper
    {
        internal static void DisplayInConsole(this string message)
        {
            if (message == string.Empty)
            {
                return;
            }

            Trace.WriteLine(message);
        }
    }

    #region Messages

    internal class OrientationChangedMessage : IVerbose
    {
        public string BriefVerbosityText { get; set; } = "Changed orientation.";
        public string DetailedVerbosityText { get; set; } = "Changed orientation.";
        public string AllVerbosityText { get; set; } = "Changed orientation to: Forward: {0}, Up: {1}, Right: {2}";
    }

    internal class EntityCreatedMessage : IVerbose
    {
        public string BriefVerbosityText { get; set; } = "Created.";
        public string DetailedVerbosityText { get; set; } = "Created at {0}.";
        public string AllVerbosityText { get; set; } = "Entity created at {0}.";
    }

    internal class GeneratingDepthValuesMessage : IVerbose
    {
        public string BriefVerbosityText { get; set; } = "Generating values...";
        public string DetailedVerbosityText { get; set; } = "Generating depth values...";
        public string AllVerbosityText { get; set; } = "Generating depth values...";
    }

    internal class GeneratedDepthValuesMessage : IVerbose
    {
        public string BriefVerbosityText { get; set; } = "Generated values.";
        public string DetailedVerbosityText { get; set; } = "Generated depth values.";
        public string AllVerbosityText { get; set; } = "Generated depth values";
    }

    #endregion



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
    }
}