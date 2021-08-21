using MikuMikuMethods.Extension;
using MikuMikuMethods.PMM.Frame;
using System.IO;

namespace MikuMikuMethods.PMM.IO
{
    internal static class PmmFileReader
    {
        public static void Read(string filePath, PolygonMovieMaker pmm)
        {
            using (FileStream stream = new(filePath, FileMode.Open))
            using (BinaryReader reader = new(stream, Encoding.ShiftJIS))
            {
                pmm.Version = reader.ReadString(30, Encoding.ShiftJIS, '\0');
                pmm.Output = new(reader.ReadInt32(), reader.ReadInt32());

                ReadViewState(reader, pmm.EditorState);

                var modelCount = reader.ReadByte();
                for (int i = 0; i < modelCount; i++)
                    pmm.Models.Add(ReadModel(reader));

                ReadCamera(reader, pmm.Camera);
                ReadLight(reader, pmm.Light);

                ReadAccessoryState(reader, pmm.EditorState);
                var accessoryCount = reader.ReadByte();
                // アクセサリ名一覧
                // 名前は各アクセサリ領域にも書いてあり、齟齬が出ることは基本無いらしいので読み飛ばす
                _ = reader.ReadBytes(accessoryCount * 100);
                for (int i = 0; i < accessoryCount; i++)
                    pmm.Accessories.Add(ReadAccessory(reader));

                ReadFrameState(reader, pmm.EditorState);
                ReadPlayConfig(reader, pmm.PlayConfig);
                ReadMediaConfig(reader, pmm.MediaConfig);
                ReadDrawConfig(reader, pmm.ViewConfig);
                ReadGravity(reader, pmm.Gravity);
                ReadSelfShadow(reader, pmm.SelfShadow);
                ReadColorConfig(reader, pmm.ViewConfig);
                ReadUncomittedFollowingState(reader, pmm.Camera);
                //謎の行列は読み飛ばす
                _ = reader.ReadBytes(64);
                ReadViewFollowing(reader, pmm.ViewConfig);
                pmm.Unknown = new PmmUnknown { TruthValue = reader.ReadBoolean() };
                ReadGroundPhysics(reader, pmm.ViewConfig);
                ReadFrameLocation(reader, pmm.ViewConfig);

                // バージョンによってはここで終わりの可能性がある
                if (reader.BaseStream.Position >= reader.BaseStream.Length)
                    return;

                pmm.EditorState.ExistRangeSelectionTargetSection = reader.ReadBoolean();
                // 普通はないが範囲選択対象セクションが無いとされていれば終了
                if (!pmm.EditorState.ExistRangeSelectionTargetSection)
                    return;

                // モデル設定値が欠落している場合もあるっぽい？ので確認処理を挟む
                for (byte i = 0; i < pmm.Models.Count; i++)
                {
                    if (reader.BaseStream.Position >= reader.BaseStream.Length)
                        break;

                    pmm.EditorState.RangeSelectionTargetIndices.Add(ReadRangeSelectionTargetIndex(reader, pmm));
                    //pmm.EditorState.RangeSelectionTargetIndices.Add((reader.ReadByte(), reader.ReadInt32()));
                }
            }
        }

        private static void ReadViewState(BinaryReader reader, PmmEditorState editorState)
        {
            editorState.KeyframeEditorWidth = reader.ReadInt32();

            editorState.CurrentViewAngle = reader.ReadSingle();

            editorState.IsCameraMode = reader.ReadBoolean();

            editorState.DoesOpenCameraPanel = reader.ReadBoolean();
            editorState.DoesOpenLightPanel = reader.ReadBoolean();
            editorState.DoesOpenAccessaryPanel = reader.ReadBoolean();
            editorState.DoesOpenBonePanel = reader.ReadBoolean();
            editorState.DoesOpenMorphPanel = reader.ReadBoolean();
            editorState.DoesOpenSelfShadowPanel = reader.ReadBoolean();

            editorState.SelectedModelIndex = reader.ReadByte();
        }

        private static PmmModel ReadModel(BinaryReader reader)
        {
            var model = new PmmModel();

            model.Index = reader.ReadByte();

            model.Name = reader.ReadString();
            model.NameEn = reader.ReadString();
            model.Path = reader.ReadString(256, Encoding.ShiftJIS, '\0');

            // キーフレームエディタの行数
            // 表情枠数から求まるので破棄
            _ = reader.ReadByte();

            var boneCount = reader.ReadInt32();
            for (int i = 0; i < boneCount; i++)
            {
                model.BoneNames.Add(reader.ReadString());
            }

            var morphCount = reader.ReadInt32();
            for (int i = 0; i < morphCount; i++)
            {
                model.MorphNames.Add(reader.ReadString());
            }

            var ikCount = reader.ReadInt32();
            for (int i = 0; i < ikCount; i++)
            {
                model.IKBoneIndices.Add(reader.ReadInt32());
            }

            var parentableBoneCount = reader.ReadInt32();
            for (int i = 0; i < parentableBoneCount; i++)
            {
                model.ParentableBoneIndices.Add(reader.ReadInt32());
            }

            model.RenderConfig.RenderOrder = reader.ReadByte();

            model.Uncomitted.Visible = reader.ReadBoolean();
            model.SelectedBoneIndex = reader.ReadInt32();
            model.SelectedMorphIndices = (reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());

            var nodeCount = reader.ReadByte();
            for (int i = 0; i < nodeCount; i++)
            {
                model.FrameEditor.DoesOpenNode.Add(reader.ReadBoolean());
            }

            model.FrameEditor.VerticalScrollState = reader.ReadInt32();
            model.FrameEditor.LastFrame = reader.ReadInt32();

            for (int i = 0; i < boneCount; i++)
            {
                model.InitialBoneFrames.Add(ReadBoneFrame(reader, null));
            }

            var boneFrameCount = reader.ReadInt32();
            for (int i = 0; i < boneFrameCount; i++)
            {
                model.BoneFrames.Add(ReadBoneFrame(reader, reader.ReadInt32()));
            }

            for (int i = 0; i < morphCount; i++)
            {
                model.InitialMorphFrames.Add(ReadMorphFrame(reader, null));
            }

            var morphFrameCount = reader.ReadInt32();
            for (int i = 0; i < morphFrameCount; i++)
            {
                model.MorphFrames.Add(ReadMorphFrame(reader, reader.ReadInt32()));
            }

            model.InitialConfigFrame = ReadConfigFrame(reader, null, ikCount, parentableBoneCount);

            var configFrameCount = reader.ReadInt32();
            for (int i = 0; i < configFrameCount; i++)
            {
                model.ConfigFrames.Add(ReadConfigFrame(reader, reader.ReadInt32(), ikCount, parentableBoneCount));
            }

            for (int i = 0; i < boneCount; i++)
            {
                TemporaryBoneState uncomittedBone = new();
                uncomittedBone.Offset = reader.ReadVector3();
                uncomittedBone.Rotate = reader.ReadQuaternion();
                uncomittedBone.IsThis = reader.ReadBoolean();
                uncomittedBone.EnablePhysic = reader.ReadBoolean();
                uncomittedBone.RowIsSelected = reader.ReadBoolean();

                model.Uncomitted.Bones.Add(uncomittedBone);
            }

            for (int i = 0; i < morphCount; i++)
            {
                model.Uncomitted.MorphWeights.Add(reader.ReadSingle());
            }

            for (int i = 0; i < ikCount; i++)
            {
                model.Uncomitted.IKEnable.Add(reader.ReadBoolean());
            }

            for (int i = 0; i < parentableBoneCount; i++)
            {
                TemporaryParentSetting uncomittedParentSetting = new();
                uncomittedParentSetting.StartFrame = reader.ReadInt32();
                uncomittedParentSetting.EndFrame = reader.ReadInt32();
                uncomittedParentSetting.ModelIndex = reader.ReadInt32();
                uncomittedParentSetting.BoneIndex = reader.ReadInt32();

                model.Uncomitted.ParentSettings.Add(uncomittedParentSetting);
            }

            model.RenderConfig.EnableAlphaBlend = reader.ReadBoolean();
            model.RenderConfig.EdgeWidth = reader.ReadSingle();
            model.RenderConfig.EnableSelfShadow = reader.ReadBoolean();
            model.RenderConfig.CalculateOrder = reader.ReadByte();

            return model;

            PmmBoneFrame ReadBoneFrame(BinaryReader reader, int? index)
            {
                var frame = new PmmBoneFrame();

                frame.Index = index;

                frame.Frame = reader.ReadInt32();
                frame.PreviousFrameIndex = reader.ReadInt32();
                frame.NextFrameIndex = reader.ReadInt32();

                frame.InterpolationCurces[InterpolationItem.XPosition].FromBytes(reader.ReadBytes(4));
                frame.InterpolationCurces[InterpolationItem.YPosition].FromBytes(reader.ReadBytes(4));
                frame.InterpolationCurces[InterpolationItem.ZPosition].FromBytes(reader.ReadBytes(4));
                frame.InterpolationCurces[InterpolationItem.Rotation].FromBytes(reader.ReadBytes(4));

                frame.Offset = reader.ReadVector3();
                frame.Rotation = reader.ReadQuaternion();
                frame.IsSelected = reader.ReadBoolean();
                frame.EnablePhysic = !reader.ReadBoolean();

                return frame;
            }

            PmmMorphFrame ReadMorphFrame(BinaryReader reader, int? index) => new()
            {
                Index = index,

                Frame = reader.ReadInt32(),
                PreviousFrameIndex = reader.ReadInt32(),
                NextFrameIndex = reader.ReadInt32(),

                Ratio = reader.ReadSingle(),

                IsSelected = reader.ReadBoolean(),
            };

            PmmConfigFrame ReadConfigFrame(BinaryReader reader, int? index, int ikCount, int parentCount)
            {
                var frame = new PmmConfigFrame();

                frame.Index = index;

                frame.Frame = reader.ReadInt32();
                frame.PreviousFrameIndex = reader.ReadInt32();
                frame.NextFrameIndex = reader.ReadInt32();
                frame.Visible = reader.ReadBoolean();

                for (int i = 0; i < ikCount; i++)
                {
                    frame.EnableIK.Add(reader.ReadBoolean());
                }

                for (int i = 0; i < parentCount; i++)
                {
                    frame.ParentSettings.Add((reader.ReadInt32(), reader.ReadInt32()));
                }

                frame.IsSelected = reader.ReadBoolean();

                return frame;
            }
        }

        private static void ReadCamera(BinaryReader reader, PmmCamera camera)
        {
            camera.InitialFrame = ReadCameraFrame(reader, null);

            var cameraCount = reader.ReadInt32();
            for (int i = 0; i < cameraCount; i++)
                camera.Frames.Add(ReadCameraFrame(reader, reader.ReadInt32()));

            camera.Uncomitted.EyePosition = reader.ReadVector3();
            camera.Uncomitted.TargetPosition = reader.ReadVector3();
            camera.Uncomitted.Rotation = reader.ReadVector3();
            camera.Uncomitted.EnablePerspective = reader.ReadBoolean();

            PmmCameraFrame ReadCameraFrame(BinaryReader reader, int? index)
            {
                var frame = new PmmCameraFrame();

                frame.Index = index;

                frame.Frame = reader.ReadInt32();
                frame.PreviousFrameIndex = reader.ReadInt32();
                frame.NextFrameIndex = reader.ReadInt32();

                frame.Distance = reader.ReadSingle();
                frame.EyePosition = reader.ReadVector3();
                frame.Rotation = reader.ReadVector3();

                frame.FollowingModelIndex = reader.ReadInt32();
                frame.FollowingBoneIndex = reader.ReadInt32();

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
        }

        private static void ReadLight(BinaryReader reader, PmmLight light)
        {
            light.InitialFrame = ReadLightFrame(reader, null);

            var lightCount = reader.ReadInt32();
            for (int i = 0; i < lightCount; i++)
                light.Frames.Add(ReadLightFrame(reader, reader.ReadInt32()));

            light.Uncomitted.Color = reader.ReadSingleRGB();
            light.Uncomitted.Position = reader.ReadVector3();

            PmmLightFrame ReadLightFrame(BinaryReader reader, int? index) => new()
            {
                Index = index,

                Frame = reader.ReadInt32(),
                PreviousFrameIndex = reader.ReadInt32(),
                NextFrameIndex = reader.ReadInt32(),

                Color = reader.ReadSingleRGB(),
                Position = reader.ReadVector3(),

                IsSelected = reader.ReadBoolean(),
            };
        }

        private static void ReadAccessoryState(BinaryReader reader, PmmEditorState editorState)
        {
            editorState.SelectedAccessoryIndex = reader.ReadByte();
            editorState.VerticalScrollOfAccessoryRow = reader.ReadInt32();
        }

        private static PmmAccessory ReadAccessory(BinaryReader reader)
        {
            var acs = new PmmAccessory();

            acs.Index = reader.ReadByte();

            acs.Name = reader.ReadString(100, Encoding.ShiftJIS, '\0');
            acs.Path = reader.ReadString(256, Encoding.ShiftJIS, '\0');

            acs.RenderOrder = reader.ReadByte();

            acs.InitialFrame = ReadAccessoryFrame(reader, null);

            var accessoryCount = reader.ReadInt32();
            for (int i = 0; i < accessoryCount; i++)
                acs.Frames.Add(ReadAccessoryFrame(reader, reader.ReadInt32()));

            acs.Uncomitted = ReadTmp(reader);

            acs.EnableAlphaBlend = reader.ReadBoolean();

            return acs;

            PmmAccessoryFrame ReadAccessoryFrame(BinaryReader reader, int? index) => new()
            {
                Index = index,

                Frame = reader.ReadInt32(),
                PreviousFrameIndex = reader.ReadInt32(),
                NextFrameIndex = reader.ReadInt32(),

                OpacityAndVisible = reader.ReadByte(),

                ParentModelIndex = reader.ReadInt32(),
                ParentBoneIndex = reader.ReadInt32(),

                Position = reader.ReadVector3(),
                Rotation = reader.ReadVector3(),
                Scale = reader.ReadSingle(),

                EnableShadow = reader.ReadBoolean(),

                IsSelected = reader.ReadBoolean(),
            };

            TemporaryAccessoryEditState ReadTmp(BinaryReader reader) => new()
            {
                OpacityAndVisible = reader.ReadByte(),

                ParentModelIndex = reader.ReadInt32(),
                ParentBoneIndex = reader.ReadInt32(),

                Position = reader.ReadVector3(),
                Rotation = reader.ReadVector3(),
                Scale = reader.ReadSingle(),

                EnableShadow = reader.ReadBoolean(),
            };
        }

        private static void ReadFrameState(BinaryReader reader, PmmEditorState editorState)
        {
            editorState.CurrentFrame = reader.ReadInt32();
            editorState.HorizontalScroll = reader.ReadInt32();
            editorState.HorizontalScrollLength = reader.ReadInt32();
            editorState.SelectedBoneOperation = (PmmEditorState.BoneOperation)reader.ReadInt32();
        }

        private static void ReadPlayConfig(BinaryReader reader, PmmPlayConfig playConfig)
        {
            playConfig.CameraTrackingTarget = (PmmPlayConfig.TrackingTarget)reader.ReadByte();

            playConfig.IsRepeat = reader.ReadBoolean();
            playConfig.IsMoveCurrentFrameToStopFrame = reader.ReadBoolean();
            playConfig.IsStartFromCurrentFrame = reader.ReadBoolean();

            playConfig.PlayStartFrame = reader.ReadInt32();
            playConfig.PlayStopFrame = reader.ReadInt32();
        }

        private static void ReadMediaConfig(BinaryReader reader, PmmMediaConfig mediaConfig)
        {
            mediaConfig.EnableAudio = reader.ReadBoolean();
            mediaConfig.AudioPath = reader.ReadString(256, Encoding.ShiftJIS, '\0');

            mediaConfig.BackgroundVideoOffset = new(reader.ReadInt32(), reader.ReadInt32());
            mediaConfig.BackgroundVideoScale = reader.ReadSingle();
            mediaConfig.BackgroundVideoPath = reader.ReadString(256, Encoding.ShiftJIS, '\0');
            mediaConfig.EnableBackgroundVideo = reader.ReadInt32() == 0b01000000;

            mediaConfig.BackgroundImageOffset = new(reader.ReadInt32(), reader.ReadInt32());
            mediaConfig.BackgroundImageScale = reader.ReadSingle();
            mediaConfig.BackgroundImagePath = reader.ReadString(256, Encoding.ShiftJIS, '\0');
            mediaConfig.EnableBackgroundImage = reader.ReadBoolean();
        }

        private static void ReadDrawConfig(BinaryReader reader, PmmViewConfig drawConfig)
        {
            drawConfig.IsShowInfomation = reader.ReadBoolean();
            drawConfig.IsShowAxis = reader.ReadBoolean();
            drawConfig.IsShowGrandShadow = reader.ReadBoolean();

            drawConfig.FPSLimit = reader.ReadSingle();
            drawConfig.ScreenCaptureSetting = (PmmViewConfig.ScreenCaptureMode)reader.ReadInt32();
            drawConfig.AccessoryModelThreshold = reader.ReadInt32();

            drawConfig.GroundShadowBrightness = reader.ReadSingle();
            drawConfig.EnableTransparentGroundShadow = reader.ReadBoolean();

            drawConfig.PhysicsSetting = (PmmViewConfig.PhysicsMode)reader.ReadByte();
        }

        private static void ReadGravity(BinaryReader reader, PmmGravity gravity)
        {
            gravity.Acceleration = reader.ReadSingle();
            gravity.NoizeAmount = reader.ReadInt32();
            gravity.Direction = reader.ReadVector3();
            gravity.EnableNoize = reader.ReadBoolean();

            gravity.InitialFrame = ReadGravityFrame(reader, null);

            var frameCount = reader.ReadInt32();
            for (int i = 0; i < frameCount; i++)
                gravity.Frames.Add(ReadGravityFrame(reader, reader.ReadInt32()));

            PmmGravityFrame ReadGravityFrame(BinaryReader reader, int? index) => new()
            {
                Index = index,

                Frame = reader.ReadInt32(),
                PreviousFrameIndex = reader.ReadInt32(),
                NextFrameIndex = reader.ReadInt32(),

                EnableNoize = reader.ReadBoolean(),
                NoizeAmount = reader.ReadInt32(),
                Acceleration = reader.ReadSingle(),
                Direction = reader.ReadVector3(),

                IsSelected = reader.ReadBoolean(),
            };
        }

        private static void ReadSelfShadow(BinaryReader reader, PmmSelfShadow selfShadow)
        {
            selfShadow.EnableSelfShadow = reader.ReadBoolean();
            selfShadow.ShadowLimit = reader.ReadSingle();

            selfShadow.InitialFrame = ReadSelfShadowFrame(reader, null);

            var selfShadowCount = reader.ReadInt32();
            for (int i = 0; i < selfShadowCount; i++)
                selfShadow.Frames.Add(ReadSelfShadowFrame(reader, reader.ReadInt32()));

            PmmSelfShadowFrame ReadSelfShadowFrame(BinaryReader reader, int? index) => new()
            {
                Index = index,

                Frame = reader.ReadInt32(),
                PreviousFrameIndex = reader.ReadInt32(),
                NextFrameIndex = reader.ReadInt32(),

                ShadowMode = (PmmSelfShadowFrame.Shadow)reader.ReadByte(),
                ShadowRange = reader.ReadSingle(),

                IsSelected = reader.ReadBoolean(),
            };
        }

        private static void ReadColorConfig(BinaryReader reader, PmmViewConfig drawConfig)
        {
            drawConfig.EdgeColor = System.Drawing.Color.FromArgb(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
            drawConfig.IsBackgroundBlack = reader.ReadBoolean();
        }

        private static void ReadUncomittedFollowingState(BinaryReader reader, PmmCamera camera)
        {
            camera.UncomittedFollowing.ModelIndex = reader.ReadInt32();
            camera.UncomittedFollowing.BoneIndex = reader.ReadInt32();
        }

        private static void ReadViewFollowing(BinaryReader reader, PmmViewConfig viewConfig)
        {
            viewConfig.IsViewFollowCamera = reader.ReadBoolean();
        }

        private static void ReadGroundPhysics(BinaryReader reader, PmmViewConfig drawConfig)
        {
            drawConfig.EnableGroundPhysics = reader.ReadBoolean();
        }

        private static void ReadFrameLocation(BinaryReader reader, PmmViewConfig viewConfig)
        {
            viewConfig.FrameLocation = reader.ReadInt32();
        }

        private static (PmmModel Model, int Target) ReadRangeSelectionTargetIndex(BinaryReader reader, PolygonMovieMaker pmm)
        {
            return (pmm.Models[reader.ReadByte()], reader.ReadInt32());
        }
    }
}
