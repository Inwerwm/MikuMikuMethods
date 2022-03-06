namespace MikuMikuMethods.Pmm.ElementState;

/// <summary>
/// 範囲選択をする対象を表すレコード
/// <para>静的メソッドで生成ができるのでそれを使うことを推奨</para>
/// </summary>
/// <param name="Index">範囲選択対象の内部インデックス</param>
public record PmmRangeSelector(int Index)
{
    /// <summary>
    /// 全フレーム
    /// </summary>
    public static readonly PmmRangeSelector AllFrames = new(0);
    /// <summary>
    /// 最初のボーン
    /// </summary>
    public static readonly PmmRangeSelector RootBone = new(1);
    /// <summary>
    /// 表示・IK・外観
    /// </summary>
    public static readonly PmmRangeSelector ConfigFrame = new(2);
    /// <summary>
    /// 選択ボーン
    /// </summary>
    public static readonly PmmRangeSelector SelectedBones = new(3);
    /// <summary>
    /// 選択表情
    /// </summary>
    public static readonly PmmRangeSelector SelectedMorphs = new(4);
    /// <summary>
    /// 全表情フレーム
    /// </summary>
    public static readonly PmmRangeSelector AllMorphFrames = new(5);
    /// <summary>
    /// 全ボーンフレーム
    /// <para>モーフが存在しないモデルでのみ有効</para>
    /// <para>モーフが存在するモデルでこれを設定すると最初のモーフになる</para>
    /// </summary>
    public static readonly PmmRangeSelector AllBoneFrames = new(6);

    /// <summary>
    /// ボーン/モーフに対応する範囲選択セレクタオブジェクトを生成する
    /// </summary>
    /// <param name="element">ボーン/モーフ</param>
    /// <param name="model"><c>element</c>が属するモデル</param>
    /// <returns>引数のボーン/モーフに対応した範囲セレクタオブジェクト</returns>
    /// <exception cref="ArgumentException"><c>element</c>にボーン/モーフ以外が入力された</exception>
    /// <exception cref="ArgumentOutOfRangeException"><c>model</c>内に存在しない<c>element</c>が指定された</exception>
    public static PmmRangeSelector Create(IPmmModelElement element, PmmModel model)
    {
        // モーフがあれば全ボーンフレームがないので先頭インデックスの数が1減る
        var offset = model.Morphs.Count > 0 ? 5 : 6;

        return element switch
        {
            PmmMorph morph => model.Morphs.Contains(morph) ? new(offset + model.Morphs.IndexOf(morph) + 1) : throw new ArgumentOutOfRangeException("The model does not contain the specified morph."),
            PmmBone bone => model.Bones.Contains(bone) ? new(offset + model.Morphs.Count + model.Bones.IndexOf(bone) + 1) : throw new ArgumentOutOfRangeException("The model does not contain the specified bone."),
            _ => throw new ArgumentException("the object of a type not covered by RangeSelector.")
        };
    }

    public PmmRangeSelector DeepCopy() => new(Index);
}
