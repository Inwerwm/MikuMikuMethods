using MikuMikuMethods.Common;
using MikuMikuMethods.Extension;
using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;

namespace MikuMikuMethods.Pmm.IO;

/// <summary>
/// PMMファイルの読込クラス
/// </summary>
public static class PmmFileReader
{
    /// <summary>
    /// 読込セクションの変更時イベントのイベントハンドラ
    /// </summary>
    /// <param name="section">変更後のイベント</param>
    public delegate void OnSectionChangeEventHandler(DataSection section);
    /// <summary>
    /// 読込セクションの変更イベント
    /// </summary>
    public static event OnSectionChangeEventHandler? OnChangeSection;

    private static Dictionary<PmmModelConfigFrame, Dictionary<PmmBone, (int ModelID, int BoneID)>> OutsideParentRelation { get; set; } = new();
    private static Dictionary<PmmModelConfigState, Dictionary<PmmBone, (int ModelID, int BoneID)>> OutsideParentRelationCurrent { get; set; } = new();
    private static Dictionary<int, PmmModel> ModelIdMap { get; set; } = new();

    private static DataSection _current = new("", null, "");
    /// <summary>
    /// 現在の読込セクション
    /// </summary>
    public static DataSection Current
    {
        get => _current;
        private set
        {
            _current = value;
            OnChangeSection?.Invoke(value);
        }
    }

    /// <summary>
    /// PMM ファイルの読込
    /// </summary>
    /// <param name="filePath">ファイルパス</param>
    /// <param name="pmm">情報書込み対象の PMM データ</param>
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

    /// <summary>
    /// PMM ファイルの読込
    /// </summary>
    /// <param name="reader">読込用バイナリリーダー</param>
    /// <param name="pmm">情報書込み対象の PMM データ</param>
    public static void Read(BinaryReader reader, PolygonMovieMaker pmm)
    {
        try
        {
            ReadHeader(reader, pmm);

            ReadPanelOpeningStatus(reader, pmm.PanelPane);

            ReadModels(reader, pmm);

            ReadCamera(reader, pmm);

            ReadLight(reader, pmm.Light);

            ReadAccessories(reader, pmm);

            ReadKeyFrameEditorCurrentTarget(reader, pmm.EditorState);

            ReadMedia(reader, pmm);

            ReadPhysics(reader, pmm.Physics);

            ReadSelfShadow(reader, pmm.SelfShadow);

            ReadRenderingColorSettings(reader, pmm);

            ReadFollowingSettings(reader, pmm);

            ReadExtendedData(reader, pmm);
        }
        catch (Exception ex)
        {
            IOException exception = new($"Failed to read PMM file. This exception occurred in {Current.Name}. See Data[\"Section\"] property, that type is {Current.GetType().Name}, of this exception for details on where exceptions are occurred.", ex);
            exception.Data.Add("Section", Current);
            throw exception;
        }
        finally
        {
            OutsideParentRelation = new();
            OutsideParentRelationCurrent = new();
            ModelIdMap = new();
        }
    }

    private static void ReadHeader(BinaryReader reader, PolygonMovieMaker pmm)
    {
        Current = new("Header", null, "Header section of the PMM file.");
        pmm.Version = reader.ReadString(30, Encoding.ShiftJIS, '\0');
        if (pmm.Version != "Polygon Movie maker 0002") throw new InvalidDataException("This is not PMM file.");
        pmm.OutputResolution = new(reader.ReadInt32(), reader.ReadInt32());

        pmm.EditorState.Width = reader.ReadInt32();
        pmm.Camera.Current.ViewAngle = (int)reader.ReadSingle();
        pmm.EditorState.IsCameraMode = reader.ReadBoolean();
    }

    private static void ReadPanelOpeningStatus(BinaryReader reader, PmmPanelPane panelPane)
    {
        Current = new("PanelOpeningStatus", null, "Section of the opening status of various operation panels.");
        // パネル開閉状態の読み込み
        panelPane.DoesOpenCameraPanel = reader.ReadBoolean();
        panelPane.DoesOpenLightPanel = reader.ReadBoolean();
        panelPane.DoesOpenAccessaryPanel = reader.ReadBoolean();
        panelPane.DoesOpenBonePanel = reader.ReadBoolean();
        panelPane.DoesOpenMorphPanel = reader.ReadBoolean();
        panelPane.DoesOpenSelfShadowPanel = reader.ReadBoolean();
    }

    private static void ReadModels(BinaryReader reader, PolygonMovieMaker pmm)
    {
        Current = new("HeaderOfModels", null, "Section of selected model index and total number of models.");
        // モデル読み込み
        var selectedModelIndex = reader.ReadByte();
        var modelCount = reader.ReadByte();
        var modelOrderDictionary = new Dictionary<PmmModel, (byte RenderOrder, byte CalculateOrder)>();
        for (int i = 0; i < modelCount; i++)
        {
            Current = new("Model", i, $"Section of {DataSection.GetOrdinal(i)} model data.");
            (var model, var modelId, var renderOrder, var calculateOrder) = ReadModel(reader);
            pmm.Models.Add(model);
            ModelIdMap.Add(modelId, model);
            modelOrderDictionary.Add(model, ((byte RenderOrder, byte CalculateOrder))(renderOrder - 1, calculateOrder - 1));
        }

        Current = new($"ModelRelationSolving", null, $"The section that resolves the relations of the selected model and the render/calculate order. In this section, no reading is done, only calculation.");
        ModelIdMap.TryGetValue(selectedModelIndex, out var selectedModel);
        pmm.EditorState.SelectedModel = selectedModel;

        // PMM 内の順序数値上限は必ずしもモデル数と一致しているわけではないので連番を振り直す
        var sortedRenderOrders = modelOrderDictionary.OrderBy(p => p.Value.RenderOrder).Select((p, i) => (p.Key, (byte)i)).ToDictionary(p => p.Key, p => p.Item2);
        var sortedCalculateOrders = modelOrderDictionary.OrderBy(p => p.Value.CalculateOrder).Select((p, i) => (p.Key, (byte)i)).ToDictionary(p => p.Key, p => p.Item2);

        foreach (var model in pmm.Models)
        {
            pmm.SetRenderOrder(model, sortedRenderOrders[model]);
            pmm.SetCalculateOrder(model, sortedCalculateOrders[model]);
        }

        // 外部親の関係解決
        Current = new("OutsideParentSolving", null, $"The section that resolves the outside parent relation. In this section, no reading is done, only calculation.");

        foreach (var frame in OutsideParentRelation)
        {
            foreach (var relation in frame.Value)
            {
                var (opModel, opBone) = resolveOutsideParent(relation);

                frame.Key.OutsideParent.Add(relation.Key, new()
                {
                    ParentModel = opModel,
                    ParentBone = opBone
                });
            }
        }

        foreach (var state in OutsideParentRelationCurrent)
        {
            foreach (var relation in state.Value)
            {
                var (opModel, opBone) = resolveOutsideParent(relation);

                state.Key.OutsideParent[relation.Key].ParentModel = opModel;
                state.Key.OutsideParent[relation.Key].ParentBone = opBone;
            }
        }

        static (PmmModel? opModel, PmmBone? opBone) resolveOutsideParent(KeyValuePair<PmmBone, (int ModelID, int BoneID)> relation)
        {
            var opModel = relation.Value.ModelID switch
            {
                -2 => new PmmGroundAsParent(),
                < 0 => null,
                int id => ModelIdMap[id]
            };
            var opBone = (relation.Value.BoneID < 0 || opModel is PmmGroundAsParent) ? null : opModel?.Bones[relation.Value.BoneID];

            return (opModel, opBone);
        }
    }

    private static void ReadCamera(BinaryReader reader, PolygonMovieMaker pmm)
    {
        var camera = pmm.Camera;

        Current = new("InitialCameraFrame", null, $"The section of initial camera frame data.");
        camera.Frames.Add(ReadCameraFrame(reader, pmm, true));

        Current = new("CameraFrameCount", null, $"The section of count of camera frames.");
        var cameraFrameCount = reader.ReadInt32();
        for (int i = 0; i < cameraFrameCount; i++)
        {
            Current = new("CameraFrame", i, $"The section of {DataSection.GetOrdinal(i)} camera frame data.");
            camera.Frames.Add(ReadCameraFrame(reader, pmm));
        }

        Current = new("CurrentCamera", null, $"The section of camera settings.");
        camera.Current.EyePosition = reader.ReadVector3();
        camera.Current.TargetPosition = reader.ReadVector3();
        camera.Current.Rotation = reader.ReadVector3();
        camera.Current.DisablePerspective = reader.ReadBoolean();
    }

    private static void ReadLight(BinaryReader reader, PmmLight light)
    {
        Current = new("LightFrame", null, $"The section of initial light frame data.");
        light.Frames.Add(ReadLightFrame(reader, true));

        Current = new("LightFrameCount", null, $"The section of count of light frames.");
        var frameCount = reader.ReadInt32();
        for (int i = 0; i < frameCount; i++)
        {
            Current = Current with { Index = i, Description = $"The section of {DataSection.GetOrdinal(i)} light frame data." };
            light.Frames.Add(ReadLightFrame(reader));
        }

        Current = new("CurrentLight", null, $"The section of light settings.");
        light.Current.Color = reader.ReadSingleRGB();
        light.Current.Position = reader.ReadVector3();
    }

    private static void ReadAccessories(BinaryReader reader, PolygonMovieMaker pmm)
    {
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
            Current = new($"Accessory", i, $"The section of {DataSection.GetOrdinal(i)} accessory data.");
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
    }

    private static void ReadKeyFrameEditorCurrentTarget(BinaryReader reader, PmmEditorState editorState)
    {
        Current = new($"KeyFrameEditorCurrentTarget", null, $"Sections for the current keyframe editor editing target and scroll amount.");
        // フレーム編集画面の状態読み込み
        editorState.CurrentFrame = reader.ReadInt32();
        editorState.HorizontalScroll = reader.ReadInt32();
        editorState.HorizontalScrollLength = reader.ReadInt32();
        editorState.SelectedBoneOperation = (PmmEditorState.BoneOperation)reader.ReadInt32();
    }

    private static void ReadMedia(BinaryReader reader, PolygonMovieMaker pmm)
    {
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
        pmm.RenderConfig.InformationVisible = reader.ReadBoolean();
        pmm.RenderConfig.AxisVisible = reader.ReadBoolean();
        pmm.RenderConfig.EnableGrandShadow = reader.ReadBoolean();

        pmm.RenderConfig.FPSLimit = (PmmRenderConfig.FPSLimitValue)(int)reader.ReadSingle();
        pmm.RenderConfig.ScreenCaptureMode = (PmmRenderConfig.ScreenCaptureModeType)reader.ReadInt32();

        pmm.RenderConfig.PostDrawingAccessoryStartIndex = reader.ReadInt32();
        pmm.RenderConfig.GroundShadowBrightness = reader.ReadSingle();
        pmm.RenderConfig.EnableTransparentGroundShadow = reader.ReadBoolean();
    }

    private static void ReadPhysics(BinaryReader reader, PmmPhysics physics)
    {
        Current = new("Physics", null, $"The section of physics config");
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
            Current = new("GravityFrame", i, $"The section of {DataSection.GetOrdinal(i)} gravity frame data.");
            physics.GravityFrames.Add(ReadGravityFrame(reader));
        }
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
            Current = new("SelfShadowFrame", i, $"The section of {DataSection.GetOrdinal(i)} self shadow frame data.");
            selfShadow.Frames.Add(ReadSelfShadowFrame(reader));
        }
    }

    private static void ReadRenderingColorSettings(BinaryReader reader, PolygonMovieMaker pmm)
    {
        Current = new("RenderingColorSettings", null, "Read edge color and background color settings");
        pmm.RenderConfig.EdgeColor = System.Drawing.Color.FromArgb(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
        pmm.BackGround.IsBlack = reader.ReadBoolean();
    }

    private static void ReadFollowingSettings(BinaryReader reader, PolygonMovieMaker pmm)
    {
        Current = new("FollowingSettings", null, "Camera following settings, and information following with the tail of the data");
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
    }

    private static void ReadExtendedData(BinaryReader reader, PolygonMovieMaker pmm)
    {
        Current = new("ExtendedData", null, "Sections of data extended to version 9.24");
        try
        {
            // バージョン 9.24 より前ならここで終了のため、 EndOfStreamException が投げられる
            // 9.24 なら後続要素が存在するかの Boolean 値が読める
            var existSelectorChoiceSection = reader.ReadBoolean();

            // 存在しなければここ以降の情報は無意味なので読み飛ばす
            // が MMD はそんな値は吐かないと思われる
            if (existSelectorChoiceSection)
            {
                // 範囲選択セレクタの読込
                for (int i = 0; i < pmm.Models.Count; i++)
                {
                    Current = new("RangeSelector", i, $"The section of {DataSection.GetOrdinal(i)} range selection target.");
                    ModelIdMap[reader.ReadByte()].SpecificEditorState.RangeSelector = new(reader.ReadInt32());
                }
            }
        }
        catch (EndOfStreamException)
        {
            // このセクションは途中でファイルが終わってても構わないので
            // ストリームの終わり例外なら来ても何もしなくてよい
        }
    }

    private static (PmmModel Model, byte modelId, byte RenderOrder, byte CalculateOrder) ReadModel(BinaryReader reader)
    {
        var model = new PmmModel();
        byte renderOrder;

        var modelId = reader.ReadByte();

        model.Name = reader.ReadString();
        model.NameEn = reader.ReadString();
        model.Path = reader.ReadString(256, Encoding.ShiftJIS, '\0');

        // キーフレームエディタの行数
        // 表示枠数から求まるので破棄
        _ = reader.ReadByte();

        Current = new("BoneCount", null, $"The section of count of bones.");
        var boneCount = reader.ReadInt32();
        for (int i = 0; i < boneCount; i++)
        {
            Current = new("Bone", i, $"The section of {DataSection.GetOrdinal(i)} bone data in {model.Name}.");
            model.Bones.Add(new PmmBone(reader.ReadString()));
        }

        Current = new("MorphCount", null, $"The section of count of morphs.");
        var morphCount = reader.ReadInt32();
        for (int i = 0; i < morphCount; i++)
        {
            Current = new("Morph", i, $"The section of {DataSection.GetOrdinal(i)} morph data in {model.Name}.");
            model.Morphs.Add(new PmmMorph(reader.ReadString()));
        }

        Current = new("IK", null, $"The section of IK bone indices.");
        var ikCount = reader.ReadInt32();
        var ikIndices = Enumerable.Range(0, ikCount).Select(_ => reader.ReadInt32()).ToArray();
        foreach (var i in ikIndices)
        {
            model.Bones[i].IsIK = true;
        }

        Current = new("ParentableBone", null, $"The section of outside parentable bone indices.");
        var parentableBoneCount = reader.ReadInt32();
        var parentableIndices = Enumerable.Range(0, parentableBoneCount).Select(_ => reader.ReadInt32()).ToArray();
        // なぜか最初に -1 が入っているのでそれは飛ばす
        foreach (var i in parentableIndices.Skip(1))
        {
            model.Bones[i].CanSetOutsideParent = true;
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

        Current = new("NodeCount", null, $"The section of count of nodes.");
        var nodeCount = reader.ReadByte();
        for (int i = 0; i < nodeCount; i++)
        {
            Current = new("Node", i, $"The section of {DataSection.GetOrdinal(i)} node.");
            model.Nodes.Add(new() { DoesOpen = reader.ReadBoolean() });
        }

        Current = new("SpecificEditorState", null, $"The section of specific editor state.");
        model.SpecificEditorState.VerticalScrollState = reader.ReadInt32();
        model.SpecificEditorState.LastFrame = reader.ReadInt32();

        // 初期ボーンフレームの読込
        var boneFrameDictionary = new Dictionary<PmmBone, int?>();
        foreach (var (bone, i) in model.Bones.Select((b, i) => (b, i)))
        {
            Current = new("InitialBoneFrame", i, $"The section of {DataSection.GetOrdinal(i)} initial bone frame data in {bone.Name}.");
            (var frame, _, var nextId, _) = ReadBoneFrame(reader, true);
            bone.Frames.Add(frame);
            boneFrameDictionary.Add(bone, nextId);
        }

        // ボーンフレームの読込
        AssignFramesToModelElements(
            reader,
            static reader => ReadBoneFrame(reader),
            boneFrameDictionary,
            static (element, frame) => element?.Frames.Add((PmmBoneFrame)frame)
        );

        // 初期モーフフレームの読込
        var morphFrameDictionary = new Dictionary<PmmMorph, int?>();
        foreach (var (morph, i) in model.Morphs.Select((m, i) => (m, i)))
        {
            Current = new("InitialMorphFrame", i, $"The section of {DataSection.GetOrdinal(i)} initial morph frame data in {morph.Name}.");
            (var frame, _, var nextId, _) = ReadMorphFrame(reader, true);
            morph.Frames.Add(frame);
            morphFrameDictionary.Add(morph, nextId);
        }

        // モーフフレームの読込
        AssignFramesToModelElements(
            reader,
            static reader => ReadMorphFrame(reader),
            morphFrameDictionary,
            static (element, frame) => element?.Frames.Add((PmmMorphFrame)frame)
        );

        Current = new("InitialConfigFrame", null, $"The section of initial config frame in {model.Name}.");
        model.ConfigFrames.Add(ReadConfigFrame(reader, model, ikIndices, parentableIndices, true));

        Current = new("ConfigFrameCount", null, $"The section of count of config frames.");
        var configFrameCount = reader.ReadInt32();
        for (int i = 0; i < configFrameCount; i++)
        {
            Current = new("ConfigFrame", i, $"The section of {DataSection.GetOrdinal(i)} initial config frame data in {model.Name}.");
            model.ConfigFrames.Add(ReadConfigFrame(reader, model, ikIndices, parentableIndices));
        }

        foreach (var (bone, i) in model.Bones.Select((b, i) => (b, i)))
        {
            Current = new("CurrentBone", i, $"The section of {DataSection.GetOrdinal(i)} current {bone.Name} bone state");
            bone.Current.Movement = reader.ReadVector3();
            bone.Current.Rotation = reader.ReadQuaternion();
            bone.IsCommitted = reader.ReadBoolean();
            bone.Current.EnablePhysic = reader.ReadBoolean();
            bone.IsSelected = reader.ReadBoolean();
        }

        foreach (var (morph, i) in model.Morphs.Select((m, i) => (m, i)))
        {
            Current = new("CurrentMorph", i, $"The section of {DataSection.GetOrdinal(i)} current {morph.Name} morph state");
            morph.Current.Weight = reader.ReadSingle();
        }

        foreach (var i in ikIndices)
        {
            Current = new("CurrentConfig", i, $"The section of {DataSection.GetOrdinal(i)} current config state (IK state of {model.Bones[i].Name}).");
            model.CurrentConfig.EnableIK.Add(model.Bones[i], reader.ReadBoolean());
        }

        OutsideParentRelationCurrent.Add(model.CurrentConfig, new());
        foreach (var i in parentableIndices)
        {
            Current = new("OutsideParentState", i, $"The section of {DataSection.GetOrdinal(i)} outside parent state");
            var op = new PmmOutsideParentState
            {
                StartFrame = reader.ReadInt32(),
                EndFrame = reader.ReadInt32()
            };
            (int, int) relation = (reader.ReadInt32(), reader.ReadInt32());
            if (i >= 0)
            {
                OutsideParentRelationCurrent[model.CurrentConfig].Add(model.Bones[i], relation);
                model.CurrentConfig.OutsideParent.Add(model.Bones[i], op);
            }
        }

        Current = new("DrawInfo", null, "The section of draw information.");
        model.EnableAlphaBlend = reader.ReadBoolean();
        model.EdgeWidth = reader.ReadSingle();
        model.EnableSelfShadow = reader.ReadBoolean();
        var calculateOrder = reader.ReadByte();

        return (model, modelId, renderOrder, calculateOrder);
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

    private static PmmCameraFrame ReadCameraFrame(BinaryReader reader, PolygonMovieMaker pmm, bool isInitial = false)
    {
        // リストの添え字で管理できるため不要なフレームインデックスを破棄
        if (!isInitial) _ = reader.ReadInt32();

        var frame = new PmmCameraFrame
        {
            Frame = reader.ReadInt32()
        };

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

    private static PmmModelConfigFrame ReadConfigFrame(BinaryReader reader, PmmModel model, IEnumerable<int> ikIndices, IEnumerable<int> parentableIndices, bool isInitial = false)
    {
        // リストの添え字で管理できるため不要なフレームインデックスを破棄
        if (!isInitial) _ = reader.ReadInt32();

        var frame = new PmmModelConfigFrame();
        OutsideParentRelation.Add(frame, new());

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
                OutsideParentRelation[frame].Add(model.Bones[i], relation);
        }

        frame.IsSelected = reader.ReadBoolean();

        return frame;
    }

    private static (PmmMorphFrame Frame, int PreviousFrameIndex, int NextFrameIndex, int? FrameIndex) ReadMorphFrame(BinaryReader reader, bool isInitial = false)
    {
        int? id = isInitial ? null : reader.ReadInt32();

        var frame = new PmmMorphFrame
        {
            Frame = reader.ReadInt32()
        };

        var preID = reader.ReadInt32();
        var nextId = reader.ReadInt32();

        frame.Weight = reader.ReadSingle();
        frame.IsSelected = reader.ReadBoolean();

        return (frame, preID, nextId, id);
    }

    private static (PmmBoneFrame Frame, int PreviousFrameIndex, int NextFrameIndex, int? FrameIndex) ReadBoneFrame(BinaryReader reader, bool isInitial = false)
    {
        int? id = isInitial ? null : reader.ReadInt32();

        var frame = new PmmBoneFrame
        {
            Frame = reader.ReadInt32()
        };
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

        return (frame, previousFrameIndex, nextFrameIndex, id);
    }

    private static PmmLightFrame ReadLightFrame(BinaryReader reader, bool isInitial = false)
    {
        // リストの添え字で管理できるため不要なフレームインデックスを破棄
        if (!isInitial) _ = reader.ReadInt32();

        var frame = new PmmLightFrame
        {
            Frame = reader.ReadInt32()
        };
        // 所属が確実にわかるので pre/next ID から探索してやる必要性がないため破棄
        _ = reader.ReadInt32();
        _ = reader.ReadInt32();

        frame.Color = reader.ReadSingleRGB();
        frame.Position = reader.ReadVector3();

        frame.IsSelected = reader.ReadBoolean();

        return frame;
    }

    private static PmmAccessoryFrame ReadAccessoryFrame(BinaryReader reader, PolygonMovieMaker pmm, bool isInitial = false)
    {
        // リストの添え字で管理できるため不要なフレームインデックスを破棄
        if (!isInitial) _ = reader.ReadInt32();

        var frame = new PmmAccessoryFrame
        {
            Frame = reader.ReadInt32()
        };
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

    private static PmmGravityFrame ReadGravityFrame(BinaryReader reader, bool isInitial = false)
    {
        // リストの添え字で管理できるため不要なフレームインデックスを破棄
        if (!isInitial) _ = reader.ReadInt32();

        var frame = new PmmGravityFrame
        {
            Frame = reader.ReadInt32()
        };
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

    private static PmmSelfShadowFrame ReadSelfShadowFrame(BinaryReader reader, bool isInitial = false)
    {
        // リストの添え字で管理できるため不要なフレームインデックスを破棄
        if (!isInitial) _ = reader.ReadInt32();

        var frame = new PmmSelfShadowFrame
        {
            Frame = reader.ReadInt32()
        };
        // 所属が確実にわかるので pre/next ID から探索してやる必要性がないため破棄
        _ = reader.ReadInt32();
        _ = reader.ReadInt32();

        frame.ShadowMode = (SelfShadow)reader.ReadByte();
        frame.ShadowRange = reader.ReadSingle();

        frame.IsSelected = reader.ReadBoolean();

        return frame;
    }

    /// <summary>
    /// ボーン/モーフの初期以外のフレームを読み込む
    /// <para>フレームの属する要素が前後フレームIDによって管理されているので、各要素内のフレームコレクションに投入するための解決処理を実施する</para>
    /// </summary>
    /// <param name="reader">バイナリ読込クラス</param>
    /// <param name="readElementFrame">フレーム読込メソッドの呼び出し関数</param>
    /// <param name="elementNextFrameDictionary">要素名とそれに対応する次フレームIDの辞書</param>
    /// <param name="addFrame">所属要素にフレームを追加する関数</param>
    private static void AssignFramesToModelElements<T>(
        BinaryReader reader,
        Func<BinaryReader, FrameInfo> readElementFrame,
        Dictionary<T, int?> elementNextFrameDictionary,
        Action<T, IPmmFrame> addFrame)
     where T : IPmmModelElement
    {
        Current = new($"{typeof(T).Name.Replace("Pmm", "")}FrameCount", null, $"The section of count of {typeof(T).Name.Replace("Pmm", "").ToLower()} frames.");
        // フレーム情報を読み込む
        var elementFrameCount = reader.ReadInt32();
        var elementFrames = Enumerable.Range(0, elementFrameCount).Select(i =>
        {
            Current = new($"{typeof(T).Name.Replace("Pmm", "")}Frame", i, $"The section of {DataSection.GetOrdinal(i)} {typeof(T).Name.Replace("Pmm", "").ToLower()} frame data.");
            return readElementFrame(reader);
        }).ToArray();

        Current = new("ResolveFrames", null, $"This section resolves which {typeof(T).Name} the frame belongs to; the PMM file stores this information by frame ID before and after.");
        // フレームのインデックス関係のディクショナリを作成
        var frameDictionary = elementFrames.Where(f => f.FrameIndex.HasValue).ToDictionary(f => f.FrameIndex!.Value);
        var nextFramesOfElements = elementNextFrameDictionary.Select(p => (Element: p.Key, NextFrameIndex: p.Value == 0 ? null : p.Value)).ToArray();

        while (nextFramesOfElements.Any())
        {
            // 次のフレームインデックスが存在するモデル要素とそのフレーム情報を取得
            var nextFrames = nextFramesOfElements.Where(p => p.NextFrameIndex.HasValue)
                                                 .Select(p => (
                                                    p.Element,
                                                    NextFrame: frameDictionary.TryGetValue(p.NextFrameIndex!.Value, out var frame) ? frame : (null, -1, -1, null))
                                                 ).ToArray();

            // 各フレームをモデル要素に追加
            foreach (var (element, nextFrame) in nextFrames.Where(f => f.NextFrame.Frame is not null))
            {
                addFrame(element, nextFrame.Frame!);
            }

            // 各要素の次のフレームインデックスを更新
            nextFramesOfElements = nextFrames.Select<(T Element, FrameInfo NextFrame), (T Element, int? NextFrameIndex)>(p => (
                p.Element,
                p.NextFrame switch
                {
                    { Frame: null } or { NextFrameIndex: 0 } => null,
                    _ => p.NextFrame.NextFrameIndex
                }
            )).Where(p => p.NextFrameIndex.HasValue).ToArray();
        }
    }

    private record struct FrameInfo(IPmmFrame? Frame, int PreviousFrameIndex, int NextFrameIndex, int? FrameIndex)
    {
        public static implicit operator (IPmmFrame? Frame, int PreviousFrameIndex, int NextFrameIndex, int? FrameIndex)(FrameInfo value)
        {
            return (value.Frame, value.PreviousFrameIndex, value.NextFrameIndex, value.FrameIndex);
        }

        public static implicit operator FrameInfo((IPmmFrame? Frame, int PreviousFrameIndex, int NextFrameIndex, int? FrameIndex) value)
        {
            return new FrameInfo(value.Frame, value.PreviousFrameIndex, value.NextFrameIndex, value.FrameIndex);
        }
    }
}