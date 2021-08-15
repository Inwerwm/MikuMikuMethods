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
    internal static class PmmFileReader
    {
        public static void Read(string filePath, PolygonMovieMaker pmm)
        {
            using (FileStream stream = new(filePath, FileMode.Open))
            using (BinaryReader reader = new(stream, Encoding.ShiftJIS))
            {
                pmm.Version = reader.ReadString(30, Encoding.ShiftJIS, '\0');

                pmm.EditorState.ReadViewState(reader);

                var modelCount = reader.ReadByte();
                for (int i = 0; i < modelCount; i++)
                    pmm.Models.Add(new(reader));

                ReadCamera(reader, pmm.Camera);
                ReadLight(reader, pmm.Light);

                ReadAccessoryState(reader, pmm.EditorState);
                var accessoryCount = reader.ReadByte();
                // アクセサリ名一覧
                // 名前は各アクセサリ領域にも書いてあり、齟齬が出ることは基本無いらしいので読み飛ばす
                _ = reader.ReadBytes(accessoryCount * 100);
                for (int i = 0; i < accessoryCount; i++)
                    pmm.Accessories.Add(ReadAccessories(reader));

                ReadFrameState(reader, pmm.EditorState);
                ReadPlayConfig(reader, pmm.PlayConfig);
                ReadMediaConfig(reader, pmm.MediaConfig);
                ReadDrawConfig(reader, pmm.DrawConfig);
                ReadGravity(reader, pmm.Gravity);
                ReadSelfShadow(reader, pmm.SelfShadow);
                ReadColorConfig(reader, pmm.DrawConfig);
                ReadUncomittedFollowingStateCamera(reader, pmm.Camera);
                //謎の行列は読み飛ばす
                _ = reader.ReadBytes(64);
                ReadViewFollowing(reader, pmm.EditorState);
                pmm.Unknown = new PmmUnknown { TruthValue = reader.ReadBoolean() };
                ReadGroundPhysics(reader, pmm.DrawConfig);
                ReadFrameLocation(reader, pmm.EditorState);

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

                    pmm.EditorState.RangeSelectionTargetIndices.Add(ReadRangeSelectionTargetIndex(reader));
                    //pmm.EditorState.RangeSelectionTargetIndices.Add((reader.ReadByte(), reader.ReadInt32()));
                }
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
            throw new NotImplementedException();
        }

        private static void ReadAccessoryState(BinaryReader reader, PmmEditorState editorState)
        {
            throw new NotImplementedException();
        }

        private static PmmAccessory ReadAccessories(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        private static void ReadFrameState(BinaryReader reader, PmmEditorState editorState)
        {
            throw new NotImplementedException();
        }

        private static void ReadPlayConfig(BinaryReader reader, PmmPlayConfig playConfig)
        {
            throw new NotImplementedException();
        }

        private static void ReadMediaConfig(BinaryReader reader, PmmMediaConfig mediaConfig)
        {
            throw new NotImplementedException();
        }

        private static void ReadDrawConfig(BinaryReader reader, PmmDrawConfig drawConfig)
        {
            throw new NotImplementedException();
        }

        private static void ReadGravity(BinaryReader reader, PmmGravity gravity)
        {
            throw new NotImplementedException();
        }

        private static void ReadSelfShadow(BinaryReader reader, PmmSelfShadow selfShadow)
        {
            throw new NotImplementedException();
        }

        private static void ReadColorConfig(BinaryReader reader, PmmDrawConfig drawConfig)
        {
            throw new NotImplementedException();
        }

        private static void ReadUncomittedFollowingStateCamera(BinaryReader reader, PmmCamera camera)
        {
            throw new NotImplementedException();
        }

        private static void ReadViewFollowing(BinaryReader reader, PmmEditorState editorState)
        {
            throw new NotImplementedException();
        }

        private static void ReadGroundPhysics(BinaryReader reader, PmmDrawConfig drawConfig)
        {
            throw new NotImplementedException();
        }

        private static void ReadFrameLocation(BinaryReader reader, PmmEditorState editorState)
        {
            throw new NotImplementedException();
        }

        private static (byte Model, int Target) ReadRangeSelectionTargetIndex(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
