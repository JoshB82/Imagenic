using System;

namespace Imagenic.SourceGenerators.Test
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CountableAttribute : Attribute
    {
        #region Fields and Properties

        public int? MaxLevel { get; set; }
        public bool IsUnending { get; set; }

        #endregion

        #region Constructors

        public CountableAttribute()
        {
            IsUnending = true;
        }

        public CountableAttribute(int maxLevel)
        {
            if (maxLevel < 1)
            {
                throw new ArgumentException("Invalid maximum level.");
            }
            MaxLevel = maxLevel;
            IsUnending = false;
        }

        #endregion
    }
}