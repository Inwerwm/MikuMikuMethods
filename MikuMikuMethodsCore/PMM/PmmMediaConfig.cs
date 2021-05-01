using MikuMikuMethods.Extension;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// メディア関連設定
    /// </summary>
    public class PmmMediaConfig
    {
        /// <summary>
        /// 音声の有無
        /// </summary>
        public bool EnableAudio { get; set; }
        /// <summary>
        /// 音声のファイルパス
        /// </summary>
        public string AudioPath { get; set; }

        /// <summary>
        /// 背景AVIの有無
        /// </summary>
        public bool EnableBackgroundVideo { get; set; }
        /// <summary>
        /// 背景AVIのファイルパス
        /// </summary>
        public string BackgroundVideoPath { get; set; }
        /// <summary>
        /// 背景AVIの表示スケール
        /// </summary>
        public float BackgroundVideoScale { get; set; }
        /// <summary>
        /// 背景AVIの表示位置
        /// </summary>
        public Point BackgroundVideoOffset { get; set; }

        /// <summary>
        /// 背景画像の有無
        /// </summary>
        public bool EnableBackgroundImage { get; set; }
        /// <summary>
        /// 背景画像のファイルパス
        /// </summary>
        public string BackgroundImagePath { get; set; }
        /// <summary>
        /// 背景画像のスケール
        /// </summary>
        public float BackgroundImageScale { get; set; }
        /// <summary>
        /// 背景画像の表示位置
        /// </summary>
        public Point BackgroundImageOffset { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmmMediaConfig() { }

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        public PmmMediaConfig(BinaryReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        public void Read(BinaryReader reader)
        {
            EnableAudio = reader.ReadBoolean();
            AudioPath = reader.ReadString(256, Encoding.ShiftJIS);

            BackgroundVideoOffset = new(reader.ReadInt32(), reader.ReadInt32());
            BackgroundVideoScale = reader.ReadSingle();
            BackgroundVideoPath = reader.ReadString(256, Encoding.ShiftJIS);
            EnableBackgroundVideo = reader.ReadInt32() == 0b01000000;

            BackgroundImageOffset = new(reader.ReadInt32(), reader.ReadInt32());
            BackgroundImageScale = reader.ReadSingle();
            BackgroundImagePath = reader.ReadString(256, Encoding.ShiftJIS);
            EnableBackgroundImage = reader.ReadBoolean();
        }

        /// <summary>
        /// ファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(EnableAudio);
            writer.Write(AudioPath, 256, Encoding.ShiftJIS, 'x');

            writer.Write(BackgroundVideoOffset.X);
            writer.Write(BackgroundVideoOffset.Y);
            writer.Write(BackgroundVideoScale);
            writer.Write(BackgroundVideoPath, 256, Encoding.ShiftJIS, 'x');
            writer.Write(EnableBackgroundVideo ? 0b01000000 : 0b01000001);

            writer.Write(BackgroundImageOffset.X);
            writer.Write(BackgroundImageOffset.Y);
            writer.Write(BackgroundImageScale);
            writer.Write(BackgroundImagePath, 256, Encoding.ShiftJIS, 'x');
            writer.Write(EnableBackgroundImage);
        }
    }
}
