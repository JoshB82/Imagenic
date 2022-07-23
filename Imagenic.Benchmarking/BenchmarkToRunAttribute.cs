namespace Imagenic.Benchmarking;

[AttributeUsage(AttributeTargets.Method)]
public class BenchmarkToRunAttribute : Attribute
{
    #region Fields and Properties

    public bool DisplayRender { get; }

    #endregion

    #region Constructors

    public BenchmarkToRunAttribute(bool displayRender)
    {
        DisplayRender = displayRender;
    }

    #endregion
}