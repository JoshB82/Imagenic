namespace Imagenic.Benchmarking;

[AttributeUsage(AttributeTargets.Method)]
public class BenchmarkToRunAttribute : Attribute
{
    #region Fields and Properties

    public bool RendersImage { get; }

    #endregion

    #region Constructors

    public BenchmarkToRunAttribute(bool rendersImage)
    {
        RendersImage = rendersImage;
    }

    #endregion
}