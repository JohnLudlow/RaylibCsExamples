// See https://aka.ms/new-console-template for more information

// See https://aka.ms/new-console-template for more information
using Raylib_cs;

namespace RaylibExamples;

public class ExampleInfo(string name, Func<int> main)
{
    public string Name { get; init; } = name;

    public Func<int> Main { get; init; } = main;
}

internal sealed class ExampleList
{
    internal static ExampleInfo[] AllExamples = [
        new ExampleInfo("Camera2dPlatformer", Core.Camera2dPlatformer.Example),
        new ExampleInfo("Camera3dFree", Core.Camera3dFree.Example)
    ];

    public static ExampleInfo? GetExample(string name) => Array.Find(AllExamples, x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
}

internal sealed class Program
{
    private static unsafe void Main(string[] args)
    {
        Raylib.SetTraceLogCallback(&Logging.LogConsole);

        if (args.Length > 0)
        {
            var example = ExampleList.GetExample(args[0]);
            example?.Main?.Invoke();
        }
        else
        {
            RunExamples(ExampleList.AllExamples);
        }
    }

    private static void RunExamples(ExampleInfo[] examples)
    {
        var configFlags = Enum.GetValues<ConfigFlags>();

        foreach (var example in examples)
        {
            example?.Main?.Invoke();
            foreach (var flag in configFlags)
            {
                Raylib.ClearWindowState(flag);
            }
        }
    }
}