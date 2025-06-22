using System.Numerics;
using Raylib_cs;

const int screenWidth = 800;
const int screenHeight = 450;

Raylib.SetConfigFlags(ConfigFlags.VSyncHint | ConfigFlags.Msaa4xHint);
Raylib.InitWindow(screenWidth, screenHeight, "raylib [core] example - window flags");

var ballPosition = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
var ballSpeed = new Vector2(5, 4);
var ballRadius = 20;
var framesCounter = 0;

while (!Raylib.WindowShouldClose())
{
    framesCounter = UpdateWindowState(framesCounter);

    ballPosition += ballSpeed;
    if (ballPosition.X >= (Raylib.GetScreenWidth() - ballRadius) || (ballPosition.X <= ballRadius))
    {
        ballPosition.X *= -1f;
    }

    if (ballPosition.Y >= (Raylib.GetScreenWidth() - ballRadius) || (ballPosition.Y <= ballRadius))
    {
        ballPosition.Y *= -1f;
    }

    Raylib.BeginDrawing();
    {
        Raylib.ClearBackground(
            Raylib.IsWindowState(ConfigFlags.TransparentWindow) ? Color.Blank : Color.RayWhite
        );

        Raylib.DrawCircleV(ballPosition, ballRadius, Color.Maroon);
        Raylib.DrawRectangleLinesEx(new Rectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight()), 4, Color.RayWhite);
        Raylib.DrawFPS(10, 10);
        Raylib.DrawText($"Screen Size {Raylib.GetScreenWidth()}, {Raylib.GetScreenHeight}", 10, 40, 10, Color.Green);

        var colorOn = Color.Lime;
        var colorOff = Color.Maroon;

            Raylib.DrawText("Following flags can be set after window creation:", 10, 60, 10, Color.Gray);

            DrawWindowState(ConfigFlags.FullscreenMode, "[F] FLAG_FULLSCREEN_MODE: ", 10, 80, 10);
            DrawWindowState(ConfigFlags.ResizableWindow, "[R] FLAG_WINDOW_RESIZABLE: ", 10, 100, 10);
            DrawWindowState(ConfigFlags.UndecoratedWindow, "[D] FLAG_WINDOW_UNDECORATED: ", 10, 120, 10);
            DrawWindowState(ConfigFlags.HiddenWindow, "[H] FLAG_WINDOW_HIDDEN: ", 10, 140, 10);
            DrawWindowState(ConfigFlags.MinimizedWindow, "[N] FLAG_WINDOW_MINIMIZED: ", 10, 160, 10);
            DrawWindowState(ConfigFlags.MaximizedWindow, "[M] FLAG_WINDOW_MAXIMIZED: ", 10, 180, 10);
            DrawWindowState(ConfigFlags.UnfocusedWindow, "[G] FLAG_WINDOW_UNFOCUSED: ", 10, 200, 10);
            DrawWindowState(ConfigFlags.TopmostWindow, "[T] FLAG_WINDOW_TOPMOST: ", 10, 220, 10);
            DrawWindowState(ConfigFlags.AlwaysRunWindow, "[A] FLAG_WINDOW_ALWAYS_RUN: ", 10, 240, 10);
            DrawWindowState(ConfigFlags.VSyncHint, "[V] FLAG_VSYNC_HINT: ", 10, 260, 10);

            Raylib.DrawText("Following flags can only be set before window creation:", 10, 300, 10, Color.Gray);

            DrawWindowState(ConfigFlags.HighDpiWindow, "[F] FLAG_WINDOW_HIGHDPI: ", 10, 320, 10);
            DrawWindowState(ConfigFlags.TransparentWindow, "[F] FLAG_WINDOW_TRANSPARENT: ", 10, 340, 10);
            DrawWindowState(ConfigFlags.Msaa4xHint, "[F] FLAG_MSAA_4X_HINT: ", 10, 360, 10);

    }
    Raylib.EndDrawing();
}

static void ToggleWindowState(ConfigFlags flag)
{
    if (Raylib.IsWindowState(flag))
    {
        Raylib.ClearWindowState(flag);
    }
    else
    {
        Raylib.SetWindowState(flag);
    }
}

static void ToggleFlagForDelayFrames(ConfigFlags flag, ref int framesCounter, int delayFrames)
{
    framesCounter++;
    if (framesCounter >= delayFrames)
    {
        Raylib.ClearWindowState(flag);
    }
}

static void DrawWindowState(ConfigFlags flag, string text, int posX, int posY, int fontSize)
{
    var onColor = Color.Lime;
    var offColor = Color.Maroon;

    if (Raylib.IsWindowState(flag))
    {
        Raylib.DrawText($"{text} on", posX, posY, fontSize, onColor);
    }
    else
    {
        Raylib.DrawText($"{text} off", posX, posY, fontSize, offColor);
    }
}

static int UpdateWindowState(int framesCounter)
{
    if (Raylib.IsKeyPressed(KeyboardKey.F))
    {
        Raylib.ToggleFullscreen();
    }

    if (Raylib.IsKeyPressed(KeyboardKey.R))
    {
        ToggleWindowState(ConfigFlags.ResizableWindow);
    }

    if (Raylib.IsKeyPressed(KeyboardKey.D))
    {
        ToggleWindowState(ConfigFlags.UndecoratedWindow);
    }

    if (Raylib.IsKeyPressed(KeyboardKey.H))
    {
        ToggleWindowState(ConfigFlags.HiddenWindow);
    }

    if (Raylib.IsWindowState(ConfigFlags.HiddenWindow))
    {
        ToggleFlagForDelayFrames(ConfigFlags.HiddenWindow, ref framesCounter, 120);
    }

    if (Raylib.IsKeyPressed(KeyboardKey.N))
    {
        if (!Raylib.IsWindowState(ConfigFlags.MinimizedWindow))
        {
            Raylib.MinimizeWindow();
        }
    }

    if (Raylib.IsWindowState(ConfigFlags.MinimizedWindow))
    {
        framesCounter++;
        if (framesCounter >= 240)
        {
            Raylib.RestoreWindow();
        }
    }

    if (Raylib.IsKeyPressed(KeyboardKey.M))
    {
        if (Raylib.IsWindowState(ConfigFlags.MaximizedWindow))
        {
            Raylib.RestoreWindow();
        }
        else
        {
            Raylib.MaximizeWindow();
        }
    }

    if (Raylib.IsKeyPressed(KeyboardKey.U))
    {
        ToggleWindowState(ConfigFlags.UnfocusedWindow);
    }

    if (Raylib.IsKeyPressed(KeyboardKey.T))
    {
        ToggleWindowState(ConfigFlags.TopmostWindow);
    }

    if (Raylib.IsKeyPressed(KeyboardKey.A))
    {
        ToggleWindowState(ConfigFlags.AlwaysRunWindow);
    }

    if (Raylib.IsKeyPressed(KeyboardKey.V))
    {
        ToggleWindowState(ConfigFlags.VSyncHint);
    }

    return framesCounter;
}