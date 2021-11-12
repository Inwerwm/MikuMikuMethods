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

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="index">オブジェクト番号</param>
    public EmmAccessory(int index) : base(index) { }
}
