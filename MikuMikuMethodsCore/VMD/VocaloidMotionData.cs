using MikuMikuMethods.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.VMD
{
    /// <summary>
    /// VMDの種類
    /// </summary>
    public enum VMDType
    {
        /// <summary>
        /// カメラ系
        /// </summary>
        Camera,
        /// <summary>
        /// モデル系
        /// </summary>
        Model
    }

    /// <summary>
    /// VMDファイルの内部表現
    /// </summary>
    public class VocaloidMotionData
    {

        /// <summary>
        /// ヘッダー
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// モデル名
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// VMDの種類
        /// </summary>
        public VMDType Type => ModelName == Specifications.CameraTypeVMDName ? VMDType.Camera : VMDType.Model;

        /// <summary>
        /// カメラフレーム
        /// </summary>
        public List<VocaloidCameraFrame> CameraFrames { get; init; }
        /// <summary>
        /// 照明フレーム
        /// </summary>
        public List<VocaloidLightFrame> LightFrames { get; init; }
        /// <summary>
        /// セルフ影フレーム
        /// </summary>
        public List<VocaloidShadowFrame> ShadowFrames { get; init; }

        /// <summary>
        /// プロパティフレーム
        /// </summary>
        public List<VocaloidPropertyFrame> PropertyFrames { get; init; }
        /// <summary>
        /// モーフフレーム
        /// </summary>
        public List<VocaloidMorphFrame> MorphFrames { get; init; }
        /// <summary>
        /// モーションフレーム
        /// </summary>
        public List<VocaloidMotionFrame> MotionFrames { get; init; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="modelName">モデル名</param>
        public VocaloidMotionData(string modelName = "")
        {
            Header = Specifications.HeaderString;
            ModelName = modelName;

            CameraFrames = new();
            LightFrames = new();
            ShadowFrames = new();

            PropertyFrames = new();
            MorphFrames = new();
            MotionFrames = new();
        }

        /// <summary>
        /// バイナリ読み込みコンストラクタ
        /// </summary>
        /// <param name="reader"></param>
        public VocaloidMotionData(BinaryReader reader)
        {
            CameraFrames = new();
            LightFrames = new();
            ShadowFrames = new();

            PropertyFrames = new();
            MorphFrames = new();
            MotionFrames = new();

            Read(reader);
        }

        /// <summary>
        /// 内容の初期化
        /// </summary>
        public void Clear()
        {
            ModelName = "";

            CameraFrames.Clear();
            LightFrames.Clear();
            ShadowFrames.Clear();

            PropertyFrames.Clear();
            MorphFrames.Clear();
            MotionFrames.Clear();
        }

        /// <summary>
        /// 読み込み
        /// </summary>
        /// <param name="reader">VMDファイルのリーダー</param>
        public void Read(BinaryReader reader)
        {
            Header = reader.ReadString(Specifications.HeaderLength, Encoding.ShiftJIS, '\0');
            ModelName = reader.ReadString(Specifications.ModelNameLength, Encoding.ShiftJIS, '\0');

            uint elementNum;

            elementNum = reader.ReadUInt32();
            for (int i = 0; i < elementNum; i++)
            {
                MotionFrames.Add(new VocaloidMotionFrame(reader));
            }

            elementNum = reader.ReadUInt32();
            for (int i = 0; i < elementNum; i++)
            {
                MorphFrames.Add(new VocaloidMorphFrame(reader));
            }

            elementNum = reader.ReadUInt32();
            for (int i = 0; i < elementNum; i++)
            {
                CameraFrames.Add(new VocaloidCameraFrame(reader));
            }

            elementNum = reader.ReadUInt32();
            for (int i = 0; i < elementNum; i++)
            {
                LightFrames.Add(new VocaloidLightFrame(reader));
            }

            elementNum = reader.ReadUInt32();
            for (int i = 0; i < elementNum; i++)
            {
                ShadowFrames.Add(new VocaloidShadowFrame(reader));
            }

            elementNum = reader.ReadUInt32();
            for (int i = 0; i < elementNum; i++)
            {
                PropertyFrames.Add(new VocaloidPropertyFrame(reader));
            }
        }

        /// <summary>
        /// VMD形式で書き出し
        /// </summary>
        public void Write(BinaryWriter writer)
        {
            writer.Write(Header, Specifications.HeaderLength, Encoding.ShiftJIS, '\0');
            writer.Write(ModelName, Specifications.ModelNameLength, Encoding.ShiftJIS, '\0');
            WriteFrames(writer, MotionFrames);
            WriteFrames(writer, MorphFrames);
            WriteFrames(writer, CameraFrames);
            WriteFrames(writer, LightFrames);
            WriteFrames(writer, ShadowFrames);
            WriteFrames(writer, PropertyFrames);
        }

        private void WriteFrames<T>(BinaryWriter writer, List<T> frames) where T : IVocaloidFrame
        {
            writer.Write((uint)frames.Count);
            // 時間で降順に書き込むと読み込みが早くなる(らしい)
            foreach (var f in frames.OrderByDescending(f => f.Frame))
            {
                f.Write(writer);
            }
        }
    }
}
