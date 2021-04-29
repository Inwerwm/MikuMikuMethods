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
    /// 描画関連設定
    /// </summary>
    public class PmmDrawConfig
    {
        /// <summary>
        /// 描画情報の表示/非表示
        /// </summary>
        public bool IsShowInfomation { get; set; }
        /// <summary>
        /// 座標軸の表示/非表示
        /// </summary>
        public bool IsShowAxis { get; set; }
        /// <summary>
        /// 地面影の表示/非表示
        /// </summary>
        public bool IsShowGrandShadow { get; set; }

        /// <summary>
        /// FPS制限量
        /// </summary>
        public float FPSLimit { get; set; }
        /// <summary>
        /// スクリーン用キャプチャモード設定
        /// </summary>
        public ScreenCaptureMode ScreenCaptureSetting { get; set; }
        /// <summary>
        /// モデルより後に描画するアクセサリのインデックス
        /// </summary>
        public int AccessoryModelThreshold { get; set; }

        /// <summary>
        /// 地面影の明るさ
        /// </summary>
        public float GroundShadowBrightness { get; set; }
        /// <summary>
        /// 地面影の透明化
        /// </summary>
        public bool EnableTransparentGroundShadow { get; set; }

        /// <summary>
        /// 物理演算モード
        /// </summary>
        public PhysicsMode PhysicsSetting { get; set; }

        #region ColorConfig
        /// <summary>
        /// エッジの色
        /// </summary>
        public Color EdgeColor { get; set; }
        /// <summary>
        /// 背景色は黒か
        /// </summary>
        public bool IsBackgroundBlack { get; set; }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmmDrawConfig() { }

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        public PmmDrawConfig(BinaryReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        public void Read(BinaryReader reader)
        {
            IsShowInfomation = reader.ReadBoolean();
            IsShowAxis = reader.ReadBoolean();
            IsShowGrandShadow = reader.ReadBoolean();

            FPSLimit = reader.ReadSingle();
            ScreenCaptureSetting = (ScreenCaptureMode)reader.ReadInt32();
            AccessoryModelThreshold = reader.ReadInt32();

            GroundShadowBrightness = reader.ReadSingle();
            EnableTransparentGroundShadow = reader.ReadBoolean();

            PhysicsSetting = (PhysicsMode)reader.ReadByte();
        }

        /// <summary>
        /// ファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(IsShowInfomation);
            writer.Write(IsShowAxis);
            writer.Write(IsShowGrandShadow);

            writer.Write(FPSLimit);
            writer.Write((int)ScreenCaptureSetting);
            writer.Write(AccessoryModelThreshold);

            writer.Write(GroundShadowBrightness);
            writer.Write(EnableTransparentGroundShadow);

            writer.Write((byte)PhysicsSetting);
        }

        /// <summary>
        /// 色関連設定を読み込む
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        public void ReadColorConfig(BinaryReader reader)
        {
            EdgeColor = Color.FromArgb(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
            IsBackgroundBlack = reader.ReadBoolean();
        }

        /// <summary>
        /// 色関連設定を書き込む
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        public void WriteColorConfig(BinaryWriter writer)
        {
            writer.Write(EdgeColor.R);
            writer.Write(EdgeColor.G);
            writer.Write(EdgeColor.B);

            writer.Write(IsBackgroundBlack);
        }

        /// <summary>
        /// スクリーン用キャプチャモード
        /// </summary>
        public enum ScreenCaptureMode : int
        {
            /// <summary>
            /// オフ
            /// </summary>
            Off,
            /// <summary>
            /// 全画面
            /// </summary>
            FullScreen,
            /// <summary>
            /// 4:3比率
            /// </summary>
            Square,
            /// <summary>
            /// 背景AVI
            /// </summary>
            BackgroundVideo
        }

        /// <summary>
        /// 物理演算モード
        /// </summary>
        public enum PhysicsMode : byte
        {
            /// <summary>
            /// 演算しない
            /// </summary>
            Disable,
            /// <summary>
            /// 常に演算
            /// </summary>
            Always,
            /// <summary>
            /// オン/オフモード
            /// </summary>
            Switchable,
            /// <summary>
            /// トレースモード
            /// </summary>
            Trace
        }
    }
}
