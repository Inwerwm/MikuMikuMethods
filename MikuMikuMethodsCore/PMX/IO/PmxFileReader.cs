using MikuMikuMethods.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MikuMikuMethods.Pmx.IO;

/// <summary>
/// PMXファイル読込クラス
/// </summary>
internal static class PmxFileReader
{
    private static StringEncoder Encoder = new(Encoding.UTF8);

    private static PmxModel Model { get; set; } = new();

    private static List<PmxFace> Faces { get; set; } = new();
    private static int LoadedFaceCount { get; set; }

    private static List<PmxTexture> Textures { get; set; } = new();

    private static List<(PmxWeight Instance, int RelationID)> TmpWeightBoneIndices { get; set; } = new();
    private static List<(PmxBone Instance, int RelationID)> TmpParentBoneIndices { get; set; } = new();
    private static List<(PmxBone Instance, int RelationID)> TmpConnectionTargetBoneIndices { get; set; } = new();
    private static List<(PmxBone Instance, int RelationID)> TmpAdditionParentBoneIndices { get; set; } = new();
    private static List<(PmxInverseKinematics Instance, int RelationID)> TmpIKTargetBoneIndices { get; set; } = new();
    private static List<(PmxIKLink Instance, int RelationID)> TmpIKLinkBoneIndices { get; set; } = new();
    private static List<(PmxOffsetGroup Instance, int RelationID)> TmpGroupedMorphIndices { get; set; } = new();
    private static List<(PmxOffsetImpulse Instance, int RelationID)> TmpImpulseTargetBodyIndices { get; set; } = new();

    private static void InitializeTmpInstances()
    {
        Faces = new();
        LoadedFaceCount = 0;

        Textures = new();

        TmpWeightBoneIndices = new();
        TmpParentBoneIndices = new();
        TmpConnectionTargetBoneIndices = new();
        TmpAdditionParentBoneIndices = new();
        TmpIKTargetBoneIndices = new();
        TmpIKLinkBoneIndices = new();
        TmpGroupedMorphIndices = new();
        TmpImpulseTargetBodyIndices = new();
    }

    private static void SolveRelations()
    {
        void Solve<T>(List<(T Instance, int RelationID)> Tmp, Action<T, int> setter)
        {
            foreach (var (instance, relationID) in Tmp.Where(item => item.RelationID >= 0))
            {
                setter(instance, relationID);
            }
        }

        Solve(TmpWeightBoneIndices, (instance, id) => instance.Bone = Model.Bones[id]);
        Solve(TmpParentBoneIndices, (instance, id) => instance.Parent = Model.Bones[id]);
        Solve(TmpConnectionTargetBoneIndices, (instance, id) => instance.ConnectionTargetBone = Model.Bones[id]);
        Solve(TmpAdditionParentBoneIndices, (instance, id) => instance.AdditionParent = Model.Bones[id]);
        Solve(TmpIKTargetBoneIndices, (instance, id) => instance.Target = Model.Bones[id]);
        Solve(TmpIKLinkBoneIndices, (instance, id) => instance.Bone = Model.Bones[id]);
        Solve(TmpGroupedMorphIndices, (instance, id) => instance.Target = Model.Morphs[id]);
        Solve(TmpImpulseTargetBodyIndices, (instance, id) => instance.Target = Model.Bodies[id]);
    }

    /// <summary>
    /// モデル読込
    /// </summary>
    /// <param name="filePath">読み込むモデルファイルのパス</param>
    /// <returns>読み込んだモデル</returns>
    public static void ReadModel(string filePath, PmxModel model)
    {
        try
        {
            using (FileStream stream = new(filePath, FileMode.Open))
            using (BinaryReader reader = new(stream))
            {
                Model = model;

                ReadHeader(reader, Model.Header);
                Encoder = new StringEncoder(Model.Header.Encoding);

                ReadInfo(reader, Model.ModelInfo);

                AddDataToList(Model.Vertices, ReadVertex);
                AddDataToList(Faces, ReadFace, 3);
                AddDataToList(Textures, ReadTexture);
                AddDataToList(Model.Materials, ReadMaterial);
                AddDataToList(Model.Bones, ReadBone);
                AddDataToList(Model.Morphs, ReadMorph);
                AddDataToList(Model.Nodes, ReadNode);
                AddDataToList(Model.Bodies, ReadBody);
                AddDataToList(Model.Joints, ReadJoint);
                if (Model.Header.Version >= 2.1)
                    AddDataToList(Model.SoftBodies, ReadSoftBody);

                SolveRelations();

                void AddDataToList<T>(List<T> list, Func<BinaryReader, T> dataReader, int divisor = 1)
                {
                    int count = reader.ReadInt32() / divisor;
                    list.AddRange(Enumerable.Range(0, count).Select(_ => dataReader(reader)));
                }
            }
        }
        finally
        {
            InitializeTmpInstances();
        }
    }

    private static void ReadHeader(BinaryReader reader, PmxHeader header)
    {
        // "PMX "
        reader.ReadBytes(4);
        Model.Header.Version = reader.ReadSingle();
        if (Model.Header.Version < 2.0) throw new InvalidDataException("PMXが非対応バージョンです。バージョン番号が未対応バージョンです。");

        var configSize = reader.ReadByte();
        if (configSize != 8) throw new InvalidDataException("PMXが非対応バージョンです。ヘッダデータが未対応の形式です。");

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

        for (int i = 0; i < Model.Header.NumOfAdditionalUV; i++)
        {
            vtx.AdditonalUVs[i] = reader.ReadVector4();
        }
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
            case PmxWeightType.QDEF:
                ReadBDEF4Weights(reader, boneIndexer);
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

    private static PmxTexture ReadTexture(BinaryReader reader) => new(Encoder.Read(reader));

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
        material.EnableVertexColor = bitFlag.HasFlag(PmxMaterial.DrawFlag.VertexColor);
        material.Primitive = Model.Header.Version < 2.1 ? PmxMaterial.PrimitiveType.Tri
                           : bitFlag.HasFlag(PmxMaterial.DrawFlag.Point) ? PmxMaterial.PrimitiveType.Point
                           : bitFlag.HasFlag(PmxMaterial.DrawFlag.Line) ? PmxMaterial.PrimitiveType.Line
                           : PmxMaterial.PrimitiveType.Tri;
        material.EdgeColor = reader.ReadSingleRGBA();
        material.EdgeWidth = reader.ReadSingle();

        var id = new Indexer(Model.Header.SizeOfTextureIndex, false);
        int textureID = id.Read(reader);
        material.Texture = textureID < 0 ? null : Textures[textureID];
        int sphereID = id.Read(reader);
        material.SphereMap = sphereID < 0 ? null : Textures[sphereID];
        material.SphereMode = (PmxMaterial.SphereModeType)reader.ReadByte();
        var IsSharedToon = reader.ReadBoolean();
        int toonID = IsSharedToon ? -1 : id.Read(reader);
        material.ToonMap = IsSharedToon ? new PmxTexture((byte)(reader.ReadByte() + 1))
                         : toonID < 0 ? null
                         : Textures[toonID];

        material.Memo = Encoder.Read(reader);

        // 材質に対応する面(頂点)数 (必ず3の倍数になる)
        var faceCount = reader.ReadInt32() / 3;
        material.Faces.AddRange(Faces.GetRange(LoadedFaceCount, faceCount));
        LoadedFaceCount += faceCount;

        return material;
    }

    private static PmxBone ReadBone(BinaryReader reader)
    {
        var bone = new PmxBone();

        bone.Name = Encoder.Read(reader);
        bone.NameEn = Encoder.Read(reader);

        bone.Position = reader.ReadVector3();

        var id = new Indexer(Model.Header.SizeOfBoneIndex, false);
        TmpParentBoneIndices.Add((bone, id.Read(reader)));
        bone.TransformOrder = reader.ReadInt32();

        var boneFlag = (PmxBone.BoneFlag)reader.ReadUInt16();

        bone.Rotatable = boneFlag.HasFlag(PmxBone.BoneFlag.Rotatable);
        bone.Movable = boneFlag.HasFlag(PmxBone.BoneFlag.Movable);
        bone.Visible = boneFlag.HasFlag(PmxBone.BoneFlag.Visible);
        bone.Controlable = boneFlag.HasFlag(PmxBone.BoneFlag.Controlable);
        bone.IsLocalAddition = boneFlag.HasFlag(PmxBone.BoneFlag.AddLocalTarget);
        bone.IsAfterPhysic = boneFlag.HasFlag(PmxBone.BoneFlag.TrAfterPhysic);

        if (boneFlag.HasFlag(PmxBone.BoneFlag.ConnectTargetType))
        {
            bone.ConnectionTarget = PmxBone.ConnectionTargetType.Bone;
            TmpConnectionTargetBoneIndices.Add((bone, id.Read(reader)));
        }
        else
        {
            bone.ConnectionTarget = PmxBone.ConnectionTargetType.Offset;
            bone.ConnectionTargetOffset = reader.ReadVector3();
        }

        if (boneFlag.HasFlag(PmxBone.BoneFlag.AddRotation) || boneFlag.HasFlag(PmxBone.BoneFlag.AddMoving))
        {
            bone.IsRotateAddition = boneFlag.HasFlag(PmxBone.BoneFlag.AddRotation);
            bone.IsMoveAddtion = boneFlag.HasFlag(PmxBone.BoneFlag.AddMoving);
            TmpAdditionParentBoneIndices.Add((bone, id.Read(reader)));
            bone.AdditonRatio = reader.ReadSingle();
        }

        if (boneFlag.HasFlag(PmxBone.BoneFlag.FixAxis))
        {
            bone.IsFixedAxis = true;
            bone.FixVector = reader.ReadVector3();
        }

        if (boneFlag.HasFlag(PmxBone.BoneFlag.LocalAxis))
        {
            bone.IsLocalAxis = true;
            bone.LocalAxisX = reader.ReadVector3();
            bone.LocalAxisZ = reader.ReadVector3();
        }

        if (boneFlag.HasFlag(PmxBone.BoneFlag.TrOuterParent))
        {
            bone.UseOuterParent = true;
            bone.OuterParentKey = reader.ReadInt32();
        }

        if (boneFlag.HasFlag(PmxBone.BoneFlag.IsIK))
        {
            bone.IsIK = true;

            var ik = new PmxInverseKinematics();
            TmpIKTargetBoneIndices.Add((ik, id.Read(reader)));
            ik.LoopNum = reader.ReadInt32();
            ik.LimitAngle = reader.ReadSingle();

            var numOfLink = reader.ReadInt32();
            ik.Links.AddRange(Enumerable.Range(0, numOfLink).Select(_ =>
            {
                var link = new PmxIKLink();

                TmpIKLinkBoneIndices.Add((link, id.Read(reader)));
                link.EnableAngleLimit = reader.ReadBoolean();
                if (link.EnableAngleLimit)
                {
                    link.LowerLimit = reader.ReadVector3();
                    link.UpperLimit = reader.ReadVector3();
                }

                return link;
            }));

            bone.IKInfo = ik;
        }

        return bone;
    }

    private static PmxMorph ReadMorph(BinaryReader reader)
    {
        var name = Encoder.Read(reader);
        var naen = Encoder.Read(reader);

        var panel = reader.ReadByte();

        var morph = new PmxMorph((PmxMorph.MorphType)reader.ReadByte())
        {
            Name = name,
            NameEn = naen,
            Panel = (PmxMorph.MorphPanel)panel,
        };

        var vid = new Indexer(Model.Header.SizeOfVertexIndex, true);
        var bnid = new Indexer(Model.Header.SizeOfBoneIndex, false);
        var moid = new Indexer(Model.Header.SizeOfMorphIndex, false);
        var mtid = new Indexer(Model.Header.SizeOfMaterialIndex, false);
        var bdid = new Indexer(Model.Header.SizeOfBodyIndex, false);

        var numOfOffset = reader.ReadInt32();
        morph.Offsets.AddRange(Enumerable.Range(0, numOfOffset).Select<int, IPmxOffset>(_ => morph.Type switch
        {
            PmxMorph.MorphType.Group => CreateGroupOffset(),
            PmxMorph.MorphType.Vertex => CreateVertexOffset(),
            PmxMorph.MorphType.Bone => CreateBoneOffset(),
            PmxMorph.MorphType.UV => CreateUVOffset(),
            PmxMorph.MorphType.AdditionalUV1 => CreateUVOffset(),
            PmxMorph.MorphType.AdditionalUV2 => CreateUVOffset(),
            PmxMorph.MorphType.AdditionalUV3 => CreateUVOffset(),
            PmxMorph.MorphType.AdditionalUV4 => CreateUVOffset(),
            PmxMorph.MorphType.Material => CreateMaterialOffset(),
            PmxMorph.MorphType.Flip => CreateGroupOffset(),
            PmxMorph.MorphType.Impulse => CreateImpulseOffset(),
            _ => throw new InvalidOperationException("モーフ種別に意図せぬ値が入っていました。")
        }));

        return morph;

        PmxOffsetGroup CreateGroupOffset()
        {
            var of = new PmxOffsetGroup();
            TmpGroupedMorphIndices.Add((of, moid.Read(reader)));
            of.Ratio = reader.ReadSingle();
            return of;
        }
        PmxOffsetVertex CreateVertexOffset() => new(Model.Vertices[vid.Read(reader)])
        {
            Offset = reader.ReadVector3()
        };
        PmxOffsetBone CreateBoneOffset() => new(Model.Bones[bnid.Read(reader)])
        {
            Offset = reader.ReadVector3(),
            Rotate = reader.ReadQuaternion()
        };
        PmxOffsetUV CreateUVOffset() => new(Model.Vertices[vid.Read(reader)])
        {
            Offset = reader.ReadVector4()
        };
        PmxOffsetMaterial CreateMaterialOffset()
        {
            var targetId = mtid.Read(reader);
            return new PmxOffsetMaterial((PmxOffsetMaterial.OperationType)reader.ReadByte())
            {
                Target = targetId < 0 ? null : Model.Materials[targetId],
                Diffuse = reader.ReadSingleRGBA(),
                Specular = reader.ReadSingleRGB(),
                ReflectionIntensity = reader.ReadSingle(),
                Ambient = reader.ReadSingleRGB(),

                EdgeColor = reader.ReadSingleRGBA(),
                EdgeWidth = reader.ReadSingle(),

                TextureRatio = reader.ReadSingleRGBA(),
                SphereRatio = reader.ReadSingleRGBA(),
                ToonRatio = reader.ReadSingleRGBA()
            };
        }
        PmxOffsetImpulse CreateImpulseOffset()
        {
            var of = new PmxOffsetImpulse();
            TmpImpulseTargetBodyIndices.Add((of, bdid.Read(reader)));
            of.IsLocal = reader.ReadBoolean();
            of.MovingSpead = reader.ReadVector3();
            of.RotationTorque = reader.ReadVector3();
            return of;
        }
    }

    private static PmxNode ReadNode(BinaryReader reader)
    {
        var node = new PmxNode()
        {
            Name = Encoder.Read(reader),
            NameEn = Encoder.Read(reader),
            IsSpecific = reader.ReadBoolean()
        };

        var bid = new Indexer(Model.Header.SizeOfBoneIndex, false);
        var mid = new Indexer(Model.Header.SizeOfMorphIndex, false);

        int elmNum = reader.ReadInt32();
        node.Elements.AddRange(Enumerable.Range(0, elmNum).Select<int, IPmxNodeElement>(_ => reader.ReadByte() switch
            {
                0 => new PmxNodeElementBone(Model.Bones[bid.Read(reader)]),
                1 => new PmxNodeElementMorph(Model.Morphs[mid.Read(reader)]),
                _ => throw new InvalidOperationException("表情枠要素種別に意図せぬ値が入っていました。"),
            }));

        return node;
    }

    private static PmxBody ReadBody(BinaryReader reader)
    {
        var bid = new Indexer(Model.Header.SizeOfBoneIndex, false);
        return new PmxBody()
        {
            Name = Encoder.Read(reader),
            NameEn = Encoder.Read(reader),

            RelationBone = Model.Bones[bid.Read(reader)],

            Group = reader.ReadByte(),
            NonCollisionFlag = reader.ReadUInt16(),

            Shape = (PmxBody.ShapeType)reader.ReadByte(),
            Size = reader.ReadVector3(),

            Position = reader.ReadVector3(),
            Rotation = reader.ReadVector3(),

            Mass = reader.ReadSingle(),
            MovingDecay = reader.ReadSingle(),
            RotationDecay = reader.ReadSingle(),
            Resiliency = reader.ReadSingle(),
            Friction = reader.ReadSingle(),

            PhysicsMode = (PmxBody.PhysicsModeType)reader.ReadByte()
        };
    }

    private static PmxJoint ReadJoint(BinaryReader reader)
    {
        var bid = new Indexer(Model.Header.SizeOfBodyIndex, false);

        return new PmxJoint()
        {
            Name = Encoder.Read(reader),
            NameEn = Encoder.Read(reader),
            Type = (PmxJoint.JointType)reader.ReadByte(),
            RelationBodyA = Model.Bodies[bid.Read(reader)],
            RelationBodyB = Model.Bodies[bid.Read(reader)],
            Position = reader.ReadVector3(),
            Rotation = reader.ReadVector3(),
            MovingLowerLimit = reader.ReadVector3(),
            MovingUpperLimit = reader.ReadVector3(),
            RotationLowerLimit = reader.ReadVector3(),
            RotationUpperLimit = reader.ReadVector3(),
            MovingSpringConstants = reader.ReadVector3(),
            RotationSpringConstants = reader.ReadVector3(),
        };
    }

    private static PmxSoftBody ReadSoftBody(BinaryReader reader)
    {
        var mid = new Indexer(Model.Header.SizeOfMaterialIndex, false);
        var bid = new Indexer(Model.Header.SizeOfBodyIndex, false);
        var vid = new Indexer(Model.Header.SizeOfVertexIndex, true);

        var sb = new PmxSoftBody()
        {
            Name = Encoder.Read(reader),
            NameEn = Encoder.Read(reader),

            Shape = (PmxSoftBody.ShapeType)reader.ReadByte(),
            RelationMaterial = Model.Materials[mid.Read(reader)],

            Group = reader.ReadByte(),
            NonCollisionFlag = reader.ReadUInt16(),
        };

        var bitFlag = (PmxSoftBody.BitFlag)reader.ReadByte();
        sb.IsCreateBLink = bitFlag.HasFlag(PmxSoftBody.BitFlag.CreateBLink);
        sb.IsCreateCluster = bitFlag.HasFlag(PmxSoftBody.BitFlag.CreateCluster);
        sb.IsLinkCrossing = bitFlag.HasFlag(PmxSoftBody.BitFlag.LinkCrossing);

        sb.BLinkCreationDistance = reader.ReadInt32();
        sb.NumOfCluster = reader.ReadInt32();

        sb.SumOfMass = reader.ReadSingle();
        sb.MarginOfCollision = reader.ReadSingle();

        sb.AeroModel = (PmxSoftBody.AeroModelType)reader.ReadInt32();

        sb.Config.VCF = reader.ReadSingle();
        sb.Config.DP = reader.ReadSingle();
        sb.Config.DG = reader.ReadSingle();
        sb.Config.LF = reader.ReadSingle();
        sb.Config.PR = reader.ReadSingle();
        sb.Config.VC = reader.ReadSingle();
        sb.Config.DF = reader.ReadSingle();
        sb.Config.MT = reader.ReadSingle();
        sb.Config.CHR = reader.ReadSingle();
        sb.Config.KHR = reader.ReadSingle();
        sb.Config.SHR = reader.ReadSingle();
        sb.Config.AHR = reader.ReadSingle();

        sb.ClusterParameter.SRHR_CL = reader.ReadSingle();
        sb.ClusterParameter.SKHR_CL = reader.ReadSingle();
        sb.ClusterParameter.SSHR_CL = reader.ReadSingle();
        sb.ClusterParameter.SR_SPLT_CL = reader.ReadSingle();
        sb.ClusterParameter.SK_SPLT_CL = reader.ReadSingle();
        sb.ClusterParameter.SS_SPLT_CL = reader.ReadSingle();

        sb.IterationParameter.V_IT = reader.ReadInt32();
        sb.IterationParameter.P_IT = reader.ReadInt32();
        sb.IterationParameter.D_IT = reader.ReadInt32();
        sb.IterationParameter.C_IT = reader.ReadInt32();

        sb.MaterialParameter.LST = reader.ReadSingle();
        sb.MaterialParameter.AST = reader.ReadSingle();
        sb.MaterialParameter.VST = reader.ReadSingle();

        var anchorNum = reader.ReadInt32();
        sb.Anchors.AddRange(Enumerable.Range(0, anchorNum).Select(_ => new PmxSoftBody.Anchor()
        {
            RelationBody = Model.Bodies[bid.Read(reader)],
            RelationVertex = Model.Vertices[vid.Read(reader)],
            IsNearMode = reader.ReadBoolean()
        }));

        var pinNum = reader.ReadInt32();
        sb.Pins.AddRange(Enumerable.Range(0, pinNum).Select(_ => Model.Vertices[vid.Read(reader)]));

        return sb;
    }
}
