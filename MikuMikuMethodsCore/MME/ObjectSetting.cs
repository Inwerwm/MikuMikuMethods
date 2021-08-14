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
    public class ObjectSetting
    {
        /// <summary>
        /// 対応するオブジェクト情報
        /// </summary>
        public ObjectInfo Entity { get; init; }

        /// <summary>
        /// オブジェクトに適用するエフェクトの情報
        /// </summary>
        public FxInfo Effect { get; init; }

        /// <summary>
        /// サブセット展開されたときの各材質に適用するエフェクトの情報
        /// </summary>
        public List<FxInfo> Subsets { get; init; }

        /// <summary>
        /// オブジェクト情報を指定してインスタンスを生成
        /// </summary>
        /// <param name="entity">オブジェクト情報</param>
        public ObjectSetting(ObjectInfo entity)
        {
            Effect = new();
            Subsets = new();
            Entity = entity;
        }
    }
}
