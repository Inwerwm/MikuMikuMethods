using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.VMD
{
    /// <summary>
    /// フレームのインターフェイス
    /// </summary>
    public interface IVmdFrame:IComparable<IVmdFrame>, ICloneable
    {
        /// <summary>
        /// フレームの種類
        /// </summary>
        VmdFrameType FrameType { get; }
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
