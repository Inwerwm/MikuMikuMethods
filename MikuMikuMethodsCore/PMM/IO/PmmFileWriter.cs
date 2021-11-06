using MikuMikuMethods.Extension;
using MikuMikuMethods.PMM.Frame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            WriteFrames(writer, model.Bones.Select(b => b.Frames.ToList<IPmmFrame>()));
        }

        private static void WriteFrames(BinaryWriter writer, IEnumerable<List<IPmmFrame>> frameContainer)
        {
            // フレーム順に整列
            foreach (var frames in frameContainer)
            {
                frames.Sort((left, right) => left.Frame - right.Frame);
            }

            var initialFrames = frameContainer.Select(frames =>
            {
                var firstFrame = frames.FirstOrDefault();
                if(firstFrame is null) return null;

                if(firstFrame.Frame == 0) return firstFrame;
                return firstFrame/*のフレームを0にしたクローン*/;
            });
            var otherFrames = frameContainer.Select(frames => frames.Skip(1));

        }
    }
}
