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

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="index">オブジェクト番号</param>
    public EmmModel(int index) : base(index) { }
}
