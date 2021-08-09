using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMX
{
    public class PmxBinaryReader
    {
        public static PmxModel ReadModel(string filePath)
        {
            using (FileStream stream = new(filePath, FileMode.Open))
            using (BinaryReader reader = new(stream))
            {
                var model = new PmxModel();

                Header.Read(reader);
                Encoder = new(Header.EncodingOfModel);

                ModelInfo.Read(reader, Encoder);
                ReadFrames(reader, r => Vertices.Add(new(r)));
                ReadFrames(reader, r => Faces.Add(new(r)));
                ReadFrames(reader, r => Textures.Add(new(reader.ReadString()))); // 仮
                ReadFrames(reader, r => Materials.Add(new(r)));
                ReadFrames(reader, r => Bones.Add(new(r)));
                ReadFrames(reader, r => Morphs.Add(new(r)));
                ReadFrames(reader, r => Nodes.Add(new(r)));
                ReadFrames(reader, r => Bodies.Add(new(r)));
                ReadFrames(reader, r => Joints.Add(new(r)));

                return model;
            }
        }

        private IEnumerable<T> ReadFrames<T>(BinaryReader reader, Func<BinaryReader, T> readData) where T : IPmxData =>
            Enumerable.Range(0, reader.ReadInt32()).Select(_ => readData(reader)).ToArray();
    }
}
