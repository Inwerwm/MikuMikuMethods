using System.Collections.Generic;
using System.Linq;

namespace MikuMikuMethods.PMM
{
    public class PmmModel : IRelationableElement<PmmModel>
    {
        internal List<PmmModel> _RenderOrderCollection { get; set; }
        internal List<PmmModel> _CalculateOrderCollection { get; set; }
        void IRelationableElement<PmmModel>.AddRelation(IEnumerable<List<PmmModel>> lists)
        {
            _RenderOrderCollection = lists.ElementAt(0);
            _CalculateOrderCollection = lists.ElementAt(1);
        }
        void IRelationableElement<PmmModel>.RemoveRelation()
        {
            _RenderOrderCollection = null;
            _CalculateOrderCollection = null;
        }
        /// <summary>
        /// 描画順
        /// <para>get/set 共にモデルが PolygonMovieMaker クラスに属していなければ例外を吐く</para>
        /// <para>PMM クラスにモデルを追加すると追加先の文脈に応じて順序が定まるようになる</para>
        /// <para>複数の PMM クラスで共有すると順序に不整合が発生するのでモデル自体を Clone すること</para>
        /// </summary>
        public byte RenderOrder
        {
            get => IRelationableElement<PmmModel>.GetOrder(this, _RenderOrderCollection, "描画");
            set => IRelationableElement<PmmModel>.SetOrder(this, _RenderOrderCollection, "描画", value);
        }
        /// <summary>
        /// 計算順
        /// <para>get/set 共にモデルが PolygonMovieMaker クラスに属していなければ例外を吐く</para>
        /// <para>PMM クラスにモデルを追加すると追加先の文脈に応じて順序が定まるようになる</para>
        /// <para>複数の PMM クラスで共有すると順序に不整合が発生するので、共有はせずモデル自体を Clone すること</para>
        /// </summary>
        public byte CalculateOrder
        {
            get => IRelationableElement<PmmModel>.GetOrder(this, _CalculateOrderCollection, "計算");
            set => IRelationableElement<PmmModel>.SetOrder(this, _CalculateOrderCollection, "計算", value);
        }
    }
}