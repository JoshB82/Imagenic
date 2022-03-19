using Imagenic.Core.Maths.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagenic.UnitTests.Helpers;

internal static class TestDataHelper
{
    private static readonly Random random = new();
    private static readonly Range defaultPermittedValues = new(-100, 100);

    internal static IList<int> GenerateRandomInts(int numberToGenerate, Range? permittedValues = null)
    {
        permittedValues ??= defaultPermittedValues;
        return new int[numberToGenerate].Select(x => random.Next(permittedValues.Value.Start.Value, permittedValues.Value.End.Value)).ToArray();
    }

    internal static float GenerateRandomFloat(Range? permittedValues = null)
    {
        permittedValues ??= defaultPermittedValues;
        int rangeLowest = permittedValues.Value.Start.Value;
        int rangeHighest = permittedValues.Value.End.Value;
        return random.NextSingle() * (rangeHighest - rangeLowest) + rangeLowest;
    }

    internal static IList<float> GenerateRandomFloats(int numberToGenerate, Range? permittedValues = null)
    {
        return new float[numberToGenerate].Select(x => GenerateRandomFloat(permittedValues)).ToArray();
    }

    internal static IList<Vector2D> GenerateRandomVector2Ds(int numberToGenerate, Range? permittedValues = null)
    {
        return new Vector2D[numberToGenerate].Select(x => new Vector2D(GenerateRandomFloats(2, permittedValues))).ToArray();
    }

    internal static IList<Vector3D> GenerateRandomVector3Ds(int numberToGenerate, Range? permittedValues = null)
    {
        return new Vector3D[numberToGenerate].Select(x => new Vector3D(GenerateRandomFloats(3, permittedValues))).ToArray();
    }

    internal static IList<Vector4D> GenerateRandomVector4Ds(int numberToGenerate, Range? permittedValues = null)
    {
        return new Vector4D[numberToGenerate].Select(x => new Vector4D(GenerateRandomFloats(4, permittedValues))).ToArray();
    }
}