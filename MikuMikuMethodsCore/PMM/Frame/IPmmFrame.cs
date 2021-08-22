using System;

namespace MikuMikuMethods.PMM.Frame
{
    public interface IPmmFrame : IEquatable<IPmmFrame>
    {
        /// <summary>
        /// フレーム位置
        /// </summary>
        int Position { get; set; }
        /// <summary>
        /// 選択状態か
        /// </summary>
        bool IsSelected { get; set; }

        // Equals では List 内で重複していたら困る情報を比較するよう実装する
        // それにより Distinct を実行するだけで重複を除去できるようにする
    }
}
