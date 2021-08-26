using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM
{
    internal interface IRelationableElement<T> where T : IRelationableElement<T>
    {
        bool RegisteredToPmm();
        internal void AddRelation(List<T> list);
        internal void RemoveRelation();
    }
}
