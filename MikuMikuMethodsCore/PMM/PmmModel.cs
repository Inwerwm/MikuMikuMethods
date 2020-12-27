using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM
{
    class PmmModel
    {
        /// <summary>
        /// モデル管理番号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// モデル名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// モデル名(英語)
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// モデルのパス
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// <para>モデル描画順</para>
        /// <para>これが狂うとモデルセレクタの選択と実際に動くモデルに齟齬が発生する</para>
        /// <para>セレクタ一覧が描画順に基づいて作られるためだと思われる</para>
        /// </summary>
        public byte RenderOrder { get; set; }

        /// <summary>
        /// <para>キーフレーム編集行数</para>
        /// <para>3(root, 表示・IK・外観, 表情) + 表情枠総数</para>
        /// </summary>
        public byte KeyframeRowCount { get; private set; }

        /// <summary>
        /// キーフレーム編集画面の垂直スクロール状態
        /// </summary>
        public int VerticalScrollState { get; set; }

        /// <summary>
        /// 最終フレーム数
        /// </summary>
        public int LastFrame { get; set; }

        /// <summary>
        /// 表示
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// ボーン数
        /// </summary>
        public int BoneCount => BoneNames.Count;
        /// <summary>
        /// ボーン名
        /// </summary>
        public List<string> BoneNames { get; init; }

        /// <summary>
        /// モーフ数
        /// </summary>
        public int MorphCount => MorphNames.Count;
        /// <summary>
        /// モーフ名
        /// </summary>
        public List<string> MorphNames { get; init; }

        /// <summary>
        /// IK数
        /// </summary>
        public int IKCount => IKBoneIndices.Count;
        /// <summary>
        /// IK設定されたボーンID
        /// </summary>
        public List<int> IKBoneIndices { get; init; }

        /// <summary>
        /// 外部親にできるボーン数
        /// </summary>
        public int ParentableBoneCount => ParentableBoneIndices.Count;
        /// <summary>
        /// 外部親にできるボーンのID
        /// </summary>
        public List<int> ParentableBoneIndices { get; init; }

        /// <summary>
        /// 選択中のボーンID
        /// </summary>
        public int SelectedBoneIndex { get; set; }

        /// <summary>
        /// 選択中の眉モーフID
        /// </summary>
        public int SelectedMorphBlowIndex { get; set; }
        /// <summary>
        /// 選択中の目モーフID
        /// </summary>
        public int SelectedMorphEyeIndex { get; set; }
        /// <summary>
        /// 選択中の口モーフID
        /// </summary>
        public int SelectedMorphLipIndex { get; set; }
        /// <summary>
        /// 選択中の他モーフID
        /// </summary>
        public int SelectedMorphOtherIndex { get; set; }

        /// <summary>
        /// 表示枠の数(表示、表情含む)
        /// </summary>
        public byte NodeCount => (byte)DoesOpenNode.Count;
        /// <summary>
        /// 表情枠展開状態
        /// </summary>
        public List<bool> DoesOpenNode { get; init; }


    }
}
