using System.Diagnostics;
using System.Numerics;
using Raylib_cs;

namespace RaylibCs.Community.Core.Models.AnimationDemo;

internal sealed class Program
{
    private static unsafe void Main(string[] args)
    {
        const int screenWidth = 800;
        const int screenHeight = 450;

        Raylib.InitWindow(screenWidth, screenHeight, "raylib [models] example - model animation");

        var camera = new Camera3D
        {
            Position = new Vector3(10, 10, 10),
            Target = Vector3.Zero,
            Up = Vector3.UnitY,
            FovY = 45,
            Projection = CameraProjection.Perspective
        };

        var model = Raylib.LoadModel(Path.GetFullPath("Resources/guy.iqm"));
        var texture = Raylib.LoadTexture(Path.GetFullPath("Resources/guytex.png"));
        Raylib.SetMaterialTexture(ref model, 0, MaterialMapIndex.Albedo, ref texture);

        var position = Vector3.Zero;
        var animationCount = 0;
        var animationFrameCounter = 0;
        var animations = Raylib.LoadModelAnimations(Path.GetFullPath("Resources/guyanim.iqm"), ref animationCount);

        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.UpdateCamera(ref camera, CameraMode.Free);

            if (Raylib.IsKeyDown(KeyboardKey.Space))
            {
                animationCount++;
                Raylib.UpdateModelAnimation(model, animations[0], animationCount);
                if (animationFrameCounter > animations[0].FrameCount)
                {
                    animationFrameCounter = 0;
                }
            }

            Raylib.BeginDrawing();
            {
                Raylib.ClearBackground(Color.RayWhite);
                Raylib.BeginMode3D(camera);
                {
                    Raylib.DrawModelEx(model, position, Vector3.UnitX, -90, Vector3.One, Color.White);

                    for (var i = 0; i < model.BoneCount; i++)
                    {
                        var framePoses = animations[0].FramePoses;
                        Raylib.DrawCube(framePoses[animationFrameCounter][i].Translation, 2, 2, 2, Color.Red);
                    }

                    Raylib.DrawGrid(10, 1);
                }
                Raylib.EndMode3D();
            }
            Raylib.EndDrawing();
        }

        Raylib.UnloadTexture(texture);
        Raylib.UnloadModelAnimations(animations, animationCount);
        Raylib.UnloadModel(model);
        Raylib.CloseWindow();
    }
}