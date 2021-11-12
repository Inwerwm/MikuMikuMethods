using System.Collections.Generic;

namespace MikuMikuMethods.Pmx;

/// <summary>
/// 材質
/// </summary>
public class PmxMaterial : IPmxData
{
    /// <summary>
    /// 材質名
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 材質名(英語)
    /// </summary>
    public string NameEn { get; set; }
    /// <summary>
    /// メモ
    /// </summary>
    public string Memo { get; set; }

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
    /// 両面描画
    /// </summary>
    public bool EnableBothSideDraw { get; set; }
    /// <summary>
    /// セルフ影描画
    /// </summary>
    public bool EnableSelfShadow { get; set; }
    /// <summary>
    /// 影マップへの描画
    /// </summary>
    public bool EnableSelfShadowMap { get; set; }
    /// <summary>
    /// 地面影描画
    /// </summary>
    public bool EnableShadow { get; set; }
    /// <summary>
    /// 頂点色有効性
    /// </summary>
    public bool EnableVertexColor { get; set; }

    /// <summary>
    /// エッジ有効
    /// </summary>
    public bool EnableEdge { get; set; }
    /// <summary>
    /// エッジ太さ
    /// </summary>
    public float EdgeWidth { get; set; }
    /// <summary>
    /// エッジ色
    /// </summary>
    public ColorF EdgeColor { get; set; }

    /// <summary>
    /// 面
    /// </summary>
    public List<PmxFace> Faces { get; } = new();
    /// <summary>
    /// 描画プリミティブタイプ
    /// </summary>
    public PrimitiveType Primitive { get; set; }

    /// <summary>
    /// テクスチャ
    /// </summary>
    public PmxTexture Texture { get; set; }

    /// <summary>
    /// スフィアマップ
    /// </summary>
    public PmxTexture SphereMap { get; set; }
    /// <summary>
    /// スフィア種別
    /// </summary>
    public SphereModeType SphereMode { get; set; }

    /// <summary>
    /// トゥーンマップ
    /// </summary>
    public PmxTexture ToonMap { get; set; }

    /// <summary>
    /// 描画プリミティブタイプ
    /// </summary>
    public enum PrimitiveType
    {
        /// <summary>
        /// Line List
        /// </summary>
        Line,
        /// <summary>
        /// Point List
        /// </summary>
        Point,
        /// <summary>
        /// Triangle List
        /// </summary>
        Tri
    }

    /// <summary>
    /// スフィア種別
    /// </summary>
    public enum SphereModeType : byte
    {
        /// <summary>
        /// 無効
        /// </summary>
        None,
        /// <summary>
        /// 乗算
        /// </summary>
        Mul,
        /// <summary>
        /// 加算
        /// </summary>
        Add,
        /// <summary>
        /// サブテクスチャ
        /// </summary>
        SubTex
    }

    /// <summary>
    /// 材質の描画フラグデータ
    /// </summary>
    internal enum DrawFlag : byte
    {
        /// <summary>
        /// 両面描画
        /// </summary>
        BothSideDraw = 0x01,
        /// <summary>
        /// 地面影
        /// </summary>
        Shadow = 0x02,
        /// <summary>
        /// セルフ影マップへの描画
        /// </summary>
        SelfShadowMap = 0x04,
        /// <summary>
        /// セルフ影
        /// </summary>
        SelfShadow = 0x08,
        /// <summary>
        /// エッジ描画
        /// </summary>
        Edge = 0x10,
        /// <summary>
        /// 頂点色
        /// </summary>
        VertexColor = 0x20,
        /// <summary>
        /// Point描画
        /// </summary>
        Point = 0x40,
        /// <summary>
        /// Line描画
        /// </summary>
        Line = 0x80
    }

    public override string ToString() => $"{Name}";
}
