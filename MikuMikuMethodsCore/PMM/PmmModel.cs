﻿using System.Collections.Generic;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// PMM内のモデル情報
    /// </summary>
    public class PmmModel
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
        /// 表示
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// <para>モデル描画順</para>
        /// <para>これが狂うとモデルセレクタの選択と実際に動くモデルに齟齬が発生する</para>
        /// <para>セレクタ一覧が描画順に基づいて作られるためだと思われる</para>
        /// </summary>
        public byte RenderOrder { get; set; }

        /// <summary>
        /// キーフレーム編集画面の状態
        /// </summary>
        public FrameEditorState FrameEditor { get; init; }

        /// <summary>
        /// ボーン名
        /// </summary>
        public List<string> BoneNames { get; init; }

        /// <summary>
        /// モーフ名
        /// </summary>
        public List<string> MorphNames { get; init; }

        /// <summary>
        /// IK設定されたボーンID
        /// </summary>
        public List<int> IKBoneIndices { get; init; }

        /// <summary>
        /// 表示枠の数(表示、表情含む)
        /// </summary>
        public byte NodeCount => (byte)FrameEditor.DoesOpenNode.Count;

        /// <summary>
        /// 外部親にできるボーンのID
        /// </summary>
        public List<int> ParentableBoneIndices { get; init; }

        /// <summary>
        /// 選択中のボーンID
        /// </summary>
        public int SelectedBoneIndex { get; set; }

        /// <summary>
        /// 選択中のモーフID
        /// </summary>
        public (int Brow, int Eye, int Lip, int Other) SelectedMorphIndices { get; set; }

        /// <summary>
        /// 初期位置のボーンフレーム
        /// </summary>
        public List<PmmBoneFrame> InitialBoneFrames { get; init; }

        /// <summary>
        /// ボーンのキーフレーム
        /// </summary>
        public List<PmmBoneFrame> BoneFrames { get; init; }



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmmModel()
        {
            FrameEditor = new();
            BoneNames = new();
            MorphNames = new();
            IKBoneIndices = new();
            ParentableBoneIndices = new();
            InitialBoneFrames = new();
            BoneFrames = new();
        }
    }

    /// <summary>
    /// キーフレーム編集画面の状態
    /// </summary>
    public class FrameEditorState
    {
        /// <summary>
        /// <para>行数</para>
        /// <para>root, 表示・IK・外観, 表情 + 表情枠</para>
        /// </summary>
        public byte RowCount => (byte)(3 + DoesOpenNode.Count);

        /// <summary>
        /// 垂直スクロール状態
        /// </summary>
        public int VerticalScrollState { get; set; }

        /// <summary>
        /// 表情枠展開状態
        /// </summary>
        public List<bool> DoesOpenNode { get; init; }

        /// <summary>
        /// 最終フレーム
        /// </summary>
        public int LastFrame { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FrameEditorState()
        {
            DoesOpenNode = new();
        }
    }
}
