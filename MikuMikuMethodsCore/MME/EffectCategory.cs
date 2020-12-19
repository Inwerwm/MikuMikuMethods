using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.MME
{
    /// <summary>
    /// エフェクト設定の種類
    /// </summary>
    public enum EffectCategory
    {
        /// <summary>
        /// <para>デフォルト設定</para>
        /// <para>MMEエフェクト割当画面の"Main"タブに相当</para>
        /// </summary>
        Effect,
        /// <summary>
        /// <para>その他のエフェクト設定</para>
        /// <para>MMEエフェクト割当画面の"Main"以外のタブに相当</para>
        /// </summary>
        Other,
    }
}
