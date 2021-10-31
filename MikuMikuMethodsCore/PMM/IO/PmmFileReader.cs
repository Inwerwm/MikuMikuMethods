using MikuMikuMethods.Extension;
using MikuMikuMethods.PMM.Frame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MikuMikuMethods.PMM.IO
{
    internal static class PmmFileReader
    {
        private static Dictionary<PmmModel, Dictionary<PmmBone, (int ModelID, int BoneID)>> OuterParentRelation { get; set; }
        private static Dictionary<PmmModel, Dictionary<PmmBone, (int ModelID, int BoneID)>> OuterParentRelationCurrent { get; set; }

        internal static PolygonMovieMaker Read(string filePath)
        {
            try
            {
                using (FileStream file = new(filePath, FileMode.Open))
                using (BinaryReader reader = new(file, Encoding.ShiftJIS))
                {
                    return Read(reader);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static PolygonMovieMaker Read(BinaryReader reader)
        {
            try
            {
                OuterParentRelation = new();
                OuterParentRelationCurrent = new();

                var pmm = new PolygonMovieMaker();
                pmm.Version = reader.ReadString(30, Encoding.ShiftJIS, '\0');
                pmm.OutputResolution = new(reader.ReadInt32(), reader.ReadInt32());

                pmm.EditorState.Width = reader.ReadInt32();
                pmm.Camera.Current.ViewAngle = reader.ReadInt32();
                pmm.EditorState.IsCameraMode = reader.ReadBoolean();

                pmm.PanelPane.DoesOpenCameraPanel = reader.ReadBoolean();
                pmm.PanelPane.DoesOpenLightPanel = reader.ReadBoolean();
                pmm.PanelPane.DoesOpenAccessaryPanel = reader.ReadBoolean();
                pmm.PanelPane.DoesOpenBonePanel = reader.ReadBoolean();
                pmm.PanelPane.DoesOpenMorphPanel = reader.ReadBoolean();
                pmm.PanelPane.DoesOpenSelfShadowPanel = reader.ReadBoolean();

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
                    (byte RenderOrder, byte CalculateOrder) order = modelOrderDictionary[model];
                    model.RenderOrder = order.RenderOrder;
                    model.CalculateOrder = order.CalculateOrder;

                    foreach (var relation in OuterParentRelation[model])
                    {
                        var opModel = pmm.Models[relation.Value.ModelID];
                        foreach (var frame in model.ConfigFrames)
                        {
                            frame.OuterParent.Add(relation.Key, new()
                            {
                                ParentModel = opModel,
                                ParentBone = opModel.Bones[relation.Value.BoneID]
                            });
                        }
                    }

                    foreach (var relation in OuterParentRelationCurrent[model])
                    {
                        var opModel = pmm.Models[relation.Value.ModelID];
                        model.CurrentConfig.OuterParent[relation.Key].ParentModel = opModel;
                        model.CurrentConfig.OuterParent[relation.Key].ParentBone = opModel.Bones[relation.Value.BoneID];
                    }

                }

                ReadCamera(reader, pmm);
                ReadLight(reader, pmm.Light);


                return pmm;
            }
            catch (Exception ex)
            {
                throw new IOException("Failed to read PMM file.", ex);
            }
            finally
            {
                OuterParentRelation = null;
            }
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

        private static (PmmModel Model, byte RenderOrder, byte CalculateOrder) ReadModel(BinaryReader reader)
        {
            var model = new PmmModel();

            OuterParentRelation.Add(model, new());
            OuterParentRelationCurrent.Add(model, new());

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
            var ikIndices = Enumerable.Range(0, ikCount).Select(_ => reader.ReadInt32());
            foreach (var i in ikIndices)
            {
                model.Bones[i].IsIK = true;
            }

            var parentableBoneCount = reader.ReadInt32();
            var parentableIndices = Enumerable.Range(0, parentableBoneCount).Select(_ => reader.ReadInt32());
            foreach (var i in parentableIndices)
            {
                model.Bones[i].CanBecomeOuterParent = true;
            }

            renderOrder = reader.ReadByte();
            model.CurrentConfig.Visible = reader.ReadBoolean();

            model.SelectedBone = model.Bones[reader.ReadInt32()];
            model.SelectedBrowMorph = model.Morphs[reader.ReadInt32()];
            model.SelectedEyeMorph = model.Morphs[reader.ReadInt32()];
            model.SelectedLipMorph = model.Morphs[reader.ReadInt32()];
            model.SelectedOtherMorph = model.Morphs[reader.ReadInt32()];

            var nodeCount = reader.ReadByte();
            for (int i = 0; i < nodeCount; i++)
            {
                model.Nodes.Add(new() { doesOpen = reader.ReadBoolean() });
            }

            model.KeyFrameEditor.VerticalScrollState = reader.ReadInt32();
            model.KeyFrameEditor.LastFrame = reader.ReadInt32();

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

            foreach (var i in parentableIndices)
            {
                var op = new ElementState.PmmOuterParentState();
                op.StartFrame = reader.ReadInt32();
                op.EndFrame = reader.ReadInt32();
                OuterParentRelationCurrent[model].Add(model.Bones[i], (reader.ReadInt32(), reader.ReadInt32()));

                model.CurrentConfig.OuterParent.Add(model.Bones[i], op);
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
                OuterParentRelation[model].Add(model.Bones[i], (reader.ReadInt32(), reader.ReadInt32()));
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
        /// <param name="targetElements">フレーム追加対象になるボーン/モーフのコレクション</param>
        /// <param name="elementCount">targetElements の要素数</param>
        /// <param name="readElementFrame">フレーム読込メソッドの呼び出し関数</param>
        /// <param name="elementNextFrameDictionary">ボーン/モーフ名とそれに対応する次フレームIDの辞書</param>
        /// <param name="addFrame">所属要素にフレームを追加する関数</param>
        private static void ReadFramesThatRequireResolving(BinaryReader reader, IEnumerable<IPmmModelElement> targetElements, int elementCount, Func<BinaryReader, (IPmmFrame Frame, int PreviousFrameIndex, int NextFrameIndex)> readElementFrame, Dictionary<string, int?> elementNextFrameDictionary, Action<IPmmModelElement, IPmmFrame> addFrame)
        {
            var elementFrameCount = reader.ReadInt32();
            var elementFrames = Enumerable.Range(0, elementFrameCount).Select(_ => readElementFrame(reader));

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
