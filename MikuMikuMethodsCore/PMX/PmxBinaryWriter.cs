using MikuMikuMethods.Extension;
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
        private static StringEncoder Encoder;

        private static PmxModel Model { get; set; }

        private static void CleanUpProperties()
        {
            Encoder = null;
            Model = null;
        }

        public static void WriteModel(string filePath, PmxModel model)
        {
            try
            {
                Model = model;
                Encoder = new StringEncoder(model.Header.Encoding);

                using (FileStream file = new(filePath, FileMode.Create))
                using (BinaryWriter writer = new(file))
                {
                    WriteHeader(writer, model.Header);
                    WriteInfo(writer, model.ModelInfo);

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
            finally
            {
                CleanUpProperties();
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

        private static void WriteInfo(BinaryWriter writer, PmxModelInfo modelInfo)
        {
            Encoder.Write(writer, modelInfo.Name);
            Encoder.Write(writer, modelInfo.NameEn);
            Encoder.Write(writer, modelInfo.Comment);
            Encoder.Write(writer, modelInfo.CommentEn);
        }

        private static void WriteVertex(BinaryWriter writer, PmxVertex vertex)
        {
            writer.Write(vertex.Position);
            writer.Write(vertex.Normal);
            writer.Write(vertex.UV);

            foreach (var item in vertex.AdditonalUVs)
            {
                writer.Write(item);
            }

            writer.Write((byte)vertex.WeightType);

            var boneIdMap = Model.Bones.Select((Bone, Index) => (Bone, Index)).ToDictionary(b => b.Bone, b => b.Index);
            var boneIndexer = new Indexer(Model.Header.SizeOfBoneIndex, false);
            switch (vertex.WeightType)
            {
                case PmxWeightType.BDEF1:
                    WriteBDEF1Weights(writer, boneIndexer, boneIdMap);
                    break;
                case PmxWeightType.BDEF2:
                    WriteBDEF2Weights(writer, boneIndexer, boneIdMap);
                    break;
                case PmxWeightType.BDEF4:
                    WriteBDEF4Weights(writer, boneIndexer, boneIdMap);
                    break;
                case PmxWeightType.SDEF:
                    WriteSDEFWeights(writer, boneIndexer, boneIdMap);
                    break;
            }

            writer.Write(vertex.EdgeScale);

            // ウェイト書込ローカル関数

            void WriteBDEF1Weights(BinaryWriter writer, Indexer boneIndexer, Dictionary<PmxBone, int> boneIdMap)
            {
                boneIndexer.Write(writer, boneIdMap[vertex.Weights[0].Bone]);
            }

            void WriteBDEF2Weights(BinaryWriter writer, Indexer boneIndexer, Dictionary<PmxBone, int> boneIdMap)
            {
                boneIndexer.Write(writer, boneIdMap[vertex.Weights[0].Bone]);
                boneIndexer.Write(writer, boneIdMap[vertex.Weights[1].Bone]);
                writer.Write(vertex.Weights[0].Value);
            }

            void WriteBDEF4Weights(BinaryWriter writer, Indexer boneIndexer, Dictionary<PmxBone, int> boneIdMap)
            {
                boneIndexer.Write(writer, boneIdMap[vertex.Weights[0].Bone]);
                boneIndexer.Write(writer, boneIdMap[vertex.Weights[1].Bone]);
                boneIndexer.Write(writer, boneIdMap[vertex.Weights[2].Bone]);
                boneIndexer.Write(writer, boneIdMap[vertex.Weights[3].Bone]);
                writer.Write(vertex.Weights[0].Value);
                writer.Write(vertex.Weights[1].Value);
                writer.Write(vertex.Weights[2].Value);
                writer.Write(vertex.Weights[3].Value);
            }

            void WriteSDEFWeights(BinaryWriter writer, Indexer boneIndexer, Dictionary<PmxBone, int> boneIdMap)
            {
                boneIndexer.Write(writer, boneIdMap[vertex.Weights[0].Bone]);
                boneIndexer.Write(writer, boneIdMap[vertex.Weights[1].Bone]);
                writer.Write(vertex.Weights[0].Value);

                writer.Write(vertex.SDEF.C);
                writer.Write(vertex.SDEF.R0);
                writer.Write(vertex.SDEF.R1);
            }
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
