using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;

namespace MikuMikuMethods.Pmm;

/// <summary>
/// PMM用モデルクラス
/// </summary>
public class PmmModel : ICloneable
{
    private PmmBone? _selectedBone;
    private PmmMorph? _selectedBrowMorph;
    private PmmMorph? _selectedEyeMorph;
    private PmmMorph? _selectedLipMorph;
    private PmmMorph? _selectedOtherMorph;

    /// <summary>
    /// モデル名
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// モデル名(英語)
    /// </summary>
    public string NameEn { get; set; } = "";

    /// <summary>
    /// ファイルパス
    /// </summary>
    public string Path { get; set; } = "";

    /// <summary>
    /// ボーン
    /// </summary>
    public List<PmmBone> Bones { get; private init; } = new();
    /// <summary>
    /// モーフ
    /// </summary>
    public List<PmmMorph> Morphs { get; private init; } = new();
    /// <summary>
    /// 表示枠
    /// </summary>
    public List<PmmNode> Nodes { get; private init; } = new();

    /// <summary>
    /// 選択中ボーンの取得/設定
    /// <para>モデルに含まれない要素を設定しようとすると <c>ArgumentOutOfRangeException</c> を投げる</para>
    /// </summary>
    public PmmBone? SelectedBone
    {
        get => _selectedBone;
        set
        {
            _selectedBone = GetIfContains(Bones, value, "bone");
            if (_selectedBone != null)
                _selectedBone.IsSelected = true;
        }
    }
    /// <summary>
    /// 選択中眉モーフの取得/設定
    /// <para>モデルに含まれない要素を設定しようとすると <c>ArgumentOutOfRangeException</c> を投げる</para>
    /// </summary>
    public PmmMorph? SelectedBrowMorph { get => _selectedBrowMorph; set => _selectedBrowMorph = GetIfContains(Morphs, value, "morph"); }
    /// <summary>
    /// 選択中目モーフの取得/設定
    /// <para>モデルに含まれない要素を設定しようとすると <c>ArgumentOutOfRangeException</c> を投げる</para>
    /// </summary>
    public PmmMorph? SelectedEyeMorph { get => _selectedEyeMorph; set => _selectedEyeMorph = GetIfContains(Morphs, value, "morph"); }
    /// <summary>
    /// 選択中口モーフの取得/設定
    /// <para>モデルに含まれない要素を設定しようとすると <c>ArgumentOutOfRangeException</c> を投げる</para>
    /// </summary>
    public PmmMorph? SelectedLipMorph { get => _selectedLipMorph; set => _selectedLipMorph = GetIfContains(Morphs, value, "morph"); }
    /// <summary>
    /// 選択中その他モーフの取得/設定
    /// <para>モデルに含まれない要素を設定しようとすると <c>ArgumentOutOfRangeException</c> を投げる</para>
    /// </summary>
    public PmmMorph? SelectedOtherMorph { get => _selectedOtherMorph; set => _selectedOtherMorph = GetIfContains(Morphs, value, "morph"); }

    private T? GetIfContains<T>(List<T> entity, T? value, string dispNameOnException) =>
        value is null || entity.Contains(value!) ? value : throw new ArgumentOutOfRangeException($"The model does not contain the {dispNameOnException} which specified as the selection target.");

    /// <summary>
    /// 表示・IK・外観のキーフレーム
    /// </summary>
    public List<PmmModelConfigFrame> ConfigFrames { get; private init; } = new();

    /// <summary>
    /// エッジ幅
    /// </summary>
    public float EdgeWidth { get; set; }

    /// <summary>
    /// 加算合成が有効か
    /// </summary>
    public bool EnableAlphaBlend { get; set; } = false;
    /// <summary>
    /// セルフ影が有効か
    /// </summary>
    public bool EnableSelfShadow { get; set; } = true;

    public PmmModelSpecificKeyFrameEditorState SpecificEditorState { get; private init; } = new();

    public PmmModelConfigState CurrentConfig { get; private init; } = new();

    /// <inheritdoc/>
    public override string ToString() => $"{Name} - {Path}";

    public PmmModel DeepCopy(out Dictionary<PmmBone, PmmBone> boneMap)
    {
        var _boneMap = Bones.ToDictionary(bone => bone, bone => bone.DeepCopy());
        var morphMap = Morphs.ToDictionary(morph => morph, morph => morph.DeepCopy());
        var modelElementMap = _boneMap.Select(bone => (Key: (IPmmModelElement)bone.Key, Value: (IPmmModelElement)bone.Value)).Concat(morphMap.Select(morph => (Key: (IPmmModelElement)morph.Key, Value: (IPmmModelElement)morph.Value))).ToDictionary(p => p.Key, p => p.Value);

        var clone = new PmmModel
        {
            Bones = _boneMap.Values.ToList(),
            Morphs = morphMap.Values.ToList(),
            Nodes = Nodes.Select(n => n.DeepCopy(modelElementMap)).ToList(),
            ConfigFrames = ConfigFrames.Select(frame => frame.DeepCopy(_boneMap)).ToList(),
            CurrentConfig = CurrentConfig.DeepCopy(_boneMap),
            EdgeWidth = EdgeWidth,
            EnableAlphaBlend = EnableAlphaBlend,
            EnableSelfShadow = EnableSelfShadow,
            Name = Name,
            NameEn = NameEn,
            Path = Path,
            SpecificEditorState = SpecificEditorState.DeepCopy(),
            _selectedBone = _boneMap.GetOrDefault(SelectedBone),
            _selectedBrowMorph = morphMap.GetOrDefault(SelectedBrowMorph),
            _selectedEyeMorph = morphMap.GetOrDefault(SelectedEyeMorph),
            _selectedLipMorph = morphMap.GetOrDefault(SelectedLipMorph),
            _selectedOtherMorph = morphMap.GetOrDefault(SelectedOtherMorph)
        };

        boneMap = _boneMap;
        return clone;
    }

    public PmmModel DeepCopy() => DeepCopy(out _);

    public object Clone() => DeepCopy();
}
