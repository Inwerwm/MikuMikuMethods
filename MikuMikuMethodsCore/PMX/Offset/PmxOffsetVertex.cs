using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMX
{
    /// <summary>
    /// 頂点モーフ
    /// </summary>
    public class PmxOffsetVertex : IPmxOffset
    {
        /// <summary>
        /// 対象頂点
        /// </summary>
        public PmxVertex Target { get; set; }

        /// <summary>
        /// 移動量
        /// </summary>
        public Vector3 Offset { get; set; }

        public void Read(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public void Write(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
