namespace MikuMikuMethods.Pmm.Frame;

/// <summary>
/// PMMデータのフレーム情報インターフェイス
/// </summary>
public interface IPmmFrame
{
    /// <summary>
    /// フレーム位置
    /// </summary>
    int Frame { get; set; }
    /// <summary>
    /// 選択状態か
    /// </summary>
    bool IsSelected { get; set; }

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public IPmmFrame DeepCopy();
}
