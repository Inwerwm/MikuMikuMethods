using MikuMikuMethods.Extension;
using System.Collections.Generic;
using System.IO;

namespace MikuMikuMethods.Vmd
{
    /// <summary>
    /// 表示・IKフレーム
    /// </summary>
    public class VmdPropertyFrame : VmdModelTypeFrame
    {
        /// <summary>
        /// フレームの種類
        /// </summary>
        public override VmdFrameType FrameType => VmdFrameType.Property;

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
        public VmdPropertyFrame(uint frame = 0)
        {
            Name = "プロパティ";
            Frame = frame;

            IKEnabled = new();
        }

        /// <summary>
        /// バイナリから読み込むコンストラクタ
        /// </summary>
        public VmdPropertyFrame(BinaryReader reader)
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
                IKEnabled.Add(reader.ReadString(VmdConstants.IKNameLength, Encoding.ShiftJIS, '\0'), reader.ReadBoolean());
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
                writer.Write(p.Key, VmdConstants.IKNameLength, Encoding.ShiftJIS);
                writer.Write(p.Value);
            }
        }

        public override object Clone() => new VmdPropertyFrame(Frame)
        {
            IKEnabled = new(IKEnabled),
            IsVisible = IsVisible,
            Name = Name
        };
    }
}
