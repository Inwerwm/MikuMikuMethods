﻿using MikuMikuMethods.PMM.Binary.Frame;
using System.Collections.Generic;
using System.Numerics;

namespace MikuMikuMethods.PMM.Binary
{
    /// <summary>
    /// PMMのアクセサリ情報
    /// </summary>
    public class PmmAccessory
    {
        /// <summary>
        /// アクセサリ管理番号
        /// </summary>
        public byte Index { get; set; }
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ファイルパス
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 描画順
        /// </summary>
        public byte RenderOrder { get; set; }
        /// <summary>
        /// 加算合成のOn/Off
        /// </summary>
        public bool EnableAlphaBlend { get; set; }

        /// <summary>
        /// 初期位置のアクセサリフレーム
        /// </summary>
        public PmmAccessoryFrame InitialFrame { get; set; }
        /// <summary>
        /// アクセサリのキーフレーム
        /// </summary>
        public List<PmmAccessoryFrame> Frames { get; init; }
        /// <summary>
        /// 未確定編集状態
        /// </summary>
        public TemporaryAccessoryEditState Uncomitted { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmmAccessory()
        {
            Frames = new();
        }
    }

    /// <summary>
    /// 未確定のアクセサリ編集状態
    /// </summary>
    public class TemporaryAccessoryEditState
    {
        /// <summary>
        /// <para>上位7bit : 半透明度</para>
        /// <para>最下位1bit : 表示/非表示</para>
        /// </summary>
        public byte OpacityAndVisible { get; set; }
        /// <summary>
        /// <para>親モデルのインデックス</para>
        /// <para>-1 なら親なし</para>
        /// </summary>
        public int ParentModelIndex { get; set; }
        /// <summary>
        /// 親ボーンのインデックス
        /// </summary>
        public int ParentBoneIndex { get; set; }
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
