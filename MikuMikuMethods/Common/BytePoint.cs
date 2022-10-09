namespace MikuMikuMethods;

public record struct BytePoint(byte X, byte Y)
{
    public static implicit operator (byte X, byte Y)(BytePoint value)
    {
        return (value.X, value.Y);
    }

    public static implicit operator BytePoint((byte X, byte Y) value)
    {
        return new BytePoint(value.X, value.Y);
    }
}
