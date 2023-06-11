using System.Numerics;

namespace MikuMikuMethods.Extension;

/// <summary>
/// 数値に関する拡張メソッドを提供するクラスです。
/// </summary>
public static class NumericExtensions
{
    /// <summary>
    /// クォータニオンの回転量をスケーリングします。
    /// </summary>
    /// <param name="source">開始点となるクォータニオン</param>
    /// <param name="destination">終点となるクォータニオン</param>
    /// <param name="scale">スケール値。この値によって、source から destination への回転量が調整されます。</param>
    /// <returns>スケーリング後の新しいクォータニオンを返します。</returns>
    public static Quaternion Scale(this Quaternion source, Quaternion destination, float scale)
    {
        var normalizedSource = Quaternion.Normalize(source);
        var normalizedDestination = Quaternion.Normalize(destination);

        var inverseSource = Quaternion.Inverse(normalizedSource);
        var rotationFromSourceToDestination = Quaternion.Multiply(inverseSource, normalizedDestination);

        var rotationAngle = 2f * (float)Math.Acos(rotationFromSourceToDestination.W);
        var rotationAxis = new Vector3(rotationFromSourceToDestination.X, rotationFromSourceToDestination.Y, rotationFromSourceToDestination.Z);
        var normalizedRotationAxis = rotationAxis.LengthSquared() > 0f ? Vector3.Normalize(rotationAxis) : Vector3.UnitX;

        var scaledRotationAngle = rotationAngle * scale;
        var scaledRotation = Quaternion.CreateFromAxisAngle(normalizedRotationAxis, scaledRotationAngle);

        return Quaternion.Multiply(normalizedSource, scaledRotation);
    }
}
