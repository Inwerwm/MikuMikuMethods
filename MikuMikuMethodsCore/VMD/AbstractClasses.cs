using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.VMD
{
    /// <summary>
    /// フレームの抽象クラス
    /// </summary>
    public abstract class VocaloidFrame : IVocaloidFrame
    {
        /// <summary>
        /// フレームの種類
        /// </summary>
        public abstract FrameType FrameType { get; }
        /// <summary>
        /// フレームの名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// フレームの時間
        /// </summary>
        public uint Frame { get; set; }
        /// <summary>
        /// カメラ系フレームか？
        /// </summary>
        public abstract bool IsCameraType { get; }
        /// <summary>
        /// モデル系フレームか？
        /// </summary>
        public abstract bool IsModelType { get; }

        /// <summary>
        /// 時間の前後を比較する
        /// </summary>
        /// <param name="other">比較対象フレーム</param>
        /// <returns>
        /// <para>0未満 - このフレームはother以前</para>
        /// <para>0 - このフレームはotherと同時</para>
        /// <para>0超 - このフレームはother以降</para>
        /// </returns>
        public int CompareTo(IVocaloidFrame other) => Frame.CompareTo(other.Frame);
        /// <summary>
        /// 読み込み
        /// </summary>
        public abstract void Read(BinaryReader reader);
        /// <summary>
        /// 書き込み
        /// </summary>
        /// <param name="writer"></param>
        public abstract void Write(BinaryWriter writer);
    }

    /// <summary>
    /// モデル系フレームの抽象クラス
    /// </summary>
    public abstract class VocaloidModelTypeFrame : VocaloidFrame
    {
        /// <summary>
        /// カメラ系フレームか？
        /// </summary>
        public override bool IsCameraType => false;

        /// <summary>
        /// モデル系フレームか？
        /// </summary>
        public override bool IsModelType => true;
    }

    /// <summary>
    /// カメラ系フレームの抽象クラス
    /// </summary>
    public abstract class VocaloidCameraTypeFrame : VocaloidFrame
    {
        /// <summary>
        /// カメラ系フレームか？
        /// </summary>
        public override bool IsCameraType => true;

        /// <summary>
        /// モデル系フレームか？
        /// </summary>
        public override bool IsModelType => false;
    }
}
