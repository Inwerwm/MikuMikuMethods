﻿using MikuMikuMethods.Extension;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace MikuMikuMethods.VMD
{
    /// <summary>
    /// 表示・IKフレーム
    /// </summary>
    public class VocaloidPropertyFrame : VocaloidModelTypeFrame
    {
        /// <summary>
        /// フレームの種類
        /// </summary>
        public override FrameType FrameType => FrameType.Property;

        /// <summary>
        /// 表示/非表示
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// IK有効/無効
        /// </summary>
        public Dictionary<string, bool> IKEnabled { get; init; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="frame">フレーム</param>
        public VocaloidPropertyFrame(uint frame = 0)
        {
            Name = "プロパティ";
            Frame = frame;

            IKEnabled = new();
        }

        /// <summary>
        /// バイナリから読み込むコンストラクタ
        /// </summary>
        public VocaloidPropertyFrame(BinaryReader reader)
        {
            IKEnabled = new();

            Read(reader);
        }

        /// <summary>
        /// VMD形式から読み込み
        /// </summary>
        public override void Read(BinaryReader reader)
        {
            Frame = reader.ReadUInt32();
            IsVisible = reader.ReadBoolean();

            var ikCount = reader.ReadUInt32();
            for (int i = 0; i < ikCount; i++)
            {
                IKEnabled.Add(reader.ReadString(Specifications.IKNameLength, Encoding.ShiftJIS, '\0'), reader.ReadBoolean());
            }
        }

        /// <summary>
        /// VMD形式に書き込み
        /// </summary>
        public override void Write(BinaryWriter writer)
        {
            writer.Write(Frame);
            writer.Write(IsVisible);

            writer.Write(IKEnabled.Count);
            foreach (var p in IKEnabled)
            {
                writer.Write(p.Key, Specifications.IKNameLength, Encoding.ShiftJIS);
                writer.Write(p.Value);
            }
        }
    }

    /// <summary>
    /// モーフフレーム
    /// </summary>
    public class VocaloidMorphFrame : VocaloidModelTypeFrame
    {
        /// <summary>
        /// フレームの種類
        /// </summary>
        public override FrameType FrameType => FrameType.Morph;

        /// <summary>
        /// モーフ適用係数
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">ボーン名</param>
        /// <param name="frame">フレーム時間</param>
        public VocaloidMorphFrame(string name, uint frame = 0)
        {
            Name = name;
            Frame = frame;
        }

        /// <summary>
        /// バイナリから読み込むコンストラクタ
        /// </summary>
        public VocaloidMorphFrame(BinaryReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// VMD形式から読み込み
        /// </summary>
        public override void Read(BinaryReader reader)
        {
            Name = reader.ReadString(Specifications.MorphNameLength, Encoding.ShiftJIS, '\0');
            Frame = reader.ReadUInt32();
            Weight = reader.ReadSingle();
        }

        /// <summary>
        /// VMD形式に書き込み
        /// </summary>
        public override void Write(BinaryWriter writer)
        {
            writer.Write(Name, Specifications.MorphNameLength, Encoding.ShiftJIS);
            writer.Write(Frame);
            writer.Write(Weight);
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
        public VocaloidMotionFrame(string name, uint frame = 0)
        {
            Name = name;
            Frame = frame;
            InterpolationCurves = new();
            InitializeInterpolationCurves();
        }

        /// <summary>
        /// バイナリから読み込むコンストラクタ
        /// </summary>
        public VocaloidMotionFrame(BinaryReader reader)
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
            Name = reader.ReadString(Specifications.BoneNameLength, Encoding.ShiftJIS, '\0');

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
            writer.Write(Name, Specifications.BoneNameLength, Encoding.ShiftJIS);
            writer.Write(Frame);
            writer.Write(Position);
            writer.Write(Rotation);
            writer.Write(InterpolationCurve.CreateVMDFormatBytes(InterpolationCurves, FrameType));
        }
    }
}
