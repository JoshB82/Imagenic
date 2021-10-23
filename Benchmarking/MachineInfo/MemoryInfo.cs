using Microsoft.VisualBasic.Devices;
using System;

namespace Benchmarking.MachineInfo
{
    internal class MemoryInfo
    {
        // Conversions
        internal static float ConvertByteToGigabyte(ulong numBytes) => (float)Math.Round(numBytes * 1E-9, 2);
        internal static float ConvertByteToTerabyte(ulong numBytes) => (float)Math.Round(numBytes * 1E-12, 2);

        /*
        // Physical memory
        internal string GetTotalPhysicalMemoryBytes() => TotalPhysicalMemory.ToString();
        internal string GetTotalPhysicalMemoryGigabytes() => ConvertByteToGigabyte(TotalPhysicalMemory).ToString();

        // Virtual memory
        internal string GetTotalVirtualMemoryBytes() => TotalVirtualMemory.ToString();
        internal string GetTotalVirtualMemoryTerabytes() => ConvertByteToTerabyte(TotalVirtualMemory).ToString();
        */
    }
}