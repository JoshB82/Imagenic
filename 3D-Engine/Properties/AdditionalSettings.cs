using Imagenic.Core.Enums;

namespace Imagenic.Core.Properties;

internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase
{
    private Verbosity outputVerbosity;

    public Verbosity OutputVerbosity
    {
        get => outputVerbosity;
        set => outputVerbosity = value;
    }
}