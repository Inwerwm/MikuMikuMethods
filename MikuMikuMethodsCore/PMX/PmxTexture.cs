using MikuMikuMethods.Extension;
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
        /// <summary>
        /// テクスチャのパス
        /// </summary>
        public string Path { get; init; }
        /// <summary>
        /// <para>共有Toonテクスチャ番号</para>
        /// <para>共有Toonでない場合はnull</para>
        /// </summary>
        public int? ToonIndex { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path">テクスチャのパス</param>
        public PmxTexture(string path)
        {
            Path = path;
            ToonIndex = null;
        }

        /// <summary>
        /// 共有Toonテクスチャのコンストラクタ
        /// </summary>
        /// <param name="toonIndex">共有Toonテクスチャの番号[1～10]</param>
        public PmxTexture(int toonIndex)
        {
            Path = toonIndex.IsWithin(1, 10)
                 ? $"toon{toonIndex:00}.bmp"
                 : throw new ArgumentOutOfRangeException("共有Toonテクスチャは[1,10]の区間で指定してください。");
            ToonIndex = toonIndex;
        }

        public override string ToString() => $"{Path}";
    }
}
