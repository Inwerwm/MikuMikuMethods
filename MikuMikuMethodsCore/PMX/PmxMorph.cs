using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMX
{
    /// <summary>
    /// モーフ
    /// </summary>
    public class PmxMorph : IPmxData
    {
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 名前(英語)
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// 表示パネル
        /// </summary>
        public MorphPanel Panel { get; set; }

        /// <summary>
        /// モーフの種類
        /// </summary>
        public MorphType Type { get; init; }

        /// <summary>
        /// モーフのオフセット
        /// </summary>
        public List<IPmxOffset> Offsets { get; } = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">モーフの種類</param>
        public PmxMorph(MorphType type)
        {
            Type = type;
        }

        /// <summary>
        /// バイナリ読込コンストラクタ
        /// </summary>
        /// <param name="reader">読み込み対象のリーダー</param>
        public PmxMorph(BinaryReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// データをバイナリから読み込む
        /// </summary>
        /// <param name="reader">読み込み対象のリーダー</param>
        public void Read(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// データをバイナリで書き込む
        /// </summary>
        /// <param name="writer">書き込み対象のライター</param>
        public void Write(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// モーフの表示パネル
    /// </summary>
    public enum MorphPanel
    {
        /// <summary>
        /// 眉(左下)
        /// </summary>
        Brow,
        /// <summary>
        /// 目(左上)
        /// </summary>
        Eye,
        /// <summary>
        /// 口(右上)
        /// </summary>
        Lip,
        /// <summary>
        /// その他(右下)
        /// </summary>
        Other
    }

    /// <summary>
    /// モーフ種類
    /// </summary>
    public enum MorphType
    {
        /// <summary>
        /// グループ
        /// </summary>
        Group,
        /// <summary>
        /// 頂点
        /// </summary>
        Vertex,
        /// <summary>
        /// ボーン
        /// </summary>
        Bone,
        /// <summary>
        /// UV
        /// </summary>
        UV,
        /// <summary>
        /// 追加UV1
        /// </summary>
        AdditionalUV1,
        /// <summary>
        /// 追加UV2
        /// </summary>
        AdditionalUV2,
        /// <summary>
        /// 追加UV3
        /// </summary>
        AdditionalUV3,
        /// <summary>
        /// 追加UV4
        /// </summary>
        AdditionalUV4,
        /// <summary>
        /// 材質
        /// </summary>
        Material
    }
}
