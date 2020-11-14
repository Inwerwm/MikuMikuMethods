using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.VMD
{
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

    public interface IVocaloidFrame:IComparable<IVocaloidFrame>
    {
        FrameType FrameType { get; }
        string Name { get; set; }
        uint Frame { get; set; }

        bool IsCameraType { get; }
        bool IsModelType { get; }

        void Read(BinaryReader reader);
        void Write(BinaryWriter writer);
    }
}
