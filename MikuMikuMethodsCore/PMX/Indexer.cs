using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMX
{
    internal class Indexer
    {
        public static int ReadAsOther(BinaryReader reader, byte indexSize) => indexSize switch
        {
            1 => reader.ReadSByte(),
            2 => reader.ReadInt16(),
            4 => reader.ReadInt32(),
            _ => throw new FormatException("インデックスサイズが不正です。")
        };

        public static int ReadAsVertex(BinaryReader reader, byte indexSize) => indexSize switch
        {
            1 => reader.ReadByte(),
            2 => reader.ReadUInt16(),
            4 => reader.ReadInt32(),
            _ => throw new FormatException("インデックスサイズが不正です。")
        };

        public static byte GetSizeCodeAsOther(int size) => size switch
        {
            <= 0 => throw new FormatException("インデックスサイズが負数です。"),
            <= sbyte.MaxValue => 1,
            <= short.MaxValue => 2,
            <= int.MaxValue => 4
        };

        public static byte GetSizeCodeAsVertex(int size) => size switch
        {
            <= 0 => throw new FormatException("インデックスサイズが負数です。"),
            <= byte.MaxValue => 1,
            <= ushort.MaxValue => 2,
            <= int.MaxValue => 4
        };

        public static void WriteAsOther(BinaryWriter writer, int index)
        {
            switch (GetSizeCodeAsOther(index))
            {
                case 1:
                    writer.Write((sbyte)index);
                    break;
                case 2:
                    writer.Write((short)index);
                    break;
                case 4:
                    writer.Write(index);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("インデックスの大きさが対応範囲外です。");
            }
        }

        public static void WriteAsVertex(BinaryWriter writer, int index)
        {
            switch (GetSizeCodeAsVertex(index))
            {
                case 1:
                    writer.Write((byte)index);
                    break;
                case 2:
                    writer.Write((ushort)index);
                    break;
                case 4:
                    writer.Write(index);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("インデックスの大きさが対応範囲外です。");
            }
        }
    }
}
