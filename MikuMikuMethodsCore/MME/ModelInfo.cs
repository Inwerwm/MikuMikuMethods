using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.MME
{
    /// <summary>
    /// エフェクト対象モデル情報
    /// </summary>
    public class ModelInfo : ObjectInfo
    {
        /// <summary>
        /// オブジェクトのキーを表す文字列
        /// </summary>
        public override string Name => $"Pmd{Index}";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="index">オブジェクト番号</param>
        public ModelInfo(int index) : base(index) { }
    }
}
