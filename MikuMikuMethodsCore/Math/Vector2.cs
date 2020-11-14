using System.Collections.Generic;

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
    }
}
