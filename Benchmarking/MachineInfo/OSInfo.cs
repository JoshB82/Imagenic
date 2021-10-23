using System;

namespace Benchmarking.MachineInfo
{
    internal static class OSInfo
    {
        internal static string OperatingSystemVersion => Environment.OSVersion.VersionString;
    }
}