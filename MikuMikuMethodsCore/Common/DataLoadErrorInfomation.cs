using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.Common
{
    public record DataLoadErrorInfomation(string Section, int? Index, string Description)
    {
        internal static string GetOrdinal(int number) => number switch
        {
            int num when num % 100 is 11 or 12 or 13 => $"{number}th",
            int num when num % 10 is 1 => $"{number}st",
            int num when num % 10 is 2 => $"{number}nd",
            int num when num % 10 is 3 => $"{number}rd",
            _ => $"{number}th"
        };
    }
}
