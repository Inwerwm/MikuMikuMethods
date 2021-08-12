using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace MikuMikuMethods.PMX
{
    /// <summary>
    /// PMXモデル
    /// </summary>
    public class PmxModel : IPmxData
    {
        /// <summary>
        /// ヘッダ
        /// </summary>
        public PmxHeader Header { get; } = new();
        /// <summary>
        /// モデル情報
        /// </summary>
        public PmxModelInfo ModelInfo { get; } = new();
        /// <summary>
        /// 剛体
        /// </summary>
        public List<PmxBody> Bodies { get; } = new();
        /// <summary>
        /// ボーン
        /// </summary>
        public List<PmxBone> Bones { get; } = new();
        /// <summary>
        /// <para>面の一覧</para>
        /// <para>面の追加は材質の面プロパティから行う</para>
        /// </summary>
        public ReadOnlyCollection<PmxFace> Faces => Materials.SelectMany(m => m.Faces).ToList().AsReadOnly();
        /// <summary>
        /// ジョイント
        /// </summary>
        public List<PmxJoint> Joints { get; } = new();
        /// <summary>
        /// 材質
        /// </summary>
        public List<PmxMaterial> Materials { get; } = new();
        /// <summary>
        /// モーフ
        /// </summary>
        public List<PmxMorph> Morphs { get; } = new();
        /// <summary>
        /// 表情枠
        /// </summary>
        public List<PmxNode> Nodes { get; } = new();
        /// <summary>
        /// 頂点
        /// </summary>
        public List<PmxVertex> Vertices { get; } = new();
        /// <summary>
        /// テクスチャ
        /// </summary>
        public List<PmxTexture> Textures { get; } = new();
        /// <summary>
        /// ソフトボディ
        /// </summary>
        public List<PmxSoftBody> SoftBodies { get; } = new();

        public override string ToString() => $"{Header} : {ModelInfo}";
    }
}
