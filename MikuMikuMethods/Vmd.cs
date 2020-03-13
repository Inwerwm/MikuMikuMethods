using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace MikuMikuMethods.Vmd
{
    /* zyando/MMDataIO https://github.com/zyando/MMDataIO から改変
     * 
     * MIT License
     * 
     * Copyright (c) 2018 zyando
     * 
     * Permission is hereby granted, free of charge, to any person obtaining a copy
     * of this software and associated documentation files (the "Software"), to deal
     * in the Software without restriction, including without limitation the rights
     * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
     * copies of the Software, and to permit persons to whom the Software is
     * furnished to do so, subject to the following conditions:
     * 
     * The above copyright notice and this permission notice shall be included in all
     * copies or substantial portions of the Software.
     * 
     * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
     * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
     * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
     * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
     * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
     * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
     * SOFTWARE.
    */

    public class VocaloidMotionData
    {
        public static readonly Encoding Encoding = Encoding.GetEncoding("Shift_JIS");

        public const int HEADER_LENGTH = 30;
        public const int MODEL_NAME_LENGTH = 20;
        public const int BONE_NAME_LENGTH = 15;
        public const int MORPH_NAME_LENGTH = 15;
        public const int IK_NAME_LENGTH = 20;

        // ヘッダ
        public string header = "Vocaloid Motion Data 0002";
        public string ModelName { get; set; }

        public List<VmdMotionFrameData> MotionFrames { get; set; }
        public List<VmdMorphFrameData> MorphFrames { get; set; }
        public List<VmdCameraFrameData> CameraFrames { get; set; }
        public List<VmdLightFrameData> LightFrames { get; set; }
        public List<VmdShadowFrameData> ShadowFrames { get; set; }
        public List<VmdPropertyFrameData> PropertyFrames { get; set; }

        public VocaloidMotionData()
        {
            ModelName = "";
            MotionFrames = new List<VmdMotionFrameData>();
            MorphFrames = new List<VmdMorphFrameData>();
            CameraFrames = new List<VmdCameraFrameData>();
            LightFrames = new List<VmdLightFrameData>();
            ShadowFrames = new List<VmdShadowFrameData>();
            PropertyFrames = new List<VmdPropertyFrameData>();
        }

        public VocaloidMotionData(BinaryReader reader)
        {
            ModelName = "";
            MotionFrames = new List<VmdMotionFrameData>();
            MorphFrames = new List<VmdMorphFrameData>();
            CameraFrames = new List<VmdCameraFrameData>();
            LightFrames = new List<VmdLightFrameData>();
            ShadowFrames = new List<VmdShadowFrameData>();
            PropertyFrames = new List<VmdPropertyFrameData>();
            Read(reader);
        }

        public void Clear()
        {
            header = "Vocaloid Motion Data 0002";
            ModelName = "";
            MotionFrames = new List<VmdMotionFrameData>();
            MorphFrames = new List<VmdMorphFrameData>();
            PropertyFrames = new List<VmdPropertyFrameData>();
        }

        public void Merge(VocaloidMotionData data)
        {
            if (data.MotionFrames != null)
                MotionFrames.AddRange(data.MotionFrames);
            if (data.MorphFrames != null)
                MorphFrames.AddRange(data.MorphFrames);
            if (data.CameraFrames != null)
                CameraFrames.AddRange(data.CameraFrames);
            if (data.LightFrames != null)
                LightFrames.AddRange(data.LightFrames);
            if (data.ShadowFrames != null)
                ShadowFrames.AddRange(data.ShadowFrames);
            if (data.PropertyFrames != null)
                PropertyFrames.AddRange(data.PropertyFrames);
        }

        public void Write(BinaryWriter writer)
        {
            writer.WriteTextWithFixedLength(header, HEADER_LENGTH);
            writer.WriteTextWithFixedLength(ModelName, MODEL_NAME_LENGTH);
            WriteVmdFrameData(MotionFrames, writer);
            WriteVmdFrameData(MorphFrames, writer);
            WriteVmdFrameData(CameraFrames, writer);
            WriteVmdFrameData(LightFrames, writer);
            WriteVmdFrameData(ShadowFrames, writer);
            WriteVmdFrameData(PropertyFrames, writer);
        }

        private void WriteVmdFrameData<T>(T data, BinaryWriter writer) where T : IVmdFrameData
        {
            data.Write(writer);
        }

        private void WriteVmdFrameData<T>(List<T> data, BinaryWriter writer) where T : IVmdFrameData
        {
            int len = data.Count;
            writer.Write(len);

            //vmd読み込み高速化のためフレーム時間逆順で書き込み
            data.Sort((x, y) => y.FrameTime.CompareTo(x.FrameTime));
            for (int i = 0; i < len; i++)
            {
                WriteVmdFrameData(data[i], writer);
            }
        }

        public void Read(BinaryReader reader)
        {
            byte[] bChar = reader.ReadBytes(HEADER_LENGTH);
            header = new string(Encoding.GetChars(bChar));
            bChar = reader.ReadBytes(MODEL_NAME_LENGTH);
            ModelName = new string(Encoding.GetChars(bChar));

            uint len = reader.ReadUInt32();
            for (int i = 0; i < len; i++)
            {
                MotionFrames.Add(new VmdMotionFrameData(reader));
            }

            len = reader.ReadUInt32();
            for (int i = 0; i < len; i++)
            {
                MorphFrames.Add(new VmdMorphFrameData(reader));
            }

            //camera
            len = reader.ReadUInt32();
            for (int i = 0; i < len; i++)
            {
                CameraFrames.Add(new VmdCameraFrameData(reader));
            }

            //light
            len = reader.ReadUInt32();
            for (int i = 0; i < len; i++)
            {
                LightFrames.Add(new VmdLightFrameData(reader));
            }

            //self shadow
            len = reader.ReadUInt32();
            for (int i = 0; i < len; i++)
            {
                ShadowFrames.Add(new VmdShadowFrameData(reader));
            }

            len = reader.ReadUInt32();
            for (int i = 0; i < len; i++)
            {
                PropertyFrames.Add(new VmdPropertyFrameData(reader));
            }
        }

        public enum GetKeyIgnoring
        {
            ZeroValue,
            FirstFrame,
            Motion,
            Morph
        }
        public List<string> GetKeyNames(params GetKeyIgnoring[] opt)
        {
            var names = new List<string>();
            IEnumerable<IGrouping<string, VmdMotionFrameData>> motions = MotionFrames.GroupBy(f => f.Name.Trim('\0')); ;
            IEnumerable<IGrouping<string, VmdMorphFrameData>> morphs = MorphFrames.GroupBy(f => f.Name.Trim('\0'));

            if (opt.Contains(GetKeyIgnoring.ZeroValue))
            {
                //motions = MotionFrames.Where(f => (f.Pos != Vector3.Zero) || (f.Rot != Quaternion.Identity)).GroupBy(f => f.Name.Trim('\0'));
                //morphs = MorphFrames.Where(f => f.Weigth!=0).GroupBy(f => f.Name.Trim('\0'));
                motions = motions.Where(g => g.Where(f => (f.Pos != Vector3.Zero) || (f.Rot != Quaternion.Identity)).Count() != 0);
                morphs = morphs.Where(g => g.Where(f => f.Weigth != 0).Count() != 0);
            }

            if (opt.Contains(GetKeyIgnoring.FirstFrame))
            {
                //フレームタイムが0でないvmdを持つグループのみを抽出
                motions = motions.Where(g => g.Where(f => f.FrameTime != 0).Count() != 0);
                morphs = morphs.Where(g => g.Where(f => f.FrameTime != 0).Count() != 0);
            }

            if (!opt.Contains(GetKeyIgnoring.Motion))
            {
                names.AddRange(motions.Select(g => g.Key));
                names.Sort(Utilities.BoneNameComparer.Compare);
            }

            if (!opt.Contains(GetKeyIgnoring.Morph))
                names.AddRange(morphs.Select(g => g.Key));

            return names;
        }
    }
}
