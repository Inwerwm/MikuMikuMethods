using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMX
{
    public class PmxBinaryWriter
    {
        public void WriteModel(string filePath, PmxModel model)
        {
            using (FileStream file = new(filePath, FileMode.Create))
            using (BinaryWriter writer = new(file))
            {
                WriteHeader(writer, model.Header);
                WriteInfo(writer, model.ModelInfo, model.Header.Encoding);

                WriteData(model.Vertices, WriteVertex);
                WriteData(model.Faces, WriteFace);
                WriteData(model.Textures, WriteTexture);
                WriteData(model.Materials, WriteMaterial);
                WriteData(model.Bones, WriteBone);
                WriteData(model.Morphs, WriteMorph);
                WriteData(model.Nodes, WriteNode);
                WriteData(model.Bodies, WriteBody);
                WriteData(model.Joints, WriteJoint);

                void WriteData<T>(IList<T> list, Action<BinaryWriter, T> DataWriter)
                {
                    writer.Write(list.Count);
                    foreach (var item in list)
                    {
                        DataWriter(writer, item);
                    }
                }
            }
        }

        private void WriteHeader(BinaryWriter writer, PmxHeader header)
        {
            throw new NotImplementedException();
        }

        private void WriteInfo(BinaryWriter writer, PmxModelInfo modelInfo, System.Text.Encoding encoding)
        {
            throw new NotImplementedException();
        }

        private void WriteVertex(BinaryWriter arg1, PmxVertex arg2)
        {
            throw new NotImplementedException();
        }

        private void WriteFace(BinaryWriter arg1, PmxFace arg2)
        {
            throw new NotImplementedException();
        }

        private void WriteTexture(BinaryWriter arg1, PmxTexture arg2)
        {
            throw new NotImplementedException();
        }

        private void WriteMaterial(BinaryWriter arg1, PmxMaterial arg2)
        {
            throw new NotImplementedException();
        }

        private void WriteBone(BinaryWriter arg1, PmxBone arg2)
        {
            throw new NotImplementedException();
        }

        private void WriteMorph(BinaryWriter arg1, PmxMorph arg2)
        {
            throw new NotImplementedException();
        }

        private void WriteNode(BinaryWriter arg1, PmxNode arg2)
        {
            throw new NotImplementedException();
        }

        private void WriteBody(BinaryWriter arg1, PmxBody arg2)
        {
            throw new NotImplementedException();
        }

        private void WriteJoint(BinaryWriter arg1, PmxJoint arg2)
        {
            throw new NotImplementedException();
        }
    }
}
