using MikuMikuMethods.MME.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.MME
{
    /// <summary>
    /// Emdファイルの内部表現
    /// </summary>
    public class EmdData
    {
        /// <summary>
        /// EMMのバージョン
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// オブジェクトに適用するエフェクトの情報
        /// </summary>
        public EmmMaterial Material { get; init; }

        /// <summary>
        /// サブセット展開されたときの各材質に適用するエフェクトの情報
        /// </summary>
        public List<EmmMaterial> Subsets { get; init; }

        public EmdData(EmmMaterial material)
        {
            Material = material;
            Subsets = new();
        }
    }
}
