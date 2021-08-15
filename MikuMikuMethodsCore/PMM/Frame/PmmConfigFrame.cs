using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM.Frame
{
    /// <summary>
    /// 表示・IK・外観フレーム情報
    /// </summary>
    public class PmmConfigFrame : PmmFrame
    {
        /// <summary>
        /// 表示
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// IKが有効か
        /// </summary>
        public List<bool> EnableIK { get; init; }

        /// <summary>
        /// 外部親設定
        /// </summary>
        public List<(int ModelId, int BoneId)> ParentSettings { get; init; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmmConfigFrame()
        {
            EnableIK = new();
            ParentSettings = new();
        }
    }
}
