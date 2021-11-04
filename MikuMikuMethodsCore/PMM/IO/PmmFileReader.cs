using MikuMikuMethods.Extension;
using MikuMikuMethods.PMM.ElementState;
using MikuMikuMethods.PMM.Frame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MikuMikuMethods.PMM.IO
{
    internal static class PmmFileReader
    {
        private static Dictionary<PmmModelConfigFrame, Dictionary<PmmBone, (int ModelID, int BoneID)>> OuterParentRelation { get; set; }
        private static Dictionary<PmmModelConfigState, Dictionary<PmmBone, (int ModelID, int BoneID)>> OuterParentRelationCurrent { get; set; }

        internal static void Read(string filePath, PolygonMovieMaker pmm)
        {
            try
            {
                using (FileStream file = new(filePath, FileMode.Open))
                using (BinaryReader reader = new(file, Encoding.ShiftJIS))
                {
                    Read(reader, pmm);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void Read(BinaryReader reader, PolygonMovieMaker pmm)
        {
            try
            {
                // 外部親の関係解決のためのプロパティ初期化
                OuterParentRelation = new();
                OuterParentRelationCurrent = new();

                pmm.Version = reader.ReadString(30, Encoding.ShiftJIS, '\0');
                pmm.OutputResolution = new(reader.ReadInt32(), reader.ReadInt32());

                pmm.EditorState.Width = reader.ReadInt32();
                pmm.Camera.Current.ViewAngle = (int)reader.ReadSingle();
                pmm.EditorState.IsCameraMode = reader.ReadBoolean();

                // パネル開閉状態の読み込み
                pmm.PanelPane.DoesOpenCameraPanel = reader.ReadBoolean();
                pmm.PanelPane.DoesOpenLightPanel = reader.ReadBoolean();
                pmm.PanelPane.DoesOpenAccessaryPanel = reader.ReadBoolean();
                pmm.PanelPane.DoesOpenBonePanel = reader.ReadBoolean();
                pmm.PanelPane.DoesOpenMorphPanel = reader.ReadBoolean();
                pmm.PanelPane.DoesOpenSelfShadowPanel = reader.ReadBoolean();

                // モデル読み込み
                var selectedModelIndex = reader.ReadByte();
                var modelCount = reader.ReadByte();
                var modelOrderDictionary = new Dictionary<PmmModel, (byte RenderOrder, byte CalculateOrder)>();
                for (int i = 0; i < modelCount; i++)
                {
                    (var model, var renderOrder, var calculateOrder) = ReadModel(reader);
                    pmm.Models.Add(model);
                    modelOrderDictionary.Add(model, (renderOrder, calculateOrder));
                }

                pmm.EditorState.SelectedModel = pmm.Models[selectedModelIndex];
                foreach (var model in pmm.Models)
                {
                    // 順序系プロパティはモデルの追加後に設定する
                    (byte RenderOrder, byte CalculateOrder) order = modelOrderDictionary[model];
                    model.RenderOrder = order.RenderOrder;
                    model.CalculateOrder = order.CalculateOrder;
                }

                // 外部親の関係解決
                foreach (var frame in OuterParentRelation)
                {
                    foreach (var relation in frame.Value)
                    {
                        var opModel = pmm.Models[relation.Value.ModelID];
                        frame.Key.OuterParent.Add(relation.Key, new()
                        {
                            ParentModel = opModel,
                            ParentBone = opModel.Bones[relation.Value.BoneID]
                        });
                    }
                }

                foreach (var state in OuterParentRelationCurrent)
                {
                    foreach (var relation in state.Value)
                    {
                        var opModel = pmm.Models[relation.Value.ModelID];
                        state.Key.OuterParent[relation.Key].ParentModel = opModel;
                        state.Key.OuterParent[relation.Key].ParentBone = opModel.Bones[relation.Value.BoneID];
                    }
                }

                ReadCamera(reader, pmm);
                ReadLight(reader, pmm.Light);

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
                    (PmmAccessory accessory, byte renderOrder) = ReadAccessory(reader, pmm);
                    pmm.Accessories.Add(accessory);
                    accessoryOrderDictionary.Add(accessory, renderOrder);
                }
                pmm.EditorState.SelectedAccessory = pmm.Accessories[selectedAccessoryIndex];
                foreach (var acs in pmm.Accessories)
                {
                    acs.RenderOrder = accessoryOrderDictionary[acs];
                }

                // フレーム編集画面の状態読み込み
                pmm.EditorState.CurrentFrame = reader.ReadInt32();
                pmm.EditorState.HorizontalScroll = reader.ReadInt32();
                pmm.EditorState.HorizontalScrollLength = reader.ReadInt32();
                pmm.EditorState.SelectedBoneOperation = (PmmEditorState.BoneOperation)reader.ReadInt32();

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
                            pmm.Models[reader.ReadByte()].SpecificEditorState.RangeSelector = new(reader.ReadInt32());
                        }
                    }
                }
                catch (EndOfStreamException)
                {
                    // このセクションは途中でファイルが終わってても構わないので
                    // ストリームの終わり例外なら来ても何もしなくてよい
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw new IOException("Failed to read PMM file.", ex);
            }
            finally
            {
                OuterParentRelation = null;
                OuterParentRelationCurrent = null;
            }
        }

        private static void ReadCamera(BinaryReader reader, PolygonMovieMaker pmm)
        {
            var camera = pmm.Camera;

            camera.Frames.Add(ReadCameraFrame(reader, pmm, true));

            var cameraFrameCount = reader.ReadInt32();
            for (int i = 0; i < cameraFrameCount; i++)
            {
                camera.Frames.Add(ReadCameraFrame(reader, pmm));
            }

            camera.Current.EyePosition = reader.ReadVector3();
            camera.Current.TargetPosition = reader.ReadVector3();
            camera.Current.Rotation = reader.ReadVector3();
            camera.Current.EnablePerspective = reader.ReadBoolean();
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

            frame.FollowingModel = pmm.Models[reader.ReadInt32()];
            frame.FollowingBone = frame.FollowingModel.Bones[reader.ReadInt32()];

            frame.InterpolationCurces[InterpolationItem.XPosition].FromBytes(reader.ReadBytes(4));
            frame.InterpolationCurces[InterpolationItem.YPosition].FromBytes(reader.ReadBytes(4));
            frame.InterpolationCurces[InterpolationItem.ZPosition].FromBytes(reader.ReadBytes(4));
            frame.InterpolationCurces[InterpolationItem.Rotation].FromBytes(reader.ReadBytes(4));
            frame.InterpolationCurces[InterpolationItem.Distance].FromBytes(reader.ReadBytes(4));
            frame.InterpolationCurces[InterpolationItem.ViewAngle].FromBytes(reader.ReadBytes(4));

            frame.EnablePerspective = reader.ReadBoolean();
            frame.ViewAngle = reader.ReadInt32();

            frame.IsSelected = reader.ReadBoolean();

            return frame;
        }

        private static void ReadLight(BinaryReader reader, PmmLight light)
        {
            light.Frames.Add(ReadLightFrame(reader, true));

            var frameCount = reader.ReadInt32();
            for (int i = 0; i < frameCount; i++)
            {
                light.Frames.Add(ReadLightFrame(reader));
            }

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

            frame.ShadowMode = (PmmSelfShadowFrame.Shadow)reader.ReadByte();
            frame.ShadowRange = reader.ReadSingle();

            frame.IsSelected = reader.ReadBoolean();

            return frame;
        }

        private static void ReadPhysics(BinaryReader reader, PmmPhysics physics)
        {
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
            var acs = new PmmAccessory();
            byte renderOrder;

            // リストの添字で管理するため Index は破棄
            _ = reader.ReadByte();
            acs.Name = reader.ReadString(100, Encoding.ShiftJIS, '\0');
            acs.Path = reader.ReadString(256, Encoding.ShiftJIS, '\0');

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
                model.Bones.Add(new PmmBone() { Name = reader.ReadString() });
            }

            var morphCount = reader.ReadInt32();
            for (int i = 0; i < morphCount; i++)
            {
                model.Morphs.Add(new PmmMorph() { Name = reader.ReadString() });
            }

            var ikCount = reader.ReadInt32();
            var ikIndices = Enumerable.Range(0, ikCount).Select(_ => reader.ReadInt32()).ToArray();
            foreach (var i in ikIndices)
            {
                model.Bones[i].IsIK = true;
            }

            var parentableBoneCount = reader.ReadInt32();
            var parentableIndices = Enumerable.Range(0, parentableBoneCount).Select(_ => reader.ReadInt32()).ToArray();
            // なぜか最初に -1 が入っているのでそれは飛ばす
            foreach (var i in parentableIndices.Skip(1))
            {
                model.Bones[i].CanBecomeOuterParent = true;
            }

            renderOrder = reader.ReadByte();
            model.CurrentConfig.Visible = reader.ReadBoolean();

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
                model.Nodes.Add(new() { doesOpen = reader.ReadBoolean() });
            }

            model.SpecificEditorState.VerticalScrollState = reader.ReadInt32();
            model.SpecificEditorState.LastFrame = reader.ReadInt32();

            // 初期ボーンフレームの読込
            var boneFrameDictionary = new Dictionary<string, int?>();
            foreach (var bone in model.Bones)
            {
                (var frame, _, var nextId) = ReadBoneFrame(reader, bone.Name, true);
                bone.Frames.Add(frame);
                boneFrameDictionary.Add(bone.Name, nextId);
            }

            // ボーンフレームの読込
            ReadFramesThatRequireResolving(
                reader,
                model.Bones,
                boneCount,
                static reader => ReadBoneFrame(reader, null),
                boneFrameDictionary,
                static (element, frame) => (element as PmmBone)?.Frames.Add(frame as PmmBoneFrame)
            );

            // 初期モーフフレームの読込
            var morphFrameDictionary = new Dictionary<string, int?>();
            foreach (var morph in model.Morphs)
            {
                (var frame, _, var nextId) = ReadMorphFrame(reader, morph.Name, true);
                morph.Frames.Add(frame);
                morphFrameDictionary.Add(morph.Name, nextId);
            }

            // モーフフレームの読込
            ReadFramesThatRequireResolving(
                reader,
                model.Morphs,
                morphCount,
                static reader => ReadMorphFrame(reader, null),
                morphFrameDictionary,
                static (element, frame) => (element as PmmMorph)?.Frames.Add(frame as PmmMorphFrame)
            );

            model.ConfigFrames.Add(ReadConfigFrame(reader, model, ikIndices, parentableIndices, true));

            var configFrameCount = reader.ReadInt32();
            for (int i = 0; i < configFrameCount; i++)
            {
                model.ConfigFrames.Add(ReadConfigFrame(reader, model, ikIndices, parentableIndices));
            }

            foreach (var bone in model.Bones)
            {
                bone.Current.Movement = reader.ReadVector3();
                bone.Current.Rotation = reader.ReadQuaternion();
                bone.IsCommitted = reader.ReadBoolean();
                bone.Current.EnablePhysic = reader.ReadBoolean();
                bone.IsSelected = reader.ReadBoolean();
            }

            foreach (var morph in model.Morphs)
            {
                morph.Current.Weight = reader.ReadSingle();
            }

            foreach (var i in ikIndices)
            {
                model.CurrentConfig.EnableIK.Add(model.Bones[i], reader.ReadBoolean());
            }

            OuterParentRelationCurrent.Add(model.CurrentConfig, new());
            foreach (var i in parentableIndices)
            {
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
        private static (PmmMorphFrame Frame, int PreviousFrameIndex, int NextFrameIndex) ReadMorphFrame(BinaryReader reader, string name, bool isInitial = false)
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
        private static (PmmBoneFrame Frame, int PreviousFrameIndex, int NextFrameIndex) ReadBoneFrame(BinaryReader reader, string name, bool isInitial = false)
        {
            // リストの添え字で管理できるため不要なフレームインデックスを破棄
            if (!isInitial) _ = reader.ReadInt32();

            var frame = new PmmBoneFrame();

            frame.Frame = reader.ReadInt32();
            var previousFrameIndex = reader.ReadInt32();
            var nextFrameIndex = reader.ReadInt32();

            frame.InterpolationCurces[InterpolationItem.XPosition].FromBytes(reader.ReadBytes(4));
            frame.InterpolationCurces[InterpolationItem.YPosition].FromBytes(reader.ReadBytes(4));
            frame.InterpolationCurces[InterpolationItem.ZPosition].FromBytes(reader.ReadBytes(4));
            frame.InterpolationCurces[InterpolationItem.Rotation].FromBytes(reader.ReadBytes(4));

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
        private static void ReadFramesThatRequireResolving(BinaryReader reader, IEnumerable<IPmmModelElement> targetElements, int elementCount, Func<BinaryReader, (IPmmFrame Frame, int PreviousFrameIndex, int NextFrameIndex)> readElementFrame, Dictionary<string, int?> elementNextFrameDictionary, Action<IPmmModelElement, IPmmFrame> addFrame)
        {
            var elementFrameCount = reader.ReadInt32();
            var elementFrames = Enumerable.Range(0, elementFrameCount).Select(_ => readElementFrame(reader)).ToArray();

            var AreThereElementLeftThatRequiredFrameSearch = true;
            while (AreThereElementLeftThatRequiredFrameSearch)
            {
                // 論理和代入演算子でループ継続判定をしたいのでまず false にしておく
                AreThereElementLeftThatRequiredFrameSearch = false;

                foreach (var element in targetElements)
                {
                    if (!elementNextFrameDictionary[element.Name].HasValue) break;

                    // 読み込んだインデックスは初期フレームの数だけ先に進んでいるので
                    // 初期フレーム数(= 要素数)の分だけ引いたのがモデル内フレームコレクションでのインデックスになる
                    var nextFrame = elementFrames.ElementAt(elementNextFrameDictionary[element.Name].Value - elementCount);

                    addFrame(element, nextFrame.Frame);

                    // この要素の次のフレームのインデックスを更新する
                    // 次のインデックスが 0 なら次の要素は無いので null を入れる
                    elementNextFrameDictionary[element.Name] = nextFrame.NextFrameIndex == 0 ? null : nextFrame.NextFrameIndex;

                    // 一つでも次のフレーム探索が必要なボーンがあればループを続ける
                    // 次のインデックスに null が入っていればフレーム探索は不要の意味になる
                    AreThereElementLeftThatRequiredFrameSearch |= elementNextFrameDictionary[element.Name].HasValue;
                }
            }
        }
    }
}
