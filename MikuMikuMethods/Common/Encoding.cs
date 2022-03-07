using System.Text;

namespace MikuMikuMethods;

/// <summary>
/// <para>MMD関係で使用するエンコード方式</para>
/// <para>Shift JIS はそのままだと使えないので</para>
/// </summary>
public static class Encoding
{
    /// <summary>
    /// ShiftJISを使うための初期設定が済んでいるか
    /// </summary>
    private static bool ProviderIsRegistered { get; set; }

    /// <summary>
    /// 日本語 Shift JIS
    /// </summary>
    public static System.Text.Encoding ShiftJIS
    {
        get
        {
            if (!ProviderIsRegistered)
            {
                // Shift JIS を使うためには初期設定が必要
                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                ProviderIsRegistered = true;
            }
            return System.Text.Encoding.GetEncoding("Shift_JIS");
        }
    }

    /// <summary>
    /// UTF-16
    /// </summary>
    public static System.Text.Encoding Unicode => System.Text.Encoding.Unicode;

    /// <summary>
    /// UTF-8
    /// </summary>
    public static System.Text.Encoding UTF8 => System.Text.Encoding.UTF8;
}
