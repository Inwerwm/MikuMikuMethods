using MikuMikuMethods.PMM.ElementState;
using MikuMikuMethods.PMM.Frame;
using System.Collections.Generic;
using System.Linq;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// PMM用アクセサリークラス
    /// <para>複数の PMM クラスで共有すると描画/計算順に不整合が発生するので Clone した別インスタンスを入れること</para>
    /// </summary>
    public class PmmAccessory : IRelationableElement<PmmAccessory>
    {
        internal List<PmmAccessory> _RenderOrderCollection { get; set; }
        void IRelationableElement<PmmAccessory>.AddRelation(IEnumerable<List<PmmAccessory>> lists)
        {
            _RenderOrderCollection = lists.ElementAt(0);
        }
        void IRelationableElement<PmmAccessory>.RemoveRelation()
        {
            _RenderOrderCollection = null;
        }
        /// <summary>
        /// 描画順
        /// <para>get/set 共にアクセサリが PolygonMovieMaker クラスに属していなければ例外を吐く</para>
        /// <para>PMM クラスにアクセサリを追加すると追加先の文脈に応じて順序が定まるようになる</para>
        /// <para>複数の PMM クラスで共有すると順序に不整合が発生するのでアクセサリ自体を Clone すること</para>
        /// </summary>
        public byte RenderOrder
        {
            get => IRelationableElement<PmmAccessory>.GetOrder(this, _RenderOrderCollection, "描画");
            set => IRelationableElement<PmmAccessory>.SetOrder(this, _RenderOrderCollection, "描画", value);
        }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ファイルパス
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 加算合成のOn/Off
        /// </summary>
        public bool EnableAlphaBlend { get; set; } = false;

        public List<PmmAccessoryFrame> Frames { get; } = new();
        public PmmAccessoryState UncomittedState { get; } = new();
    }
}