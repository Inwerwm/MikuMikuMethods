using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.Math
{
    /// <summary>
    /// 三次元ベクトル
    /// </summary>
    class Vector3<T>:VectorBase<T>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Vector3()
        {
            InitializeElements(3);
        }

        /// <summary>
        /// コレクションから生成する
        /// </summary>
        /// <param name="origin">元となるコレクション</param>
        public Vector3(IEnumerable<T> origin)
        {
            InitializeElements(3, origin);
        }

        /// <summary>
        /// 要素を指定して生成する。
        /// </summary>
        public Vector3(T x,T y,T z)
        {
            InitializeElements(new T[] { x, y, z });
        }

        #region 演算子

        /// <summary>
        /// 加算
        /// </summary>
        public static Vector3<T> operator +(Vector3<T> x, Vector3<T> y) => x.Add(y);
        /// <summary>
        /// 減算
        /// </summary>
        public static Vector3<T> operator -(Vector3<T> x, Vector3<T> y) => x.Subtract(y);

        /// <summary>
        /// 全要素に加算
        /// </summary>
        public static Vector3<T> operator +(Vector3<T> x, dynamic scalar) => x.AddAll(scalar);
        /// <summary>
        /// 全要素に加算
        /// </summary>
        public static Vector3<T> operator +(dynamic scalar, Vector3<T> x) => x.AddAll(scalar);

        /// <summary>
        /// 全要素に減算
        /// </summary>
        public static Vector3<T> operator -(Vector3<T> x, dynamic scalar) => x.SubtractAll(scalar);
        /// <summary>
        /// 全要素に減算
        /// </summary>
        public static Vector3<T> operator -(dynamic scalar, Vector3<T> x) => x.SubtractAllInvert(scalar);

        /// <summary>
        /// スカラー倍
        /// </summary>
        public static Vector3<T> operator *(Vector3<T> x, dynamic scalar) => x.Times(scalar);
        /// <summary>
        /// スカラー倍
        /// </summary>
        public static Vector3<T> operator *(dynamic scalar, Vector3<T> x) => x.Times(scalar);

        /// <summary>
        /// 内積
        /// </summary>
        public static T operator *(Vector3<T> x, Vector3<T> y) => x.Dot(y);

        /// <summary>
        /// <para>外積(クロス積)</para>
        /// <para>二項で次元数が異なる場合、小さい方の次元を大きい方に合わせ、初期値を充填する</para>
        /// <para>1,3,7次元ではクロス積を計算する</para>
        /// <para>2,4-6次元では不足要素に初期値を充填した3,7次元ベクトルとみなしクロス積を計算する</para>
        /// <para>8次元以上では例外を吐く</para>
        /// </summary>
        public static Vector3<T> operator ^(Vector3<T> x, Vector3<T> y) => x.Cross(y);

        /// <summary>
        /// 全要素に除算
        /// </summary>
        public static Vector3<T> operator /(Vector3<T> x, dynamic scalar) => x.DivideAll(scalar);
        /// <summary>
        /// 全要素に減算
        /// </summary>
        public static Vector3<T> operator /(dynamic scalar, Vector3<T> x) => x.DivideAllInvert(scalar);

        /// <summary>
        /// 全要素に剰余算
        /// </summary>
        public static Vector3<T> operator %(Vector3<T> x, dynamic scalar) => x.ModAll(scalar);
        /// <summary>
        /// 全要素に剰余算
        /// </summary>
        public static Vector3<T> operator %(dynamic scalar, Vector3<T> x) => x.ModAllInvert(scalar);

        #endregion

        #region ベクトル演算
        /// <summary>
        /// 加算
        /// </summary>
        public Vector3<T> Add(Vector3<T> other) => new Vector3<T>(this.Zip(other, (x, y) => (T)((dynamic)x + y)));

        /// <summary>
        /// 減算
        /// </summary>
        public Vector3<T> Subtract(Vector3<T> other) => new Vector3<T>(this.Zip(other, (x, y) => (T)((dynamic)x - y)));

        /// <summary>
        /// 全要素に加算
        /// </summary>
        public new Vector3<T> AddAll(dynamic scalar) => new Vector3<T>(this.Select(e => (T)(e + scalar)));

        /// <summary>
        /// 全要素に減算
        /// </summary>
        public new Vector3<T> SubtractAll(dynamic scalar) => new Vector3<T>(this.Select(e => (T)(e - scalar)));
        /// <summary>
        /// 全要素に逆順で減算
        /// </summary>
        public new Vector3<T> SubtractAllInvert(dynamic scalar) => new Vector3<T>(this.Select(e => (T)(scalar - e)));

        /// <summary>
        /// スカラー倍
        /// </summary>
        public new Vector3<T> Times(dynamic scalar) => new Vector3<T>(this.Select(e => (T)(e * scalar)));

        /// <summary>
        /// 内積
        /// </summary>
        public T Dot(Vector3<T> other) => this.Zip(other, (x, y) => (dynamic)x * y).Aggregate((sum, elm) => sum + elm);

        /// <summary>
        /// 外積(クロス積)
        /// </summary>
        public Vector3<T> Cross(Vector3<T> other) => new Vector3<T>(
                    (dynamic)this[1] * other[2] - (dynamic)this[2] * other[1],
                    (dynamic)this[2] * other[0] - (dynamic)this[0] * other[2],
                    (dynamic)this[0] * other[1] - (dynamic)this[1] * other[0]
                   );

        /// <summary>
        /// 全要素に除算
        /// </summary>
        public new Vector3<T> DivideAll(dynamic scalar) => new Vector3<T>(this.Select(e => (T)(e / scalar)));
        /// <summary>
        /// 全要素に逆順で除算
        /// </summary>
        public new Vector3<T> DivideAllInvert(dynamic scalar) => new Vector3<T>(this.Select(e => (T)(scalar / e)));

        /// <summary>
        /// 全要素に剰余算
        /// </summary>
        public new Vector3<T> ModAll(dynamic scalar) => new Vector3<T>(this.Select(e => (T)(e % scalar)));
        /// <summary>
        /// 全要素に逆順で剰余算
        /// </summary>
        public new Vector3<T> ModAllInvert(dynamic scalar) => new Vector3<T>(this.Select(e => (T)(scalar % e)));

        #endregion
    }
}
