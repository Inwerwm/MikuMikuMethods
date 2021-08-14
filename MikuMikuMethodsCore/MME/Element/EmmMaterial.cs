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
        /// </summary>
        public string Path { get; set; }
    }
}
