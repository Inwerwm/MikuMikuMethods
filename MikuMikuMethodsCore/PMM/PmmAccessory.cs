using MikuMikuMethods.Extension;
using MikuMikuMethods.PMM.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MikuMikuMethods.PMM
{
    public class PmmAccessory : IRelationableElement<PmmAccessory>
    {
        internal List<PmmAccessory> _RenderOrderCollection { get; set; }
        void IRelationableElement<PmmAccessory>.AddRelation(IEnumerable<List<PmmAccessory>> lists)
        {
            _RenderOrderCollection = lists.ElementAt(0);
        }
        void IRelationableElement<PmmAccessory>.RemoveRelation()
        {
            _RenderOrderCollection = null;
        }
        public bool RegisteredToPmm() => _RenderOrderCollection?.Contains(this) ?? false;

        /// <summary>
        /// 描画順
        /// <para>get/set 共に PolygonMovieMaker クラスに属していなければ例外を吐く</para>
        /// </summary>
        public byte RenderOrder
        {
            get => RegisteredToPmm() ? (byte)_RenderOrderCollection.IndexOf(this) : throw new InvalidOperationException("描画順操作は PolygonMovieMaker クラスに登録されていなければできません。");
            set
            {
                if (!RegisteredToPmm())
                {
                    throw new InvalidOperationException("描画順操作は PolygonMovieMaker クラスに登録されていなければできません。");
                }

                var oldIndex = _RenderOrderCollection.IndexOf(this);
                _RenderOrderCollection.Remove(this);
                try
                {
                    _RenderOrderCollection.Insert(value, this);
                }
                catch (Exception)
                {
                    _RenderOrderCollection.Insert(oldIndex, this);
                    throw;
                }
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