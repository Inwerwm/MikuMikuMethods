namespace MikuMikuMethods.Common;
public class BinaryReaderWithEvent : BinaryReader
{
    public delegate void ReadValueEventHandler(object value, Type type);
    public event ReadValueEventHandler? OnRead;

    public BinaryReaderWithEvent(Stream input, System.Text.Encoding encoding) : base(input, encoding)
    {
    }

    public override bool ReadBoolean()
    {
        bool v = base.ReadBoolean();
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override byte ReadByte()
    {
        byte v = base.ReadByte();
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override byte[] ReadBytes(int count)
    {
        byte[] v = base.ReadBytes(count);
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override int ReadInt32()
    {
        int v = base.ReadInt32();
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override float ReadSingle()
    {
        float v = base.ReadSingle();
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override int Read()
    {
        int v = base.Read();
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override int Read(byte[] buffer, int index, int count)
    {
        int v = base.Read(buffer, index, count);
        OnRead?.Invoke(buffer, buffer.GetType());
        return v;
    }

    public override int Read(char[] buffer, int index, int count)
    {
        int v = base.Read(buffer, index, count);
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override int Read(Span<byte> buffer)
    {
        int v = base.Read(buffer);
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override int Read(Span<char> buffer)
    {
        int v = base.Read(buffer);
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override char ReadChar()
    {
        char v = base.ReadChar();
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override char[] ReadChars(int count)
    {
        char[] v = base.ReadChars(count);
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override decimal ReadDecimal()
    {
        decimal v = base.ReadDecimal();
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override double ReadDouble()
    {
        double v = base.ReadDouble();
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override Half ReadHalf()
    {
        Half v = base.ReadHalf();
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override short ReadInt16()
    {
        short v = base.ReadInt16();
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override long ReadInt64()
    {
        long v = base.ReadInt64();
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override sbyte ReadSByte()
    {
        sbyte v = base.ReadSByte();
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override string ReadString()
    {
        string v = base.ReadString();
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override ushort ReadUInt16()
    {
        ushort v = base.ReadUInt16();
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override uint ReadUInt32()
    {
        uint v = base.ReadUInt32();
        OnRead?.Invoke(v, v.GetType());
        return v;
    }

    public override ulong ReadUInt64()
    {
        ulong v = base.ReadUInt64();
        OnRead?.Invoke(v, v.GetType());
        return v;
    }
}
