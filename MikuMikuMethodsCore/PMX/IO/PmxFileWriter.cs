using MikuMikuMethods.Extension;

namespace MikuMikuMethods.Pmx.IO;

/// <summary>
/// PMXファイル書込クラス
/// </summary>
internal static class PmxFileWriter
{
    // 書き込み処理開始時にモデルのデータを使ってプロパティの要素が作られるので、クラス生成時には値が入らないが使用時には必ずインスタンスが入っている
    // そういう情報は与えられないので宣言部のみ nullable を無効化してしまう
    // ついでにメソッド終了後の参照解放のための null 入力も無効化部に入れてしまう
#nullable disable
    private static StringEncoder Encoder;

    private static PmxModel Model { get; set; }
    private static List<PmxTexture> Textures { get; set; }

    private static Indexer VtxID { get; set; }
    private static Indexer TexID { get; set; }
    private static Indexer MatID { get; set; }
    private static Indexer BoneID { get; set; }
    private static Indexer MphID { get; set; }
    private static Indexer BodyID { get; set; }

    private static Dictionary<PmxVertex, int> VtxMap { get; set; }
    private static Dictionary<PmxTexture, int> TexMap { get; set; }
    private static Dictionary<PmxMaterial, int> MatMap { get; set; }
    private static Dictionary<PmxBone, int> BoneMap { get; set; }
    private static Dictionary<PmxMorph, int> MphMap { get; set; }
    private static Dictionary<PmxBody, int> BodyMap { get; set; }

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

        VtxMap = null;
        TexMap = null;
        MatMap = null;
        BoneMap = null;
        MphMap = null;
        BodyMap = null;
    }
#nullable enable

    private static void CreatePropaties()
    {
        Encoder = new(Model.Header.Encoding);
        // 共有トゥーンでない全てのテクスチャ情報
        Textures = Model.Materials.SelectMany(m => new[] { m.Texture, m.SphereMap, m.ToonMap }.Where(t => t is not null && t.ToonIndex is null)).Distinct().ToList();

        VtxID = new(Model.Header.SizeOfVertexIndex, true);
        TexID = new(Model.Header.SizeOfTextureIndex, false);
        MatID = new(Model.Header.SizeOfMaterialIndex, false);
        BoneID = new(Model.Header.SizeOfBoneIndex, false);
        MphID = new(Model.Header.SizeOfMorphIndex, false);
        BodyID = new(Model.Header.SizeOfBodyIndex, false);

        VtxMap = Model.Vertices.Select((item, id) => (item, id)).ToDictionary(b => b.item, b => b.id);
        TexMap = Textures.Select((item, id) => (item, id)).ToDictionary(b => b.item, b => b.id);
        MatMap = Model.Materials.Select((item, id) => (item, id)).ToDictionary(b => b.item, b => b.id);
        BoneMap = Model.Bones.Select((item, id) => (item, id)).ToDictionary(b => b.item, b => b.id);
        MphMap = Model.Morphs.Select((item, id) => (item, id)).ToDictionary(b => b.item, b => b.id);
        BodyMap = Model.Bodies.Select((item, id) => (item, id)).ToDictionary(b => b.item, b => b.id);
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

            using FileStream file = new(filePath, FileMode.Create);
            using BinaryWriter writer = new(file);
            WriteHeader(writer, Model.Header);
            WriteInfo(writer, Model.ModelInfo);

            WriteData(Model.Vertices, WriteVertex);
            WriteData(Model.Faces, WriteFace, 2, 3);
            WriteData(Textures, WriteTexture);
            WriteData(Model.Materials, WriteMaterial);
            WriteData(Model.Bones, WriteBone);
            WriteData(Model.Morphs, WriteMorph);
            WriteData(Model.Nodes, WriteNode);
            WriteData(Model.Bodies, WriteBody);
            WriteData(Model.Joints, WriteJoint);
            WriteData(Model.SoftBodies, WriteSoftBody, 2.1f);

            void WriteData<T>(IList<T> list, Action<BinaryWriter, T> DataWriter, float requireVersion = 2.0f, int countScale = 1)
            {
                if (Model.Header.Version >= requireVersion)
                {
                    writer.Write(list.Count * countScale);
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

        for (int i = 0; i < Model.Header.NumOfAdditionalUV; i++)
        {
            writer.Write(vertex.AdditonalUVs[i]);
        }

        writer.Write((byte)vertex.WeightType);

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
            BoneID.Write(writer, BoneMap[vertex.Weights[0].Bone]);
        }

        void WriteBDEF2Weights()
        {
            BoneID.Write(writer, BoneMap[vertex.Weights[0].Bone]);
            BoneID.Write(writer, BoneMap[vertex.Weights[1].Bone]);
            writer.Write(vertex.Weights[0].Value);
        }

        void WriteBDEF4Weights()
        {
            BoneID.Write(writer, BoneMap[vertex.Weights[0].Bone]);
            BoneID.Write(writer, BoneMap[vertex.Weights[1].Bone]);
            BoneID.Write(writer, BoneMap[vertex.Weights[2].Bone]);
            BoneID.Write(writer, BoneMap[vertex.Weights[3].Bone]);
            writer.Write(vertex.Weights[0].Value);
            writer.Write(vertex.Weights[1].Value);
            writer.Write(vertex.Weights[2].Value);
            writer.Write(vertex.Weights[3].Value);
        }

        void WriteSDEFWeights()
        {
            BoneID.Write(writer, BoneMap[vertex.Weights[0].Bone]);
            BoneID.Write(writer, BoneMap[vertex.Weights[1].Bone]);
            writer.Write(vertex.Weights[0].Value);

            writer.Write(vertex.SDEF.C);
            writer.Write(vertex.SDEF.R0);
            writer.Write(vertex.SDEF.R1);
        }
    }

    private static void WriteFace(BinaryWriter writer, PmxFace face)
    {
        foreach (var vtx in face.Vertices)
        {
            VtxID.Write(writer, VtxMap[vtx]);
        }
    }

    private static void WriteTexture(BinaryWriter writer, PmxTexture texture)
    {
        Encoder.Write(writer, texture.Path);
    }

    private static void WriteMaterial(BinaryWriter writer, PmxMaterial material)
    {
        Encoder.Write(writer, material.Name);
        Encoder.Write(writer, material.NameEn);

        writer.Write(material.Diffuse, true);
        writer.Write(material.Specular, false);
        writer.Write(material.ReflectionIntensity);
        writer.Write(material.Ambient, false);

        (bool Value, PmxMaterial.DrawFlag Enum)[] flags =
        {
                (material.EnableBothSideDraw, PmxMaterial.DrawFlag.BothSideDraw),
                (material.EnableShadow, PmxMaterial.DrawFlag.Shadow),
                (material.EnableSelfShadowMap, PmxMaterial.DrawFlag.SelfShadowMap),
                (material.EnableSelfShadow, PmxMaterial.DrawFlag.SelfShadow),
                (material.EnableEdge, PmxMaterial.DrawFlag.Edge),
                (material.EnableVertexColor, PmxMaterial.DrawFlag.VertexColor),
                (material.Primitive == PmxMaterial.PrimitiveType.Point, PmxMaterial.DrawFlag.Point),
                (material.Primitive == PmxMaterial.PrimitiveType.Line, PmxMaterial.DrawFlag.Line)
            };
        var acmFlag = flags.Aggregate((byte)0, (acm, elm) => (byte)(elm.Value ? acm + (byte)elm.Enum : acm));
        writer.Write(acmFlag);

        writer.Write(material.EdgeColor, true);
        writer.Write(material.EdgeWidth);

        TexID.Write(writer, material.Texture == null ? -1 : TexMap[material.Texture]);
        TexID.Write(writer, material.SphereMap == null ? -1 : TexMap[material.SphereMap]);
        writer.Write((byte)material.SphereMode);
        writer.Write(material.ToonMap != null && material.ToonMap.ToonIndex != null);
        if (material.ToonMap != null && material.ToonMap.ToonIndex != null)
            writer.Write((byte)(material.ToonMap.ToonIndex.Value - 1));
        else
            TexID.Write(writer, material.ToonMap == null ? -1 : TexMap[material.ToonMap]);

        Encoder.Write(writer, material.Memo);

        writer.Write(material.Faces.Count * 3);
    }

    private static void WriteBone(BinaryWriter writer, PmxBone bone)
    {
        Encoder.Write(writer, bone.Name);
        Encoder.Write(writer, bone.NameEn);

        writer.Write(bone.Position);
        BoneID.Write(writer, bone.Parent != null ? BoneMap[bone.Parent] : -1);
        writer.Write(bone.TransformOrder);

        (bool Value, PmxBone.BoneFlag Enum)[] flags =
        {
                (bone.ConnectionTarget == PmxBone.ConnectionTargetType.Bone, PmxBone.BoneFlag.ConnectTargetType),
                (bone.Rotatable, PmxBone.BoneFlag.Rotatable),
                (bone.Movable, PmxBone.BoneFlag.Movable),
                (bone.Visible, PmxBone.BoneFlag.Visible),
                (bone.Controlable, PmxBone.BoneFlag.Controlable),
                (bone.IsIK, PmxBone.BoneFlag.IsIK),
                (bone.IsLocalAddition, PmxBone.BoneFlag.AddLocalTarget),
                (bone.IsRotateAddition, PmxBone.BoneFlag.AddRotation),
                (bone.IsMoveAddtion, PmxBone.BoneFlag.AddMoving),
                (bone.IsFixedAxis, PmxBone.BoneFlag.FixAxis),
                (bone.IsLocalAxis, PmxBone.BoneFlag.LocalAxis),
                (bone.IsAfterPhysic, PmxBone.BoneFlag.TrAfterPhysic),
                (bone.UseOuterParent, PmxBone.BoneFlag.TrOuterParent)
            };
        var acmFlag = flags.Aggregate((ushort)0, (acm, elm) => (ushort)(elm.Value ? acm + (ushort)elm.Enum : acm));
        writer.Write(acmFlag);

        if (bone.ConnectionTarget == PmxBone.ConnectionTargetType.Offset)
            writer.Write(bone.ConnectionTargetOffset);
        else
            BoneID.Write(writer, bone.ConnectionTargetBone == null ? -1 : BoneMap[bone.ConnectionTargetBone]);

        if (bone.IsRotateAddition || bone.IsMoveAddtion)
        {
            BoneID.Write(writer, bone.AdditionParent == null ? -1 : BoneMap[bone.AdditionParent]);
            writer.Write(bone.AdditonRatio);
        }

        if (bone.IsFixedAxis)
            writer.Write(bone.FixVector);

        if (bone.IsLocalAxis)
        {
            writer.Write(bone.LocalAxisX);
            writer.Write(bone.LocalAxisZ);
        }

        if (bone.UseOuterParent)
            writer.Write(bone.OuterParentKey);

        if (bone.IsIK)
        {
            if (bone.IKInfo is null) throw new InvalidOperationException("IKInfo is null even though it is an IK bone.");

            BoneID.Write(writer, bone.IKInfo.Target == null ? -1 : BoneMap[bone.IKInfo.Target]);
            writer.Write(bone.IKInfo.LoopNum);
            writer.Write(bone.IKInfo.LimitAngle);

            writer.Write(bone.IKInfo.Links.Count);
            foreach (var link in bone.IKInfo.Links)
            {
                BoneID.Write(writer, link.Bone == null ? -1 : BoneMap[link.Bone]);
                writer.Write(link.EnableAngleLimit);
                if (link.EnableAngleLimit)
                {
                    writer.Write(link.LowerLimit);
                    writer.Write(link.UpperLimit);
                }
            }
        }
    }

    private static void WriteMorph(BinaryWriter writer, PmxMorph morph)
    {
        Encoder.Write(writer, morph.Name);
        Encoder.Write(writer, morph.NameEn);

        writer.Write((byte)morph.Panel);
        writer.Write((byte)morph.Type);
        writer.Write(morph.Offsets.Count);

        Action<IPmxOffset> offsetWriter = morph.Type switch
        {
            PmxMorph.MorphType.Group => WriteGroupOffset,
            PmxMorph.MorphType.Vertex => WriteVertexOffset,
            PmxMorph.MorphType.Bone => WriteBoneOffset,
            PmxMorph.MorphType.UV => WriteUVOffset,
            PmxMorph.MorphType.AdditionalUV1 => WriteUVOffset,
            PmxMorph.MorphType.AdditionalUV2 => WriteUVOffset,
            PmxMorph.MorphType.AdditionalUV3 => WriteUVOffset,
            PmxMorph.MorphType.AdditionalUV4 => WriteUVOffset,
            PmxMorph.MorphType.Material => WriteMaterialOffset,
            PmxMorph.MorphType.Flip => WriteGroupOffset,
            PmxMorph.MorphType.Impulse => WriteImpulseOffset,
            _ => throw new InvalidOperationException("モーフ種別に意図せぬ値が入っていました。"),
        };
        foreach (var offset in morph.Offsets)
        {
            offsetWriter(offset);
        }

        void WriteGroupOffset(IPmxOffset offset)
        {
            var of = (PmxOffsetGroup)offset;
            MphID.Write(writer, of.Target == null ? -1 : MphMap[of.Target]);
            writer.Write(of.Ratio);
        }
        void WriteVertexOffset(IPmxOffset offset)
        {
            var of = (PmxOffsetVertex)offset;
            VtxID.Write(writer, of.Target == null ? -1 : VtxMap[of.Target]);
            writer.Write(of.Offset);
        }
        void WriteBoneOffset(IPmxOffset offset)
        {
            var of = (PmxOffsetBone)offset;
            BoneID.Write(writer, of.Target == null ? -1 : BoneMap[of.Target]);
            writer.Write(of.Offset);
            writer.Write(of.Rotate);
        }
        void WriteUVOffset(IPmxOffset offset)
        {
            var of = (PmxOffsetUV)offset;
            VtxID.Write(writer, of.Target == null ? -1 : VtxMap[of.Target]);
            writer.Write(of.Offset);
        }
        void WriteMaterialOffset(IPmxOffset offset)
        {
            var of = (PmxOffsetMaterial)offset;
            MatID.Write(writer, of.Target == null ? -1 : MatMap[of.Target]);
            writer.Write((byte)of.Operation);
            writer.Write(of.Diffuse, true);
            writer.Write(of.Specular, false);
            writer.Write(of.ReflectionIntensity);
            writer.Write(of.Ambient, false);
            writer.Write(of.EdgeColor, true);
            writer.Write(of.EdgeWidth);
            writer.Write(of.TextureRatio, true);
            writer.Write(of.SphereRatio, true);
            writer.Write(of.ToonRatio, true);
        }
        void WriteImpulseOffset(IPmxOffset offset)
        {
            var of = (PmxOffsetImpulse)offset;
            BodyID.Write(writer, of.Target == null ? -1 : BodyMap[of.Target]);
            writer.Write(of.IsLocal);
            writer.Write(of.MovingSpead);
            writer.Write(of.RotationTorque);
        }
    }

    private static void WriteNode(BinaryWriter writer, PmxNode node)
    {
        Encoder.Write(writer, node.Name);
        Encoder.Write(writer, node.NameEn);
        writer.Write(node.IsSpecific);

        writer.Write(node.Elements.Count);
        foreach (var elm in node.Elements)
        {
            writer.Write(elm.TypeNumber);
            switch (elm.TypeNumber)
            {
                case 0:
                    BoneID.Write(writer, elm.Entity == null ? -1 : BoneMap[(PmxBone)elm.Entity]);
                    break;
                case 1:
                    MphID.Write(writer, elm.Entity == null ? -1 : MphMap[(PmxMorph)elm.Entity]);
                    break;
                default:
                    break;
            }
        }
    }

    private static void WriteBody(BinaryWriter writer, PmxBody body)
    {
        Encoder.Write(writer, body.Name);
        Encoder.Write(writer, body.NameEn);
        BoneID.Write(writer, body.RelationBone == null ? -1 : BoneMap[body.RelationBone]);
        writer.Write(body.Group);
        writer.Write(body.NonCollisionFlag);
        writer.Write((byte)body.Shape);
        writer.Write(body.Size);
        writer.Write(body.Position);
        writer.Write(body.Rotation);
        writer.Write(body.Mass);
        writer.Write(body.MovingDecay);
        writer.Write(body.RotationDecay);
        writer.Write(body.Resiliency);
        writer.Write(body.Friction);
        writer.Write((byte)body.PhysicsMode);
    }

    private static void WriteJoint(BinaryWriter writer, PmxJoint joint)
    {
        Encoder.Write(writer, joint.Name);
        Encoder.Write(writer, joint.NameEn);
        writer.Write((byte)joint.Type);

        BodyID.Write(writer, joint.RelationBodyA == null ? -1 : BodyMap[joint.RelationBodyA]);
        BodyID.Write(writer, joint.RelationBodyB == null ? -1 : BodyMap[joint.RelationBodyB]);

        writer.Write(joint.Position);
        writer.Write(joint.Rotation);
        writer.Write(joint.MovingLowerLimit);
        writer.Write(joint.MovingUpperLimit);
        writer.Write(joint.RotationLowerLimit);
        writer.Write(joint.RotationUpperLimit);
        writer.Write(joint.MovingSpringConstants);
        writer.Write(joint.RotationSpringConstants);
    }

    private static void WriteSoftBody(BinaryWriter writer, PmxSoftBody softBody)
    {
        Encoder.Write(writer, softBody.Name);
        Encoder.Write(writer, softBody.NameEn);

        writer.Write((byte)softBody.Shape);
        MatID.Write(writer, softBody.RelationMaterial == null ? -1 : MatMap[softBody.RelationMaterial]);

        writer.Write(softBody.Group);
        writer.Write(softBody.NonCollisionFlag);

        (bool Value, PmxSoftBody.BitFlag Enum)[] flags =
{
                (softBody.IsCreateBLink, PmxSoftBody.BitFlag.CreateBLink),
                (softBody.IsCreateCluster, PmxSoftBody.BitFlag.CreateCluster),
                (softBody.IsLinkCrossing, PmxSoftBody.BitFlag.LinkCrossing),
            };
        var acmFlag = flags.Aggregate((byte)0, (acm, elm) => (byte)(elm.Value ? acm + (byte)elm.Enum : acm));
        writer.Write(acmFlag);

        writer.Write(softBody.BLinkCreationDistance);
        writer.Write(softBody.NumOfCluster);

        writer.Write(softBody.SumOfMass);
        writer.Write(softBody.MarginOfCollision);

        writer.Write((int)softBody.AeroModel);

        writer.Write(softBody.Config.VCF);
        writer.Write(softBody.Config.DP);
        writer.Write(softBody.Config.DG);
        writer.Write(softBody.Config.LF);
        writer.Write(softBody.Config.PR);
        writer.Write(softBody.Config.VC);
        writer.Write(softBody.Config.DF);
        writer.Write(softBody.Config.MT);
        writer.Write(softBody.Config.CHR);
        writer.Write(softBody.Config.KHR);
        writer.Write(softBody.Config.SHR);
        writer.Write(softBody.Config.AHR);

        writer.Write(softBody.ClusterParameter.SRHR_CL);
        writer.Write(softBody.ClusterParameter.SKHR_CL);
        writer.Write(softBody.ClusterParameter.SSHR_CL);
        writer.Write(softBody.ClusterParameter.SR_SPLT_CL);
        writer.Write(softBody.ClusterParameter.SK_SPLT_CL);
        writer.Write(softBody.ClusterParameter.SS_SPLT_CL);

        writer.Write(softBody.IterationParameter.V_IT);
        writer.Write(softBody.IterationParameter.P_IT);
        writer.Write(softBody.IterationParameter.D_IT);
        writer.Write(softBody.IterationParameter.C_IT);

        writer.Write(softBody.MaterialParameter.LST);
        writer.Write(softBody.MaterialParameter.AST);
        writer.Write(softBody.MaterialParameter.VST);

        writer.Write(softBody.Anchors.Count);
        foreach (var anchor in softBody.Anchors)
        {
            BodyID.Write(writer, anchor.RelationBody == null ? -1 : BodyMap[anchor.RelationBody]);
            VtxID.Write(writer, anchor.RelationVertex == null ? -1 : VtxMap[anchor.RelationVertex]);
            writer.Write(anchor.IsNearMode);
        }

        writer.Write(softBody.Pins.Count);
        foreach (var pin in softBody.Pins)
        {
            VtxID.Write(writer, pin == null ? -1 : VtxMap[pin]);
        }
    }
}
