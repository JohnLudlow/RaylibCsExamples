using System.Numerics;
using Raylib_cs;

namespace RaylibExamples.Core.Camera3dMode;

public class Camera3dMode
{
    public static int Main()
    {
        const int screenWidth = 800;
        const int screenHeight = 450;

        Raylib.InitWindow(screenWidth, screenHeight, "raylib [core] example - 3d camera mode");

        var camera = new Camera3D
        {
            Position    = new Vector3(0f, 10f, 10f),
            Target      = Vector3.Zero,
            Up          = Vector3.UnitY,
            FovY        = 45,
            Projection  = CameraProjection.Perspective
        };

        var cubePosition = Vector3.Zero;

        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RayWhite);

            Raylib.BeginMode3D(camera);
            Raylib.DrawCube(cubePosition, 2, 2, 2, Color.Red);
            Raylib.DrawCubeWires(cubePosition, 2, 2, 2, Color.Maroon);
            Raylib.DrawGrid(10, 1);
            Raylib.EndMode3D();

            Raylib.DrawText("Welcome to the third dimension!", 10, 40, 20, Color.DarkGray);
            Raylib.DrawFPS(10, 10);
            Raylib.EndDrawing();            
        }

        Raylib.CloseWindow();

        return 0;
    }

}