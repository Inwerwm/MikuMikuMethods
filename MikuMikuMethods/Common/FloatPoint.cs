namespace MikuMikuMethods;

public record struct FloatPoint(float X, float Y)
{
    public static implicit operator (float X, float Y)(FloatPoint value)
    {
        return (value.X, value.Y);
    }

    public static implicit operator FloatPoint((float X, float Y) value)
    {
        return new FloatPoint(value.X, value.Y);
    }
}