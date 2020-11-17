using MikuMikuMethods.Extension;
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
        public Vector3 Move { get; set; }
        /// <summary>
        /// 回転量
        /// </summary>
        public Quaternion Rotation { get; set; }

        /// <summary>
        /// 補間曲線
        /// </summary>
        public Dictionary<InterpolationItem,InterpolationCurve> InterpolationCurves { get; init; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VocaloidMotionFrame()
        {
            InterpolationCurves = new();
            CreateInterpolationCurves();
        }

        /// <summary>
        /// バイナリから読み込むコンストラクタ
        /// </summary>
        public VocaloidMotionFrame(BinaryReader reader)
        {
            InterpolationCurves = new();
            CreateInterpolationCurves();
            Read(reader);
        }

        private void CreateInterpolationCurves()
        {
            InterpolationCurves.Add(InterpolationItem.XMove, new());
            InterpolationCurves.Add(InterpolationItem.YMove, new());
            InterpolationCurves.Add(InterpolationItem.ZMove, new());
            InterpolationCurves.Add(InterpolationItem.Rotation, new());
        }

        private byte[] CreateInterpolationMatrix()
        {
            // 補間曲線をbyte配列化
            var xMovePoints = InterpolationCurves[InterpolationItem.XMove].ToBytes();
            var yMovePoints = InterpolationCurves[InterpolationItem.YMove].ToBytes();
            var zMovePoints = InterpolationCurves[InterpolationItem.ZMove].ToBytes();
            var rotationPoints = InterpolationCurves[InterpolationItem.Rotation].ToBytes();

            // 形式に合わせて1行に整列
            var pointsRow = new byte[16];
            for (int i = 0; i < 4; i++)
            {
                pointsRow[i * 4 + 0] = xMovePoints[i];
                pointsRow[i * 4 + 1] = yMovePoints[i];
                pointsRow[i * 4 + 2] = zMovePoints[i];
                pointsRow[i * 4 + 3] = rotationPoints[i];
            }

            // 形式に合わせた行列に整形
            List<byte> interpolateMatrix = new();
            for (int i = 0; i < 4; i++)
            {
                // pointsRowから始めのi個を抜かしてrowへ転写
                var row = new byte[16];
                pointsRow.CopyTo(row, i);

                interpolateMatrix.AddRange(row);
            }

            return interpolateMatrix.ToArray();
        }

        /// <summary>
        /// VMD形式から読み込み
        /// </summary>
        public override void Read(BinaryReader reader)
        {
            //ボーン名を読み込み
            byte[] nameBytes = reader.ReadBytes(Specifications.BoneNameLength);
            Name = Encoding.ShiftJIS.GetString(nameBytes).Trim('\0');
            
            //各種パラメータを読み込み
            Frame = reader.ReadUInt32();
            Move = reader.ReadVector3();
            Rotation = reader.ReadQuaternion();

            //補間曲線を読み込み
            var interpolationMatrix = reader.ReadBytes(64).Select((num,i)=>(num,i));
            InterpolationCurves[InterpolationItem.XMove].FromBytes(interpolationMatrix.Where(elm => elm.i % 4 == 0).Select(elm => elm.num));
            InterpolationCurves[InterpolationItem.YMove].FromBytes(interpolationMatrix.Where(elm => elm.i % 4 == 1).Select(elm => elm.num));
            InterpolationCurves[InterpolationItem.ZMove].FromBytes(interpolationMatrix.Where(elm => elm.i % 4 == 2).Select(elm => elm.num));
            InterpolationCurves[InterpolationItem.Rotation].FromBytes(interpolationMatrix.Where(elm => elm.i % 4 == 3).Select(elm => elm.num));
        }

        /// <summary>
        /// VMD形式に書き込み
        /// </summary>
        public override void Write(BinaryWriter writer)
        {
            writer.Write(Name, Specifications.BoneNameLength, Encoding.ShiftJIS);
            writer.Write(Frame);
            writer.Write(Move);
            writer.Write(Rotation);
            writer.Write(CreateInterpolationMatrix());
        }
    }
}
