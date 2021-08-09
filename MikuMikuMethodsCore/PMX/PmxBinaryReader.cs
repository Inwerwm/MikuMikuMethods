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
        public PmxModel ReadModel(string filePath)
        {
            using (FileStream stream = new(filePath, FileMode.Open))
            using (BinaryReader reader = new(stream))
            {
                var model = new PmxModel();

                ReadHeader(reader, model.Header);
                ReadInfo(reader, model.ModelInfo, model.Header.Encoding);

                AddDataToList(model.Vertices, ReadVertex);
                AddDataToList(model.Faces, ReadFace);
                AddDataToList(model.Textures, ReadTexture);
                AddDataToList(model.Materials, ReadMaterial);
                AddDataToList(model.Bones, ReadBone);
                AddDataToList(model.Morphs, ReadMorph);
                AddDataToList(model.Nodes, ReadNode);
                AddDataToList(model.Bodies, ReadBody);
                AddDataToList(model.Joints, ReadJoint);

                return model;

                void AddDataToList<T>(IList<T> list, Func<BinaryReader, T> dataReader)
                {
                    int count = reader.ReadInt32();
                    foreach (var item in Enumerable.Range(0, count).Select(_ => dataReader(reader)))
                    {
                        list.Add(item);
                    }
                }
            }
        }

        private void ReadHeader(BinaryReader reader, PmxHeader header)
        {
            throw new NotImplementedException();
        }

        private void ReadInfo(BinaryReader reader, PmxModelInfo modelInfo, System.Text.Encoding encoding)
        {
            throw new NotImplementedException();
        }

        private PmxVertex ReadVertex(BinaryReader arg)
        {
            throw new NotImplementedException();
        }

        private PmxFace ReadFace(BinaryReader arg)
        {
            throw new NotImplementedException();
        }

        private PmxTexture ReadTexture(BinaryReader arg)
        {
            throw new NotImplementedException();
        }

        private PmxMaterial ReadMaterial(BinaryReader arg)
        {
            throw new NotImplementedException();
        }

        private PmxBone ReadBone(BinaryReader arg)
        {
            throw new NotImplementedException();
        }

        private PmxMorph ReadMorph(BinaryReader arg)
        {
            throw new NotImplementedException();
        }

        private PmxNode ReadNode(BinaryReader arg)
        {
            throw new NotImplementedException();
        }

        private PmxBody ReadBody(BinaryReader arg)
        {
            throw new NotImplementedException();
        }

        private PmxJoint ReadJoint(BinaryReader arg)
        {
            throw new NotImplementedException();
        }
    }
}
