using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.MME
{
    /// <summary>
    /// エフェクト対象モデルごと設定でのオブジェクト情報
    /// </summary>
    public class EmdObjectInfo : ObjectInfo
    {
        /// <summary>
        /// オブジェクトのキーを表す文字列
        /// </summary>
        public override string Name => $"Obj{Index}";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="index">オブジェクト番号</param>
        public EmdObjectInfo(int index) : base(index) { }
    }
}
