using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM
{
    internal interface IRelationableElement<T> where T : IRelationableElement<T>
    {
        bool RegisteredToPmm { get; }

        internal void AddRelation(IEnumerable<List<T>> lists);
        internal void RemoveRelation();
    }
}
