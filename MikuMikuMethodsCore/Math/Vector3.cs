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
    }
}
