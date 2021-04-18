using System.Collections.Generic;
using System.Management;

namespace Benchmarking.MachineInfo
{
    internal static class CPUInfo
    {
        internal static Dictionary<string, object> CPUInfoDictionary => GetCPUInfo();
        internal static Dictionary<string, object> GetCPUInfo()
        {
            Dictionary<string, object> cpuInfoDictionary = new();
            ManagementClass managementClass = new("Win32_Processor");
            ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();
            PropertyDataCollection propertyDataCollection = managementClass.Properties;

            foreach (ManagementObject managementObject in managementObjectCollection)
            {
                foreach (PropertyData propertyData in propertyDataCollection)
                {
                    cpuInfoDictionary.Add(propertyData.Name, managementObject.Properties[propertyData.Name].Value);
                }
            }

            return cpuInfoDictionary;
        }

        internal static string GetCPUName => CPUInfoDictionary["Name"].ToString();
    }
}