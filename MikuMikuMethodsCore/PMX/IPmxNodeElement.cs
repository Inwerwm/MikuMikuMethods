namespace MikuMikuMethods.Pmx;

/// <summary>
/// 表情枠要素
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPmxNodeElement
{
    /// <summary>
    /// 表情枠要素の型フラグ番号
    /// </summary>
    byte TypeNumber { get; }
    /// <summary>
    /// 表情枠要素本体
    /// </summary>
    IPmxData Entity { get; set; }

    /// <summary>
    /// この要素のインデックスを探す
    /// </summary>
    /// <param name="model">探索対象のモデル</param>
    /// <returns>この要素のインデックス</returns>
    int FindIndex(PmxModel model);
}
