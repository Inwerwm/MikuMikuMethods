using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods
{
    /// <summary>
    /// <para>MMD関係で使用するエンコードのアダプタクラス</para>
    /// <para>Shift JIS がそのままだと使えないので作った</para>
    /// </summary>
    static class Encoding
    {
        private static bool ProviderIsRegistered { get; set; }

        public static System.Text.Encoding ShiftJIS
        {
            get
            {
                if (!ProviderIsRegistered)
                {
                    System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    ProviderIsRegistered = true;
                }
                return System.Text.Encoding.GetEncoding("Shift_JIS");
            }
        }

        public static System.Text.Encoding Unicode => System.Text.Encoding.Unicode;

        public static System.Text.Encoding UTF8 => System.Text.Encoding.UTF8;
    }
}
