using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MikuMikuMethods.PMM.ElementState;

namespace MikuMikuMethods.PMM.Frame
{
    public class PmmCameraFrame : PmmCameraState, IPmmFrame
    {
        public int Frame { get; set; } = 0;
        public bool IsSelected { get; set; } = false;

        /// <summary>
        /// カメラの距離
        /// </summary>
        public float Distance { get; set; } = 45;
        /// <summary>
        /// 補間曲線
        /// </summary>
        public ReadOnlyDictionary<InterpolationItem, InterpolationCurve> InterpolationCurces { get; } = new(new Dictionary<InterpolationItem, InterpolationCurve>()
        {
            { InterpolationItem.XPosition, new() },
            { InterpolationItem.YPosition, new() },
            { InterpolationItem.ZPosition, new() },
            { InterpolationItem.Rotation, new() },
            { InterpolationItem.Distance, new() },
            { InterpolationItem.ViewAngle, new() },
        });
    }
}
