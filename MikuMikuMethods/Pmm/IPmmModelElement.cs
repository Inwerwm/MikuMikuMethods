namespace MikuMikuMethods.Pmm;

/// <summary>
/// モデルに属する要素のインターフェイス
/// </summary>
public interface IPmmModelElement : ICloneable
{
    /// <summary>
    /// 名前
    /// </summary>
    string Name { get; }
}
