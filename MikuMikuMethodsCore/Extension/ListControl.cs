using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.Extension
{
    internal static class ListControl
    {
        public static void Move<T>(this IList<T> list, int index, T item) where T : class
        {
            if (list == null) return;
            if (!list.Any()) return;
            if(list[index] == item) return;

            if (list.Remove(item))
                list.Insert(index, item);
        }
    }
}
