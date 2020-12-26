using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.MME
{
    /// <summary>
    /// オブジェクト情報
    /// </summary>
    public abstract class ObjectInfo
    {
        /// <summary>
        /// オブジェクトの番号
        /// </summary>
        public int Index { get; init; }

        /// <summary>
        /// オブジェクトのキーを表す文字列
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// オブジェクトのパス
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="index">オブジェクト番号</param>
        protected ObjectInfo(int index)
        {
            Index = index;
        }
    }
}
