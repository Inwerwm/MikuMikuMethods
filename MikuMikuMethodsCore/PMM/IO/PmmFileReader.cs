using MikuMikuMethods.Common;
using MikuMikuMethods.Extension;
using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;

namespace MikuMikuMethods.Pmm.IO;

public static class PmmFileReader
{
    private static Dictionary<PmmModelConfigFrame, Dictionary<PmmBone, (int ModelID, int BoneID)>> OuterParentRelation { get; set; } = new();
    private static Dictionary<PmmModelConfigState, Dictionary<PmmBone, (int ModelID, int BoneID)>> OuterParentRelationCurrent { get; set; } = new();

    private static DataLoadErrorInfomation Current = new("", null, "");

    public static void Read(string filePath, PolygonMovieMaker pmm)
    {
        try
        {
            using FileStream file = new(filePath, FileMode.Open);
            using BinaryReader reader = new(file, Encoding.ShiftJIS);
            Read(reader, pmm);
        }
        catch (Exception)
        {
            throw;
        }
    }


    public static void Read(BinaryReader reader, PolygonMovieMaker pmm)
    {
        try
        {
            Current = new("Header", null, "Header section of the PMM file.");
            pmm.Version = reader.ReadString(30, Encoding.ShiftJIS, '\0');
            if (pmm.Version != "Polygon Movie maker 0002") throw new InvalidDataException("This is not PMM file.");
            pmm.OutputResolution = new(reader.ReadInt32(), reader.ReadInt32());

            pmm.EditorState.Width = reader.ReadInt32();
            pmm.Camera.Current.ViewAngle = (int)reader.ReadSingle();
            pmm.EditorState.IsCameraMode = reader.ReadBoolean();

            Current = new("PanelOpeningStatus", null, "Section of the opening status of various operation panels.");
            // パネル開閉状態の読み込み
            pmm.PanelPane.DoesOpenCameraPanel = reader.ReadBoolean();
            pmm.PanelPane.DoesOpenLightPanel = reader.ReadBoolean();
            pmm.PanelPane.DoesOpenAccessaryPanel = reader.ReadBoolean();
            pmm.PanelPane.DoesOpenBonePanel = reader.ReadBoolean();
            pmm.PanelPane.DoesOpenMorphPanel = reader.ReadBoolean();
            pmm.PanelPane.DoesOpenSelfShadowPanel = reader.ReadBoolean();

            Current = new("HeaderOfModels", null, "Section of selected model index and total number of models.");
            // モデル読み込み
            var selectedModelIndex = reader.ReadByte();
            var modelCount = reader.ReadByte();
            var modelOrderDictionary = new Dictionary<PmmModel, (byte RenderOrder, byte CalculateOrder)>();
            for (int i = 0; i < modelCount; i++)
            {
                Current = new("Model", i, $"Section of {DataLoadErrorInfomation.GetOrdinal(i)} model data.");
                (var model, var renderOrder, var calculateOrder) = ReadModel(reader);
                pmm.Models.Add(model);
                modelOrderDictionary.Add(model, ((byte RenderOrder, byte CalculateOrder))(renderOrder - 1, calculateOrder - 1));
            }

            Current = new($"ModelRelationSolving", null, $"The section that resolves the relations of the selected model and the render/calculate order. In this section, no reading is done, only calculation.");
            pmm.EditorState.SelectedModel = selectedModelIndex < pmm.Models.Count ? pmm.Models[selectedModelIndex] : null;
            foreach (var model in pmm.Models)
            {
                // 順序系プロパティはモデルの追加後に設定する
                var (renderOrder, calculateOrder) = modelOrderDictionary[model];
                pmm.SetRenderOrder(model, renderOrder);
                pmm.SetCalculateOrder(model, calculateOrder);
            }

            // 外部親の関係解決
            Current = new("OuterParentSolving", null, $"The section that resolves the outer parent relation. In this section, no reading is done, only calculation.");
            foreach (var frame in OuterParentRelation)
            {
                foreach (var relation in frame.Value)
                {
                    var opModel = relation.Value.ModelID < 0 ? null : pmm.Models[relation.Value.ModelID];
                    frame.Key.OuterParent.Add(relation.Key, new()
                    {
                        ParentModel = opModel,
                        ParentBone = opModel?.Bones[relation.Value.BoneID]
                    });
                }
            }

            foreach (var state in OuterParentRelationCurrent)
            {
                foreach (var relation in state.Value)
                {
                    var opModel = relation.Value.ModelID < 0 ? null : pmm.Models[relation.Value.ModelID];
                    state.Key.OuterParent[relation.Key].ParentModel = opModel;
                    state.Key.OuterParent[relation.Key].ParentBone = opModel?.Bones[relation.Value.BoneID];
                }
            }

            ReadCamera(reader, pmm);
            ReadLight(reader, pmm.Light);

            Current = new("HeaderOfAccessories", null, "Section of selected accessory index and total number of accessories.");
            var selectedAccessoryIndex = reader.ReadByte();
            pmm.EditorState.VerticalScrollOfAccessory = reader.ReadInt32();

            // アクセサリ読み込み
            var accessoryOrderDictionary = new Dictionary<PmmAccessory, byte>();
            var accessoryCount = reader.ReadByte();
            // アクセサリ名一覧
            // 名前は各アクセサリ領域にも書いてあり、齟齬が出ることは基本無いらしいので読み飛ばす
            _ = reader.ReadBytes(accessoryCount * 100);
            for (int i = 0; i < accessoryCount; i++)
            {
                Current = new($"Accessory", i, $"The section of {DataLoadErrorInfomation.GetOrdinal(i)} accessory data.");
                (PmmAccessory accessory, byte renderOrder) = ReadAccessory(reader, pmm);
                pmm.Accessories.Add(accessory);
                accessoryOrderDictionary.Add(accessory, renderOrder);
            }

            Current = new($"AccessoryRelationSolving", null, $"The section that resolves the relations of the selected accessory and the render order. In this section, no reading is done, only calculation.");
            pmm.EditorState.SelectedAccessory = selectedAccessoryIndex < pmm.Accessories.Count ? pmm.Accessories[selectedAccessoryIndex] : null;
            foreach (var acs in pmm.Accessories)
            {
                pmm.SetRenderOrder(acs, accessoryOrderDictionary[acs]);
            }

            Current = new($"KeyFrameEditorCurrentTarget", null, $"Sections for the current keyframe editor editing target and scroll amount.");
            // フレーム編集画面の状態読み込み
            pmm.EditorState.CurrentFrame = reader.ReadInt32();
            pmm.EditorState.HorizontalScroll = reader.ReadInt32();
            pmm.EditorState.HorizontalScrollLength = reader.ReadInt32();
            pmm.EditorState.SelectedBoneOperation = (PmmEditorState.BoneOperation)reader.ReadInt32();

            Current = new($"Media", null, $"The sections of play config ,background media, render config.");
            // 再生関連設定の読込
            pmm.PlayConfig.CameraTrackingTarget = (PmmPlayConfig.TrackingTarget)reader.ReadByte();

            pmm.PlayConfig.EnableRepeat = reader.ReadBoolean();
            pmm.PlayConfig.EnableMoveCurrentFrameToPlayStopping = reader.ReadBoolean();
            pmm.PlayConfig.EnableStartFromCurrentFrame = reader.ReadBoolean();

            pmm.PlayConfig.PlayStartFrame = reader.ReadInt32();
            pmm.PlayConfig.PlayStopFrame = reader.ReadInt32();

            // 背景メディア設定の読込
            var existsBgm = reader.ReadBoolean();
            var bgmPath = reader.ReadString(256, Encoding.ShiftJIS, '\0');
            pmm.BackGround.Audio = existsBgm ? bgmPath : null;

            pmm.BackGround.VideoOffset = new(reader.ReadInt32(), reader.ReadInt32());
            pmm.BackGround.VideoScale = reader.ReadSingle();
            var bgvPath = reader.ReadString(256, Encoding.ShiftJIS, '\0');
            var existsBgv = reader.ReadInt32() == 0b01000000;
            pmm.BackGround.Video = existsBgv ? bgvPath : null;

            pmm.BackGround.ImageOffset = new(reader.ReadInt32(), reader.ReadInt32());
            pmm.BackGround.ImageScale = reader.ReadSingle();
            var bgiPath = reader.ReadString(256, Encoding.ShiftJIS, '\0');
            var existsBgi = reader.ReadBoolean();
            pmm.BackGround.Image = existsBgi ? bgiPath : null;

            // 描画設定の読込
            pmm.RenderConfig.InfomationVisible = reader.ReadBoolean();
            pmm.RenderConfig.AxisVisible = reader.ReadBoolean();
            pmm.RenderConfig.EnableGrandShadow = reader.ReadBoolean();

            pmm.RenderConfig.FPSLimit = (PmmRenderConfig.FPSLimitValue)(int)reader.ReadSingle();
            pmm.RenderConfig.ScreenCaptureMode = (PmmRenderConfig.ScreenCaptureModeType)reader.ReadInt32();

            pmm.RenderConfig.PostDrawingAccessoryStartIndex = reader.ReadInt32();
            pmm.RenderConfig.GroundShadowBrightness = reader.ReadSingle();
            pmm.RenderConfig.EnableTransparentGroundShadow = reader.ReadBoolean();

            ReadPhysics(reader, pmm.Physics);
            ReadSelfShadow(reader, pmm.SelfShadow);

            Current = new("FollowingSettings", null, "Camera following settings and more");
            pmm.RenderConfig.EdgeColor = System.Drawing.Color.FromArgb(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
            pmm.BackGround.IsBlack = reader.ReadBoolean();

            int currentCameraFollowingModelIndex = reader.ReadInt32();
            int currentCameraFollowingBoneIndex = reader.ReadInt32();
            pmm.Camera.Current.FollowingModel = currentCameraFollowingModelIndex > 0 ? pmm.Models[currentCameraFollowingModelIndex] : null;
            pmm.Camera.Current.FollowingBone = pmm.Camera.Current.FollowingModel?.Bones[currentCameraFollowingBoneIndex];

            // 意図不明な謎の行列
            _ = reader.ReadBytes(64);

            pmm.PlayConfig.EnableFollowCamera = reader.ReadBoolean();

            // 意図不明な謎の値
            _ = reader.ReadByte();

            pmm.Physics.EnableGroundPhysics = reader.ReadBoolean();
            pmm.RenderConfig.JumpFrameLocation = reader.ReadInt32();

            try
            {
                // バージョン 9.24 より前ならここで終了のため、 EndOfStreamException が投げられる
                // 9.24 なら後続要素が存在するかの Boolean 値が読める
                var existSelectorChoiseSection = reader.ReadBoolean();

                // 存在しなければここ以降の情報は無意味なので読み飛ばす
                // が MMD はそんな値は吐かないと思われる
                if (existSelectorChoiseSection)
                {
                    // 範囲選択セレクタの読込
                    for (int i = 0; i < modelCount; i++)
                    {
                        Current = new("RangeSelector", i, $"The section of {DataLoadErrorInfomation.GetOrdinal(i)} range selection target.");
                        pmm.Models[reader.ReadByte()].SpecificEditorState.RangeSelector = new(reader.ReadInt32());
                    }
                }
            }
            catch (EndOfStreamException)
            {
                // このセクションは途中でファイルが終わってても構わないので
                // ストリームの終わり例外なら来ても何もしなくてよい
            }
        }
        catch (Exception ex)
        {
            IOException exception = new($"Failed to read PMM file. This exception occurred in {Current.Section}. See Data[\"Section\"] property, that type is DataLoadErrorInfomation, of this exception for details on where exceptions are occurred.", ex);
            exception.Data.Add("Section", Current);
            throw exception;
        }
        finally
        {
            OuterParentRelation = new();
            OuterParentRelationCurrent = new();
        }
    }

    private static void ReadCamera(BinaryReader reader, PolygonMovieMaker pmm)
    {

        var camera = pmm.Camera;

        Current = new("CameraFrame", null, $"The section of initial camera frame data.");
        camera.Frames.Add(ReadCameraFrame(reader, pmm, true));

        var cameraFrameCount = reader.ReadInt32();
        for (int i = 0; i < cameraFrameCount; i++)
        {
            Current = Current with { Index = i, Description = $"The section of {DataLoadErrorInfomation.GetOrdinal(i)} camera frame data." };
            camera.Frames.Add(ReadCameraFrame(reader, pmm));
        }

        Current = new("CurrentCamera", null, $"The section of camera settings.");
        camera.Current.EyePosition = reader.ReadVector3();
        camera.Current.TargetPosition = reader.ReadVector3();
        camera.Current.Rotation = reader.ReadVector3();
        camera.Current.DisablePerspective = reader.ReadBoolean();
    }
    private static PmmCameraFrame ReadCameraFrame(BinaryReader reader, PolygonMovieMaker pmm, bool isInitial = false)
    {
        // リストの添え字で管理できるため不要なフレームインデックスを破棄
        if (!isInitial) _ = reader.ReadInt32();

        var frame = new PmmCameraFrame();

        frame.Frame = reader.ReadInt32();

        // 所属が確実にわかるので pre/next ID から探索してやる必要性がないため破棄
        _ = reader.ReadInt32();
        _ = reader.ReadInt32();

        frame.Distance = reader.ReadSingle();
        frame.EyePosition = reader.ReadVector3();
        frame.Rotation = reader.ReadVector3();

        int followingModelIndex = reader.ReadInt32();
        int followingBoneIndex = reader.ReadInt32();

        frame.FollowingModel = followingModelIndex < 0 ? null : pmm.Models[followingModelIndex];
        frame.FollowingBone = frame.FollowingModel?.Bones[followingBoneIndex];

        frame.InterpolationCurves[InterpolationItem.XPosition].FromBytes(reader.ReadBytes(4));
        frame.InterpolationCurves[InterpolationItem.YPosition].FromBytes(reader.ReadBytes(4));
        frame.InterpolationCurves[InterpolationItem.ZPosition].FromBytes(reader.ReadBytes(4));
        frame.InterpolationCurves[InterpolationItem.Rotation].FromBytes(reader.ReadBytes(4));
        frame.InterpolationCurves[InterpolationItem.Distance].FromBytes(reader.ReadBytes(4));
        frame.InterpolationCurves[InterpolationItem.ViewAngle].FromBytes(reader.ReadBytes(4));

        frame.DisablePerspective = reader.ReadBoolean();
        frame.ViewAngle = reader.ReadInt32();

        frame.IsSelected = reader.ReadBoolean();

        return frame;
    }

    private static void ReadLight(BinaryReader reader, PmmLight light)
    {
        Current = new("LightFrame", null, $"The section of initial light frame data.");
        light.Frames.Add(ReadLightFrame(reader, true));

        var frameCount = reader.ReadInt32();
        for (int i = 0; i < frameCount; i++)
        {
            Current = Current with { Index = i, Description = $"The section of {DataLoadErrorInfomation.GetOrdinal(i)} light frame data." };
            light.Frames.Add(ReadLightFrame(reader));
        }

        Current = new("CurrentLight", null, $"The section of light settings.");
        light.Current.Color = reader.ReadSingleRGB();
        light.Current.Position = reader.ReadVector3();
    }
    private static PmmLightFrame ReadLightFrame(BinaryReader reader, bool isInitial = false)
    {
        // リストの添え字で管理できるため不要なフレームインデックスを破棄
        if (!isInitial) _ = reader.ReadInt32();

        var frame = new PmmLightFrame();

        frame.Frame = reader.ReadInt32();
        // 所属が確実にわかるので pre/next ID から探索してやる必要性がないため破棄
        _ = reader.ReadInt32();
        _ = reader.ReadInt32();

        frame.Color = reader.ReadSingleRGB();
        frame.Position = reader.ReadVector3();

        frame.IsSelected = reader.ReadBoolean();

        return frame;
    }

    private static void ReadSelfShadow(BinaryReader reader, PmmSelfShadow selfShadow)
    {
        Current = new("SelfShadow", null, $"The section of self shadow config and frames");
        selfShadow.EnableSelfShadow = reader.ReadBoolean();
        selfShadow.ShadowRange = reader.ReadSingle();

        selfShadow.Frames.Add(ReadSelfShadowFrame(reader, true));
        var frameCount = reader.ReadInt32();
        for (int i = 0; i < frameCount; i++)
        {
            selfShadow.Frames.Add(ReadSelfShadowFrame(reader));
        }
    }
    private static PmmSelfShadowFrame ReadSelfShadowFrame(BinaryReader reader, bool isInitial = false)
    {
        // リストの添え字で管理できるため不要なフレームインデックスを破棄
        if (!isInitial) _ = reader.ReadInt32();

        var frame = new PmmSelfShadowFrame();
        frame.Frame = reader.ReadInt32();
        // 所属が確実にわかるので pre/next ID から探索してやる必要性がないため破棄
        _ = reader.ReadInt32();
        _ = reader.ReadInt32();

        frame.ShadowMode = (SelfShadow)reader.ReadByte();
        frame.ShadowRange = reader.ReadSingle();

        frame.IsSelected = reader.ReadBoolean();

        return frame;
    }

    private static void ReadPhysics(BinaryReader reader, PmmPhysics physics)
    {
        Current = new("Physics", null, $"The section of physics config and gravity frames");
        physics.CalculationMode = (PmmPhysics.PhysicsMode)reader.ReadByte();
        physics.CurrentGravity.Acceleration = reader.ReadSingle();
        var currentNoiseAmount = reader.ReadInt32();
        physics.CurrentGravity.Direction = reader.ReadVector3();
        var enableNoise = reader.ReadBoolean();
        physics.CurrentGravity.Noize = enableNoise ? currentNoiseAmount : null;

        physics.GravityFrames.Add(ReadGravityFrame(reader, true));
        var gravityFrameCount = reader.ReadInt32();
        for (int i = 0; i < gravityFrameCount; i++)
        {
            physics.GravityFrames.Add(ReadGravityFrame(reader));
        }
    }
    private static PmmGravityFrame ReadGravityFrame(BinaryReader reader, bool isInitial = false)
    {
        // リストの添え字で管理できるため不要なフレームインデックスを破棄
        if (!isInitial) _ = reader.ReadInt32();

        var frame = new PmmGravityFrame();
        frame.Frame = reader.ReadInt32();
        // 所属が確実にわかるので pre/next ID から探索してやる必要性がないため破棄
        _ = reader.ReadInt32();
        _ = reader.ReadInt32();

        var enableNoise = reader.ReadBoolean();
        var noiseAmount = reader.ReadInt32();
        frame.Noize = enableNoise ? noiseAmount : null;
        frame.Acceleration = reader.ReadSingle();
        frame.Direction = reader.ReadVector3();

        frame.IsSelected = reader.ReadBoolean();

        return frame;
    }

    private static (PmmAccessory Accessory, byte RenderOrder) ReadAccessory(BinaryReader reader, PolygonMovieMaker pmm)
    {
        byte renderOrder;

        // リストの添字で管理するため Index は破棄
        _ = reader.ReadByte();
        var name = reader.ReadString(100, Encoding.ShiftJIS, '\0');
        var path = reader.ReadString(256, Encoding.ShiftJIS, '\0');
        var acs = new PmmAccessory(name, path);

        renderOrder = reader.ReadByte();

        acs.Frames.Add(ReadAccessoryFrame(reader, pmm, true));

        var accessoryCount = reader.ReadInt32();
        for (int i = 0; i < accessoryCount; i++)
        {
            acs.Frames.Add(ReadAccessoryFrame(reader, pmm));
        }

        acs.Current.TransAndVisible = reader.ReadByte();
        int parentModelIndex = reader.ReadInt32();
        int parentBoneIndex = reader.ReadInt32();
        acs.Current.ParentModel = parentModelIndex < 0 ? null : pmm.Models[parentModelIndex];
        acs.Current.ParentBone = acs.Current.ParentModel?.Bones[parentBoneIndex];

        acs.Current.Position = reader.ReadVector3();
        acs.Current.Rotation = reader.ReadVector3();
        acs.Current.Scale = reader.ReadSingle();

        acs.Current.EnableShadow = reader.ReadBoolean();

        acs.EnableAlphaBlend = reader.ReadBoolean();

        return (acs, renderOrder);
    }
    private static PmmAccessoryFrame ReadAccessoryFrame(BinaryReader reader, PolygonMovieMaker pmm, bool isInitial = false)
    {
        // リストの添え字で管理できるため不要なフレームインデックスを破棄
        if (!isInitial) _ = reader.ReadInt32();

        var frame = new PmmAccessoryFrame();

        frame.Frame = reader.ReadInt32();
        // 所属が確実にわかるので pre/next ID から探索してやる必要性がないため破棄
        _ = reader.ReadInt32();
        _ = reader.ReadInt32();

        frame.TransAndVisible = reader.ReadByte();

        int parentModelIndex = reader.ReadInt32();
        int parentBoneIndex = reader.ReadInt32();

        frame.ParentModel = parentModelIndex < 0 ? null : pmm.Models[parentModelIndex];
        frame.ParentBone = frame.ParentModel?.Bones[parentBoneIndex];

        frame.Position = reader.ReadVector3();
        frame.Rotation = reader.ReadVector3();
        frame.Scale = reader.ReadSingle();

        frame.EnableShadow = reader.ReadBoolean();
        frame.IsSelected = reader.ReadBoolean();

        return frame;
    }

    private static (PmmModel Model, byte RenderOrder, byte CalculateOrder) ReadModel(BinaryReader reader)
    {
        var model = new PmmModel();
        byte renderOrder;

        // モデルのインデックス
        // 手動で書き換える理由はなく、書込時に自動計算できるので破棄
        _ = reader.ReadByte();

        model.Name = reader.ReadString();
        model.NameEn = reader.ReadString();
        model.Path = reader.ReadString(256, Encoding.ShiftJIS, '\0');

        // キーフレームエディタの行数
        // 表示枠数から求まるので破棄
        _ = reader.ReadByte();

        var boneCount = reader.ReadInt32();
        for (int i = 0; i < boneCount; i++)
        {
            Current = new("Bone", i, $"The section of {DataLoadErrorInfomation.GetOrdinal(i)} bone data in {model.Name}.");
            model.Bones.Add(new PmmBone(reader.ReadString()));
        }

        var morphCount = reader.ReadInt32();
        for (int i = 0; i < morphCount; i++)
        {
            Current = new("Morph", i, $"The section of {DataLoadErrorInfomation.GetOrdinal(i)} morph data in {model.Name}.");
            model.Morphs.Add(new PmmMorph(reader.ReadString()));
        }

        Current = new("IK", null, $"The section of IK bone indices.");
        var ikCount = reader.ReadInt32();
        var ikIndices = Enumerable.Range(0, ikCount).Select(_ => reader.ReadInt32()).ToArray();
        foreach (var i in ikIndices)
        {
            model.Bones[i].IsIK = true;
        }

        Current = new("ParentableBone", null, $"The section of outer parentable bone indices.");
        var parentableBoneCount = reader.ReadInt32();
        var parentableIndices = Enumerable.Range(0, parentableBoneCount).Select(_ => reader.ReadInt32()).ToArray();
        // なぜか最初に -1 が入っているのでそれは飛ばす
        foreach (var i in parentableIndices.Skip(1))
        {
            model.Bones[i].CanBecomeOuterParent = true;
        }

        Current = new("RenderOrder", null, $"The section of render order.");
        renderOrder = reader.ReadByte();
        model.CurrentConfig.Visible = reader.ReadBoolean();

        Current = new("SelectedItem", null, $"The section of selected bone/morph index.");
        int selectedBoneIndex = reader.ReadInt32();
        int selectedBrowMorphIndex = reader.ReadInt32();
        int selectedEyeMorphIndex = reader.ReadInt32();
        int selectedLipMorphIndex = reader.ReadInt32();
        int selectedOtherMorphIndex = reader.ReadInt32();

        if (selectedBoneIndex >= 0)
            model.SelectedBone = model.Bones[selectedBoneIndex];
        if (selectedBrowMorphIndex >= 0)
            model.SelectedBrowMorph = model.Morphs[selectedBrowMorphIndex];
        if (selectedEyeMorphIndex >= 0)
            model.SelectedEyeMorph = model.Morphs[selectedEyeMorphIndex];
        if (selectedLipMorphIndex >= 0)
            model.SelectedLipMorph = model.Morphs[selectedLipMorphIndex];
        if (selectedOtherMorphIndex >= 0)
            model.SelectedOtherMorph = model.Morphs[selectedOtherMorphIndex];

        var nodeCount = reader.ReadByte();
        for (int i = 0; i < nodeCount; i++)
        {
            Current = new("Morph", i, $"The section of {DataLoadErrorInfomation.GetOrdinal(i)} node.");
            model.Nodes.Add(new() { DoesOpen = reader.ReadBoolean() });
        }

        Current = new("SpecificEditorState", null, $"The section of specific editor state.");
        model.SpecificEditorState.VerticalScrollState = reader.ReadInt32();
        model.SpecificEditorState.LastFrame = reader.ReadInt32();

        // 初期ボーンフレームの読込
        var boneFrameDictionary = new Dictionary<PmmBone, int?>();
        foreach (var (bone, i) in model.Bones.Select((b, i) => (b,i)))
        {
            Current = new("InitialBoneFrame", i, $"The section of {DataLoadErrorInfomation.GetOrdinal(i)} initial bone frame data in {bone.Name}.");
            (var frame, _, var nextId) = ReadBoneFrame(reader, true);
            bone.Frames.Add(frame);
            boneFrameDictionary.Add(bone, nextId);
        }

        // ボーンフレームの読込
        ReadFramesThatRequireResolving(
            reader,
            model.Bones,
            boneCount,
            static reader => ReadBoneFrame(reader),
            boneFrameDictionary,
            static (element, frame) => element?.Frames.Add((PmmBoneFrame)frame)
        );

        // 初期モーフフレームの読込
        var morphFrameDictionary = new Dictionary<PmmMorph, int?>();
        foreach (var (morph, i) in model.Morphs.Select((m, i) => (m, i)))
        {
            Current = new("InitialMorphFrame", i, $"The section of {DataLoadErrorInfomation.GetOrdinal(i)} initial morph frame data in {morph.Name}.");
            (var frame, _, var nextId) = ReadMorphFrame(reader, true);
            morph.Frames.Add(frame);
            morphFrameDictionary.Add(morph, nextId);
        }

        // モーフフレームの読込
        ReadFramesThatRequireResolving(
            reader,
            model.Morphs,
            morphCount,
            static reader => ReadMorphFrame(reader),
            morphFrameDictionary,
            static (element, frame) => element?.Frames.Add((PmmMorphFrame)frame)
        );

        Current = new("InitialConfigFrame", null, $"The section of initial config frame in {model.Name}.");
        model.ConfigFrames.Add(ReadConfigFrame(reader, model, ikIndices, parentableIndices, true));

        var configFrameCount = reader.ReadInt32();
        for (int i = 0; i < configFrameCount; i++)
        {
            Current = new("ConfigFrame", i, $"The section of {DataLoadErrorInfomation.GetOrdinal(i)} initial config frame data in {model.Name}.");
            model.ConfigFrames.Add(ReadConfigFrame(reader, model, ikIndices, parentableIndices));
        }

        foreach (var (bone, i) in model.Bones.Select((b, i) => (b, i)))
        {
            Current = new("CurrentBone", i, $"The section of {DataLoadErrorInfomation.GetOrdinal(i)} current {bone.Name} bone state");
            bone.Current.Movement = reader.ReadVector3();
            bone.Current.Rotation = reader.ReadQuaternion();
            bone.IsCommitted = reader.ReadBoolean();
            bone.Current.EnablePhysic = reader.ReadBoolean();
            bone.IsSelected = reader.ReadBoolean();
        }

        foreach (var (morph, i) in model.Morphs.Select((m, i) => (m, i)))
        {
            Current = new("CurrentMorph", i, $"The section of {DataLoadErrorInfomation.GetOrdinal(i)} current {morph.Name} morph state");
            morph.Current.Weight = reader.ReadSingle();
        }

        foreach (var i in ikIndices)
        {
            Current = new("CurrentConfig", i, $"The section of {DataLoadErrorInfomation.GetOrdinal(i)} current config state (IK state of {model.Bones[i].Name}).");
            model.CurrentConfig.EnableIK.Add(model.Bones[i], reader.ReadBoolean());
        }

        OuterParentRelationCurrent.Add(model.CurrentConfig, new());
        foreach (var i in parentableIndices)
        {
            Current = new("OuterParentState", i, $"The section of {DataLoadErrorInfomation.GetOrdinal(i)} outer parent state");
            var op = new PmmOuterParentState();
            op.StartFrame = reader.ReadInt32();
            op.EndFrame = reader.ReadInt32();
            (int, int) relation = (reader.ReadInt32(), reader.ReadInt32());
            if (i >= 0)
            {
                OuterParentRelationCurrent[model.CurrentConfig].Add(model.Bones[i], relation);
                model.CurrentConfig.OuterParent.Add(model.Bones[i], op);
            }
        }

        Current = new("DrawInfo", null, "The section of draw information.");
        model.EnableAlphaBlend = reader.ReadBoolean();
        model.EdgeWidth = reader.ReadSingle();
        model.EnableSelfShadow = reader.ReadBoolean();
        var calculateOrder = reader.ReadByte();

        return (model, renderOrder, calculateOrder);
    }
    private static PmmModelConfigFrame ReadConfigFrame(BinaryReader reader, PmmModel model, IEnumerable<int> ikIndices, IEnumerable<int> parentableIndices, bool isInitial = false)
    {
        // リストの添え字で管理できるため不要なフレームインデックスを破棄
        if (!isInitial) _ = reader.ReadInt32();

        var frame = new PmmModelConfigFrame();
        OuterParentRelation.Add(frame, new());

        frame.Frame = reader.ReadInt32();

        // ConfigFrame は属するモデルが確実にわかるので pre/next ID から探索してやる必要性がない
        // なので pre/next ID は破棄する
        _ = reader.ReadInt32();
        _ = reader.ReadInt32();

        frame.Visible = reader.ReadBoolean();

        foreach (var i in ikIndices)
        {
            frame.EnableIK[model.Bones[i]] = reader.ReadBoolean();
        }

        foreach (var i in parentableIndices)
        {
            (int, int) relation = (reader.ReadInt32(), reader.ReadInt32());
            // parentableIndices 最初の要素は -1 なので飛ばす
            if (i >= 0)
                OuterParentRelation[frame].Add(model.Bones[i], relation);
        }

        frame.IsSelected = reader.ReadBoolean();

        return frame;
    }
    private static (PmmMorphFrame Frame, int PreviousFrameIndex, int NextFrameIndex) ReadMorphFrame(BinaryReader reader, bool isInitial = false)
    {
        // リストの添え字で管理できるため不要なフレームインデックスを破棄
        if (!isInitial) _ = reader.ReadInt32();

        var frame = new PmmMorphFrame();

        frame.Frame = reader.ReadInt32();

        var preID = reader.ReadInt32();
        var nextId = reader.ReadInt32();

        frame.Weight = reader.ReadSingle();
        frame.IsSelected = reader.ReadBoolean();

        return (frame, preID, nextId);
    }
    private static (PmmBoneFrame Frame, int PreviousFrameIndex, int NextFrameIndex) ReadBoneFrame(BinaryReader reader, bool isInitial = false)
    {
        // リストの添え字で管理できるため不要なフレームインデックスを破棄
        if (!isInitial) _ = reader.ReadInt32();

        var frame = new PmmBoneFrame();

        frame.Frame = reader.ReadInt32();
        var previousFrameIndex = reader.ReadInt32();
        var nextFrameIndex = reader.ReadInt32();

        frame.InterpolationCurves[InterpolationItem.XPosition].FromBytes(reader.ReadBytes(4));
        frame.InterpolationCurves[InterpolationItem.YPosition].FromBytes(reader.ReadBytes(4));
        frame.InterpolationCurves[InterpolationItem.ZPosition].FromBytes(reader.ReadBytes(4));
        frame.InterpolationCurves[InterpolationItem.Rotation].FromBytes(reader.ReadBytes(4));

        frame.Movement = reader.ReadVector3();
        frame.Rotation = reader.ReadQuaternion();
        frame.IsSelected = reader.ReadBoolean();
        frame.EnablePhysic = !reader.ReadBoolean();

        return (frame, previousFrameIndex, nextFrameIndex);
    }

    /// <summary>
    /// ボーン/モーフの初期以外のフレームを読み込む
    /// <para>フレームの属する要素が前後フレームIDによって管理されているので、各要素内のフレームコレクションに投入するための解決処理を実施する</para>
    /// </summary>
    /// <param name="reader">バイナリ読込クラス</param>
    /// <param name="targetElements">フレーム追加対象になるPMM要素のコレクション</param>
    /// <param name="elementCount">targetElements の要素数</param>
    /// <param name="readElementFrame">フレーム読込メソッドの呼び出し関数</param>
    /// <param name="elementNextFrameDictionary">要素名とそれに対応する次フレームIDの辞書</param>
    /// <param name="addFrame">所属要素にフレームを追加する関数</param>
    private static void ReadFramesThatRequireResolving<T>(
        BinaryReader reader,
        IEnumerable<T> targetElements,
        int elementCount,
        Func<BinaryReader, (IPmmFrame Frame, int PreviousFrameIndex, int NextFrameIndex)> readElementFrame,
        Dictionary<T, int?> elementNextFrameDictionary,
        Action<T, IPmmFrame> addFrame)
     where T: IPmmModelElement{
        var elementFrameCount = reader.ReadInt32();
        var elementFrames = Enumerable.Range(0, elementFrameCount).Select(i =>
        {
            Current = new("Frame", i, $"The section of {DataLoadErrorInfomation.GetOrdinal(i)} {typeof(T).Name} frame data.");
            return readElementFrame(reader);
        }).ToArray();

        // 最初から next が 0 である場合 null に変えておく
        foreach (var elmNext in elementNextFrameDictionary)
        {
            if (elmNext.Value.HasValue && elmNext.Value.Value == 0)
                elementNextFrameDictionary[elmNext.Key] = null;
        }

        var AreThereElementLeftThatRequiredFrameSearch = true;
        while (AreThereElementLeftThatRequiredFrameSearch)
        {
            // 論理和代入演算子でループ継続判定をしたいのでまず false にしておく
            AreThereElementLeftThatRequiredFrameSearch = false;

            foreach (var element in targetElements)
            {
                var nextIndex = elementNextFrameDictionary[element];
                if (nextIndex is null) continue;

                // 読み込んだインデックスは初期フレームの数だけ先に進んでいるので
                // 初期フレーム数(= 要素数)の分だけ引いたのがモデル内フレームコレクションでのインデックスになる
                var nextFrame = elementFrames.ElementAt(nextIndex.Value - elementCount);

                addFrame(element, nextFrame.Frame);

                // この要素の次のフレームのインデックスを更新する
                // 次のインデックスが 0 なら次の要素は無いので null を入れる
                elementNextFrameDictionary[element] = nextFrame.NextFrameIndex == 0 ? null : nextFrame.NextFrameIndex;

                // 一つでも次のフレーム探索が必要なボーンがあればループを続ける
                // 次のインデックスに null が入っていればフレーム探索は不要の意味になる
                AreThereElementLeftThatRequiredFrameSearch |= elementNextFrameDictionary[element].HasValue;
            }
        }
    }
}
