using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.MME
{
    /// <summary>
    /// オブジェクト単位のエフェクト設定
    /// </summary>
    public class TargetObject
    {
        /// <summary>
        /// 対応するオブジェクト情報
        /// </summary>
        public ObjectInfo Key { get; init; }

        /// <summary>
        /// オブジェクトに適用するエフェクトの情報
        /// </summary>
        public FxInfo Effect => new();

        /// <summary>
        /// サブセット展開されたときの各材質に適用するエフェクトの情報
        /// </summary>
        public List<FxInfo> Subsets => new();

        /// <summary>
        /// オブジェクト情報を指定してインスタンスを生成
        /// </summary>
        /// <param name="key">オブジェクト情報</param>
        public TargetObject(ObjectInfo key)
        {
            Key = key;
        }
    }
}
