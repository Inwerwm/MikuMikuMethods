using MikuMikuMethods.Extension;
using MikuMikuMethods.PMM.Frame;
using System.IO;
using System.Linq;

namespace MikuMikuMethods.PMM.IO
{
    internal static class PmmFileWriter
    {
        public static void Write(string filePath, PolygonMovieMaker pmm)
        {
            using (FileStream file = new(filePath, FileMode.Create))
            using (BinaryWriter writer = new(file, Encoding.ShiftJIS))
            {
                writer.Write(pmm.Version, 30, Encoding.ShiftJIS);
                writer.Write(pmm.Output.Width);
                writer.Write(pmm.Output.Height);

                pmm.EditorState.WriteViewState(writer);

                writer.Write((byte)pmm.Models.Count);
                foreach (var model in pmm.Models)
                    model.Write(writer);

                pmm.Camera.Write(writer);
                pmm.Light.Write(writer);

                pmm.EditorState.WriteAccessoryState(writer);
                writer.Write((byte)pmm.Accessories.Count);
                foreach (var acs in pmm.Accessories)
                    writer.Write(acs.Name, 100, Encoding.ShiftJIS);
                foreach (var acs in pmm.Accessories)
                    acs.Write(writer);

                pmm.EditorState.WriteFrameState(writer);
                pmm.PlayConfig.Write(writer);
                pmm.MediaConfig.Write(writer);
                pmm.ViewConfig.Write(writer);
                pmm.Gravity.Write(writer);
                pmm.SelfShadow.Write(writer);
                pmm.ViewConfig.WriteColorConfig(writer);
                pmm.Camera.UncomittedFollowing.Write(writer);
                writer.Write(PmmUnknown.Matrix);
                pmm.ViewConfig.WriteViewFollowing(writer);
                writer.Write(pmm.Unknown.TruthValue);
                pmm.ViewConfig.WriteGroundPhysics(writer);
                pmm.ViewConfig.WriteFrameLocation(writer);

                // 範囲選択対象セクションがないバージョンなら終了
                if (!pmm.EditorState.ExistRangeSelectionTargetSection)
                    return;

                writer.Write(pmm.EditorState.ExistRangeSelectionTargetSection);
                var modelIdMap = pmm.Models.Select((Model, Index) => (Model, Index)).ToDictionary(p => p.Model, p => (byte)p.Index);
                foreach (var index in pmm.EditorState.RangeSelectionTargetIndices)
                {
                    writer.Write(modelIdMap[index.Model]);
                    writer.Write(index.Target);
                }
            }
        }

        private static void WriteViewState(this PmmEditorState editorState, BinaryWriter writer)
        {
            writer.Write(editorState.KeyframeEditorWidth);

            writer.Write(editorState.CurrentViewAngle);

            writer.Write(editorState.IsCameraMode);

            writer.Write(editorState.DoesOpenCameraPanel);
            writer.Write(editorState.DoesOpenLightPanel);
            writer.Write(editorState.DoesOpenAccessaryPanel);
            writer.Write(editorState.DoesOpenBonePanel);
            writer.Write(editorState.DoesOpenMorphPanel);
            writer.Write(editorState.DoesOpenSelfShadowPanel);

            writer.Write(editorState.SelectedModelIndex);
        }

        private static void WriteAccessoryState(this PmmEditorState editorState, BinaryWriter writer)
        {
            writer.Write(editorState.SelectedAccessoryIndex);
            writer.Write(editorState.VerticalScrollOfAccessoryRow);
        }

        private static void WriteFrameState(this PmmEditorState editorState, BinaryWriter writer)
        {
            writer.Write(editorState.CurrentFrame);
            writer.Write(editorState.HorizontalScroll);
            writer.Write(editorState.HorizontalScrollLength);
            writer.Write((int)editorState.SelectedBoneOperation);
        }

        private static void WriteFrameLocation(this PmmViewConfig viewConfig, BinaryWriter writer)
        {
            writer.Write(viewConfig.FrameLocation);
        }

        private static void WriteViewFollowing(this PmmViewConfig viewConfig, BinaryWriter writer)
        {
            writer.Write(viewConfig.IsViewFollowCamera);
        }

        private static void Write(this PmmAccessory accessory, BinaryWriter writer)
        {
            writer.Write(accessory.Index);

            writer.Write(accessory.Name, 100, Encoding.ShiftJIS);
            writer.Write(accessory.Path, 256, Encoding.ShiftJIS);

            writer.Write(accessory.RenderOrder);

            accessory.InitialFrame.Write(writer);

            writer.Write(accessory.Frames.Count);
            foreach (var frame in accessory.Frames)
                frame.Write(writer);

            accessory.Uncomitted.Write(writer);

            writer.Write(accessory.EnableAlphaBlend);
        }

        private static void Write(this TemporaryAccessoryEditState tmp, BinaryWriter writer)
        {
            writer.Write(tmp.OpacityAndVisible);

            writer.Write(tmp.ParentModelIndex);
            writer.Write(tmp.ParentBoneIndex);

            writer.Write(tmp.Position);
            writer.Write(tmp.Rotation);
            writer.Write(tmp.Scale);

            writer.Write(tmp.EnableShadow);
        }

        private static void Write(this PmmCamera camera,BinaryWriter writer)
        {
            camera.InitialFrame.Write(writer);

            writer.Write(camera.Frames.Count);
            foreach (var frame in camera.Frames)
                frame.Write(writer);

            writer.Write(camera.Uncomitted.EyePosition);
            writer.Write(camera.Uncomitted.TargetPosition);
            writer.Write(camera.Uncomitted.Rotation);
            writer.Write(camera.Uncomitted.EnablePerspective);
        }

        private static void Write(this TemporaryCameraFollowingState tmp, BinaryWriter writer)
        {
            writer.Write(tmp.ModelIndex);
            writer.Write(tmp.BoneIndex);
        }

        private static void Write(this PmmViewConfig drawConfig, BinaryWriter writer)
        {
            writer.Write(drawConfig.IsShowInfomation);
            writer.Write(drawConfig.IsShowAxis);
            writer.Write(drawConfig.IsShowGrandShadow);

            writer.Write(drawConfig.FPSLimit);
            writer.Write((int)drawConfig.ScreenCaptureSetting);
            writer.Write(drawConfig.AccessoryModelThreshold);

            writer.Write(drawConfig.GroundShadowBrightness);
            writer.Write(drawConfig.EnableTransparentGroundShadow);

            writer.Write((byte)drawConfig.PhysicsSetting);
        }

        private static void WriteColorConfig(this PmmViewConfig drawConfig, BinaryWriter writer)
        {
            writer.Write((int)drawConfig.EdgeColor.R);
            writer.Write((int)drawConfig.EdgeColor.G);
            writer.Write((int)drawConfig.EdgeColor.B);

            writer.Write(drawConfig.IsBackgroundBlack);
        }

        private static void WriteGroundPhysics(this PmmViewConfig drawConfig, BinaryWriter writer)
        {
            writer.Write(drawConfig.EnableGroundPhysics);
        }

        private static void Write(this PmmGravity gravity,BinaryWriter writer)
        {
            writer.Write(gravity.Acceleration);
            writer.Write(gravity.NoizeAmount);
            writer.Write(gravity.Direction);
            writer.Write(gravity.EnableNoize);

            gravity.InitialFrame.Write(writer);

            writer.Write(gravity.Frames.Count);
            foreach (var frame in gravity.Frames)
                frame.Write(writer);
        }

        private static void Write(this PmmLight light, BinaryWriter writer)
        {
            light.InitialFrame.Write(writer);

            writer.Write(light.Frames.Count);
            foreach (var frame in light.Frames)
                frame.Write(writer);

            writer.Write(light.Uncomitted.Color, false);
            writer.Write(light.Uncomitted.Position);
        }

        private static void Write(this PmmMediaConfig mediaConfig, BinaryWriter writer)
        {
            writer.Write(mediaConfig.EnableAudio);
            writer.Write(mediaConfig.AudioPath, 256, Encoding.ShiftJIS);

            writer.Write(mediaConfig.BackgroundVideoOffset.X);
            writer.Write(mediaConfig.BackgroundVideoOffset.Y);
            writer.Write(mediaConfig.BackgroundVideoScale);
            writer.Write(mediaConfig.BackgroundVideoPath, 256, Encoding.ShiftJIS);
            writer.Write(mediaConfig.EnableBackgroundVideo ? 0b01000000 : 0b01000001);

            writer.Write(mediaConfig.BackgroundImageOffset.X);
            writer.Write(mediaConfig.BackgroundImageOffset.Y);
            writer.Write(mediaConfig.BackgroundImageScale);
            writer.Write(mediaConfig.BackgroundImagePath, 256, Encoding.ShiftJIS);
            writer.Write(mediaConfig.EnableBackgroundImage);
        }

        private static void Write(this PmmModel model, BinaryWriter writer)
        {
            writer.Write(model.Index);

            writer.Write(model.Name);
            writer.Write(model.NameEn);
            writer.Write(model.Path, 256, Encoding.ShiftJIS);

            // キーフレームエディタの行数
            // 3([root]、表示・IK・外観、表情) + 表示枠の数
            writer.Write(model.NodeCount);

            // ボーン数
            writer.Write(model.BoneNames.Count);
            foreach (var item in model.BoneNames)
            {
                writer.Write(item);
            }

            // モーフ数
            writer.Write(model.MorphNames.Count);
            foreach (var item in model.MorphNames)
            {
                writer.Write(item);
            }

            // IK数
            writer.Write(model.IKBoneIndices.Count);
            foreach (var item in model.IKBoneIndices)
            {
                writer.Write(item);
            }

            // 外部親設定可能なボーン数
            writer.Write(model.ParentableBoneIndices.Count);
            foreach (var item in model.ParentableBoneIndices)
            {
                writer.Write(item);
            }

            writer.Write(model.RenderConfig.RenderOrder);

            writer.Write(model.Uncomitted.Visible);
            writer.Write(model.SelectedBoneIndex);
            writer.Write(model.SelectedMorphIndices.Brow);
            writer.Write(model.SelectedMorphIndices.Eye);
            writer.Write(model.SelectedMorphIndices.Lip);
            writer.Write(model.SelectedMorphIndices.Other);

            // 表情枠数
            writer.Write(model.NodeCount);
            foreach (var item in model.FrameEditor.DoesOpenNode)
            {
                writer.Write(item);
            }

            writer.Write(model.FrameEditor.VerticalScrollState);
            writer.Write(model.FrameEditor.LastFrame);

            foreach (var item in model.InitialBoneFrames)
                item.Write(writer);

            writer.Write(model.BoneFrames.Count);
            foreach (var item in model.BoneFrames)
                item.Write(writer);

            foreach (var item in model.InitialMorphFrames)
                item.Write(writer);

            writer.Write(model.MorphFrames.Count);
            foreach (var item in model.MorphFrames)
                item.Write(writer);

            model.InitialConfigFrame.Write(writer);

            writer.Write(model.ConfigFrames.Count);
            foreach (var item in model.ConfigFrames)
                item.Write(writer);

            foreach (var item in model.Uncomitted.Bones)
            {
                writer.Write(item.Offset);
                writer.Write(item.Rotate);
                writer.Write(item.IsThis);
                writer.Write(item.EnablePhysic);
                writer.Write(item.RowIsSelected);
            }

            foreach (var item in model.Uncomitted.MorphWeights)
                writer.Write(item);

            foreach (var item in model.Uncomitted.IKEnable)
                writer.Write(item);

            foreach (var item in model.Uncomitted.ParentSettings)
            {
                writer.Write(item.StartFrame);
                writer.Write(item.EndFrame);
                writer.Write(item.ModelIndex);
                writer.Write(item.BoneIndex);
            }

            writer.Write(model.RenderConfig.EnableAlphaBlend);
            writer.Write(model.RenderConfig.EdgeWidth);
            writer.Write(model.RenderConfig.EnableSelfShadow);
            writer.Write(model.RenderConfig.CalculateOrder);
        }

        private static void Write(this PmmPlayConfig playConfig, BinaryWriter writer)
        {
            writer.Write((byte)playConfig.CameraTrackingTarget);

            writer.Write(playConfig.IsRepeat);
            writer.Write(playConfig.IsMoveCurrentFrameToStopFrame);
            writer.Write(playConfig.IsStartFromCurrentFrame);

            writer.Write(playConfig.PlayStartFrame);
            writer.Write(playConfig.PlayStopFrame);
        }

        private static void Write(this PmmSelfShadow selfShadow, BinaryWriter writer)
        {
            writer.Write(selfShadow.EnableSelfShadow);
            writer.Write(selfShadow.ShadowLimit);

            selfShadow.InitialFrame.Write(writer);

            writer.Write(selfShadow.Frames.Count);
            foreach (var frame in selfShadow.Frames)
                frame.Write(writer);
        }

        private static void Write(this PmmAccessoryFrame frame, BinaryWriter writer)
        {
            if (frame.Index.HasValue)
                writer.Write(frame.Index.Value);

            writer.Write(frame.Frame);
            writer.Write(frame.PreviousFrameIndex);
            writer.Write(frame.NextFrameIndex);

            writer.Write(frame.OpacityAndVisible);

            writer.Write(frame.ParentModelIndex);
            writer.Write(frame.ParentBoneIndex);

            writer.Write(frame.Position);
            writer.Write(frame.Rotation);
            writer.Write(frame.Scale);

            writer.Write(frame.EnableShadow);

            writer.Write(frame.IsSelected);
        }

        private static void Write(this PmmBoneFrame frame, BinaryWriter writer)
        {
            if (frame.Index.HasValue)
                writer.Write(frame.Index.Value);

            writer.Write(frame.Frame);
            writer.Write(frame.PreviousFrameIndex);
            writer.Write(frame.NextFrameIndex);

            writer.Write(frame.InterpolationCurces[InterpolationItem.XPosition].ToBytes());
            writer.Write(frame.InterpolationCurces[InterpolationItem.YPosition].ToBytes());
            writer.Write(frame.InterpolationCurces[InterpolationItem.ZPosition].ToBytes());
            writer.Write(frame.InterpolationCurces[InterpolationItem.Rotation].ToBytes());

            writer.Write(frame.Offset);
            writer.Write(frame.Rotation);
            writer.Write(frame.IsSelected);
            writer.Write(!frame.EnablePhysic);
        }

        private static void Write(this PmmCameraFrame frame, BinaryWriter writer)
        {
            if (frame.Index.HasValue)
                writer.Write(frame.Index.Value);

            writer.Write(frame.Frame);
            writer.Write(frame.PreviousFrameIndex);
            writer.Write(frame.NextFrameIndex);

            writer.Write(frame.Distance);
            writer.Write(frame.EyePosition);
            writer.Write(frame.Rotation);

            writer.Write(frame.FollowingModelIndex);
            writer.Write(frame.FollowingBoneIndex);

            writer.Write(frame.InterpolationCurces[InterpolationItem.XPosition].ToBytes());
            writer.Write(frame.InterpolationCurces[InterpolationItem.YPosition].ToBytes());
            writer.Write(frame.InterpolationCurces[InterpolationItem.ZPosition].ToBytes());
            writer.Write(frame.InterpolationCurces[InterpolationItem.Rotation].ToBytes());
            writer.Write(frame.InterpolationCurces[InterpolationItem.Distance].ToBytes());
            writer.Write(frame.InterpolationCurces[InterpolationItem.ViewAngle].ToBytes());

            writer.Write(frame.EnablePerspective);
            writer.Write(frame.ViewAngle);

            writer.Write(frame.IsSelected);
        }

        private static void Write(this PmmConfigFrame frame, BinaryWriter writer)
        {
            if (frame.Index.HasValue)
                writer.Write(frame.Index.Value);

            writer.Write(frame.Frame);
            writer.Write(frame.PreviousFrameIndex);
            writer.Write(frame.NextFrameIndex);
            writer.Write(frame.Visible);

            foreach (var f in frame.EnableIK)
            {
                writer.Write(f);
            }

            foreach (var f in frame.ParentSettings)
            {
                writer.Write(f.ModelId);
                writer.Write(f.BoneId);
            }

            writer.Write(frame.IsSelected);
        }

        private static void Write(this PmmGravityFrame frame, BinaryWriter writer)
        {
            if (frame.Index.HasValue)
                writer.Write(frame.Index.Value);

            writer.Write(frame.Frame);
            writer.Write(frame.PreviousFrameIndex);
            writer.Write(frame.NextFrameIndex);

            writer.Write(frame.EnableNoize);
            writer.Write(frame.NoizeAmount);
            writer.Write(frame.Acceleration);
            writer.Write(frame.Direction);

            writer.Write(frame.IsSelected);
        }

        private static void Write(this PmmLightFrame frame, BinaryWriter writer)
        {
            if (frame.Index.HasValue)
                writer.Write(frame.Index.Value);

            writer.Write(frame.Frame);
            writer.Write(frame.PreviousFrameIndex);
            writer.Write(frame.NextFrameIndex);

            writer.Write(frame.Color, false);
            writer.Write(frame.Position);

            writer.Write(frame.IsSelected);
        }

        private static void Write(this PmmMorphFrame frame, BinaryWriter writer)
        {
            if (frame.Index.HasValue)
                writer.Write(frame.Index.Value);

            writer.Write(frame.Frame);
            writer.Write(frame.PreviousFrameIndex);
            writer.Write(frame.NextFrameIndex);

            writer.Write(frame.Ratio);

            writer.Write(frame.IsSelected);
        }

        private static void Write(this PmmSelfShadowFrame frame, BinaryWriter writer)
        {
            if (frame.Index.HasValue)
                writer.Write(frame.Index.Value);

            writer.Write(frame.Frame);
            writer.Write(frame.PreviousFrameIndex);
            writer.Write(frame.NextFrameIndex);

            writer.Write((byte)frame.ShadowMode);
            writer.Write(frame.ShadowRange);

            writer.Write(frame.IsSelected);
        }
    }
}
