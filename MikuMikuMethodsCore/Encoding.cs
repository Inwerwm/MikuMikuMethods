using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods
{
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
    }
}
