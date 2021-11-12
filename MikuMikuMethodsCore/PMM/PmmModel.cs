using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MikuMikuMethods.Pmm
{
    /// <summary>
    /// PMM用モデルクラス
    /// </summary>
    public class PmmModel
    {
        private PmmBone _selectedBone;
        private PmmMorph _selectedBrowMorph;
        private PmmMorph _selectedEyeMorph;
        private PmmMorph _selectedLipMorph;
        private PmmMorph _selectedOtherMorph;

        /// <summary>
        /// モデル名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// モデル名(英語)
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// ファイルパス
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// ボーン
        /// </summary>
        public List<PmmBone> Bones { get; } = new();
        /// <summary>
        /// モーフ
        /// </summary>
        public List<PmmMorph> Morphs { get; } = new();
        /// <summary>
        /// 表示枠
        /// </summary>
        public List<PmmNode> Nodes { get; } = new();

        /// <summary>
        /// 選択中ボーンの取得/設定
        /// <para>モデルに含まれない要素を設定しようとすると <c>ArgumentOutOfRangeException</c> を投げる</para>
        /// </summary>
        public PmmBone SelectedBone { get => _selectedBone; set => _selectedBone = GetIfContains(Bones, value, "bone"); }
        /// <summary>
        /// 選択中眉モーフの取得/設定
        /// <para>モデルに含まれない要素を設定しようとすると <c>ArgumentOutOfRangeException</c> を投げる</para>
        /// </summary>
        public PmmMorph SelectedBrowMorph { get => _selectedBrowMorph; set => _selectedBrowMorph = GetIfContains(Morphs, value, "morph"); }
        /// <summary>
        /// 選択中目モーフの取得/設定
        /// <para>モデルに含まれない要素を設定しようとすると <c>ArgumentOutOfRangeException</c> を投げる</para>
        /// </summary>
        public PmmMorph SelectedEyeMorph { get => _selectedEyeMorph; set => _selectedEyeMorph = GetIfContains(Morphs, value, "morph"); }
        /// <summary>
        /// 選択中口モーフの取得/設定
        /// <para>モデルに含まれない要素を設定しようとすると <c>ArgumentOutOfRangeException</c> を投げる</para>
        /// </summary>
        public PmmMorph SelectedLipMorph { get => _selectedLipMorph; set => _selectedLipMorph = GetIfContains(Morphs, value, "morph"); }
        /// <summary>
        /// 選択中その他モーフの取得/設定
        /// <para>モデルに含まれない要素を設定しようとすると <c>ArgumentOutOfRangeException</c> を投げる</para>
        /// </summary>
        public PmmMorph SelectedOtherMorph { get => _selectedOtherMorph; set => _selectedOtherMorph = GetIfContains(Morphs, value, "morph"); }

        private T GetIfContains<T>(List<T> entity, T value, string dispNameOnException) =>
            entity.Contains(value) ? value : throw new ArgumentOutOfRangeException($"The model does not contain the {dispNameOnException} which specified as the selection target.");

        /// <summary>
        /// 表示・IK・外観のキーフレーム
        /// </summary>
        public List<PmmModelConfigFrame> ConfigFrames { get; } = new();

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

        public PmmModelSpecificKeyFrameEditorState SpecificEditorState { get; } = new();

        public PmmModelConfigState CurrentConfig { get; } = new();

        public override string ToString() => $"{Name} - {Path}";
    }
}