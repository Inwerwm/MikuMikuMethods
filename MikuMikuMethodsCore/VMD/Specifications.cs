﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.VMD
{
    /// <summary>
    /// 仕様によって定められた変数
    /// </summary>
    public static class Specifications
    {
        /// <summary>
        /// カメラVMDファイルのモデル名
        /// </summary>
        public static readonly string CameraTypeVMDName = "カメラ・照明";

        /// <summary>
        /// VMDファイルのヘッダー文字列
        /// </summary>
        public static readonly string HeaderString = "Vocaloid Motion Data 0002";

        /// <summary>
        /// VMDファイルのヘッダー部の領域長
        /// </summary>
        public static readonly int HeaderLength = 30;

        /// <summary>
        /// モデル名の領域長
        /// </summary>
        public static readonly int ModelNameLength = 20;

        /// <summary>
        /// ボーン名の領域長
        /// </summary>
        public static readonly int BoneNameLength = 15;

        /// <summary>
        /// モーフ名の領域長
        /// </summary>
        public static readonly int MorphNameLength = 15;

        /// <summary>
        /// IK名の領域長
        /// </summary>
        public static readonly int IKNameLength = 20;
    }
}
