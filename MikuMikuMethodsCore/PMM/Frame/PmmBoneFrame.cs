using MikuMikuMethods.PMM.ElementState;
using System.Collections.ObjectModel;

namespace MikuMikuMethods.PMM.Frame
{
    public class PmmBoneFrame : PmmBoneState, IPmmFrame
    {
        public int Frame { get; set; } = 0;
        public bool IsSelected { get; set; } = false;

        /// <summary>
        /// 補間曲線
        /// </summary>
        public ReadOnlyDictionary<InterpolationItem, InterpolationCurve> InterpolationCurces { get; init; }
    }
}
