using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagenic.UnitTests.Helpers;

internal static class TestDataHelper
{
    private static readonly Random random = new();

    internal static IEnumerable<int> GenerateRandomInts(int numberToGenerate, int rangeLowest, int rangeHighest)
    {
        int[] result = new int[numberToGenerate];
        return result.Select(x => random.Next(rangeLowest, rangeHighest));
    }

    internal static IEnumerable<float> GenerateRandomFloats(int numberToGenerate, float rangeLowest, float rangeHighest)
    {
        float[] result = new float[numberToGenerate];
        float rangeDifference = rangeHighest - rangeLowest;
        return result.Select(x => random.NextSingle() * rangeDifference + rangeLowest);
    }
}