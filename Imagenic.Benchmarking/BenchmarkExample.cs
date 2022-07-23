using BenchmarkDotNet.Attributes;

namespace Imagenic.Benchmarking;

public partial class Benchmarks
{
    [BenchmarkToRun(true)]
    [Benchmark]
    public void Test()
    {
        Console.WriteLine("Test");
    }
}