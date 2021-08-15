using MikuMikuMethods.PMM.Frame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// セルフ影情報
    /// </summary>
    public class PmmSelfShadow
    {
        /// <summary>
        /// セルフ影のOn/Off
        /// </summary>
        public bool EnableSelfShadow { get; set; }
        /// <summary>
        /// 影範囲
        /// </summary>
        public float ShadowLimit { get; set; }

        /// <summary>
        /// 初期位置のセルフ影フレーム
        /// </summary>
        public PmmSelfShadowFrame InitialFrame { get; set; }
        /// <summary>
        /// セルフ影のキーフレーム
        /// </summary>
        public List<PmmSelfShadowFrame> Frames { get; init; } = new();
    }
}
