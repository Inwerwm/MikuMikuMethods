namespace MikuMikuMethods.Pmm;

/// <summary>
/// 外部親の地面用
/// </summary>
public class PmmGroundAsParent : PmmModel
{
    /// <summary>
    /// モデル名
    /// </summary>
    public new string Name => "地面";

    /// <summary>
    /// モデル名(英語)
    /// </summary>
    public new string NameEn => "Ground";

    /// <summary>
    /// ファイルパス
    /// </summary>
    public new string Path => "";

    /// <inheritdoc/>
    public override string ToString() => "地面(外部親用)";
}
