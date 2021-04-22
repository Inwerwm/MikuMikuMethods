using MikuMikuMethods.Extension;
using MikuMikuMethods.PMM.Frame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// PMM内のカメラ情報
    /// </summary>
    public class PmmCamera
    {
        /// <summary>
        /// 初期位置のカメラフレーム
        /// </summary>
        public PmmCameraFrame InitialFrame { get; set; }
        /// <summary>
        /// カメラのキーフレーム
        /// </summary>
        public List<PmmCameraFrame> Frames { get; init; }

        /// <summary>
        /// 未確定編集状態
        /// </summary>
        public TemporaryCameraEditState Uncomitted { get; init; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmmCamera()
        {
            InitialFrame = new();
            Frames = new();
            Uncomitted = new();
        }

        /// <summary>
        /// バイナリデータからカメラ情報を読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        public PmmCamera(BinaryReader reader) : this()
        {
            Read(reader);
        }

        /// <summary>
        /// バイナリデータから読込
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        public void Read(BinaryReader reader)
        {
            InitialFrame = new(reader, null);

            var cameraCount = reader.ReadInt32();
            for (int i = 0; i < cameraCount; i++)
                Frames.Add(new(reader, i));

            Uncomitted.EyePosition = reader.ReadVector3();
            Uncomitted.TargetPosition = reader.ReadVector3();
            Uncomitted.Rotation = reader.ReadVector3();
            Uncomitted.EnablePerspective = reader.ReadBoolean();
        }
        /// <summary>
        /// ファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        public void Write(BinaryWriter writer)
        {
            InitialFrame.Write(writer);

            writer.Write(Frames.Count);
            foreach (var frame in Frames)
                frame.Write(writer);

            writer.Write(Uncomitted.EyePosition);
            writer.Write(Uncomitted.TargetPosition);
            writer.Write(Uncomitted.Rotation);
            writer.Write(Uncomitted.EnablePerspective);
        }
    }

    /// <summary>
    /// 未確定のカメラ編集状態
    /// </summary>
    public class TemporaryCameraEditState
    {
        /// <summary>
        /// カメラ位置
        /// </summary>
        public Vector3 EyePosition { get; set; }
        /// <summary>
        /// カメラ中心の位置
        /// </summary>
        public Vector3 TargetPosition { get; set; }
        /// <summary>
        /// カメラ回転
        /// </summary>
        public Vector3 Rotation { get; set; }
        /// <summary>
        /// パースのOn/Off
        /// </summary>
        public bool EnablePerspective { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TemporaryCameraEditState()
        {
            EnablePerspective = true;
        }
    }
}
