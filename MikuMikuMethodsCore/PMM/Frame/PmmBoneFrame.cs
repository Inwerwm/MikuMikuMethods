using MikuMikuMethods.Pmm.ElementState;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MikuMikuMethods.Pmm.Frame
{
    public class PmmBoneFrame : PmmBoneState, IPmmFrame
    {
        public int Frame { get; set; } = 0;
        public bool IsSelected { get; set; } = false;

        /// <summary>
        /// 補間曲線
        /// </summary>
        public ReadOnlyDictionary<InterpolationItem, InterpolationCurve> InterpolationCurves { get; private init; } = new(new Dictionary<InterpolationItem, InterpolationCurve>()
        {
            { InterpolationItem.XPosition, new() },
            { InterpolationItem.YPosition, new() },
            { InterpolationItem.ZPosition, new() },
            { InterpolationItem.Rotation, new() },
        });

        public PmmBoneFrame DeepCopy() => new()
        {
            Frame = Frame,
            IsSelected = IsSelected,
            EnablePhysic = EnablePhysic,
            Rotation = Rotation,
            Movement = Movement,
            InterpolationCurves = new(new Dictionary<InterpolationItem, InterpolationCurve>()
            {
                { InterpolationItem.XPosition, InterpolationCurves[InterpolationItem.XPosition].Clone() as InterpolationCurve },
                { InterpolationItem.YPosition, InterpolationCurves[InterpolationItem.YPosition].Clone() as InterpolationCurve },
                { InterpolationItem.ZPosition, InterpolationCurves[InterpolationItem.ZPosition].Clone() as InterpolationCurve },
                { InterpolationItem.Rotation, InterpolationCurves[InterpolationItem.Rotation].Clone() as InterpolationCurve },
            })
        };

        IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();

        public override string ToString() => Frame.ToString();
    }
}
