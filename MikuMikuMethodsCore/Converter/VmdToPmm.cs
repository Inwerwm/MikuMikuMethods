using MikuMikuMethods.Pmm;
using MikuMikuMethods.Vmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.Converter;
public static class VmdToPmm
{
    public static void ApplyVmd(this PolygonMovieMaker pmm, VocaloidMotionData cameraVmd)
    {
        if (cameraVmd.Kind != VmdKind.Camera) throw new ArgumentException("The Model VMD was passed as the argument where the Camera VMD was expected.");


    }

    public static void ApplyVmd(this PmmModel model, VocaloidMotionData motionVmd)
    {
        if (motionVmd.Kind != VmdKind.Model) throw new ArgumentException("The Camera VMD was passed as the argument where the Model VMD was expected.");


    }
}
