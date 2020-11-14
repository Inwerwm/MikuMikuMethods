using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.VMD
{
    /// <summary>
    /// 表示・IKフレーム
    /// </summary>
    class VocaloidPropertyFrame : VocaloidModelTypeFrame
    {
        public override FrameType FrameType => FrameType.Property;

        public override void Read(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public override void Write(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// モーフフレーム
    /// </summary>
    class VocaloidMorphFrame : VocaloidModelTypeFrame
    {
        public override FrameType FrameType => FrameType.Morph;

        public override void Read(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public override void Write(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// モーションフレーム
    /// </summary>
    class VocaloidMotionFrame : VocaloidModelTypeFrame
    {
        public override FrameType FrameType => FrameType.Motion;

        public override void Read(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public override void Write(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
