using MikuMikuMethods.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMX.IO
{
    /// <summary>
    /// PMXファイル書込クラス
    /// </summary>
    public static class PmxFileWriter
    {
        private static StringEncoder Encoder;

        private static PmxModel Model { get; set; }
        private static List<PmxTexture> Textures { get; set; }

        private static Indexer VtxID { get; set; }
        private static Indexer TexID { get; set; }
        private static Indexer MatID { get; set; }
        private static Indexer BoneID { get; set; }
        private static Indexer MphID { get; set; }
        private static Indexer BodyID { get; set; }

        private static void CleanUpProperties()
        {
            Encoder = null;
            Model = null;
            Textures = null;

            VtxID = null;
            TexID = null;
            MatID = null;
            BoneID = null;
            MphID = null;
            BodyID = null;
        }

        private static void CreatePropaties()
        {
            Encoder = new(Model.Header.Encoding);
            Textures = Model.Materials.SelectMany(m => new[] { m.Texture, m.SphereMap, m.ToonMap }.Where(t => t != null)).Distinct().ToList();

            VtxID = new(Model.Header.SizeOfVertexIndex, true);
            TexID = new(Model.Header.SizeOfTextureIndex, false);
            MatID = new(Model.Header.SizeOfMaterialIndex, false);
            BoneID = new(Model.Header.SizeOfBoneIndex, false);
            MphID = new(Model.Header.SizeOfMorphIndex, false);
            BodyID = new(Model.Header.SizeOfBodyIndex, false);
        }

        /// <summary>
        /// モデル書込
        /// </summary>
        /// <param name="filePath">書込むファイルパス</param>
        /// <param name="model">書き込むモデル</param>
        public static void WriteModel(string filePath, PmxModel model)
        {
            try
            {
                Model = model;
                Model.ValidateVersion();
                CreatePropaties();

                using (FileStream file = new(filePath, FileMode.Create))
                using (BinaryWriter writer = new(file))
                {
                    WriteHeader(writer, Model.Header);
                    WriteInfo(writer, Model.ModelInfo);

                    WriteData(Model.Vertices, WriteVertex);
                    WriteData(Model.Faces, WriteFace);
                    WriteData(Textures, WriteTexture);
                    WriteData(Model.Materials, WriteMaterial);
                    WriteData(Model.Bones, WriteBone);
                    WriteData(Model.Morphs, WriteMorph);
                    WriteData(Model.Nodes, WriteNode);
                    WriteData(Model.Bodies, WriteBody);
                    WriteData(Model.Joints, WriteJoint);
                    WriteData(Model.SoftBodies, WriteSoftBody, 2.1f);

                    void WriteData<T>(IList<T> list, Action<BinaryWriter, T> DataWriter, float requireVersion = 2.0f)
                    {
                        if (Model.Header.Version >= requireVersion)
                        {
                            writer.Write(list.Count);
                            foreach (var item in list)
                            {
                                DataWriter(writer, item);
                            }
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
                    WriteBDEF1Weights();
                    break;
                case PmxWeightType.BDEF2:
                    WriteBDEF2Weights();
                    break;
                case PmxWeightType.BDEF4:
                    WriteBDEF4Weights();
                    break;
                case PmxWeightType.SDEF:
                    WriteSDEFWeights();
                    break;
                case PmxWeightType.QDEF:
                    WriteBDEF4Weights();
                    break;
            }

            writer.Write(vertex.EdgeScale);

            // ウェイト書込ローカル関数

            void WriteBDEF1Weights()
            {
                boneIndexer.Write(writer, boneIdMap[vertex.Weights[0].Bone]);
            }

            void WriteBDEF2Weights()
            {
                boneIndexer.Write(writer, boneIdMap[vertex.Weights[0].Bone]);
                boneIndexer.Write(writer, boneIdMap[vertex.Weights[1].Bone]);
                writer.Write(vertex.Weights[0].Value);
            }

            void WriteBDEF4Weights()
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

            void WriteSDEFWeights()
            {
                boneIndexer.Write(writer, boneIdMap[vertex.Weights[0].Bone]);
                boneIndexer.Write(writer, boneIdMap[vertex.Weights[1].Bone]);
                writer.Write(vertex.Weights[0].Value);

                writer.Write(vertex.SDEF.C);
                writer.Write(vertex.SDEF.R0);
                writer.Write(vertex.SDEF.R1);
            }
        }

        private static void WriteFace(BinaryWriter writer, PmxFace face)
        {
            
        }

        private static void WriteTexture(BinaryWriter writer, PmxTexture texture)
        {
            throw new NotImplementedException();
        }

        private static void WriteMaterial(BinaryWriter writer, PmxMaterial material)
        {
            throw new NotImplementedException();
        }

        private static void WriteBone(BinaryWriter writer, PmxBone bone)
        {
            throw new NotImplementedException();
        }

        private static void WriteMorph(BinaryWriter writer, PmxMorph morph)
        {
            throw new NotImplementedException();
        }

        private static void WriteNode(BinaryWriter writer, PmxNode node)
        {
            throw new NotImplementedException();
        }

        private static void WriteBody(BinaryWriter writer, PmxBody body)
        {
            throw new NotImplementedException();
        }

        private static void WriteJoint(BinaryWriter writer, PmxJoint joint)
        {
            throw new NotImplementedException();
        }

        private static void WriteSoftBody(BinaryWriter writer, PmxSoftBody softBody)
        {
            throw new NotImplementedException();
        }
    }
}
