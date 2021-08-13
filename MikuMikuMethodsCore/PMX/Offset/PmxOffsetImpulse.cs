﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMX
{
    /// <summary>
    /// インパルスモーフのオフセット
    /// </summary>
    public class PmxOffsetImpulse : IPmxOffset
    {
        /// <summary>
        /// 対象剛体
        /// </summary>
        public PmxBody Target { get; set; }
        /// <summary>
        /// ローカルフラグ
        /// </summary>
        public bool IsLocal { get; set; }
        /// <summary>
        /// 移動速度
        /// </summary>
        public Vector3 MovingSpead { get; set; }
        /// <summary>
        /// 回転トルク
        /// </summary>
        public Vector3 RotationTorque { get; set; }

        public override string ToString() => $"{Target.Name} : {{{MovingSpead} - {RotationTorque}}}";
    }
}
