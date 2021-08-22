using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM.Frame
{
    public interface IPmmFrame
    {
        /// <summary>
        /// フレーム位置
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// 選択状態か
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
