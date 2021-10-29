using MikuMikuMethods.PMM.ElementState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM.Frame
{
    public class PmmBoneFrame: PmmBoneState, IPmmFrame
    {
        public int Frame { get; set; } = 0;
        public bool IsSelected { get; set; } = false;
    }
}
