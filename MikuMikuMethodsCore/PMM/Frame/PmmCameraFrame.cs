﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM.Frame
{
    public class PmmCameraFrame : IPmmFrame
    {
        public int Frame { get; set; } = 0;
        public bool IsSelected { get; set; } = false;

        /// <summary>
        /// カメラの距離
        /// </summary>
        public float Distance { get; set; } = 45;
        /// <summary>
        /// カメラの位置
        /// </summary>
        public Vector3 EyePosition { get; set; } = new(0, 10, 0);
        /// <summary>
        /// カメラの回転
        /// </summary>
        public Vector3 Rotation { get; set; } = new(0);
        /// <summary>
        /// <para>追従モデルのインデックス</para>
        /// <para>-1 で非選択</para>
        /// </summary>
        public PmmModel FollowingModel { get; set; }
        /// <summary>
        /// 追従ボーンのインデックス
        /// </summary>
        public PmmBone FollowingBone { get; set; }
        /// <summary>
        /// 補間曲線
        /// </summary>
        public ReadOnlyDictionary<InterpolationItem, InterpolationCurve> InterpolationCurces { get; } = new(new Dictionary<InterpolationItem, InterpolationCurve>()
        {
            { InterpolationItem.XPosition, new() },
            { InterpolationItem.YPosition, new() },
            { InterpolationItem.ZPosition, new() },
            { InterpolationItem.Rotation, new() },
            { InterpolationItem.Distance, new() },
            { InterpolationItem.ViewAngle, new() },
        });
        /// <summary>
        /// パースのOn/Off
        /// </summary>
        public bool EnablePerspective { get; set; } = true;
        /// <summary>
        /// 視野角
        /// </summary>
        public int ViewAngle { get; set; } = 30;

        public bool Equals(IPmmFrame other) =>
            other is PmmCameraFrame f && f.Frame == Frame;
    }
}