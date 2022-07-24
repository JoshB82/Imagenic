namespace Imagenic.Benchmarking;

[AttributeUsage(AttributeTargets.Method)]
public sealed class PostBenchmarkAttribute : Attribute
{
    #region Fields and Properties

    public bool DisplayRender { get; }

    #endregion

    #region Constructors

    public PostBenchmarkAttribute(bool displayRender)
    {
        DisplayRender = displayRender;
    }

    #endregion
}