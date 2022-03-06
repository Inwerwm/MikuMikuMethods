namespace MikuMikuMethods.Pmm.ElementState;

public class PmmMorphState
{
    /// <summary>
    /// 係数
    /// </summary>
    public float Weight { get; set; }

    public PmmMorphState DeepCopy() => new() { Weight = Weight };
}
