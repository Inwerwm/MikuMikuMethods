namespace MikuMikuMethods.Pmx;

/// <summary>
/// モデル情報
/// </summary>
public class PmxModelInfo : IPmxData
{
    /// <summary>
    /// モデル名
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// モデル名(英語)
    /// </summary>
    public string NameEn { get; set; }
    /// <summary>
    /// モデルのコメント
    /// </summary>
    public string Comment { get; set; }
    /// <summary>
    /// モデルのコメント(英語)
    /// </summary>
    public string CommentEn { get; set; }

    public override string ToString() => $"{Name}";
}
