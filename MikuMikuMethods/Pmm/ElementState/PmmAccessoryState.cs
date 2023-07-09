using MikuMikuMethods.Extension;
using System.Numerics;

namespace MikuMikuMethods.Pmm.ElementState;

/// <summary>
/// アクセサリの状態
/// </summary>
public class PmmAccessoryState : ICloneable
{
    private float transparency;

    internal byte TransAndVisible
    {
        get => CreateTransAndVisible(Transparency, Visible);
        set => (Visible, Transparency) = SeparateTransAndVisible(value);
    }

    /// <summary>
    /// 表示状態
    /// </summary>
    public bool Visible { get; set; } = true;
    /// <summary>
    /// 透過度
    /// </summary>
    public float Transparency
    {
        get => transparency;
        set
        {
            if (value.IsWithin(0, 1))
                transparency = value;
            else
                throw new ArgumentOutOfRangeException(nameof(Transparency), "透明度は [0, 1] の範囲である必要があります。");
        }
    }
    /// <summary>
    /// 親モデル
    /// </summary>
    public PmmModel? ParentModel { get; set; }
    /// <summary>
    /// 親ボーン
    /// </summary>
    public PmmBone? ParentBone { get; set; }

    /// <summary>
    /// 位置
    /// </summary>
    public Vector3 Position { get; set; }
    /// <summary>
    /// 回転
    /// </summary>
    public Vector3 Rotation { get; set; }
    /// <summary>
    /// 拡縮
    /// </summary>
    public float Scale { get; set; }
    /// <summary>
    /// 影のOn/Off
    /// </summary>
    public bool EnableShadow { get; set; }

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public PmmAccessoryState DeepCopy() => new()
    {
        TransAndVisible = TransAndVisible,
        EnableShadow = EnableShadow,
        ParentModel = ParentModel,
        ParentBone = ParentBone,
        Position = Position,
        Rotation = Rotation,
        Scale = Scale,
    };

    /// <summary>
    /// PMM移行用ディープコピー
    /// </summary>
    /// <param name="parentModel">移行先の対応する参照親モデル</param>
    /// <param name="parentBone">移行先の対応する参照親ボーン</param>
    /// <returns>複製</returns>
    public PmmAccessoryState DeepCopy(PmmModel? parentModel, PmmBone? parentBone) => new()
    {
        TransAndVisible = TransAndVisible,
        EnableShadow = EnableShadow,
        ParentModel = parentModel,
        ParentBone = parentBone,
        Position = Position,
        Rotation = Rotation,
        Scale = Scale,
    };

    internal static (bool Visible, float Transparency) SeparateTransAndVisible(byte value) =>
        ((value & 0x1) == 0x1, (100 - (value >> 1)) / 100f);
    internal static byte CreateTransAndVisible(float transparency, bool visible)
    {
        var tr = 100 - (byte)Math.Round(transparency * 100);
        return (byte)((tr << 1) | (visible ? 1 : 0));
    }

    /// <inheritdoc/>
    public object Clone() => DeepCopy();
}
