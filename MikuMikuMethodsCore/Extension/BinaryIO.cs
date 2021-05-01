using System.IO;
using System.Linq;
using System.Numerics;

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
        public static Vector2 ReadVector2(this BinaryReader reader) =>
            new Vector2(reader.ReadSingle(), reader.ReadSingle());

        /// <summary>
        /// Reads a 3 dimension vector including three 4-byte floating point values from the current stream and advances the current position of the stream by 12 bytes.
        /// </summary>
        /// <returns>A 3 dimension vector including three 4-byte floating point values read from the current stream.</returns>
        public static Vector3 ReadVector3(this BinaryReader reader) =>
            new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

        /// <summary>
        /// Reads a 4 dimension vector including four 4-byte floating point values from the current stream and advances the current position of the stream by 16 bytes.
        /// </summary>
        /// <returns>A 4 dimension vector including four 4-byte floating point values read from the current stream.</returns>
        public static Vector4 ReadVector4(this BinaryReader reader) =>
            new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

        /// <summary>
        /// Reads a quaternion including four 4-byte floating point values from the current stream and advances the current position of the stream by 16 bytes.
        /// </summary>
        /// <returns>A quaternion including four 4-byte floating point values read from the current stream.</returns>
        public static Quaternion ReadQuaternion(this BinaryReader reader) =>
            new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

        /// <summary>
        /// 指定文字数の文字列をバイナリから読み込む
        /// </summary>
        /// <param name="length">文字数</param>
        /// <param name="encoding">エンコード形式</param>
        /// <param name="filler">終端文字</param>
        /// <returns>読み込んだ文字列</returns>
        public static string ReadString(this BinaryReader reader, int length, System.Text.Encoding encoding, char? filler = '\0')
        {
            var readBytes = reader.ReadBytes(length);
            string str = encoding.GetString(readBytes);
            if (filler is null)
                return str;

            return str.IndexOf(filler.Value) switch
            {
                < 0 => str,
                int i => str.Remove(i)
            };
        }

        /// <summary>
        /// <para>[0, 1]の浮動小数点数で表された色情報をバイナリから読み込む</para>
        /// <para>{A, R, G, B}の順で読み込む</para>
        /// </summary>
        /// <returns>読み込んだ浮動小数点数表現色</returns>
        public static ColorF ReadSingleARGB(this BinaryReader reader) =>
            ColorF.FromARGB(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

        /// <summary>
        /// <para>[0, 1]の浮動小数点数で表された色情報をバイナリから読み込む</para>
        /// <para>{R, G, B}の順で読み込む</para>
        /// </summary>
        /// <returns>読み込んだ浮動小数点数表現色</returns>
        public static ColorF ReadSingleRGB(this BinaryReader reader) =>
            ColorF.FromARGB(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
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

        /// <summary>
        /// 指定長で文字列をバイナリに書き込み
        /// <para>文字列の最後には<c>'\0'</c>が付加される</para>
        /// </summary>
        /// <param name="value">書き込む文字列</param>
        /// <param name="length">書き込む長さ</param>
        /// <param name="encoding">エンコード形式</param>
        /// <param name="filler">余った領域に充填する文字</param>
        public static void Write(this BinaryWriter writer, string value, int length, System.Text.Encoding encoding, char filler = '\0')
        {
            //fillerで充填した書き込み用配列を生成
            var bytesToWrite = encoding.GetBytes(Enumerable.Repeat(filler, length).ToArray());

            //書き込み用配列に文字列を転写
            encoding.GetBytes(value.Append('\0').ToArray()).Take(length).ToArray().CopyTo(bytesToWrite, 0);

            writer.Write(bytesToWrite);
        }

        /// <summary>
        /// 浮動小数点色をバイナリに書き込み
        /// </summary>
        /// <param name="value">書き込む色</param>
        /// <param name="isWriteAlpha">アルファ値の情報を書き込むか</param>
        public static void Write(this BinaryWriter writer, ColorF value, bool isWriteAlpha)
        {
            if (isWriteAlpha)
                writer.Write(value.A);

            writer.Write(value.R);
            writer.Write(value.G);
            writer.Write(value.B);
        }
    }
}
