namespace MikuMikuMethods.MME.Element
{
    /// <summary>
    /// エフェクト対象モデルごと設定でのオブジェクト情報
    /// </summary>
    public class EmdObject : EmmObject
    {
        /// <summary>
        /// オブジェクトのキーを表す文字列
        /// </summary>
        public override string Name => $"Obj";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="index">オブジェクト番号</param>
        public EmdObject() : base(1) { }
    }
}
