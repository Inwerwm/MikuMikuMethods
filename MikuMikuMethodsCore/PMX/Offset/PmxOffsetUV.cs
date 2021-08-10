﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMX
{
    /// <summary>
    /// UVモーフのオフセット
    /// </summary>
    public class PmxOffsetUV : IPmxOffset
    {
        /// <summary>
        /// 対象頂点
        /// </summary>
        public PmxVertex Target { get; set; }
        /// <summary>
        /// 移動量
        /// </summary>
        public Vector4 Offset { get; set; }
    }
}
