using MikuMikuMethods.PMM.ElementState;
using MikuMikuMethods.PMM.Frame;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// PMM用モデルクラス
    /// <para>複数の PMM クラスで共有すると描画/計算順に不整合が発生するので Clone した別インスタンスを入れること</para>
    /// </summary>
    public class PmmModel : IRelationableElement<PmmModel>
    {
        private PmmBone _selectedBone;
        private PmmMorph _selectedBrowMorph;
        private PmmMorph _selectedEyeMorph;
        private PmmMorph _selectedLipMorph;
        private PmmMorph _selectedOtherMorph;

        internal List<PmmModel> _RenderOrderCollection { get; set; }
        internal List<PmmModel> _CalculateOrderCollection { get; set; }
        void IRelationableElement<PmmModel>.AddRelation(IEnumerable<List<PmmModel>> lists)
        {
            _RenderOrderCollection = lists.ElementAt(0);
            _CalculateOrderCollection = lists.ElementAt(1);
        }
        void IRelationableElement<PmmModel>.RemoveRelation()
        {
            _RenderOrderCollection = null;
            _CalculateOrderCollection = null;
        }
        /// <summary>
        /// 描画順
        /// <para>get/set 共にモデルが PolygonMovieMaker クラスに属していなければ例外を吐く</para>
        /// <para>PMM クラスにモデルを追加すると追加先の文脈に応じて順序が定まるようになる</para>
        /// <para>複数の PMM クラスで共有すると順序に不整合が発生するのでモデル自体を Clone すること</para>
        /// </summary>
        public byte RenderOrder
        {
            get => IRelationableElement<PmmModel>.GetOrder(this, _RenderOrderCollection, "描画");
            set => IRelationableElement<PmmModel>.SetOrder(this, _RenderOrderCollection, "描画", value);
        }
        /// <summary>
        /// 計算順
        /// <para>get/set 共にモデルが PolygonMovieMaker クラスに属していなければ例外を吐く</para>
        /// <para>PMM クラスにモデルを追加すると追加先の文脈に応じて順序が定まるようになる</para>
        /// <para>複数の PMM クラスで共有すると順序に不整合が発生するので、共有はせずモデル自体を Clone すること</para>
        /// </summary>
        public byte CalculateOrder
        {
            get => IRelationableElement<PmmModel>.GetOrder(this, _CalculateOrderCollection, "計算");
            set => IRelationableElement<PmmModel>.SetOrder(this, _CalculateOrderCollection, "計算", value);
        }

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
        public PmmBone SelectedBone { get => _selectedBone; set => _selectedBone = GetIfContains(Bones, value, "ボーン"); }
        /// <summary>
        /// 選択中眉モーフの取得/設定
        /// <para>モデルに含まれない要素を設定しようとすると <c>ArgumentOutOfRangeException</c> を投げる</para>
        /// </summary>
        public PmmMorph SelectedBrowMorph { get => _selectedBrowMorph; set => _selectedBrowMorph = GetIfContains(Morphs, value, "モーフ"); }
        /// <summary>
        /// 選択中目モーフの取得/設定
        /// <para>モデルに含まれない要素を設定しようとすると <c>ArgumentOutOfRangeException</c> を投げる</para>
        /// </summary>
        public PmmMorph SelectedEyeMorph { get => _selectedEyeMorph; set => _selectedEyeMorph = GetIfContains(Morphs, value, "モーフ"); }
        /// <summary>
        /// 選択中口モーフの取得/設定
        /// <para>モデルに含まれない要素を設定しようとすると <c>ArgumentOutOfRangeException</c> を投げる</para>
        /// </summary>
        public PmmMorph SelectedLipMorph { get => _selectedLipMorph; set => _selectedLipMorph = GetIfContains(Morphs, value, "モーフ"); }
        /// <summary>
        /// 選択中その他モーフの取得/設定
        /// <para>モデルに含まれない要素を設定しようとすると <c>ArgumentOutOfRangeException</c> を投げる</para>
        /// </summary>
        public PmmMorph SelectedOtherMorph { get => _selectedOtherMorph; set => _selectedOtherMorph = GetIfContains(Morphs, value, "モーフ"); }

        private T GetIfContains<T>(List<T> entity, T value, string dispNameOnException) =>
            entity.Contains(value) ? value : throw new ArgumentOutOfRangeException($"モデルに含まれない{dispNameOnException}を選択対象にしようとしました");

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
    }
}