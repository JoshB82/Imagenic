using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unit_Tests
{
    [TestClass]
    public class OrientationTests
    {
        [TestMethod]
        public void SetDirectionForwardUpTest()
        {
            Orientation orientation = Orientation.CreateOrientationForwardUp(Vector3D.UnitZ, Vector3D.UnitY);

            Orientation expectedOutcome = Orientation.CreateOrientationForwardUp(Vector3D.UnitNegativeZ, Vector3D.UnitX);
            orientation.SetDirectionForwardUp(Vector3D.UnitNegativeZ, Vector3D.UnitX);
            Orientation actualOutcome = orientation;

            Assert.AreEqual(expectedOutcome, actualOutcome);
        }
    }
}