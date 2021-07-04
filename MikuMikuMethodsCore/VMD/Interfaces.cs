using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.VMD
{
    /// <summary>
    /// フレームの種類
    /// </summary>
    public enum FrameType
    {
        /// <summary>
        /// カメラ
        /// </summary>
        Camera,
        /// <summary>
        /// 照明
        /// </summary>
        Light,
        /// <summary>
        /// 影
        /// </summary>
        Shadow,
        /// <summary>
        /// 表示・IK
        /// </summary>
        Property,
        /// <summary>
        /// モーフ
        /// </summary>
        Morph,
        /// <summary>
        /// モーション
        /// </summary>
        Motion,
    }

    /// <summary>
    /// フレームのインターフェイス
    /// </summary>
    public interface IVocaloidFrame:IComparable<IVocaloidFrame>, ICloneable
    {
        /// <summary>
        /// フレームの種類
        /// </summary>
        FrameType FrameType { get; }
        /// <summary>
        /// フレームの名前
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// フレームの時間
        /// </summary>
        uint Frame { get; set; }

        /// <summary>
        /// カメラ系フレームか？
        /// </summary>
        bool IsCameraType { get; }
        /// <summary>
        /// モデル系フレームか？
        /// </summary>
        bool IsModelType { get; }

        /// <summary>
        /// 読み込み
        /// </summary>
        /// <param name="reader"></param>
        void Read(BinaryReader reader);
        /// <summary>
        /// 書き込み
        /// </summary>
        /// <param name="writer"></param>
        void Write(BinaryWriter writer);
    }
}
