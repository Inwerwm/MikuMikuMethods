using MikuMikuMethods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM
{
    public class PolygonMovieMaker
    {
        public string Version { get; internal set; }
        public Size OutputResolution { get; set; }

        public PmmCamera Camera { get; }
        public PmmLight Light { get; }
        public PmmSelfShadow SelfShadow { get; }
        public PmmPhysics Physics { get; }
        public List<PmmAccessory> Accessories { get; }
        public List<PmmModel> Models { get; }
        
        public PmmMedia Media { get; }

        public PmmRenderPane RenderPane { get; }
        public PmmKeyFramePane KeyFramePane { get; }
        public PmmPanelPane PanelPane { get; }
    }
}
