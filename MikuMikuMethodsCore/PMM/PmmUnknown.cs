﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// 意図不明な値
    /// </summary>
    public struct PmmUnknown
    {
        /// <summary>
        /// 64byteの行列らしき値
        /// </summary>
        public static readonly byte[] Matrix = new byte[]
        {
            0x00,0x00,0x80,0x3F,
            0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,

            0x00,0x00,0x00,0x00,
            0x00,0x00,0x80,0x3F,
            0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,

            0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,
            0x00,0x00,0x80,0x3F,
            0x00,0x00,0x00,0x00,

            0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,
            0x00,0x00,0x80,0x3F
        };

        /// <summary>
        /// 真理値らしき値
        /// </summary>
        public bool TruthValue { get; set; }
    }
}
