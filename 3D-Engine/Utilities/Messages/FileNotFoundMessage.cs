using System.Collections.Generic;
using System;

namespace Imagenic.Core.Utilities.Messages;

internal class FileNotFoundMessage : IMessage<FileNotFoundMessage>
{
    public static MessageInterpolatedStringHandler<FileNotFoundMessage> BriefText => $"File not found.";
    public static MessageInterpolatedStringHandler<FileNotFoundMessage> DetailedText => $"{0} was not found.";
    public static MessageInterpolatedStringHandler<FileNotFoundMessage> AllText => $"The file {0} was not found.";


    public static List<string>? ConstantParameters { get; set; }
    public static List<Func<string?>>? ResolvableParameters { get; set; }
}