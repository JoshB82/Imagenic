using Imagenic.Core.Maths.Vectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.MathF;

namespace Imagenic.UnitTests.Tests;

[TestClass]
public class Vector2DTests
{
    [TestMethod]
    public void AngleTest()
    {
        Vector2D firstVector = new(100, 0);
        Vector2D secondVector = new(0, 10);

        float expectedOutcome = PI / 2;
        float actualOutcome = firstVector.Angle(secondVector);

        Assert.AreEqual(expectedOutcome, actualOutcome);
    }

    [TestMethod]
    public void AddTest()
    {
        Vector2D firstVector = new(12, 34);
        Vector2D secondVector = new(7.3f, 3);

        Vector2D expectedOutcome = new(19.3f, 37);
        Vector2D actualOutcome = firstVector + secondVector;

        Assert.AreEqual(expectedOutcome, actualOutcome);
    }

    [TestMethod]
    public void SubtractTest()
    {
        Vector2D firstVector = new(1, 4.3f);
        Vector2D secondVector = new(1.9f, 33);

        Vector2D expectedOutcome = new(-0.9f, -28.7f);
        Vector2D actualOutcome = firstVector - secondVector;

        Assert.AreEqual(expectedOutcome, actualOutcome);
    }

    [TestMethod]
    public void DotProductTest()
    {
        Vector2D firstVector = new(2.4f, 2);
        Vector2D secondVector = new(2, 3);

        float expectedOutcome = 10.8f;
        float actualOutcome = firstVector * secondVector;

        Assert.AreEqual(expectedOutcome, actualOutcome);
    }

    [TestMethod]
    public void MultiplyTest1()
    {
        Vector2D vector = new(2, 3);
        float scalar = 32;

        Vector2D expectedOutcome = new(64, 96);
        Vector2D actualOutcome = vector * scalar;

        Assert.AreEqual(expectedOutcome, actualOutcome);
    }

    [TestMethod]
    public void MultiplyTest2()
    {
        float scalar = -3;
        Vector2D vector = new(23, 33);

        Vector2D expectedOutcome = new(-69, -99);
        Vector2D actualOutcome = scalar * vector;

        Assert.AreEqual(expectedOutcome, actualOutcome);
    }

    [TestMethod]
    public void DivisionTest()
    {
        Vector2D vector = new(2, 3);
        float scalar = 5;

        Vector2D expectedOutcome = new(0.4f, 0.6f);
        Vector2D actualOutcome = vector / scalar;

        Assert.AreEqual(expectedOutcome, actualOutcome);
    }

    [TestMethod]
    public void NegationTest()
    {
        Vector2D vector = new(31, 24);

        Vector2D expectedOutcome = new(-31, -24);
        Vector2D actualOutcome = -vector;

        Assert.AreEqual(expectedOutcome, actualOutcome);
    }
}