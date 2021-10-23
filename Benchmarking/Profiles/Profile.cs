using System;

namespace Benchmarking.Profiles
{
    internal class Profile
    {
        internal string Name { get; set; }
        internal Action Action { get; set; }

        internal Profile(string name, Action action) => (Name, Action) = (name, action);
    }
}