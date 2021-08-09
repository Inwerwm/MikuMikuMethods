using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMX
{
    /// <summary>
    /// PMX用エンコードを気にせず文字列を読み書きできるようにしたクラス
    /// </summary>
    internal class StringEncoder
    {
        private System.Text.Encoding Encoding { get; init; }

        public StringEncoder(System.Text.Encoding encoding)
        {
            Encoding = encoding;
        }

        public byte[] Encode(string str) => Encoding.GetBytes(str);

        public string Decode(byte[] bytes) => Encoding.GetString(bytes, 0, bytes.Length);

        public string Read(BinaryReader reader)
        {
            var length = reader.ReadInt32();
            return Decode(reader.ReadBytes(length));
        }

        public void Write(BinaryWriter writer, string str)
        {
            var bytes = Encode(str);
            writer.Write(bytes.Length);
            writer.Write(bytes);
        }
    }
}
