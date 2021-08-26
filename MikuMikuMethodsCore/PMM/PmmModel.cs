using System;
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

        public bool RegisteredToPmm => _RenderOrderCollection?.Contains(this) ?? false;

        /// <summary>
        /// 描画順
        /// <para>get/set 共に PolygonMovieMaker クラスに属していなければ例外を吐く</para>
        /// </summary>
        public byte RenderOrder
        {
            get => RegisteredToPmm ? (byte)_RenderOrderCollection.IndexOf(this) : throw new InvalidOperationException("描画順操作は PolygonMovieMaker クラスに登録されていなければできません。");
            set
            {
                if (!RegisteredToPmm)
                {
                    throw new InvalidOperationException("描画順操作は PolygonMovieMaker クラスに登録されていなければできません。");
                }

                var oldIndex = _RenderOrderCollection.IndexOf(this);
                _RenderOrderCollection.Remove(this);
                try
                {
                    _RenderOrderCollection.Insert(value, this);
                }
                catch (Exception)
                {
                    _RenderOrderCollection.Insert(oldIndex, this);
                    throw;
                }
            }
        }

        /// <summary>
        /// 描画順
        /// <para>get/set 共に PolygonMovieMaker クラスに属していなければ例外を吐く</para>
        /// </summary>
        public byte CalculateOrder
        {
            get => RegisteredToPmm ? (byte)_CalculateOrderCollection.IndexOf(this) : throw new InvalidOperationException("計算順操作は PolygonMovieMaker クラスに登録されていなければできません。");
            set
            {
                if (!RegisteredToPmm)
                {
                    throw new InvalidOperationException("計算順操作は PolygonMovieMaker クラスに登録されていなければできません。");
                }

                var oldIndex = _CalculateOrderCollection.IndexOf(this);
                _CalculateOrderCollection.Remove(this);
                try
                {
                    _CalculateOrderCollection.Insert(value, this);
                }
                catch (Exception)
                {
                    _CalculateOrderCollection.Insert(oldIndex, this);
                    throw;
                }
            }
        }
    }
}