namespace MikuMikuMethods.Mme.Element;

/// <summary>
/// エフェクト対象アクセサリ情報
/// </summary>
public class EmmAccessory : EmmObject
{
    /// <summary>
    /// オブジェクトのキーを表す文字列
    /// </summary>
    public override string Name => $"Acs{Index}";

    /// <inheritdoc/>
    public EmmAccessory(int index, string path) : base(index, path) { }
}
