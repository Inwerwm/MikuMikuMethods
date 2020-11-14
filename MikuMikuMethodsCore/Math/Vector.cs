using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.Math
{
    /// <summary>
    /// ベクトル
    /// </summary>
    public class Vector<T>:VectorBase<T>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dimension">次元数</param>
        public Vector(int dimension)
        {
            InitializeElements(dimension);
        }

        /// <summary>
        /// コレクションから生成する
        /// </summary>
        /// <param name="origin">元となるコレクション</param>
        public Vector(IEnumerable<T> origin)
        {
            InitializeElements(origin);
        }

        /// <summary>
        /// <para>コレクションから生成する</para>
        /// <para>次元数が元より少なければ余った要素は捨てる</para>
        /// <para>次元数が元より多ければ不足した要素は初期値で埋める</para>
        /// </summary>
        /// <param name="dimension">次元数</param>
        /// <param name="origin">元となるコレクション</param>
        public Vector(int dimension, IEnumerable<T> origin)
        {
            InitializeElements(dimension, origin);
        }
    }
}
