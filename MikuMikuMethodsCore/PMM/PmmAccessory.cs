using MikuMikuMethods.Extension;
using MikuMikuMethods.PMM.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MikuMikuMethods.PMM
{
    public class PmmAccessory
    {
        internal static List<PmmAccessory> _RenderOrderCollection { get; set; }
        /// <summary>
        /// 描画順
        /// </summary>
        public byte RenderOrder
        {
            get => _RenderOrderCollection.Contains(this) ? (byte)_RenderOrderCollection.IndexOf(this) : throw new InvalidOperationException("描画順操作は PolygonMovieMaker クラスに登録されていなければできません。");
            set
            {
                if (!_RenderOrderCollection.Contains(this))
                {
                    throw new InvalidOperationException("描画順操作は PolygonMovieMaker クラスに登録されていなければできません。");
                }

                _RenderOrderCollection.Remove(this);
                _RenderOrderCollection.Insert(value, this);
            }
        }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ファイルパス
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 加算合成のOn/Off
        /// </summary>
        public bool EnableAlphaBlend { get; set; } = false;

        public List<PmmAccessoryFrame> Frames { get; } = new();
        public TemporaryAccessoryState Current { get; } = new();

        public class TemporaryAccessoryState
        {
            private float transparency;

            internal byte TransAndVisible
            {
                get => PmmAccessoryFrame.CreateTransAndVisible(Transparency, Visible);
                set => (Visible, Transparency) = PmmAccessoryFrame.SeparateTransAndVisible(value);
            }

            public bool Visible { get; set; } = true;
            public float Transparency
            {
                get => transparency;
                set
                {
                    if (value.IsWithin(0, 1))
                        transparency = value;
                    else 
                        throw new ArgumentOutOfRangeException("透明度は [0, 1] の範囲である必要があります。");
                }
            }

            /// <summary>
            /// 親モデル
            /// </summary>
            public PmmModel ParentModel { get; set; }
            /// <summary>
            /// 親ボーン
            /// </summary>
            public PmmBone ParentBone { get; set; }

            /// <summary>
            /// 位置
            /// </summary>
            public Vector3 Position { get; set; }
            /// <summary>
            /// 回転
            /// </summary>
            public Vector3 Rotation { get; set; }
            /// <summary>
            /// 拡縮
            /// </summary>
            public float Scale { get; set; }
            /// <summary>
            /// 影のOn/Off
            /// </summary>
            public bool EnableShadow { get; set; }
        }
    }
}