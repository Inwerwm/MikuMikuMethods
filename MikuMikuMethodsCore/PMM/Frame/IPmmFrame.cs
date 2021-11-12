using System;

namespace MikuMikuMethods.Pmm.Frame
{
    public interface IPmmFrame
    {
        /// <summary>
        /// フレーム位置
        /// </summary>
        int Frame { get; set; }
        /// <summary>
        /// 選択状態か
        /// </summary>
        bool IsSelected { get; set; }

        public IPmmFrame DeepCopy();
    }
}
