namespace MikuMikuMethods.PMX
{
    /// <summary>
    /// 材質モーフのオフセット
    /// </summary>
    public class PmxOffsetMaterial : IPmxOffset
    {
        /// <summary>
        /// 対象材質
        /// </summary>
        public PmxMaterial Target { get; set; }
        /// <summary>
        /// 演算形式
        /// </summary>
        public OperationType Operation { get; set; }

        /// <summary>
        /// 拡散色
        /// </summary>
        public ColorF Diffuse { get; set; }
        /// <summary>
        /// 反射色
        /// </summary>
        public ColorF Specular { get; set; }
        /// <summary>
        /// 環境色
        /// </summary>
        public ColorF Ambient { get; set; }
        /// <summary>
        /// 反射強度
        /// </summary>
        public float ReflectionIntensity { get; set; }

        /// <summary>
        /// エッジ色
        /// </summary>
        public ColorF EdgeColor { get; set; }
        /// <summary>
        /// エッジ太さ
        /// </summary>
        public float EdgeWidth { get; set; }

        /// <summary>
        /// テクスチャ係数
        /// </summary>
        public ColorF TextureRatio { get; set; }
        /// <summary>
        /// スフィア係数
        /// </summary>
        public ColorF SphereRatio { get; set; }
        /// <summary>
        /// トゥーン係数
        /// </summary>
        public ColorF ToonRatio { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmxOffsetMaterial(OperationType operation)
        {
            Operation = operation;

            switch (Operation)
            {
                case OperationType.Multiply:
                    Diffuse = ColorF.FromARGB(1, 1, 1, 1);
                    Specular = ColorF.FromARGB(1, 1, 1);
                    ReflectionIntensity = 1;
                    Ambient = ColorF.FromARGB(1, 1, 1);
                    EdgeColor = ColorF.FromARGB(1, 1, 1, 1);
                    EdgeWidth = 1;
                    TextureRatio = ColorF.FromARGB(1, 1, 1, 1);
                    SphereRatio = ColorF.FromARGB(1, 1, 1, 1);
                    ToonRatio = ColorF.FromARGB(1, 1, 1, 1);
                    break;
                case OperationType.Addition:
                default:
                    break;
            }
        }

        /// <summary>
        /// 演算形式
        /// </summary>
        public enum OperationType : byte
        {
            /// <summary>
            /// 乗算
            /// </summary>
            Multiply,
            /// <summary>
            /// 加算
            /// </summary>
            Addition,
        }

        public override string ToString() => $"{Target.Name} : {Operation}";
    }
}
