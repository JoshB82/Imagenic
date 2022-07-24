using BenchmarkDotNet.Running;
using System.Reflection;

namespace Imagenic.Benchmarking;

public class Program
{
    public static void Main(string[] args)
    {
        Introduction();
        var selectedBenchmark = GetBenchmarkToRun();
        RunBenchmark(selectedBenchmark);
    }

    private static void Introduction()
    {
        Console.WriteLine("Benchmarking");
        Console.WriteLine("============\n");
    }

    private static BenchmarkMethodInfo GetBenchmarkToRun()
    {
        Console.WriteLine("Please select a benchmark to run:\n");
        var benchmarks = GetBenchmarks();
        for (int i = 0; i < benchmarks.Count; i++)
        {
            Console.WriteLine($"[{i + 1}] - {benchmarks[i].Name}");
        }
        Console.WriteLine();
        var input = Console.ReadLine();
        Console.WriteLine();
        return benchmarks[int.Parse(input) - 1];
    }

    private static void RunBenchmark(BenchmarkMethodInfo selectedBenchmark)
    {
        var results = BenchmarkRunner.Run<Benchmarks>();
        var result = selectedBenchmark.Reference.Invoke(new Benchmarks(), selectedBenchmark.Reference.GetParameters());

        /*if (selectedBenchmark.DisplayRender)
        {
            var path = "";
            var image = ((Image)result).Save(path);
            Process.Start(path);
        }*/
    }

    private static List<BenchmarkMethodInfo> GetBenchmarks()
    {
        var methods = typeof(Benchmarks).GetMethods().Where(m =>
            m.GetCustomAttributes(typeof(PostBenchmarkAttribute), false).Length > 0);

        if (!methods.Any())
        {
            throw new Exception("Could not find any benchmarks to run.");
        }

        var methodNames = methods.Select(m => m.Name);
        var benchmarkMethodInfos = new List<BenchmarkMethodInfo>();

        foreach (var method in methods)
        {
            var attributes = (PostBenchmarkAttribute[])method.GetCustomAttributes(typeof(PostBenchmarkAttribute), false);
            foreach (var attribute in attributes)
            {
                benchmarkMethodInfos.Add(new BenchmarkMethodInfo
                {
                    Name = method.Name,
                    DisplayRender = attribute.DisplayRender,
                    Reference = method
                });
            }
        }

        return benchmarkMethodInfos;
    }
}

public class BenchmarkMethodInfo
{
    public string Name { get; set; }
    public bool DisplayRender { get; set; }
    public MethodInfo Reference { get; set; }
}

public partial class Benchmarks { }