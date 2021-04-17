using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Numerics;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// カメラフレーム情報
    /// </summary>
    public class PmmCameraFrame : PmmFrame
    {
        /// <summary>
        /// カメラの距離
        /// </summary>
        public float Distance { get; set; }
        /// <summary>
        /// カメラの位置
        /// </summary>
        public Vector3 EyePosition { get; set; }
        /// <summary>
        /// カメラの回転
        /// </summary>
        public Vector3 Rotation { get; set; }
        /// <summary>
        /// <para>追従モデルのインデックス</para>
        /// <para>-1 で非選択</para>
        /// </summary>
        public int FollowingModelIndex { get; set; }
        /// <summary>
        /// 追従ボーンのインデックス
        /// </summary>
        public int FollowingBoneIndex { get; set; }
        /// <summary>
        /// 補間曲線
        /// </summary>
        public ReadOnlyDictionary<InterpolationItem, InterpolationCurve> InterpolationCurces { get; init; }
        /// <summary>
        /// パースのOn/Off
        /// </summary>
        public bool EnablePerspective { get; set; }
        /// <summary>
        /// 視野角
        /// </summary>
        public int ViewAngle { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmmCameraFrame()
        {
            Dictionary<InterpolationItem, InterpolationCurve> curveDic = new(6);
            curveDic.Add(InterpolationItem.XPosition, new());
            curveDic.Add(InterpolationItem.YPosition, new());
            curveDic.Add(InterpolationItem.ZPosition, new());
            curveDic.Add(InterpolationItem.Rotation, new());
            curveDic.Add(InterpolationItem.Distance, new());
            curveDic.Add(InterpolationItem.ViewAngle, new());

            InterpolationCurces = new(curveDic);
        }

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        /// <param name="index">フレームID</param>
        public void Read(BinaryReader reader, int? index)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// ファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        public void Write(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
