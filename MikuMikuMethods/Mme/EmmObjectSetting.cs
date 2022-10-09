using MikuMikuMethods.Mme.Element;

namespace MikuMikuMethods.Mme;

/// <summary>
/// オブジェクト単位のエフェクト設定
/// </summary>
public class EmmObjectSetting
{
    /// <summary>
    /// 対応するオブジェクト情報
    /// </summary>
    public EmmObject Object { get; init; }

    /// <summary>
    /// オブジェクトに適用するエフェクトの情報
    /// </summary>
    public EmmMaterial Material { get; init; }

    /// <summary>
    /// サブセット展開されたときの各材質に適用するエフェクトの情報
    /// </summary>
    public List<EmmMaterial> Subsets { get; init; }

    /// <summary>
    /// オブジェクト情報を指定してインスタンスを生成
    /// </summary>
    /// <param name="obj">オブジェクト情報</param>
    public EmmObjectSetting(EmmObject obj)
    {
        Material = new();
        Subsets = new();
        Object = obj;
    }

    /// <summary>
    /// EMDファイルに変換する
    /// </summary>
    /// <returns>変換結果Emd</returns>
    public EmdData ToEmd() => new(Material) { Subsets = Subsets };

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Subset[{Subsets.Count}] - {Object} - {Material}";
    }
}
