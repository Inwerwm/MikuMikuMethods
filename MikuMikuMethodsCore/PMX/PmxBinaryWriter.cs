using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMX
{
    public static class PmxBinaryWriter
    {
        public static void WriteModel(string filePath, PmxModel model)
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

        private static void WriteHeader(BinaryWriter writer, PmxHeader header)
        {
            writer.Write(header.FormatName.ToCharArray(), 0, 4);
            writer.Write(header.Version);

            writer.Write(header.ConfigSize);

            writer.Write(header.EncodingFormat);
            writer.Write(header.NumOfAdditionalUV);
            writer.Write(header.SizeOfVertexIndex);
            writer.Write(header.SizeOfTextureIndex);
            writer.Write(header.SizeOfMaterialIndex);
            writer.Write(header.SizeOfBoneIndex);
            writer.Write(header.SizeOfMorphIndex);
            writer.Write(header.SizeOfBodyIndex);
        }

        private static void WriteInfo(BinaryWriter writer, PmxModelInfo modelInfo, System.Text.Encoding encoding)
        {
            throw new NotImplementedException();
        }

        private static void WriteVertex(BinaryWriter arg1, PmxVertex arg2)
        {
            throw new NotImplementedException();
        }

        private static void WriteFace(BinaryWriter arg1, PmxFace arg2)
        {
            throw new NotImplementedException();
        }

        private static void WriteTexture(BinaryWriter arg1, PmxTexture arg2)
        {
            throw new NotImplementedException();
        }

        private static void WriteMaterial(BinaryWriter arg1, PmxMaterial arg2)
        {
            throw new NotImplementedException();
        }

        private static void WriteBone(BinaryWriter arg1, PmxBone arg2)
        {
            throw new NotImplementedException();
        }

        private static void WriteMorph(BinaryWriter arg1, PmxMorph arg2)
        {
            throw new NotImplementedException();
        }

        private static void WriteNode(BinaryWriter arg1, PmxNode arg2)
        {
            throw new NotImplementedException();
        }

        private static void WriteBody(BinaryWriter arg1, PmxBody arg2)
        {
            throw new NotImplementedException();
        }

        private static void WriteJoint(BinaryWriter arg1, PmxJoint arg2)
        {
            throw new NotImplementedException();
        }
    }
}
