using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.Mme.Element;
internal class EmmObjectReference : EmmObject
{
    public EmmObject Reference { get; }
    public string ReferenceName { get; }

    public override string Name => $"{Reference.Name}@{ReferenceName}";

    public EmmObjectReference(EmmObject reference, string referenceName) : base(reference.Index, reference.Path)
    {
        Reference = reference;
        ReferenceName = referenceName;
    }
}
