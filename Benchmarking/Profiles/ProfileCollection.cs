using System.Collections.Generic;

namespace Benchmarking.Profiles
{
    internal static partial class ProfileCollection
    {
        internal static List<Profile> Profiles { get; set; } = new();

        internal static void PopulateProfileList()
        {
            // Add profiles in this method
            Profiles.Add(new Profile("Cube render", CubeRender));
        }
    }
}