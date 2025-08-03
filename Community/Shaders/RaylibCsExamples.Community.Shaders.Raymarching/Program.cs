using System.Numerics;
using Raylib_cs;

namespace RaylibCsExamples.Community.Shaders.Raymarching;

public class Program
{
    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        var screenWidth = 1600;
        var screenHeight = 900;

        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        Raylib.InitWindow(screenWidth, screenHeight, "raylib [shaders] example - raymarching shapes");
        var camera = new Camera3D
        {
            Position = new Vector3(2.5f, 2.5f, 3.0f),
            Target = new Vector3(0.0f, 0.0f, 0.7f),
            Up = new Vector3(0.0f, 1.0f, 0.0f),
            FovY = 65.0f,
        };

        var shader = Raylib.LoadShader(null, "resources/shaders/glsl330/raymarching.fs");

        var viewEyeLoc = Raylib.GetShaderLocation(shader, "viewEye");
        var viewCenterLoc = Raylib.GetShaderLocation(shader, "viewCenter");
        var runTimeLoc = Raylib.GetShaderLocation(shader, "runTime");
        var resolutionLoc = Raylib.GetShaderLocation(shader, "resolution");

        var resolution = new Vector2(screenWidth, screenHeight);

        Raylib.SetShaderValue(shader, resolutionLoc, resolution, ShaderUniformDataType.Vec2);

        var runTime = 0.0f;
        Raylib.SetTargetFPS(60);
        Raylib.SetMousePosition(screenWidth / 2, screenHeight / 2);

        while (!Raylib.WindowShouldClose())
        {
            if (Raylib.IsWindowResized())
            {
                screenWidth = Raylib.GetScreenWidth();
                screenHeight = Raylib.GetScreenHeight();
                resolution = new Vector2(screenWidth, screenHeight);

                Raylib.SetShaderValue(shader, resolutionLoc, resolution, ShaderUniformDataType.Vec2);
            }

            Raylib.UpdateCamera(ref camera, CameraMode.Free);

            runTime += Raylib.GetFrameTime();

            Raylib.SetShaderValue(shader, viewEyeLoc, camera.Position, ShaderUniformDataType.Vec3);
            Raylib.SetShaderValue(shader, viewCenterLoc, camera.Target, ShaderUniformDataType.Vec3);
            Raylib.SetShaderValue(shader, runTimeLoc, runTime, ShaderUniformDataType.Float);

            Raylib.BeginDrawing();
            {
                Raylib.ClearBackground(Color.RayWhite);

                Raylib.BeginShaderMode(shader);
                {
                    Raylib.DrawRectangle(0, 0, screenWidth, screenHeight, Color.White);
                }
                Raylib.EndShaderMode();

                Raylib.DrawText(
                    "(c) Raymarching shader by Iñigo Quilez. MIT License.",
                    screenWidth - 280,
                    screenHeight - 20,
                    10,
                    Color.Black
                );

            }
            Raylib.EndDrawing();
        }

        Raylib.UnloadShader(shader);
        Raylib.CloseWindow(); // Close window and OpenGL context

        return 0; // Exit the program
    }
}