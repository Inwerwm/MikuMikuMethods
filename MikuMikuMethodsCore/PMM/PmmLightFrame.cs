using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// 照明フレーム情報
    /// </summary>
    public class PmmLightFrame : PmmFrame
    {
        ColorF Color { get; set; }
        Vector3 Position { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmmLightFrame()
        {
            Color = ColorF.FromARGB(154, 154, 154);
        }
    }
}
