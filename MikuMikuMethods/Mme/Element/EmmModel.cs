namespace MikuMikuMethods.Mme.Element;

/// <summary>
/// エフェクト対象モデル情報
/// </summary>
public class EmmModel : EmmObject
{
    /// <summary>
    /// オブジェクトのキーを表す文字列
    /// </summary>
    public override string Name => $"Pmd{Index}";

    /// <inheritdoc/>
    public EmmModel(int index, string path) : base(index, path) { }
}
