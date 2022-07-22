namespace Imagenic.Benchmarking;

public class Program
{
    public static void Main(string[] args)
    {
        Introduction();
    }

    private static void Introduction()
    {
        Console.WriteLine("Benchmarking");
        Console.WriteLine("============\n");

        Console.WriteLine("Please select a benchmark to run:");
        foreach (var benchmarkMethodInfo in GetBenchmarks())
        {
            Console.WriteLine($"- {benchmarkMethodInfo}");
        }
    }

    private static IEnumerable<BenchmarkMethodInfo> GetBenchmarks()
    {
        var methods = typeof(Benchmarks).GetMethods().Where(m =>
            m.GetCustomAttributes(typeof(BenchmarkToRunAttribute), false).Length > 0);

        if (!methods.Any())
        {
            throw new Exception("Could not find any benchmarks to run.");
        }

        var methodNames = methods.Select(m => m.Name);
        return methods.Select(m => new BenchmarkMethodInfo
        {
            Name = m.Name
        });
    }
}

public class BenchmarkMethodInfo
{
    public string Name { get; set; }
}

public partial class Benchmarks { }