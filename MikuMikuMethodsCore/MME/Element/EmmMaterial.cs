namespace MikuMikuMethods.MME.Element
{
    /// <summary>
    /// 個々の要素に対するエフェクト情報
    /// </summary>
    public class EmmMaterial
    {
        /// <summary>
        /// 表示切り替え
        /// </summary>
        public bool? Show { get; set; } = null;

        /// <summary>
        /// エフェクトの位置
        /// <para>出力時、nullなら書き込まれず、 "none" なら書き込まれる</para>
        /// </summary>
        public string Path { get; set; }
    }
}
