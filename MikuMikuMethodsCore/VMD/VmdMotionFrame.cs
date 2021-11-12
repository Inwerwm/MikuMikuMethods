using MikuMikuMethods.Extension;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace MikuMikuMethods.Vmd
{
    /// <summary>
    /// モーションフレーム
    /// </summary>
    public class VmdMotionFrame : VmdModelTypeFrame, IVmdInterpolatable
    {
        /// <summary>
        /// フレームの種類
        /// </summary>
        public override VmdFrameType FrameType => VmdFrameType.Motion;

        /// <summary>
        /// 移動量
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// 回転量
        /// </summary>
        public Quaternion Rotation { get; set; }

        /// <summary>
        /// 補間曲線
        /// </summary>
        public Dictionary<InterpolationItem, InterpolationCurve> InterpolationCurves { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">ボーン名</param>
        /// <param name="frame">フレーム時間</param>
        public VmdMotionFrame(string name, uint frame = 0)
        {
            Name = name;
            Frame = frame;
            InterpolationCurves = new();
            InitializeInterpolationCurves();
        }

        /// <summary>
        /// バイナリから読み込むコンストラクタ
        /// </summary>
        public VmdMotionFrame(BinaryReader reader)
        {
            InterpolationCurves = new();
            Read(reader);
        }

        private void InitializeInterpolationCurves()
        {
            InterpolationCurves.Add(InterpolationItem.XPosition, new());
            InterpolationCurves.Add(InterpolationItem.YPosition, new());
            InterpolationCurves.Add(InterpolationItem.ZPosition, new());
            InterpolationCurves.Add(InterpolationItem.Rotation, new());
        }

        /// <summary>
        /// VMD形式から読み込み
        /// </summary>
        public override void Read(BinaryReader reader)
        {
            //ボーン名を読み込み
            Name = reader.ReadString(VmdConstants.BoneNameLength, Encoding.ShiftJIS, '\0');

            //各種パラメータを読み込み
            Frame = reader.ReadUInt32();
            Position = reader.ReadVector3();
            Rotation = reader.ReadQuaternion();

            //補間曲線を読み込み
            InterpolationCurves = InterpolationCurve.CreateByVMDFormat(reader.ReadBytes(64), FrameType);
        }

        /// <summary>
        /// VMD形式に書き込み
        /// </summary>
        public override void Write(BinaryWriter writer)
        {
            writer.Write(Name, VmdConstants.BoneNameLength, Encoding.ShiftJIS);
            writer.Write(Frame);
            writer.Write(Position);
            writer.Write(Rotation);
            writer.Write(InterpolationCurve.CreateVMDFormatBytes(InterpolationCurves, FrameType));
        }

        public override object Clone() => new VmdMotionFrame(Name, Frame)
        {
            Position = Position,
            Rotation = Rotation,
            InterpolationCurves = InterpolationCurve.Clone(InterpolationCurves)
        };
    }
}
