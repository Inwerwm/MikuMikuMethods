using MikuMikuMethods.Mme.Element;

namespace MikuMikuMethods.Mme;

/// <summary>
/// <para>EMMファイルのエフェクト単位の設定</para>
/// <para>MMEエフェクト割当画面の各タブに相当</para>
/// </summary>
public class EmmEffectSettings
{
    /// <summary>
    /// 設定種別
    /// </summary>
    public bool IsMain { get; init; }

    /// <summary>
    /// <para>設定名</para>
    /// <para>MMEエフェクト割当画面のタブ名に相当</para>
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Mainタブの(default)に設定されたエフェクト
    /// </summary>
    public EmmMaterial? Default { get; set; }
    /// <summary>
    /// <para>エフェクトが依拠するオブジェクト</para>
    /// <para>オブジェクト定義で記述された名前が入る</para>
    /// </summary>
    public EmmObject? Owner { get; set; }

    /// <summary>
    /// 設定対象オブジェクトのリスト
    /// </summary>
    public List<EmmObjectSetting> ObjectSettings { get; init; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="name">エフェクト設定のタブ名 "Main"でメインタブ扱いになる</param>
    public EmmEffectSettings(string name)
    {
        ObjectSettings = new();
        IsMain = name == "Main";
        Name = name;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return IsMain ? "Effect Main" : $"Effect@{Name}";
    }
}
