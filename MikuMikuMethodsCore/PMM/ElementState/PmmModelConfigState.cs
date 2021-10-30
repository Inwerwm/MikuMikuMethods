using System.Collections.Generic;

namespace MikuMikuMethods.PMM.ElementState
{
    public class PmmModelConfigState
    {
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 表示
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// IKが有効か
        /// </summary>
        public Dictionary<PmmBone, bool> EnableIK { get; } = new();
        /// <summary>
        /// 外部親設定
        /// <list>
        ///     <item>
        ///         <term>Key</term>
        ///         <description>子側ボーン</description>
        ///     </item>
        ///     <item>
        ///         <term>Value</term>
        ///         <description>外部親設定情報</description>
        ///     </item>
        /// </list>
        /// </summary>
        public Dictionary<PmmBone, PmmOuterParentState> OuterParent { get; } = new();
    }
}