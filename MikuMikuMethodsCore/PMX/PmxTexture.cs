using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMX
{
    /// <summary>
    /// テクスチャ情報
    /// </summary>
    public record PmxTexture
    {
        public string Path { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path">テクスチャのパス</param>
        public PmxTexture(string path)
        {
            Path = path;
        }
    }
}
