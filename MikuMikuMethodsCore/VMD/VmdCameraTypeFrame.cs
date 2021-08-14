using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.VMD
{
    /// <summary>
    /// カメラ系フレームの抽象クラス
    /// </summary>
    public abstract class VmdCameraTypeFrame : VmdFrame
    {
        /// <summary>
        /// カメラ系フレームか？
        /// </summary>
        public override bool IsCameraType => true;

        /// <summary>
        /// モデル系フレームか？
        /// </summary>
        public override bool IsModelType => false;
    }
}
