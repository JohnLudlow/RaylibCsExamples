using System.Numerics;
using Raylib_cs;

const int screenWidth = 800;
const int screenHeight = 450;

Raylib.InitWindow(screenWidth, screenHeight, "raylib [core] example - 3d picking");

var camera = new Camera3D
{
    Position = new Vector3(10, 10, 10),
    Target = Vector3.Zero,
    Up = Vector3.UnitY,
    FovY = 45,
    Projection = CameraProjection.Perspective
};

var cubePosition = Vector3.UnitY;
var cubeSize = new Vector3(2, 2, 2);

var ray = new Ray(Vector3.Zero, Vector3.Zero);
var rayCollision = new RayCollision();

Raylib.SetTargetFPS(60);
Raylib.EnableCursor();

while (!Raylib.WindowShouldClose())
{
    Raylib.UpdateCamera(ref camera, CameraMode.Custom);

    if (Raylib.IsMouseButtonPressed(MouseButton.Left))
    {
        if (!rayCollision.Hit)
        {
            ray = Raylib.GetScreenToWorldRay(Raylib.GetMousePosition(), camera);
            var boundingBox = new BoundingBox(
                cubePosition - cubeSize / 2,
                cubePosition + cubeSize / 2
            );

            rayCollision = Raylib.GetRayCollisionBox(ray, boundingBox);
        }
        else
        {
            rayCollision.Hit = false;
        }
    }

    // ray = Raylib.GetScreenToWorldRay(Raylib.GetMousePosition(), camera);

    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.RayWhite);
    Raylib.BeginMode3D(camera);

    if (rayCollision.Hit)
    {
        Raylib.DrawCube(cubePosition, cubeSize.X, cubeSize.Y, cubeSize.Z, Color.Red);
        Raylib.DrawCubeWires(cubePosition, cubeSize.X, cubeSize.Y, cubeSize.Z, Color.Maroon);
        Raylib.DrawCubeWires(cubePosition, cubeSize.X + .2f, cubeSize.Y + .2f, cubeSize.Z + .2f, Color.DarkGray);
    }
    else
    {
        Raylib.DrawCube(cubePosition, cubeSize.X, cubeSize.Y, cubeSize.Z, Color.Gray);
        Raylib.DrawCubeWires(cubePosition, cubeSize.X, cubeSize.Y, cubeSize.Z, Color.DarkGray);
    }

    Raylib.DrawRay(ray, Color.Green);
    Raylib.DrawGrid(10, 1);

    Raylib.EndMode3D();

    Raylib.DrawText("Try selecting the box with mouse!", 240, 10, 20, Color.DarkGray);

    if (rayCollision.Hit)
    {
        var posX = (screenWidth - Raylib.MeasureText("BOX SELECTED", 30)) / 2;
        Raylib.DrawText("BOX SELECTED", posX, (int)(screenHeight * 0.1f), 30, Color.Green);
    }

    Raylib.DrawFPS(10, 10);

    Raylib.EndDrawing();
}

Raylib.CloseWindow();