using BenchmarkDotNet.Attributes;

namespace Imagenic.Benchmarking;

public partial class Benchmarks
{
    [PostBenchmark(true)]
    [Benchmark]
    public void Test()
    {
        Console.WriteLine("Test");
    }
}