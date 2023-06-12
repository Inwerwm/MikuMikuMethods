using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.Extension;
using System.Numerics;

namespace UnitTest;

[TestClass]
internal class UnitTestExtensions
{
    [TestMethod]
    public void TestScale()
    {
        // Arrange
        var source = Quaternion.CreateFromYawPitchRoll(0f, 0f, 0f); // Yaw, Pitch, Roll are all 0
        var destination = Quaternion.CreateFromYawPitchRoll((float)Math.PI, 0f, 0f); // Yaw is PI, Pitch and Roll are 0
        var scale = 0.5f;

        // Act
        var result = source.Scale(destination, scale);

        // Assert
        // Here you can use any appropriate assertion method to validate the result
        // For instance, you might want to check if the result is close to a certain expected quaternion
        // The following is just an example
        var expected = Quaternion.CreateFromYawPitchRoll((float)Math.PI * scale, 0f, 0f); // Expected is halfway rotation (Yaw is PI/2, Pitch and Roll are 0)
        Assert.IsTrue(ApproximatelyEqual(result, expected, 0.0001f));
    }

    private bool ApproximatelyEqual(Quaternion a, Quaternion b, float tolerance)
    {
        // Extract Yaw, Pitch, Roll from the quaternions
        Vector3 eulerA = a.ToEulerAngles();
        Vector3 eulerB = b.ToEulerAngles();

        // Check if the Yaw, Pitch, Roll values are approximately equal
        return Math.Abs(eulerA.X - eulerB.X) <= tolerance &&
               Math.Abs(eulerA.Y - eulerB.Y) <= tolerance &&
               Math.Abs(eulerA.Z - eulerB.Z) <= tolerance;
    }
}
