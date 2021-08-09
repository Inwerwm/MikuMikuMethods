using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMX
{
    public static class PmxBinaryReader
    {
        public static PmxModel ReadModel(string filePath)
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

        private static void ReadHeader(BinaryReader reader, PmxHeader header)
        {
            // "PMX "
            reader.ReadBytes(4);
            var version = reader.ReadSingle();
            if (version < 2.0) throw new FormatException("PMXが非対応バージョンです。バージョン番号が未対応バージョンです。");

            var configSize = reader.ReadByte();
            if (configSize != 8) throw new FormatException("PMXが非対応バージョンです。ヘッダデータが未対応の形式です。");

            var config = reader.ReadBytes(header.ConfigSize);
            header.EncodingFormat = config[0];
            header.NumOfAdditionalUV = config[1];
            header.SizeOfVertexIndex = config[2];
            header.SizeOfTextureIndex = config[3];
            header.SizeOfMaterialIndex = config[4];
            header.SizeOfBoneIndex = config[5];
            header.SizeOfMorphIndex = config[6];
            header.SizeOfBodyIndex = config[7];
        }

        private static void ReadInfo(BinaryReader reader, PmxModelInfo modelInfo, System.Text.Encoding encoding)
        {
            throw new NotImplementedException();
        }

        private static PmxVertex ReadVertex(BinaryReader arg)
        {
            throw new NotImplementedException();
        }

        private static PmxFace ReadFace(BinaryReader arg)
        {
            throw new NotImplementedException();
        }

        private static PmxTexture ReadTexture(BinaryReader arg)
        {
            throw new NotImplementedException();
        }

        private static PmxMaterial ReadMaterial(BinaryReader arg)
        {
            throw new NotImplementedException();
        }

        private static PmxBone ReadBone(BinaryReader arg)
        {
            throw new NotImplementedException();
        }

        private static PmxMorph ReadMorph(BinaryReader arg)
        {
            throw new NotImplementedException();
        }

        private static PmxNode ReadNode(BinaryReader arg)
        {
            throw new NotImplementedException();
        }

        private static PmxBody ReadBody(BinaryReader arg)
        {
            throw new NotImplementedException();
        }

        private static PmxJoint ReadJoint(BinaryReader arg)
        {
            throw new NotImplementedException();
        }
    }
}
