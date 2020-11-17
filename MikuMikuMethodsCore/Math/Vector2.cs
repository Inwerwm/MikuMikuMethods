using System;
using System.Collections.Generic;
using System.Linq;

namespace MikuMikuMethods.Math
{
    /// <summary>
    /// 二次元ベクトル
    /// </summary>
    class Vector2<T> : VectorBase<T>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Vector2()
        {
            InitializeElements(2);
        }

        /// <summary>
        /// コレクションから生成する
        /// </summary>
        /// <param name="origin">元となるコレクション</param>
        public Vector2(IEnumerable<T> origin)
        {
            InitializeElements(2, origin);
        }

        /// <summary>
        /// 要素を指定して生成する。
        /// </summary>
        public Vector2(T x, T y)
        {
            InitializeElements(new T[] { x, y });
        }

        #region 演算子

        /// <summary>
        /// 加算
        /// </summary>
        public static Vector2<T> operator +(Vector2<T> x, Vector2<T> y) => x.Add(y);
        /// <summary>
        /// 減算
        /// </summary>
        public static Vector2<T> operator -(Vector2<T> x, Vector2<T> y) => x.Subtract(y);

        /// <summary>
        /// 全要素に加算
        /// </summary>
        public static Vector2<T> operator +(Vector2<T> x, dynamic scalar) => x.AddAll(scalar);
        /// <summary>
        /// 全要素に加算
        /// </summary>
        public static Vector2<T> operator +(dynamic scalar, Vector2<T> x) => x.AddAll(scalar);

        /// <summary>
        /// 全要素に減算
        /// </summary>
        public static Vector2<T> operator -(Vector2<T> x, dynamic scalar) => x.SubtractAll(scalar);
        /// <summary>
        /// 全要素に減算
        /// </summary>
        public static Vector2<T> operator -(dynamic scalar, Vector2<T> x) => x.SubtractAllInvert(scalar);

        /// <summary>
        /// スカラー倍
        /// </summary>
        public static Vector2<T> operator *(Vector2<T> x, dynamic scalar) => x.Times(scalar);
        /// <summary>
        /// スカラー倍
        /// </summary>
        public static Vector2<T> operator *(dynamic scalar, Vector2<T> x) => x.Times(scalar);

        /// <summary>
        /// 内積
        /// </summary>
        public static T operator *(Vector2<T> x, Vector2<T> y) => x.Dot(y);

        /// <summary>
        /// <para>外積(クロス積)</para>
        /// <para>第3次元を初期値で充填した3次元ベクトルとして計算する</para>
        /// </summary>
        public static Vector3<T> operator ^(Vector2<T> x, Vector2<T> y) => x.Cross(y);

        /// <summary>
        /// 全要素に除算
        /// </summary>
        public static Vector2<T> operator /(Vector2<T> x, dynamic scalar) => x.DivideAll(scalar);
        /// <summary>
        /// 全要素に減算
        /// </summary>
        public static Vector2<T> operator /(dynamic scalar, Vector2<T> x) => x.DivideAllInvert(scalar);

        /// <summary>
        /// 全要素に剰余算
        /// </summary>
        public static Vector2<T> operator %(Vector2<T> x, dynamic scalar) => x.ModAll(scalar);
        /// <summary>
        /// 全要素に剰余算
        /// </summary>
        public static Vector2<T> operator %(dynamic scalar, Vector2<T> x) => x.ModAllInvert(scalar);

        #endregion

        #region ベクトル演算
        /// <summary>
        /// 加算
        /// </summary>
        public Vector2<T> Add(Vector2<T> other) => new Vector2<T>(this.Zip(other, (x, y) => (T)((dynamic)x + y)));

        /// <summary>
        /// 減算
        /// </summary>
        public Vector2<T> Subtract(Vector2<T> other) => new Vector2<T>(this.Zip(other, (x, y) => (T)((dynamic)x - y)));

        /// <summary>
        /// 全要素に加算
        /// </summary>
        public new Vector2<T> AddAll(dynamic scalar) => new Vector2<T>(this.Select(e => (T)(e + scalar)));

        /// <summary>
        /// 全要素に減算
        /// </summary>
        public new Vector2<T> SubtractAll(dynamic scalar) => new Vector2<T>(this.Select(e => (T)(e - scalar)));
        /// <summary>
        /// 全要素に逆順で減算
        /// </summary>
        public new Vector2<T> SubtractAllInvert(dynamic scalar) => new Vector2<T>(this.Select(e => (T)(scalar - e)));

        /// <summary>
        /// スカラー倍
        /// </summary>
        public new Vector2<T> Times(dynamic scalar) => new Vector2<T>(this.Select(e => (T)(e * scalar)));

        /// <summary>
        /// 内積
        /// </summary>
        public T Dot(Vector2<T> other) => this.Zip(other, (x, y) => (dynamic)x * y).Aggregate((sum, elm) => sum + elm);

        /// <summary>
        /// <para>外積(クロス積)</para>
        /// <para>第3次元を初期値で充填した3次元ベクトルとして計算する</para>
        /// </summary>
        public Vector3<T> Cross(Vector2<T> other)
        {
            Vector3<T> x = new(this);
            Vector3<T> y = new(other);

            return new Vector3<T>(
                    (dynamic)x[1] * y[2] - (dynamic)x[2] * y[1],
                    (dynamic)x[2] * y[0] - (dynamic)x[0] * y[2],
                    (dynamic)x[0] * y[1] - (dynamic)x[1] * y[0]
                );
        }

        /// <summary>
        /// 全要素に除算
        /// </summary>
        public new Vector2<T> DivideAll(dynamic scalar) => new Vector2<T>(this.Select(e => (T)(e / scalar)));
        /// <summary>
        /// 全要素に逆順で除算
        /// </summary>
        public new Vector2<T> DivideAllInvert(dynamic scalar) => new Vector2<T>(this.Select(e => (T)(scalar / e)));

        /// <summary>
        /// 全要素に剰余算
        /// </summary>
        public new Vector2<T> ModAll(dynamic scalar) => new Vector2<T>(this.Select(e => (T)(e % scalar)));
        /// <summary>
        /// 全要素に逆順で剰余算
        /// </summary>
        public new Vector2<T> ModAllInvert(dynamic scalar) => new Vector2<T>(this.Select(e => (T)(scalar % e)));

        #endregion
    }
}
