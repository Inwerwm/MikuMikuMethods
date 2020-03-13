using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods
{
    public static class BinaryIOExtensions
    {
        public static void WriteTextWithFixedLength(this BinaryWriter writer, string text, int fixedLength, string filled = "\0")
        {
            byte[] bytes = Encoding.GetEncoding("Shift_JIS").GetBytes(text);

            if (bytes.Length > fixedLength)
            {
                byte[] fixedBytes = new byte[fixedLength];
                Array.Copy(bytes, fixedBytes, fixedLength);
                bytes = fixedBytes;
            }

            writer.Write(bytes);

            int remain = fixedLength - bytes.Length;
            if (remain > 0)
            {
                writer.WriteFiller(Encoding.GetEncoding("Shift_JIS").GetBytes(filled), remain);
            }
        }

        private static void WriteFiller(this BinaryWriter writer, byte[] filler, int fillerLength)
        {
            if (filler.Length <= 0 || fillerLength <= 0) return;

            byte lastData = filler[filler.Length - 1];

            int fillerIdx = 0;
            for (int remain = fillerLength; remain > 0; remain--)
            {
                byte bVal = fillerIdx < filler.Length ? filler[fillerIdx++] : lastData;
                writer.Write(bVal);
            }
        }

        public static void Write(this BinaryWriter writer, Vector2 vec)
        {
            writer.Write(vec.X);
            writer.Write(vec.Y);
        }

        public static void Write(this BinaryWriter writer, Vector3 vec)
        {
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
        }

        public static void Write(this BinaryWriter writer, Vector4 vec)
        {
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
            writer.Write(vec.W);
        }

        public static void Write(this BinaryWriter writer, Quaternion vec)
        {
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
            writer.Write(vec.W);
        }

        public static Vector2 ReadVector2(this BinaryReader reader)
        {
            Vector2 vec = new Vector2();
            vec.X = reader.ReadSingle();
            vec.Y = reader.ReadSingle();
            return vec;
        }

        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            Vector3 vec = new Vector3();
            vec.X = reader.ReadSingle();
            vec.Y = reader.ReadSingle();
            vec.Z = reader.ReadSingle();
            return vec;
        }

        public static Vector4 ReadVector4(this BinaryReader reader)
        {
            Vector4 vec = new Vector4();
            vec.X = reader.ReadSingle();
            vec.Y = reader.ReadSingle();
            vec.Z = reader.ReadSingle();
            vec.W = reader.ReadSingle();
            return vec;
        }

        public static Quaternion ReadQuaternion(this BinaryReader reader)
        {
            Quaternion vec = new Quaternion();
            vec.X = reader.ReadSingle();
            vec.Y = reader.ReadSingle();
            vec.Z = reader.ReadSingle();
            vec.W = reader.ReadSingle();
            return vec;
        }
    }
}
