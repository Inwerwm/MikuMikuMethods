using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// モーフフレーム情報
    /// </summary>
    public class PmmMorphFrame
    {
        /// <summary>
        /// <para>フレーム番号</para>
        /// <para>初期フレームには振られないのでnullを入れる</para>
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        /// 所在フレーム
        /// </summary>
        public int Frame { get; set; }

        /// <summary>
        /// <para>直前のキーフレームのID</para>
        /// <para>存在しなければ0</para>
        /// </summary>
        public int PreviousFrameIndex { get; set; }
        /// <summary>
        /// 直後のキーフレームのID
        /// </summary>
        public int NextFrameIndex { get; set; }

        /// <summary>
        /// モーフ適用係数
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// 選択されているか
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
