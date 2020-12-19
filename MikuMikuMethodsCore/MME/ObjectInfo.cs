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
    public class ObjectInfo
    {
        /// <summary>
        /// オブジェクトの種別
        /// </summary>
        public ObjectType Type { get; init; }

        private string TypeString => Type switch
        {
            ObjectType.Model => "Pmd",
            ObjectType.Accessory => "Acs",
            ObjectType.Object => "Obj",
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// オブジェクトの番号
        /// </summary>
        public int Index { get; init; }

        /// <summary>
        /// オブジェクトのパス
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// オブジェクトのキーを表す文字列
        /// </summary>
        public string Name => $"{TypeString}{Index}";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">オブジェクトの種別</param>
        /// <param name="index">オブジェクトの番号</param>
        public ObjectInfo(ObjectType type, int index)
        {
            Type = type;
            Index = index;
        }
    }
}
