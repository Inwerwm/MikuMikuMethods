using MikuMikuMethods.PMM.ElementState;
using System.Linq;

namespace MikuMikuMethods.PMM.Frame
{
    public class PmmModelConfigFrame : PmmModelConfigState, IPmmFrame
    {
        public int Frame { get; set; }
        public bool IsSelected { get; set; }

        public PmmModelConfigFrame DeepCopy() => new()
        {
            Frame = Frame,
            IsSelected = IsSelected,
            EnableIK = new(EnableIK),
            OuterParent = OuterParent.ToDictionary(p => p.Key, p => p.Value.DeepCopy()),
            Visible = Visible
        };

        IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();

        public override string ToString() => Frame.ToString();
    }
}
