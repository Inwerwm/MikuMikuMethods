using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.VMD
{
    /// <summary>
    /// 表示・IKフレーム
    /// </summary>
    public class VocaloidPropertyFrame : VocaloidModelTypeFrame
    {
        public override FrameType FrameType => FrameType.Property;

        public override void Read(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public override void Write(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// モーフフレーム
    /// </summary>
    public class VocaloidMorphFrame : VocaloidModelTypeFrame
    {
        public override FrameType FrameType => FrameType.Morph;

        public override void Read(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public override void Write(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// モーションフレーム
    /// </summary>
    public class VocaloidMotionFrame : VocaloidModelTypeFrame
    {
        /// <summary>
        /// フレームの種類
        /// </summary>
        public override FrameType FrameType => FrameType.Motion;

        /// <summary>
        /// 移動量
        /// </summary>
        public Vector3 Trans { get; set; }
        /// <summary>
        /// 回転量
        /// </summary>
        public Quaternion Rot { get; set; }

        /// <summary>
        /// 補間曲線
        /// </summary>
        public Dictionary<InterpolationItem,InterpolationCurve> InterpolationCurves { get; init; }

        public VocaloidMotionFrame()
        {
            InterpolationCurves = new();
            InterpolationCurves.Add(InterpolationItem.XMove, new());
            InterpolationCurves.Add(InterpolationItem.YMove, new());
            InterpolationCurves.Add(InterpolationItem.ZMove, new());
            InterpolationCurves.Add(InterpolationItem.Rotation, new());
        }

        public override void Read(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public override void Write(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
