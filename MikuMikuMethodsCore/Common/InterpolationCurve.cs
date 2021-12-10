using System.Collections.ObjectModel;

namespace MikuMikuMethods;

/// <summary>
/// 補間項目
/// </summary>
public enum InterpolationItem
{
    /// <summary>
    /// x軸移動
    /// </summary>
    XPosition,
    /// <summary>
    /// Y軸移動
    /// </summary>
    YPosition,
    /// <summary>
    /// Z軸移動
    /// </summary>
    ZPosition,
    /// <summary>
    /// 回転
    /// </summary>
    Rotation,
    /// <summary>
    /// 距離
    /// </summary>
    Distance,
    /// <summary>
    /// 視野角
    /// </summary>
    ViewAngle,
}

/// <summary>
/// 補間曲線
/// </summary>
public class InterpolationCurve : ICloneable
{
    private (byte X, byte Y) earlyControlePoint;
    private (byte X, byte Y) lateControlePoint;

    /// <summary>
    /// 始点側制御点
    /// [0,127]
    /// </summary>
    public (byte X, byte Y) EarlyControlePoint
    {
        get => earlyControlePoint;
        set
        {
            if (value.X is < 0 or > 127 || value.Y is < 0 or > 127)
                throw new ArgumentOutOfRangeException("InterpolationCurve.EarlyControlePoint has seted to out of range value.");
            earlyControlePoint = value;
        }
    }
    /// <summary>
    /// 終点側制御点
    /// [0,127]
    /// </summary>
    public (byte X, byte Y) LateControlePoint
    {
        get => lateControlePoint;
        set
        {
            if (value.X is < 0 or > 127 || value.Y is < 0 or > 127)
                throw new ArgumentOutOfRangeException("InterpolationCurve.LateControlePoint has seted to out of range value.");
            lateControlePoint = value;
        }
    }

    /// <summary>
    /// 始点側制御点
    /// [0.0,1.0]
    /// </summary>
    public (float X, float Y) EarlyControlePointFloat
    {
        get => (EarlyControlePoint.X / 127.0f, EarlyControlePoint.Y / 127.0f);
        set
        {
            if (value.X is < 0 or > 1 || value.Y is < 0 or > 1)
                throw new ArgumentOutOfRangeException("InterpolationCurve.EarlyControlePointFloat has seted to out of range value.");
            EarlyControlePoint = ((byte)(value.X * 127), (byte)(value.Y * 127));
        }
    }
    /// <summary>
    /// 終点側制御点
    /// [0.0,1.0]
    /// </summary>
    public (float X, float Y) LateControlePointFloat
    {
        get => (LateControlePoint.X / 127.0f, LateControlePoint.Y / 127.0f);
        set
        {
            if (value.X is < 0 or > 1 || value.Y is < 0 or > 1)
                throw new ArgumentOutOfRangeException("InterpolationCurve.LateControlePointFloat has seted to out of range value.");
            LateControlePoint = ((byte)(value.X * 127), (byte)(value.Y * 127));
        }
    }

    /// <summary>
    /// バイト配列から値を指定する
    /// </summary>
    /// <param name="bytes">{EarlyControlePoint.X, EarlyControlePoint.Y, LateControlePoint.X, LateControlePoint.Y}</param>
    public void FromBytes(IEnumerable<byte> bytes)
    {
        try
        {
            earlyControlePoint.X = bytes.ElementAt(0);
            earlyControlePoint.Y = bytes.ElementAt(1);
            lateControlePoint.X = bytes.ElementAt(2);
            lateControlePoint.Y = bytes.ElementAt(3);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            throw new ArgumentException("When setting up an interpolation curve from a byte sequence, at least four elements are required.", ex);
        }
    }

    /// <summary>
    /// バイト配列で返す
    /// {始点側制御点X, 始点側制御点Y, 終点側制御点X, 終点側制御点Y}
    /// </summary>
    public byte[] ToBytes() => new byte[] { EarlyControlePoint.X, EarlyControlePoint.Y, LateControlePoint.X, LateControlePoint.Y };

    /// <summary>
    /// VMD形式のバイト列から補間曲線クラスの連想配列を生成する。
    /// </summary>
    /// <param name="data">バイト列</param>
    /// <param name="type">フレームの種類</param>
    /// <returns>補間曲線の連想配列</returns>
    public static Dictionary<InterpolationItem, InterpolationCurve> CreateByVMDFormat(byte[] data, Vmd.VmdFrameKind type) => type switch
    {
        Vmd.VmdFrameKind.Camera => CreateCameraCurves(data),
        Vmd.VmdFrameKind.Motion => CreateMotionCurves(data),
        _ => throw new InvalidOperationException(),
    };

    private static Dictionary<InterpolationItem, InterpolationCurve> CreateCameraCurves(byte[] data)
    {
        static IEnumerable<byte> Swap_1_2(IEnumerable<byte> bytes) =>
            new byte[] { bytes.ElementAt(0) }.Append(bytes.ElementAt(2)).Append(bytes.ElementAt(1)).Concat(bytes.Skip(3));

        Dictionary<InterpolationItem, InterpolationCurve> interpolationCurves = new()
        {
            { InterpolationItem.XPosition, new() },
            { InterpolationItem.YPosition, new() },
            { InterpolationItem.ZPosition, new() },
            { InterpolationItem.Rotation, new() },
            { InterpolationItem.Distance, new() },
            { InterpolationItem.ViewAngle, new() },
        };

        interpolationCurves[InterpolationItem.XPosition].FromBytes(Swap_1_2(data.Skip(0)));
        interpolationCurves[InterpolationItem.YPosition].FromBytes(Swap_1_2(data.Skip(4)));
        interpolationCurves[InterpolationItem.ZPosition].FromBytes(Swap_1_2(data.Skip(8)));
        interpolationCurves[InterpolationItem.Rotation].FromBytes(Swap_1_2(data.Skip(12)));
        interpolationCurves[InterpolationItem.Distance].FromBytes(Swap_1_2(data.Skip(16)));
        interpolationCurves[InterpolationItem.ViewAngle].FromBytes(Swap_1_2(data.Skip(20)));

        return interpolationCurves;
    }

    private static Dictionary<InterpolationItem, InterpolationCurve> CreateMotionCurves(byte[] data)
    {
        Dictionary<InterpolationItem, InterpolationCurve> interpolationCurves = new()
        {
            { InterpolationItem.XPosition, new() },
            { InterpolationItem.YPosition, new() },
            { InterpolationItem.ZPosition, new() },
            { InterpolationItem.Rotation, new() },
        };

        var interpolationNum = data.Select((num, i) => (num, i)).Where(elm => elm.i % 4 == 0);
        interpolationCurves[InterpolationItem.XPosition].FromBytes(interpolationNum.Skip(0).Select(elm => elm.num));
        interpolationCurves[InterpolationItem.YPosition].FromBytes(interpolationNum.Skip(4).Select(elm => elm.num));
        interpolationCurves[InterpolationItem.ZPosition].FromBytes(interpolationNum.Skip(8).Select(elm => elm.num));
        interpolationCurves[InterpolationItem.Rotation].FromBytes(interpolationNum.Skip(12).Select(elm => elm.num));

        return interpolationCurves;
    }

    /// <summary>
    /// 補間曲線の連想配列からVMD形式のバイト列を生成する。
    /// </summary>
    /// <param name="curves">補間曲線の連想配列</param>
    /// <param name="type">フレームの種類</param>
    /// <returns>バイト列</returns>
    public static byte[] CreateVMDFormatBytes(Dictionary<InterpolationItem, InterpolationCurve> curves, Vmd.VmdFrameKind type)
    {
        return type switch
        {
            Vmd.VmdFrameKind.Camera => CreateCameraBytes(curves),
            Vmd.VmdFrameKind.Motion => CreateMotionBytes(curves),
            _ => throw new InvalidOperationException(),
        };
    }

    private static byte[] CreateCameraBytes(Dictionary<InterpolationItem, InterpolationCurve> curves)
    {
        static byte[] CreateBytes(InterpolationCurve curve) =>
            new byte[] { curve.EarlyControlePoint.X, curve.LateControlePoint.X, curve.EarlyControlePoint.Y, curve.LateControlePoint.Y };

        return CreateBytes(curves[InterpolationItem.XPosition])
               .Concat(CreateBytes(curves[InterpolationItem.YPosition]))
               .Concat(CreateBytes(curves[InterpolationItem.ZPosition]))
               .Concat(CreateBytes(curves[InterpolationItem.Rotation]))
               .Concat(CreateBytes(curves[InterpolationItem.Distance]))
               .Concat(CreateBytes(curves[InterpolationItem.ViewAngle]))
               .ToArray();
    }

    private static byte[] CreateMotionBytes(Dictionary<InterpolationItem, InterpolationCurve> curves)
    {
        // 補間曲線をbyte配列化
        var xPositionPoints = curves[InterpolationItem.XPosition].ToBytes();
        var yPositionPoints = curves[InterpolationItem.YPosition].ToBytes();
        var zPositionPoints = curves[InterpolationItem.ZPosition].ToBytes();
        var rotationPoints = curves[InterpolationItem.Rotation].ToBytes();

        // 形式に合わせて1行に整列
        var pointsRow = new byte[16];
        for (int i = 0; i < 4; i++)
        {
            pointsRow[i * 4 + 0] = xPositionPoints[i];
            pointsRow[i * 4 + 1] = yPositionPoints[i];
            pointsRow[i * 4 + 2] = zPositionPoints[i];
            pointsRow[i * 4 + 3] = rotationPoints[i];
        }

        // 形式に合わせた行列に整形
        List<byte> interpolateMatrix = new();
        for (int i = 0; i < 4; i++)
        {
            // pointsRowから始めのi個を抜かしてrowへ転写
            var row = new byte[16];
            pointsRow.Skip(i).ToArray().CopyTo(row, 0);

            interpolateMatrix.AddRange(row);
        }

        return interpolateMatrix.ToArray();
    }

    public static ReadOnlyDictionary<InterpolationItem, InterpolationCurve> Clone(ReadOnlyDictionary<InterpolationItem, InterpolationCurve> curves) =>
        new(curves.ToDictionary(p => p.Key, p => (InterpolationCurve)p.Value.Clone()));


    public object Clone() => new InterpolationCurve()
    {
        earlyControlePoint = earlyControlePoint,
        lateControlePoint = lateControlePoint
    };
}
