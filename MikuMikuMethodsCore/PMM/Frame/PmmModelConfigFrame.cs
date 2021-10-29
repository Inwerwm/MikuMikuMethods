using MikuMikuMethods.PMM.ElementState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM.Frame
{
    public class PmmModelConfigFrame : PmmModelConfigState,IPmmFrame
    {
        public int Frame { get; set; }
        public bool IsSelected { get; set; }
        public PmmModel Parent { get; set; }

        public bool Equals(IPmmFrame other) =>
            other is PmmModelConfigFrame f && f.Parent == Parent && f.Frame == Frame;
    }
}
