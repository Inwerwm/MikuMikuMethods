using System.Collections.Generic;
using System.Linq;

namespace MikuMikuMethods.Pmm.ElementState
{
    public class PmmModelConfigState
    {
        /// <summary>
        /// 表示
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// IKが有効か
        /// </summary>
        public Dictionary<PmmBone, bool> EnableIK { get; protected init; } = new();
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
        public Dictionary<PmmBone, PmmOuterParentState> OuterParent { get; protected init; } = new();

        public PmmModelConfigState DeepCopy() => new()
        {
            Visible = Visible,
            EnableIK = new(EnableIK),
            OuterParent = OuterParent.ToDictionary(p => p.Key, p => p.Value.DeepCopy()),
        };
    }
}