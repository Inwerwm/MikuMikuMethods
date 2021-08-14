﻿namespace MikuMikuMethods.VMD
{
    /// <summary>
    /// モデル系フレームの抽象クラス
    /// </summary>
    public abstract class VmdModelTypeFrame : VmdFrame
    {
        /// <summary>
        /// カメラ系フレームか？
        /// </summary>
        public override bool IsCameraType => false;

        /// <summary>
        /// モデル系フレームか？
        /// </summary>
        public override bool IsModelType => true;
    }
}
