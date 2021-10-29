using MikuMikuMethods.PMM.Frame;
using System.Collections.Generic;

namespace MikuMikuMethods.PMM
{
    public class PmmBone : IPmmModelElement
    {
        public string Name { get; set; }
        public List<PmmBoneFrame> Frames { get; } = new();
        /// <summary>
        /// IKボーンか
        /// </summary>
        public bool IsIK { get; set; }
        /// <summary>
        /// 外部親になれるか
        /// </summary>
        public bool CanBecomeOuterParent { get; set; }
    }
}