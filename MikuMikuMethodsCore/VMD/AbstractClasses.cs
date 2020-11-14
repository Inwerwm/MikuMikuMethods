using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.VMD
{
    public abstract class VocaloidFrame : IVocaloidFrame
    {
        public abstract FrameType FrameType { get; }
        public string Name { get; set; }
        public uint Frame { get; set; }
        public abstract bool IsCameraType { get; }
        public abstract bool IsModelType { get; }

        public int CompareTo(IVocaloidFrame other) => Frame.CompareTo(other.Frame);
        public abstract void Read(BinaryReader reader);
        public abstract void Write(BinaryWriter writer);
    }

    public abstract class VocaloidModelTypeFrame : VocaloidFrame
    {
        public override bool IsCameraType => false;

        public override bool IsModelType => true;
    }

    public abstract class VocaloidCameraTypeFrame : VocaloidFrame
    {
        public override bool IsCameraType => true;

        public override bool IsModelType => false;
    }
}
