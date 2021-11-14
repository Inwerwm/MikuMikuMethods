namespace MikuMikuMethods.Pmx.IO;

internal class Indexer
{
    public byte IndexSize { get; init; }
    public bool IsVertex { get; init; }

    public Indexer(byte indexSize, bool isVertex)
    {
        if (!new byte[] { 1, 2, 4 }.Contains(indexSize))
            throw new ArgumentOutOfRangeException("インデックスの大きさ指定が不正です。");

        IndexSize = indexSize;
        IsVertex = isVertex;
    }

    public int Read(BinaryReader reader) => IsVertex ? ReadAsVertex(reader, IndexSize) : ReadAsOther(reader, IndexSize);
    public void Write(BinaryWriter writer, int index)
    {
        if (IsVertex)
        {
            WriteAsVertex(writer, index);
        }
        else
        {
            WriteAsOther(writer, index);
        }
    }

    private int ReadAsOther(BinaryReader reader, byte indexSize) => indexSize switch
    {
        1 => reader.ReadSByte(),
        2 => reader.ReadInt16(),
        4 => reader.ReadInt32(),
        _ => throw new FormatException("インデックスサイズが不正です。")
    };

    private int ReadAsVertex(BinaryReader reader, byte indexSize) => indexSize switch
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

    private void WriteAsOther(BinaryWriter writer, int index)
    {
        switch (IndexSize)
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

    private void WriteAsVertex(BinaryWriter writer, int index)
    {
        switch (IndexSize)
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
