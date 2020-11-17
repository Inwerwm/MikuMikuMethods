using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.Extension
{
    /// <summary>
    /// BinaryReader向け拡張メソッド
    /// </summary>
    public static class BinaryRead
    {
        /// <summary>
        /// Reads a 2 dimension vector including two 4-byte floating point values from the current stream and advances the current position of the stream by 8 bytes.
        /// </summary>
        /// <returns>A 2 dimension vector including two 4-byte floating point values read from the current stream.</returns>
        public static Vector2 ReadVector2(this BinaryReader reader)
        {
            Vector2 vec = new Vector2();
            vec.X = reader.ReadSingle();
            vec.Y = reader.ReadSingle();
            return vec;
        }

        /// <summary>
        /// Reads a 3 dimension vector including three 4-byte floating point values from the current stream and advances the current position of the stream by 12 bytes.
        /// </summary>
        /// <returns>A 3 dimension vector including three 4-byte floating point values read from the current stream.</returns>
        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            Vector3 vec = new Vector3();
            vec.X = reader.ReadSingle();
            vec.Y = reader.ReadSingle();
            vec.Z = reader.ReadSingle();
            return vec;
        }

        /// <summary>
        /// Reads a 4 dimension vector including four 4-byte floating point values from the current stream and advances the current position of the stream by 16 bytes.
        /// </summary>
        /// <returns>A 4 dimension vector including four 4-byte floating point values read from the current stream.</returns>
        public static Vector4 ReadVector4(this BinaryReader reader)
        {
            Vector4 vec = new Vector4();
            vec.X = reader.ReadSingle();
            vec.Y = reader.ReadSingle();
            vec.Z = reader.ReadSingle();
            vec.W = reader.ReadSingle();
            return vec;
        }

        /// <summary>
        /// Reads a quaternion including four 4-byte floating point values from the current stream and advances the current position of the stream by 16 bytes.
        /// </summary>
        /// <returns>A quaternion including four 4-byte floating point values read from the current stream.</returns>
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

    /// <summary>
    /// BinaryWriter向け拡張メソッド
    /// </summary>
    public static class BinaryWrite
    {
        /// <summary>
        /// Writes a 2 dimension vector including two four-byte floating-point value to the current stream and advances the stream position by 8 bytes.
        /// </summary>
        /// <param name="value">The 2 dimension vector including two four-byte floating-point value to write.</param>
        public static void Write(this BinaryWriter writer, Vector2 value)
        {
            writer.Write(value.X);
            writer.Write(value.Y);
        }

        /// <summary>
        /// Writes a 3 dimension vector including three four-byte floating-point value to the current stream and advances the stream position by 12 bytes.
        /// </summary>
        /// <param name="value">The 3 dimension vector including three four-byte floating-point value to write.</param>
        public static void Write(this BinaryWriter writer, Vector3 value)
        {
            writer.Write(value.X);
            writer.Write(value.Y);
            writer.Write(value.Z);
        }

        /// <summary>
        /// Writes a 4 dimension vector including four four-byte floating-point value to the current stream and advances the stream position by 16 bytes.
        /// </summary>
        /// <param name="value">The 4 dimension vector including four four-byte floating-point value to write.</param>
        public static void Write(this BinaryWriter writer, Vector4 value)
        {
            writer.Write(value.X);
            writer.Write(value.Y);
            writer.Write(value.Z);
            writer.Write(value.W);
        }

        /// <summary>
        /// Writes a quaternion including four four-byte floating-point value to the current stream and advances the stream position by 16 bytes.
        /// </summary>
        /// <param name="value">The quaternion including four four-byte floating-point value to write.</param>
        public static void Write(this BinaryWriter writer, Quaternion value)
        {
            writer.Write(value.X);
            writer.Write(value.Y);
            writer.Write(value.Z);
            writer.Write(value.W);
        }
    }
}
