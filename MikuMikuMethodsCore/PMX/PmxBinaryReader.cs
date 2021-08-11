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
        private static List<(PmxWeight Instance, int RelationID)> TmpWeightBoneIndices { get; set; }
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

                    void AddDataToList<T>(List<T> list, Func<BinaryReader, T> dataReader)
                    {
                        int count = reader.ReadInt32();
                        list.AddRange(Enumerable.Range(0, count).Select(_ => dataReader(reader)));
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
                int boneID = indexer.Read(reader);
                PmxWeight weight = new(null, 1.0f);

                vtx.Weights.Add(weight);
                TmpWeightBoneIndices.Add((weight, boneID));
            }

            void ReadBDEF2Weights(BinaryReader reader, Indexer indexer)
            {
                int boneID1 = indexer.Read(reader);
                int boneID2 = indexer.Read(reader);

                float weight = reader.ReadSingle();
                PmxWeight weight1 = new(null, weight);
                PmxWeight weight2 = new(null, 1 - weight);

                vtx.Weights.Add(weight1);
                vtx.Weights.Add(weight2);

                TmpWeightBoneIndices.Add((weight1, boneID1));
                TmpWeightBoneIndices.Add((weight2, boneID2));
            }

            void ReadBDEF4Weights(BinaryReader reader, Indexer indexer)
            {
                int boneID1 = indexer.Read(reader);
                int boneID2 = indexer.Read(reader);
                int boneID3 = indexer.Read(reader);
                int boneID4 = indexer.Read(reader);

                PmxWeight weight1 = new(null, reader.ReadSingle());
                PmxWeight weight2 = new(null, reader.ReadSingle());
                PmxWeight weight3 = new(null, reader.ReadSingle());
                PmxWeight weight4 = new(null, reader.ReadSingle());

                vtx.Weights.Add(weight1);
                vtx.Weights.Add(weight2);
                vtx.Weights.Add(weight3);
                vtx.Weights.Add(weight4);

                TmpWeightBoneIndices.Add((weight1, boneID1));
                TmpWeightBoneIndices.Add((weight2, boneID2));
                TmpWeightBoneIndices.Add((weight3, boneID3));
                TmpWeightBoneIndices.Add((weight4, boneID4));
            }

            void ReadSDEFWeights(BinaryReader reader, Indexer indexer)
            {
                int boneID1 = indexer.Read(reader);
                int boneID2 = indexer.Read(reader);

                float weight = reader.ReadSingle();
                PmxWeight weight1 = new(null, weight);
                PmxWeight weight2 = new(null, 1 - weight);

                vtx.Weights.Add(weight1);
                vtx.Weights.Add(weight2);

                TmpWeightBoneIndices.Add((weight1, boneID1));
                TmpWeightBoneIndices.Add((weight2, boneID2));

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

        private static PmxMaterial ReadMaterial(BinaryReader reader)
        {
            var material = new PmxMaterial();

            material.Name = Encoder.Read(reader);
            material.NameEn = Encoder.Read(reader);

            material.Diffuse = reader.ReadSingleRGBA();
            material.Specular = reader.ReadSingleRGB();
            material.ReflectionIntensity = reader.ReadSingle();
            material.Ambient = reader.ReadSingleRGB();

            var bitFlag = (PmxMaterial.DrawFlag)reader.ReadByte();
            material.EnableBothSideDraw = bitFlag.HasFlag(PmxMaterial.DrawFlag.BothSideDraw);
            material.EnableShadow = bitFlag.HasFlag(PmxMaterial.DrawFlag.Shadow);
            material.EnableSelfShadowMap = bitFlag.HasFlag(PmxMaterial.DrawFlag.SelfShadowMap);
            material.EnableSelfShadow = bitFlag.HasFlag(PmxMaterial.DrawFlag.SelfShadow);
            material.EnableEdge = bitFlag.HasFlag(PmxMaterial.DrawFlag.Edge);

            material.EdgeColor = reader.ReadSingleRGBA();
            material.EdgeWidth = reader.ReadSingle();

            var id = new Indexer(Model.Header.SizeOfTextureIndex, false);
            material.Texture = Model.Textures[id.Read(reader)];
            material.SphereMap = Model.Textures[id.Read(reader)];
            material.SphereMode = (PmxMaterial.SphereModeType)reader.ReadByte();
            var IsSharedToon = reader.ReadBoolean();
            material.ToonMap = IsSharedToon ? new PmxTexture(reader.ReadByte()) : Model.Textures[id.Read(reader)];

            material.Memo = Encoder.Read(reader);

            // 材質に対応する面(頂点)数 (必ず3の倍数になる)
            reader.ReadInt32();

            return material;
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
