using System;

namespace MikuMikuMethods.PMM.Frame
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
    }
}
