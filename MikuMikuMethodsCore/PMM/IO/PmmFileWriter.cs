using MikuMikuMethods.Extension;
using MikuMikuMethods.PMM.Frame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MikuMikuMethods.PMM.IO
{
    internal static class PmmFileWriter
    {
        internal static void Write(string filePath, PolygonMovieMaker pmm)
        {
            try
            {
                using (FileStream file = new(filePath, FileMode.Create))
                using (BinaryWriter writer = new(file, Encoding.ShiftJIS))
                {
                    Write(writer, pmm);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void Write(BinaryWriter writer, PolygonMovieMaker pmm)
        {
            writer.Write(pmm.Version, 30, Encoding.ShiftJIS);
            writer.Write(pmm.OutputResolution.Width);
            writer.Write(pmm.OutputResolution.Height);


            writer.Write(pmm.EditorState.Width);

            writer.Write((float)pmm.Camera.Current.ViewAngle);

            writer.Write(pmm.EditorState.IsCameraMode);

            writer.Write(pmm.PanelPane.DoesOpenCameraPanel);
            writer.Write(pmm.PanelPane.DoesOpenLightPanel);
            writer.Write(pmm.PanelPane.DoesOpenAccessaryPanel);
            writer.Write(pmm.PanelPane.DoesOpenBonePanel);
            writer.Write(pmm.PanelPane.DoesOpenMorphPanel);
            writer.Write(pmm.PanelPane.DoesOpenSelfShadowPanel);

            writer.Write((byte)pmm.Models.IndexOf(pmm.EditorState.SelectedModel));
            writer.Write((byte)pmm.Models.Count);
            foreach (var item in pmm.Models.Select((Model, Index) => (Model, Index)))
            {
                Write(writer, pmm, item.Model, item.Index);
            }
        }

        private static void Write(BinaryWriter writer, PolygonMovieMaker pmm, PmmModel model, int index)
        {
            writer.Write((byte)index);

            writer.Write(model.Name);
            writer.Write(model.NameEn);
            writer.Write(model.Path, 256, Encoding.ShiftJIS);

            // キーフレームエディタの行数
            writer.Write((byte)model.Nodes.Count);

            writer.Write(model.Bones.Count);
            foreach (var bone in model.Bones)
            {
                writer.Write(bone.Name);
            }

            writer.Write(model.Morphs.Count);
            foreach (var morph in model.Morphs)
            {
                writer.Write(morph.Name);
            }


            var ikBoneIndices = model.Bones.Select((Bone, Index) => (Bone, Index)).Where(p => p.Bone.IsIK).Select(p => p.Index).ToArray();
            writer.Write(ikBoneIndices.Length);
            foreach (var ikBoneIndex in ikBoneIndices)
            {
                writer.Write(ikBoneIndex);
            }

            var parentableBoneIndices = model.Bones.Select((Bone, Index) => (Bone, Index)).Where(p => p.Bone.CanBecomeOuterParent).Select(p => p.Index).ToArray();
            // 内部表現では無視されているが、実際のPMMの外部親可能インデックスには最初に -1 が入っている
            writer.Write(parentableBoneIndices.Length + 1);
            writer.Write(-1);
            foreach (var parentableBoneIndex in parentableBoneIndices)
            {
                writer.Write(parentableBoneIndex);
            }

            writer.Write(pmm.GetRenderOrder(model).Value);
            writer.Write(model.CurrentConfig.Visible);

            writer.Write(model.Bones.IndexOf(model.SelectedBone));
            writer.Write(model.Morphs.IndexOf(model.SelectedBrowMorph));
            writer.Write(model.Morphs.IndexOf(model.SelectedEyeMorph));
            writer.Write(model.Morphs.IndexOf(model.SelectedLipMorph));
            writer.Write(model.Morphs.IndexOf(model.SelectedOtherMorph));

            writer.Write(model.Nodes.Count);
            foreach (var node in model.Nodes)
            {
                writer.Write(node.doesOpen);
            }

            writer.Write(model.SpecificEditorState.VerticalScrollState);
            writer.Write(model.SpecificEditorState.LastFrame);

            WriteFrames(
                writer,
                model.Bones.Select(b => b.Frames.ToList<IPmmFrame>()),
                () => new PmmBoneFrame(),
                (writer, f) =>
                {
                    var frame = (PmmBoneFrame)f;

                    writer.Write(frame.InterpolationCurves[InterpolationItem.XPosition].ToBytes());
                    writer.Write(frame.InterpolationCurves[InterpolationItem.YPosition].ToBytes());
                    writer.Write(frame.InterpolationCurves[InterpolationItem.ZPosition].ToBytes());
                    writer.Write(frame.InterpolationCurves[InterpolationItem.Rotation].ToBytes());

                    writer.Write(frame.Movement);
                    writer.Write(frame.Rotation);
                    writer.Write(frame.IsSelected);
                    writer.Write(!frame.EnablePhysic);
                }
            );

            WriteFrames(
                writer,
                model.Morphs.Select(m => m.Frames.ToList<IPmmFrame>()),
                () => new PmmMorphFrame(),
                (writer, f) =>
                {
                    var frame = (PmmMorphFrame)f;

                    writer.Write(frame.Weight);
                    writer.Write(frame.IsSelected);
                }
            );

            WriteFrames(
                writer,
                new[] { model.ConfigFrames.ToList<IPmmFrame>() },
                () => new PmmModelConfigFrame(),
                (writer, f) =>
                {
                    var frame = (PmmModelConfigFrame)f;

                    writer.Write(frame.Visible);

                    foreach (var ikBoneId in ikBoneIndices)
                    {
                        writer.Write(frame.EnableIK[model.Bones[ikBoneId]]);
                    }
                    foreach (var parentableId in parentableBoneIndices)
                    {
                        var op = frame.OuterParent[model.Bones[parentableId]];
                        writer.Write(pmm.Models.IndexOf(op.ParentModel));
                        writer.Write(op.ParentModel.Bones.IndexOf(op.ParentBone));
                    }

                    writer.Write(frame.IsSelected);
                }
            );

            foreach (var boneState in model.Bones)
            {
                var bc = boneState.Current;

                writer.Write(bc.Movement);
                writer.Write(bc.Rotation);
                writer.Write(boneState.IsCommitted);
                writer.Write(bc.EnablePhysic);
                writer.Write(boneState.IsSelected);
            }

            foreach (var morph in model.Morphs)
            {
                writer.Write(morph.Current.Weight);
            }

            foreach (var i in ikBoneIndices)
            {
                writer.Write(model.CurrentConfig.EnableIK[model.Bones[i]]);
            }

            // 最初の -1 が入っている外部親情報
            writer.Write(-1);
            writer.Write(-1);
            writer.Write(-1);
            writer.Write(-1);
            foreach (var i in parentableBoneIndices)
            {
                var op = model.CurrentConfig.OuterParent[model.Bones[i]];
                writer.Write(op.StartFrame ?? 0);
                writer.Write(op.EndFrame ?? 0);
                writer.Write(pmm.Models.IndexOf(op.ParentModel));
                writer.Write(op.ParentModel is null ? 0 : model.Bones.IndexOf(op.ParentBone));
            }

            writer.Write(model.EnableAlphaBlend);
            writer.Write(model.EdgeWidth);
            writer.Write(model.EnableSelfShadow);
            writer.Write(pmm.GetCalculateOrder(model).Value);
        }

        private static void WriteFrames(BinaryWriter writer, IEnumerable<List<IPmmFrame>> frameContainer, Func<IPmmFrame> constructor, Action<BinaryWriter, IPmmFrame> stateWriter)
        {
            // フレーム順に整列
            foreach (var frames in frameContainer)
            {
                frames.Sort((left, right) => left.Frame - right.Frame);
            }

            var initialFrames = frameContainer.Select(frames =>
            {
                var firstFrame = frames.FirstOrDefault();

                return firstFrame switch
                {
                    null => constructor(),
                    { Frame: 0 } => firstFrame,
                    _ => CreateZeroFrame(firstFrame)
                };
            }).Select(f => new InitFrameContainer() { Frame = f, NextIndex = 0}).ToArray();

            IPmmFrame[][] otherFramesContainer = frameContainer.Select(frames => frames.FirstOrDefault() is { Frame: 0 } ? frames.Skip(1).ToArray() : frames.ToArray()).ToArray();

            // 各フレームに非初期な全フレーム内におけるインデックスを付与
            int id = initialFrames.Length;
            (IPmmFrame Frame, int Index)[][] IndexedOtherFrameContainer = otherFramesContainer.Select(frames => frames.Select(frame => (Frame: frame, Index: id++)).ToArray()).ToArray();

            // 各フレームについて前後フレーム番号を得る
            (IPmmFrame Frame, int Index, int PreIndex, int NextIndex)[] IndexedFrames = IndexedOtherFrameContainer.SelectMany((frames, i) => frames.Select((p, j) =>
            {
                // i は行のインデックス
                // j は行内でのインデックス
                // Index は全フレーム内でのインデックス
                var (frame, currentIndex) = p;

                // 行頭フレームなら前フレームはなし
                var pre = j == 0 ? i : frames[j - 1].Index;
                // 行末フレームなら次フレームはなし
                var next = j == frames.Length - 1 ? 0 : frames[j + 1].Index;

                if (j == 0) initialFrames[i].NextIndex = currentIndex;

                return (Frame: frame, Index: currentIndex, PreIndex: pre, NextIndex: next);
            })).ToArray();

            // フレームを書込み
            foreach (var frame in initialFrames)
            {
                writer.Write(frame.Frame.Frame);
                writer.Write(0);
                writer.Write(frame.NextIndex);

                stateWriter(writer, frame.Frame);
            }
            writer.Write(IndexedFrames.Length);
            foreach (var frame in IndexedFrames)
            {
                writer.Write(frame.Index);

                writer.Write(frame.Frame.Frame);
                writer.Write(frame.PreIndex);
                writer.Write(frame.NextIndex);

                stateWriter(writer, frame.Frame);
            }

            static IPmmFrame CreateZeroFrame(IPmmFrame frame)
            {
                var f = frame.DeepCopy();
                f.Frame = 0;
                return f;
            }
        }

        private class InitFrameContainer
        {
            public IPmmFrame Frame { get; init; }
            public int NextIndex { get; set; }
        }
    }
}
