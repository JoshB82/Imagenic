using Imagenic.Core.Maths.Vectors;
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

    internal static float GenerateRandomFloat(float rangeLowest, float rangeHighest) => random.NextSingle() * (rangeHighest - rangeLowest) + rangeLowest;

    internal static IEnumerable<float> GenerateRandomFloats(int numberToGenerate, float rangeLowest, float rangeHighest)
    {
        float[] result = new float[numberToGenerate];
        return result.Select(x => GenerateRandomFloat(rangeLowest, rangeHighest));
    }

    internal static IEnumerable<Vector2D> GenerateRandomVector2Ds(int numberToGenerate, float rangeLowest, float rangeHighest)
    {
        Vector2D[] result = new Vector2D[numberToGenerate];
        return result.Select(x => new Vector2D(GenerateRandomFloats(2, rangeLowest, rangeHighest)));
    }

    internal static IEnumerable<Vector3D> GenerateRandomVector3Ds(int numberToGenerate, float rangeLowest, float rangeHighest)
    {
        Vector3D[] result = new Vector3D[numberToGenerate];
        return result.Select(x => new Vector3D(GenerateRandomFloats(3, rangeLowest, rangeHighest)));
    }

    internal static IEnumerable<Vector4D> GenerateRandomVector4Ds(int numberToGenerate, int rangeLowest, int rangeHighest)
    {
        Vector4D[] result = new Vector4D[numberToGenerate];
        return result.Select(x => new Vector4D(GenerateRandomFloats(4, rangeLowest, rangeHighest)));
    }
}