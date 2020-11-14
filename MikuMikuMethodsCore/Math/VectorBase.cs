using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MikuMikuMethods.Math
{
    /// <summary>
    /// ベクトル計算
    /// </summary>
    public abstract class VectorBase<T> : IEnumerable<T>
    {
        /// <summary>
        /// 実体
        /// </summary>
        private T[] elements;

        /// <summary>
        /// 次元数
        /// </summary>
        public int Dimension => elements.Length;

        /// <summary>
        /// 添字により要素を指定
        /// </summary>
        /// <param name="dimension">取得する次元</param>
        /// <returns>要素</returns>
        public T this[int dimension] { get => elements[dimension]; set => elements[dimension] = value; }

        #region 演算子

        /// <summary>
        /// 加算
        /// </summary>
        public static VectorBase<T> operator +(VectorBase<T> x, VectorBase<T> y) => x.Add(y);
        /// <summary>
        /// 減算
        /// </summary>
        public static VectorBase<T> operator -(VectorBase<T> x, VectorBase<T> y) => x.Subtract(y);

        /// <summary>
        /// 全要素に加算
        /// </summary>
        public static VectorBase<T> operator +(VectorBase<T> x, dynamic scalar) => x.AddAll(scalar);
        /// <summary>
        /// 全要素に加算
        /// </summary>
        public static VectorBase<T> operator +(dynamic scalar, VectorBase<T> x) => x.AddAll(scalar);

        /// <summary>
        /// 全要素に減算
        /// </summary>
        public static VectorBase<T> operator -(VectorBase<T> x, dynamic scalar) => x.SubtractAll(scalar);
        /// <summary>
        /// 全要素に減算
        /// </summary>
        public static VectorBase<T> operator -(dynamic scalar, VectorBase<T> x) => x.SubtractAllInvert(scalar);

        /// <summary>
        /// スカラー倍
        /// </summary>
        public static VectorBase<T> operator *(VectorBase<T> x, dynamic scalar) => x.Times(scalar);
        /// <summary>
        /// スカラー倍
        /// </summary>
        public static VectorBase<T> operator *(dynamic scalar, VectorBase<T> x) => x.Times(scalar);

        /// <summary>
        /// 内積
        /// </summary>
        public static T operator *(VectorBase<T> x, VectorBase<T> y) => x.Dot(y);

        /// <summary>
        /// <para>外積(クロス積)</para>
        /// <para>二項で次元数が異なる場合、小さい方の次元を大きい方に合わせ、初期値を充填する</para>
        /// <para>1,3,7次元ではクロス積を計算する</para>
        /// <para>2,4-6次元では不足要素に初期値を充填した3,7次元ベクトルとみなしクロス積を計算する</para>
        /// <para>8次元以上では例外を吐く</para>
        /// </summary>
        public static VectorBase<T> operator ^(VectorBase<T> x, VectorBase<T> y) => x.Cross(y);

        /// <summary>
        /// 全要素に除算
        /// </summary>
        public static VectorBase<T> operator /(VectorBase<T> x, dynamic scalar) => x.DivideAll(scalar);
        /// <summary>
        /// 全要素に減算
        /// </summary>
        public static VectorBase<T> operator /(dynamic scalar, VectorBase<T> x) => x.DivideAllInvert(scalar);

        /// <summary>
        /// 全要素に剰余算
        /// </summary>
        public static VectorBase<T> operator %(VectorBase<T> x, dynamic scalar) => x.ModAll(scalar);
        /// <summary>
        /// 全要素に剰余算
        /// </summary>
        public static VectorBase<T> operator %(dynamic scalar, VectorBase<T> x) => x.ModAllInvert(scalar);

        #endregion

        #region ベクトル演算
        /// <summary>
        /// 加算
        /// </summary>
        public VectorBase<T> Add(VectorBase<T> other)
        {
            return new Vector<T>(elements.Zip(other, (x, y) => (T)((dynamic)x + y)));
        }

        /// <summary>
        /// 減算
        /// </summary>
        public VectorBase<T> Subtract(VectorBase<T> other)
        {
            return new Vector<T>(elements.Zip(other, (x, y) => (T)((dynamic)x - y)));
        }

        /// <summary>
        /// 全要素に加算
        /// </summary>
        public VectorBase<T> AddAll(dynamic scalar) => new Vector<T>(this.Select(e => (T)(e + scalar)));

        /// <summary>
        /// 全要素に減算
        /// </summary>
        public VectorBase<T> SubtractAll(dynamic scalar) => new Vector<T>(this.Select(e => (T)(e - scalar)));
        /// <summary>
        /// 全要素に逆順で減算
        /// </summary>
        public VectorBase<T> SubtractAllInvert(dynamic scalar) => new Vector<T>(this.Select(e => (T)(scalar - e)));

        /// <summary>
        /// スカラー倍
        /// </summary>
        public VectorBase<T> Times(dynamic scalar) => new Vector<T>(this.Select(e => (T)(e * scalar)));

        /// <summary>
        /// 内積
        /// </summary>
        public T Dot(VectorBase<T> other)
        {
            if (Dimension != other.Dimension)
                throw new ArgumentException("The inner product can only be calculated between vectors with the same number of dimensions.");

            return elements.Zip(other, (x, y) => (dynamic)x * y).Aggregate((sum, elm) => sum + elm);
        }

        /// <summary>
        /// <para>外積(クロス積)</para>
        /// <para>二項で次元数が異なる場合、小さい方の次元を大きい方に合わせ、初期値を充填する</para>
        /// <para>1,3,7次元ではクロス積を計算する</para>
        /// <para>2,4-6次元では不足要素に初期値を充填した3,7次元ベクトルとみなしクロス積を計算する</para>
        /// <para>8次元以上では例外を吐く</para>
        /// </summary>
        public VectorBase<T> Cross(VectorBase<T> other)
        {
            int maxDimension = Dimension > other.Dimension ? Dimension : other.Dimension;

            if (maxDimension > 7)
                throw new ArgumentOutOfRangeException("It is impossible to calculate the cross product in more than 8 dimensions.");

            if (maxDimension == 1)
                return new Vector<T>(1);

            int dim = maxDimension <= 3 ? 3 : 7;
            Vector<T> x = new(dim, this);
            Vector<T> y = new(dim, other);

            return dim switch
            {
                3 => new Vector3<T>(
                    (dynamic)x[1] * y[2] - (dynamic)x[2] * y[1],
                    (dynamic)x[2] * y[0] - (dynamic)x[0] * y[2], 
                    (dynamic)x[0] * y[1] - (dynamic)x[1] * y[0]
                ),
                7 => new Vector<T>(new T[] {
                        (dynamic)x[1] * y[2] - (dynamic)x[2] * y[1] + (dynamic)x[3] * y[4] - (dynamic)x[4] * y[3] + (dynamic)x[6] * y[5] - (dynamic)x[5] * y[6],
                        (dynamic)x[2] * y[0] - (dynamic)x[0] * y[2] + (dynamic)x[3] * y[5] - (dynamic)x[5] * y[3] + (dynamic)x[4] * y[6] - (dynamic)x[6] * y[4],
                        (dynamic)x[0] * y[1] - (dynamic)x[1] * y[0] + (dynamic)x[3] * y[6] - (dynamic)x[6] * y[3] + (dynamic)x[5] * y[4] - (dynamic)x[4] * y[5],
                        (dynamic)x[4] * y[0] - (dynamic)x[0] * y[4] + (dynamic)x[5] * y[1] - (dynamic)x[1] * y[5] + (dynamic)x[6] * y[2] - (dynamic)x[2] * y[6],
                        (dynamic)x[0] * y[3] - (dynamic)x[3] * y[0] + (dynamic)x[6] * y[1] - (dynamic)x[1] * y[6] + (dynamic)x[2] * y[5] - (dynamic)x[5] * y[2],
                        (dynamic)x[0] * y[6] - (dynamic)x[6] * y[0] + (dynamic)x[1] * y[3] - (dynamic)x[3] * y[1] + (dynamic)x[4] * y[2] - (dynamic)x[2] * y[4],
                        (dynamic)x[5] * y[0] - (dynamic)x[0] * y[5] + (dynamic)x[1] * y[4] - (dynamic)x[4] * y[1] + (dynamic)x[2] * y[3] - (dynamic)x[3] * y[2]
                    }
                ),
                _ => null
            };
        }

        /// <summary>
        /// 全要素に除算
        /// </summary>
        public VectorBase<T> DivideAll(dynamic scalar) => new Vector<T>(this.Select(e => (T)(e / scalar)));
        /// <summary>
        /// 全要素に逆順で除算
        /// </summary>
        public VectorBase<T> DivideAllInvert(dynamic scalar) => new Vector<T>(this.Select(e => (T)(scalar / e)));

        /// <summary>
        /// 全要素に剰余算
        /// </summary>
        public VectorBase<T> ModAll(dynamic scalar) => new Vector<T>(this.Select(e => (T)(e % scalar)));
        /// <summary>
        /// 全要素に逆順で剰余算
        /// </summary>
        public VectorBase<T> ModAllInvert(dynamic scalar) => new Vector<T>(this.Select(e => (T)(scalar % e)));

        #endregion

        #region InitializeElement Methods

        /// <summary>
        /// 要素の初期化
        /// </summary>
        /// <param name="dimension"></param>
        protected void InitializeElements(int dimension)
        {
            if (dimension < 1)
                throw new ArgumentOutOfRangeException("Dimension of Vector require at least 1.");
            elements = new T[dimension];
        }

        /// <summary>
        /// コレクションから生成する
        /// </summary>
        /// <param name="origin">元となるコレクション</param>
        protected void InitializeElements(IEnumerable<T> origin)
        {
            elements = origin.ToArray();
        }

        /// <summary>
        /// <para>コレクションから生成する</para>
        /// <para>次元数が元より少なければ余った要素は捨てる</para>
        /// <para>次元数が元より多ければ不足した要素は初期値で埋める</para>
        /// </summary>
        /// <param name="dimension">次元数</param>
        /// <param name="origin">元となるコレクション</param>
        public void InitializeElements(int dimension, IEnumerable<T> origin)
        {
            InitializeElements(dimension);
            origin.Take(Dimension).ToArray().CopyTo(elements, 0);
        }

        #endregion


        #region IEnumerable<T>
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)elements).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => elements.GetEnumerator();
        #endregion
    }
}
