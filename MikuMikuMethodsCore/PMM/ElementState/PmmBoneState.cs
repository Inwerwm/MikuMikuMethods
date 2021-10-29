using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM.ElementState
{
    public class PmmBoneState
    {
        /// <summary>
        /// ボーン名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 移動量
        /// </summary>
        public Vector3 Movement { get; set; }

        /// <summary>
        /// 回転量
        /// </summary>
        public Quaternion Rotation { get; set; }

        /// <summary>
        /// 物理が有効か
        /// </summary>
        public bool EnablePhysic { get; set; }

        /// <summary>
        /// 現在選択状態か
        /// </summary>
        public bool RowIsSelected { get; set; }

        /// <summary>
        /// 未確定ボーンか
        /// </summary>
        public bool IsThis { get; set; }
    }
}
