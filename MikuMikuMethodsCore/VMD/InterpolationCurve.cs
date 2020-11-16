﻿using System;

namespace MikuMikuMethods.VMD
{
    /// <summary>
    /// 補間項目
    /// </summary>
    public enum InterpolationItem
    {
        /// <summary>
        /// x軸移動
        /// </summary>
        XMove,
        /// <summary>
        /// Y軸移動
        /// </summary>
        YMove,
        /// <summary>
        /// Z軸移動
        /// </summary>
        ZMove,
        /// <summary>
        /// 回転
        /// </summary>
        Rotation,
        /// <summary>
        /// 距離
        /// </summary>
        Distance,
        /// <summary>
        /// 視野角
        /// </summary>
        ViewAngle,
    }

    /// <summary>
    /// 補間曲線
    /// </summary>
    public class InterpolationCurve
    {
        private (byte X, byte Y) earlyControlePoint;
        private (byte X, byte Y) lateControlePoint;

        /// <summary>
        /// 始点側制御点
        /// [0,127]
        /// </summary>
        public (byte X, byte Y) EarlyControlePoint
        {
            get => earlyControlePoint;
            set
            {
                if (value.X is < 0 or > 127 || value.Y is < 0 or > 127)
                    throw new ArgumentOutOfRangeException("InterpolationCurve.EarlyControlePoint has seted to out of range value.");
                earlyControlePoint = value;
            }
        }
        /// <summary>
        /// 終点側制御点
        /// [0,127]
        /// </summary>
        public (byte X, byte Y) LateControlePoint
        {
            get => lateControlePoint;
            set
            {
                if (value.X is < 0 or > 127 || value.Y is < 0 or > 127)
                    throw new ArgumentOutOfRangeException("InterpolationCurve.LateControlePoint has seted to out of range value.");
                lateControlePoint = value;
            }
        }

        /// <summary>
        /// 始点側制御点
        /// [0.0,1.0]
        /// </summary>
        public (float X, float Y) EarlyControlePointFloat
        {
            get => (EarlyControlePoint.X / 127.0f, EarlyControlePoint.Y / 127.0f);
            set
            {
                if (value.X is < 0 or > 1 || value.Y is < 0 or > 1)
                    throw new ArgumentOutOfRangeException("InterpolationCurve.EarlyControlePointFloat has seted to out of range value.");
                EarlyControlePoint = ((byte)(value.X * 127), (byte)(value.Y * 127));
            }
        }
        /// <summary>
        /// 終点側制御点
        /// [0.0,1.0]
        /// </summary>
        public (float X, float Y) LateControlePointFloat
        {
            get => (LateControlePoint.X / 127.0f, LateControlePoint.Y / 127.0f);
            set
            {
                if (value.X is < 0 or > 1 || value.Y is < 0 or > 1)
                    throw new ArgumentOutOfRangeException("InterpolationCurve.LateControlePointFloat has seted to out of range value.");
                LateControlePoint = ((byte)(value.X * 127), (byte)(value.Y * 127));
            }
        }

        /// <summary>
        /// バイト配列で返す
        /// {始点側制御点X, 始点側制御点Y, 終点側制御点X, 終点側制御点Y}
        /// </summary>
        public byte[] Tobytes => new byte[] { EarlyControlePoint.X, EarlyControlePoint.Y, LateControlePoint.X, LateControlePoint.Y };
    }
}