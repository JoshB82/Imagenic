/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a MessageBuilder<T> which creates customisable messages.
 */

using _3D_Engine.Constants;
using _3D_Engine.Enums;
using System;
using System.Diagnostics;
using System.IO;

namespace _3D_Engine.Utilities;

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
            message = $"[{GetTime()}]";
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

    internal MessageBuilder<T> AddType<U>() => AddType(typeof(U));

    internal MessageBuilder<T> AddParameters(params string[] parameters)
    {
        this.parameters = parameters;
        return this;
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

    internal U BuildIntoException<U>() where U : Exception
    {
        return Activator.CreateInstance(typeof(U), Build()) as U;
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

internal static class ExceptionHelper
{
    internal static void ThrowIfParameterIsNull<T>(T parameter, string parameterName)
    {
        if (parameter is null)
        {
            throw new MessageBuilder<ParameterCannotBeNullException>()
                    .AddParameters(parameterName)
                    .BuildIntoException<ParameterCannotBeNullException>();
        }
    }

    internal static void ThrowIfFileNotFound(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new MessageBuilder<FileNotFoundMessage>()
                .AddParameters(filePath)
                .BuildIntoException<FileNotFoundException>();
        }
    }
}

#region Messages

internal class FileNotFoundMessage : IVerbose
{
    public string BriefVerbosityText { get; set; } = "File not found.";
    public string DetailedVerbosityText { get; set; } = "{0} was not found.";
    public string AllVerbosityText { get; set; } = "The file {0} was not found.";
}

internal class ParameterNotSupportedMessage : IVerbose
{
    public string BriefVerbosityText { get; set; } = "Parameter not supported.";
    public string DetailedVerbosityText { get; set; } = "{0} is not supported.";
    public string AllVerbosityText { get; set; } = "The parameter {0} is not supported.";
}

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