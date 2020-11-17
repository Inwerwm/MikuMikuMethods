using MikuMikuMethods.Extension;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace MikuMikuMethods.VMD
{
    /// <summary>
    /// カメラフレーム
    /// </summary>
    public class VocaloidCameraFrame : VocaloidCameraTypeFrame
    {
        /// <summary>
        /// フレームの種類
        /// </summary>
        public override FrameType FrameType => VMD.FrameType.Camera;

        /// <summary>
        /// 距離
        /// </summary>
        public float Distance { get; set; }

        /// <summary>
        /// 移動量
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// 回転量
        /// </summary>
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// 視野角
        /// </summary>
        public uint ViewAngle { get; set; }

        /// <summary>
        /// パースの切/入 trueで切
        /// </summary>
        public bool IsPerspectiveOff { get; set; }

        /// <summary>
        /// 補間曲線
        /// </summary>
        public Dictionary<InterpolationItem, InterpolationCurve> InterpolationCurves { get; init; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="frame">フレーム時間</param>
        public VocaloidCameraFrame(uint frame = 0)
        {
            Name = "Camera";
            Frame = frame;
            ViewAngle = 30;
            InterpolationCurves = new();
            InitializeInterpolationCurves();
        }

        /// <summary>
        /// バイナリから読み込むコンストラクタ
        /// </summary>
        public VocaloidCameraFrame(BinaryReader reader)
        {
            InterpolationCurves = new();
            InitializeInterpolationCurves();
            Read(reader);
        }

        private void InitializeInterpolationCurves()
        {
            InterpolationCurves.Add(InterpolationItem.XPosition, new());
            InterpolationCurves.Add(InterpolationItem.YPosition, new());
            InterpolationCurves.Add(InterpolationItem.ZPosition, new());
            InterpolationCurves.Add(InterpolationItem.Rotation, new());
            InterpolationCurves.Add(InterpolationItem.Distance, new());
            InterpolationCurves.Add(InterpolationItem.ViewAngle, new());
        }

        private byte[] CreateInterpolationMatrix()
        {
            // 補間曲線をbyte配列化
            var xPositionPoints = InterpolationCurves[InterpolationItem.XPosition].ToBytes();
            var yPositionPoints = InterpolationCurves[InterpolationItem.YPosition].ToBytes();
            var zPositionPoints = InterpolationCurves[InterpolationItem.ZPosition].ToBytes();
            var rotationPoints = InterpolationCurves[InterpolationItem.Rotation].ToBytes();
            var distancePoints = InterpolationCurves[InterpolationItem.Distance].ToBytes();
            var viewAnglePoints = InterpolationCurves[InterpolationItem.ViewAngle].ToBytes();

            // 形式に合わせて1行に整列
            var pointsRow = new byte[24];
            for (int i = 0; i < 6; i++)
            {
                pointsRow[i * 6 + 0] = xPositionPoints[i];
                pointsRow[i * 6 + 1] = yPositionPoints[i];
                pointsRow[i * 6 + 2] = zPositionPoints[i];
                pointsRow[i * 6 + 3] = rotationPoints[i];
                pointsRow[i * 6 + 4] = distancePoints[i];
                pointsRow[i * 6 + 5] = viewAnglePoints[i];
            }

            return pointsRow;
        }

        /// <summary>
        /// VMD形式から読み込み
        /// </summary>
        public override void Read(BinaryReader reader)
        {
            Frame = reader.ReadUInt32();
            Distance = reader.ReadSingle();
            Position = reader.ReadVector3();
            Rotation = reader.ReadVector3();

            //補間曲線を読み込み
            var interpolationMatrix = reader.ReadBytes(24).Select((num, i) => (num, i));
            InterpolationCurves[InterpolationItem.XPosition].FromBytes(interpolationMatrix.Where(elm => elm.i % 6 == 0).Select(elm => elm.num));
            InterpolationCurves[InterpolationItem.YPosition].FromBytes(interpolationMatrix.Where(elm => elm.i % 6 == 1).Select(elm => elm.num));
            InterpolationCurves[InterpolationItem.ZPosition].FromBytes(interpolationMatrix.Where(elm => elm.i % 6 == 2).Select(elm => elm.num));
            InterpolationCurves[InterpolationItem.Rotation].FromBytes(interpolationMatrix.Where(elm => elm.i % 6 == 3).Select(elm => elm.num));
            InterpolationCurves[InterpolationItem.Distance].FromBytes(interpolationMatrix.Where(elm => elm.i % 6 == 4).Select(elm => elm.num));
            InterpolationCurves[InterpolationItem.ViewAngle].FromBytes(interpolationMatrix.Where(elm => elm.i % 6 == 5).Select(elm => elm.num));

            ViewAngle = reader.ReadUInt32();
            IsPerspectiveOff = reader.ReadBoolean();
        }

        /// <summary>
        /// VMD形式に書き込み
        /// </summary>
        public override void Write(BinaryWriter writer)
        {
            writer.Write(Frame);
            writer.Write(Distance);
            writer.Write(Position);
            writer.Write(Rotation);
            writer.Write(CreateInterpolationMatrix());
            writer.Write(ViewAngle);
            writer.Write(IsPerspectiveOff);
        }
    }

    /// <summary>
    /// 照明フレーム
    /// </summary>
    public class VocaloidLightFrame : VocaloidCameraTypeFrame
    {
        /// <summary>
        /// フレームの種類
        /// </summary>
        public override FrameType FrameType => FrameType.Light;

        /// <summary>
        /// 照明色
        /// </summary>
        public Vector3 Color { get; set; }

        /// <summary>
        /// 照明位置
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="frame">フレーム</param>
        public VocaloidLightFrame(uint frame = 0)
        {
            Name = "照明";
            Frame = frame;
            Color = new Vector3(154f / 255, 154f / 255, 154f / 255);
            Position = new Vector3(-0.5f, -1.0f, 0.5f);
        }

        /// <summary>
        /// バイナリから読み込むコンストラクタ
        /// </summary>
        public VocaloidLightFrame(BinaryReader reader)
        {
            Name = "照明";
            Read(reader);
        }

        /// <summary>
        /// VMD形式から読み込み
        /// </summary>
        public override void Read(BinaryReader reader)
        {
            Frame = reader.ReadUInt32();
            Color = reader.ReadVector3();
            Position = reader.ReadVector3();
        }

        /// <summary>
        /// VMD形式に書き込み
        /// </summary>
        public override void Write(BinaryWriter writer)
        {
            writer.Write(Frame);
            writer.Write(Color);
            writer.Write(Position);
        }
    }

    /// <summary>
    /// 影フレーム
    /// </summary>
    public class VocaloidShadowFrame : VocaloidCameraTypeFrame
    {
        /// <summary>
        /// フレームの種類
        /// </summary>
        public override FrameType FrameType => FrameType.Shadow;

        /// <summary>
        /// セルフ影モード
        /// </summary>
        public byte Mode { get; set; }

        /// <summary>
        /// 影範囲
        /// </summary>
        public float Range { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="frame">フレーム</param>
        public VocaloidShadowFrame(uint frame = 0)
        {
            Name = "セルフ影";
            Frame = frame;
            Mode = 1;
            Range = 8875 * 0.00001f;
        }

        /// <summary>
        /// バイナリから読み込むコンストラクタ
        /// </summary>
        public VocaloidShadowFrame(BinaryReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// VMD形式から読み込み
        /// </summary>
        public override void Read(BinaryReader reader)
        {
            Frame = reader.ReadUInt32();
            Mode = reader.ReadByte();
            Range = reader.ReadSingle();
        }

        /// <summary>
        /// VMD形式に書き込み
        /// </summary>
        public override void Write(BinaryWriter writer)
        {
            writer.Write(Frame);
            writer.Write(Mode);
            writer.Write(Range);
        }
    }
}
