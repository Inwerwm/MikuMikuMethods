using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.VMD
{
    /// <summary>
    /// カメラフレーム
    /// </summary>
    public class VocaloidCameraFrame : VocaloidCameraTypeFrame
    {
        public override FrameType FrameType => VMD.FrameType.Camera;

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
    /// 照明フレーム
    /// </summary>
    public class VocaloidLightFrame : VocaloidCameraTypeFrame
    {
        public override FrameType FrameType => FrameType.Light;

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
    /// 影フレーム
    /// </summary>
    public class VocaloidShadowFrame : VocaloidCameraTypeFrame
    {
        public override FrameType FrameType => FrameType.Shadow;

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
