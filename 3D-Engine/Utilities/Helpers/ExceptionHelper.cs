using Imagenic.Core.Utilities.Messages;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Imagenic.Core.Utilities.Helpers;

internal static class ExceptionHelper
{
    internal static void ThrowIfNull(object? param,
        [CallerArgumentExpression("param")] string? paramName = null)
    {
        if (param is null)
        {
            throw MessageBuilder<ArgumentCannotBeNullMessage>.Instance()
                .AddParameter(paramName)
                .BuildIntoException<ArgumentNullException>();
        }
    }

    internal static void ThrowIfNull(object? param1, object? param2,
        [CallerArgumentExpression("param1")] string? param1Name = null,
        [CallerArgumentExpression("param2")] string? param2Name = null)
    {
        if (param1 is null || param2 is null)
        {
            throw MessageBuilder<ArgumentCannotBeNullMessage>.Instance()
                .AddParameter(param1Name)
                .AddParameter(param2Name)
                .BuildIntoException<ArgumentNullException>();
        }
    }

    internal static void ThrowIfNull(object? param1, object? param2, object? param3,
        [CallerArgumentExpression("param1")] string? param1Name = null,
        [CallerArgumentExpression("param2")] string? param2Name = null,
        [CallerArgumentExpression("param3")] string? param3Name = null)
    {
        if (param1 is null || param2 is null || param3 is null)
        {
            throw MessageBuilder<ArgumentCannotBeNullMessage>.Instance()
                .AddParameter(param1Name)
                .AddParameter(param2Name)
                .AddParameter(param3Name)
                .BuildIntoException<ArgumentNullException>();
        }
    }

    internal static void ThrowIfNull(object? param1, object? param2, object? param3, object? param4,
        [CallerArgumentExpression("param1")] string? param1Name = null,
        [CallerArgumentExpression("param2")] string? param2Name = null,
        [CallerArgumentExpression("param3")] string? param3Name = null,
        [CallerArgumentExpression("param4")] string? param4Name = null)
    {
        if (param1 is null || param2 is null || param3 is null || param4 is null)
        {
            throw MessageBuilder<ArgumentCannotBeNullMessage>.Instance()
                .AddParameter(param1Name)
                .AddParameter(param2Name)
                .AddParameter(param3Name)
                .AddParameter(param4Name)
                .BuildIntoException<ArgumentNullException>();
        }
    }

    internal static void ThrowIfFileNotFound(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw MessageBuilder<FileNotFoundMessage>.Instance()
                .AddParameter(filePath)
                .BuildIntoException<FileNotFoundException>();
        }
    }

    internal static void ThrowIfValueOutsideOfInclusiveRange(float rangeLowest, float rangeHighest, float value)
    {

        if (value < rangeLowest || value > rangeHighest)
        {
            new NumberOfItemsOutOfRangeMessage
            {
                ContainerName = "",
                ItemsName = ""
            };
        }
    }

    internal static void ThrowIfOutsideExclusiveRange(this float value, float rangeLowest, float rangeHighest)
    {
        if (value <= rangeLowest)
        {
            ThrowException(RangeViolationType.TooLow);
        }
        if (value >= rangeHighest)
        {
            ThrowException(RangeViolationType.TooHigh);
        }

        void ThrowException(RangeViolationType rangeViolationType)
        {

        }
    }
}