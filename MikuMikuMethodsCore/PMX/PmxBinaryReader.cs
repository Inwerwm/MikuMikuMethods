using MikuMikuMethods.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MikuMikuMethods.PMX
{
    public static class PmxBinaryReader
    {
        private static StringEncoder Encoder;

        private static PmxModel Model { get; set; }
        private static List<int> TmpWeightBoneIndices { get; set; }

        private static void CleanUpProperties()
        {
            Encoder = null;
            Model = null;
            TmpWeightBoneIndices = null;
        }

        public static PmxModel ReadModel(string filePath)
        {
            try
            {
                using (FileStream stream = new(filePath, FileMode.Open))
                using (BinaryReader reader = new(stream))
                {
                    Model = new PmxModel();

                    ReadHeader(reader, Model.Header);
                    Encoder = new StringEncoder(Model.Header.Encoding);

                    ReadInfo(reader, Model.ModelInfo);

                    AddDataToList(Model.Vertices, ReadVertex);
                    AddDataToList(Model.Faces, ReadFace);
                    AddDataToList(Model.Textures, ReadTexture);
                    AddDataToList(Model.Materials, ReadMaterial);
                    AddDataToList(Model.Bones, ReadBone);
                    AddDataToList(Model.Morphs, ReadMorph);
                    AddDataToList(Model.Nodes, ReadNode);
                    AddDataToList(Model.Bodies, ReadBody);
                    AddDataToList(Model.Joints, ReadJoint);

                    return Model;

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
            finally
            {
                CleanUpProperties();
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

        private static void ReadInfo(BinaryReader reader, PmxModelInfo modelInfo)
        {
            modelInfo.Name = Encoder.Read(reader);
            modelInfo.NameEn = Encoder.Read(reader);
            modelInfo.Comment = Encoder.Read(reader);
            modelInfo.CommentEn = Encoder.Read(reader);
        }

        private static PmxVertex ReadVertex(BinaryReader reader)
        {
            PmxVertex vtx = new();

            vtx.Position = reader.ReadVector3();
            vtx.Normal = reader.ReadVector3();
            vtx.UV = reader.ReadVector2();

            vtx.AdditonalUVs = Model.Header.NumOfAdditionalUV <= 0 ? null : Enumerable.Range(0, Model.Header.NumOfAdditionalUV).Select(_ => reader.ReadVector4()).ToArray();
            vtx.WeightType = (PmxWeightType)reader.ReadByte();

            var boneIndexer = new Indexer(Model.Header.SizeOfBoneIndex, false);
            switch (vtx.WeightType)
            {
                case PmxWeightType.BDEF1:
                    ReadBDEF1Weights(reader, boneIndexer);
                    break;
                case PmxWeightType.BDEF2:
                    ReadBDEF2Weights(reader, boneIndexer);
                    break;
                case PmxWeightType.BDEF4:
                    ReadBDEF4Weights(reader, boneIndexer);
                    break;
                case PmxWeightType.SDEF:
                    ReadSDEFWeights(reader, boneIndexer);
                    break;
            }

            vtx.EdgeScale = reader.ReadSingle();

            return vtx;

            // ウェイト読込ローカル関数

            void ReadBDEF1Weights(BinaryReader reader, Indexer indexer)
            {
                TmpWeightBoneIndices.Add(indexer.Read(reader));
                vtx.Weights.Add(new(null, 1.0f));
            }

            void ReadBDEF2Weights(BinaryReader reader, Indexer indexer)
            {
                TmpWeightBoneIndices.Add(indexer.Read(reader));
                TmpWeightBoneIndices.Add(indexer.Read(reader));

                float weight = reader.ReadSingle();
                vtx.Weights.Add(new(null, weight));
                vtx.Weights.Add(new(null, 1 - weight));
            }

            void ReadBDEF4Weights(BinaryReader reader, Indexer indexer)
            {
                TmpWeightBoneIndices.Add(indexer.Read(reader));
                TmpWeightBoneIndices.Add(indexer.Read(reader));
                TmpWeightBoneIndices.Add(indexer.Read(reader));
                TmpWeightBoneIndices.Add(indexer.Read(reader));

                vtx.Weights.Add(new(null, reader.ReadSingle()));
                vtx.Weights.Add(new(null, reader.ReadSingle()));
                vtx.Weights.Add(new(null, reader.ReadSingle()));
                vtx.Weights.Add(new(null, reader.ReadSingle()));
            }

            void ReadSDEFWeights(BinaryReader reader, Indexer indexer)
            {
                TmpWeightBoneIndices.Add(indexer.Read(reader));
                TmpWeightBoneIndices.Add(indexer.Read(reader));

                float weight = reader.ReadSingle();
                vtx.Weights.Add(new(null, weight));
                vtx.Weights.Add(new(null, 1 - weight));

                vtx.SDEF = new(reader.ReadVector3(), reader.ReadVector3(), reader.ReadVector3());
            }
        }

        private static PmxFace ReadFace(BinaryReader reader)
        {
            var vtxIndexer = new Indexer(Model.Header.SizeOfVertexIndex, true);
            return new(Model.Vertices[vtxIndexer.Read(reader)], Model.Vertices[vtxIndexer.Read(reader)], Model.Vertices[vtxIndexer.Read(reader)]);
        }

        private static PmxTexture ReadTexture(BinaryReader reader)
        {
            return new(Encoder.Read(reader));
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
