﻿using MikuMikuMethods.PMM.Frame;
using System.Collections.Generic;

namespace MikuMikuMethods.PMM
{
    public class PmmSelfShadow
    {
        /// <summary>
        /// セルフ影のOn/Off
        /// </summary>
        public bool EnableSelfShadow { get; set; } = true;
        /// <summary>
        /// 影範囲
        /// </summary>
        public float ShadowRange { get; set; } = 8875;

        public List<PmmSelfShadowFrame> Frames { get; } = new();
    }
}