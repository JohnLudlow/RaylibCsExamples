using System.Runtime.InteropServices;
using Raylib_cs;

namespace RaylibExamples.Core.CustomLogging;

public unsafe class Program
{
    [UnmanagedCallersOnly(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static void LogCustom(int logLevel, sbyte* text, sbyte* args)
    {
        var message = Logging.GetLogMessage(new IntPtr(text), new IntPtr(args));

        Console.ForegroundColor = (TraceLogLevel)logLevel switch
        {
            TraceLogLevel.All      => ConsoleColor.White,
            TraceLogLevel.Trace    => ConsoleColor.Black,
            TraceLogLevel.Debug    => ConsoleColor.Blue,
            TraceLogLevel.Info     => ConsoleColor.Black,
            TraceLogLevel.Warning  => ConsoleColor.DarkYellow,
            TraceLogLevel.Error    => ConsoleColor.Red,
            TraceLogLevel.Fatal    => ConsoleColor.Red,
            TraceLogLevel.None     => ConsoleColor.White,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };

        Console.WriteLine($"Custom " + message);
        // Console.ResetColor();
    }

    public static int Main()
    {
        const int screenWidth = 800;
        const int screenHeight = 450;

        Raylib.SetTraceLogCallback(&LogCustom);
        Raylib.InitWindow(screenWidth, screenHeight, "raylib [core] example - custom logging");
        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RayWhite);
            Raylib.DrawText("Check out the console output to see the custom logger in action!", 60, 200, 20, Color.LightGray);
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
        Raylib.SetTraceLogCallback(&Logging.LogConsole);

        return 0;
    }
}