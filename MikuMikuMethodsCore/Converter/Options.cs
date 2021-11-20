using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.Converter;

public sealed record ExtractCameraMotionOptions(bool ExtractCamera = true, bool ExtractLight = false, bool ExtractShadow = false, uint StartFrame = 0, uint? EndFrame = null);
