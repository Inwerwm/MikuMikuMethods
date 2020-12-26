using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.MME
{
    /// <summary>
    /// エフェクト対象アクセサリ情報
    /// </summary>
    public class AccessoryInfo : ObjectInfo
    {
        /// <summary>
        /// オブジェクトのキーを表す文字列
        /// </summary>
        public override string Name => $"Acs{Index}";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="index">オブジェクト番号</param>
        public AccessoryInfo(int index) : base(index) { }
    }
}
