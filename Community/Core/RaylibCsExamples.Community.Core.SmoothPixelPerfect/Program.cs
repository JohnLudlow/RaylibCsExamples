// using System.Numerics;
// using Raylib_cs;

// const int screenWidth = 800;
// const int screenHeight = 450;

// const int virtualScreenWidth = 160;
// const int virtualScreenHeight = 90;

// const float virtualRatio = screenWidth / virtualScreenWidth;

// Raylib.InitWindow(screenWidth, screenHeight, "raylib [core] example - smooth pixel-perfect camera");

// var worldSpaceCamera = new Camera2D { Zoom = 1 };
// var screenSpaceCamera = new Camera2D { Zoom = 1 };

// var targetTexture = Raylib.LoadRenderTexture(virtualScreenWidth, virtualScreenHeight);

// var rect1 = new Rectangle(70, 35, 20, 20);
// var rect2 = new Rectangle(90, 55, 30, 10);
// var rect3 = new Rectangle(80, 65, 15, 25);

// var sourceRect = new Rectangle(
//     x: 0,
//     y: 0,
//     width: targetTexture.Texture.Width,
//     height: targetTexture.Texture.Height);

// var destRect = new Rectangle(
//     x: -virtualRatio,
//     y: -virtualRatio,
//     width: screenWidth + (virtualRatio * 2),
//     height: screenWidth + (virtualRatio * 2)
// );

// var origin = Vector2.Zero;
// var rotation = 0f;
// var cameraX = 0f;
// var cameraY = 0f;

// Raylib.SetTargetFPS(60);
// while (!Raylib.WindowShouldClose())
// {
//     rotation += 60f * Raylib.GetFrameTime();

//     cameraX = MathF.Sin((float)Raylib.GetTime() * 50) - 10f;
//     cameraY = MathF.Cos((float)Raylib.GetTime() * 30);

//     screenSpaceCamera.Target = new Vector2(cameraX, cameraY);

//     worldSpaceCamera.Target.X = (int)screenSpaceCamera.Target.X;
//     screenSpaceCamera.Target.X -= worldSpaceCamera.Target.X;
//     screenSpaceCamera.Target.X *= virtualRatio;

//     worldSpaceCamera.Target.Y = (int)screenSpaceCamera.Target.Y;
//     screenSpaceCamera.Target.Y -= worldSpaceCamera.Target.Y;
//     screenSpaceCamera.Target.Y *= virtualRatio;

//     Raylib.BeginTextureMode(targetTexture);
//     {
//         Raylib.ClearBackground(Color.RayWhite);

//         Raylib.BeginMode2D(worldSpaceCamera);
//         {
//             Raylib.DrawRectanglePro(rect1, origin, rotation, Color.Black);
//             Raylib.DrawRectanglePro(rect2, origin, -rotation, Color.Red);
//             Raylib.DrawRectanglePro(rect3, origin, rotation + 45f, Color.Blue);
//         }
//         Raylib.EndMode2D();
//     }
//     Raylib.EndTextureMode();

//     Raylib.BeginDrawing();
//     {
//         Raylib.ClearBackground(Color.RayWhite);

//         Raylib.BeginMode2D(screenSpaceCamera);
//         {
//             Raylib.DrawTexturePro(targetTexture.Texture, sourceRect, destRect, origin, 0, Color.White);
//         }
//         Raylib.EndMode2D();

//         Raylib.DrawText($"Screen resolution: {screenWidth}x{screenHeight}", 10, 10, 20, Color.DarkBlue);
//         Raylib.DrawText($"World resolution: {virtualScreenWidth}x{virtualScreenHeight}", 10, 40, 20, Color.DarkGreen);
//         Raylib.DrawFPS(Raylib.GetScreenWidth() - 95, 10);
//     }
//     Raylib.EndDrawing();
// }

// Raylib.UnloadRenderTexture(targetTexture);
// Raylib.CloseWindow();

/*******************************************************************************************
*
*   raylib [core] example - smooth pixel-perfect camera
*
*   This example has been created using raylib 3.7 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Giancamillo Alessandroni (@NotManyIdeasDev) and
*   reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2021 Giancamillo Alessandroni (@NotManyIdeasDev) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using Raylib_cs;

namespace RaylibCsExamples.Community.Core.SmoothPixelPerfect;

public static class SmoothPixelPerfect
{
    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        const int virtualScreenWidth = 160;
        const int virtualScreenHeight = 90;

        const float virtualRatio = screenWidth / (float)virtualScreenWidth;

        Raylib.InitWindow(screenWidth, screenHeight, "raylib [core] example - smooth pixel-perfect camera");

        // Game world camera
        var worldSpaceCamera = new Camera2D
        {
            Zoom = 1.0f
        };

        // Smoothing camera
        var screenSpaceCamera = new Camera2D()
        {
            Zoom = 1.0f
        };

        // This is where we'll draw all our objects.
        var target = Raylib.LoadRenderTexture(virtualScreenWidth, virtualScreenHeight);

        var rec01 = new Rectangle(70.0f, 35.0f, 20.0f, 20.0f);
        var rec02 = new Rectangle(90.0f, 55.0f, 30.0f, 10.0f);
        var rec03 = new Rectangle(80.0f, 65.0f, 15.0f, 25.0f);

        // The target's height is flipped (in the source Rectangle), due to OpenGL reasons
        var sourceRec = new Rectangle(
            0.0f,
            0.0f,
            target.Texture.Width,
            -(float)target.Texture.Height
        );

        var destRec = new Rectangle(
            -virtualRatio,
            -virtualRatio,
            screenWidth + virtualRatio * 2,
            screenHeight + virtualRatio * 2
        );

        var origin = new Vector2(0.0f, 0.0f);

        var rotation = 0.0f;

        var cameraX = 0.0f;
        var cameraY = 0.0f;

        Raylib.SetTargetFPS(60);
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!Raylib.WindowShouldClose())
        {
            // Update
            //----------------------------------------------------------------------------------
            rotation += 60.0f * Raylib.GetFrameTime();   // Rotate the rectangles, 60 degrees per second

            // Make the camera move to demonstrate the effect
            cameraX = MathF.Sin((float)Raylib.GetTime()) * 50.0f - 10.0f;
            cameraY = MathF.Cos((float)Raylib.GetTime()) * 30.0f;

            // Set the camera's target to the values computed above
            screenSpaceCamera.Target = new Vector2(cameraX, cameraY);

            // Round worldSpace coordinates, keep decimals into screenSpace coordinates
            worldSpaceCamera.Target.X = (int)screenSpaceCamera.Target.X;
            screenSpaceCamera.Target.X -= worldSpaceCamera.Target.X;
            screenSpaceCamera.Target.X *= virtualRatio;

            worldSpaceCamera.Target.Y = (int)screenSpaceCamera.Target.Y;
            screenSpaceCamera.Target.Y -= worldSpaceCamera.Target.Y;
            screenSpaceCamera.Target.Y *= virtualRatio;
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            Raylib.BeginTextureMode(target);
            {
                Raylib.ClearBackground(Color.RayWhite);

                Raylib.BeginMode2D(worldSpaceCamera);
                {
                    Raylib.DrawRectanglePro(rec01, origin, rotation, Color.Black);
                    Raylib.DrawRectanglePro(rec02, origin, -rotation, Color.Red);
                    Raylib.DrawRectanglePro(rec03, origin, rotation + 45.0f, Color.Blue);
                }
                Raylib.EndMode2D();
            }
            Raylib.EndTextureMode();

            Raylib.BeginDrawing();
            {
                Raylib.ClearBackground(Color.Red);

                Raylib.BeginMode2D(screenSpaceCamera);
                {
                    Raylib.DrawTexturePro(target.Texture, sourceRec, destRec, origin, 0, Color.White);
                }
                Raylib.EndMode2D();

                Raylib.DrawText($"Screen resolution: {screenWidth}x{screenHeight}", 10, 10, 20, Color.DarkBlue);
                Raylib.DrawText($"World resolution: {virtualScreenWidth}x{virtualScreenHeight}", 10, 40, 20, Color.DarkGreen);
                Raylib.DrawFPS(Raylib.GetScreenWidth() - 95, 10);
            }
            Raylib.EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        Raylib.UnloadRenderTexture(target);
        Raylib.CloseWindow();
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
