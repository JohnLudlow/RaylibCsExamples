/*******************************************************************************************
*
*   raylib [shaders] example - Apply a postprocessing shader and connect a custom uniform variable
*
*   NOTE: This example requires raylib OpenGL 3.3 or ES2 versions for shaders support,
*         OpenGL 1.1 does not support shaders, recompile raylib to OpenGL 3.3 version.
*
*   NOTE: Shaders used in this example are #version 330 (OpenGL 3.3), to test this example
*         on OpenGL ES 2.0 platforms (Android, Raspberry Pi, HTML5), use #version 100 shaders
*         raylib comes with shaders ready for both versions, check raylib/shaders install folder
*
*   This example has been created using raylib 1.3 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2015 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Diagnostics;
using System.Numerics;
using Raylib_cs;

namespace RaylibCsExamples.Community.Shaders.CustomUniform;

public class CustomUniform
{
    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 1600;
        const int screenHeight = 900;

        // Enable Multi Sampling Anti Aliasing 4x (if available)
        Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint);
        Raylib.InitWindow(screenWidth, screenHeight, "raylib [shaders] example - custom uniform variable");
        {
            // Define the camera to look into our 3d world
            Camera3D camera = new()
            {
                Position = new Vector3(8.0f, 8.0f, 8.0f),
                Target = new Vector3(0.0f, 1.5f, 0.0f),
                Up = new Vector3(0.0f, 1.0f, 0.0f),
                FovY = 45.0f,
                Projection = CameraProjection.Perspective
            };

            var model = Raylib.LoadModel("resources/models/obj/barracks.obj");
            var texture = Raylib.LoadTexture("resources/models/obj/barracks_diffuse.png");

            // Set model diffuse texture
            Raylib.SetMaterialTexture(ref model, 0, MaterialMapIndex.Albedo, ref texture);

            Vector3 position = new(0.0f, 0.0f, 0.0f);

            // Load postpro shader
            var shader = Raylib.LoadShader(
                "resources/shaders/glsl330/base.vs",
                "resources/shaders/glsl330/swirl.fs"
            );

            // Get variable (uniform) location on the shader to connect with the program
            // NOTE: If uniform variable could not be found in the shader, function returns -1
            var swirlCenterLoc = Raylib.GetShaderLocation(shader, "center");
            var screenSizeLoc = Raylib.GetShaderLocation(shader, "texSize");

            var swirlCenter = new Vector2((float)screenWidth / 2, (float)screenHeight / 2);

            // Create a RenderTexture2D to be used for render to texture
            var target = Raylib.LoadRenderTexture(screenWidth, screenHeight);

            Raylib.SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!Raylib.WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                var mousePosition = Raylib.GetMousePosition();

                swirlCenter.X = mousePosition.X;
                swirlCenter.Y = screenHeight - mousePosition.Y;

                // Send new value to the shader to be used on drawing
                Raylib.SetShaderValue(shader, swirlCenterLoc, swirlCenter, ShaderUniformDataType.Vec2);
                Raylib.SetShaderValue(shader, screenSizeLoc, new Vector2(screenWidth, screenHeight), ShaderUniformDataType.Vec2);

                Raylib.UpdateCamera(ref camera, CameraMode.Orbital);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                Raylib.BeginDrawing();
                {
                    Raylib.ClearBackground(Color.RayWhite);

                    // Enable drawing to texture
                    Raylib.BeginTextureMode(target);
                    {
                        Raylib.ClearBackground(Color.RayWhite);

                        Raylib.BeginMode3D(camera);
                        {
                            Raylib.DrawModel(model, position, 0.5f, Color.White);
                            Raylib.DrawGrid(10, 1.0f);
                        }
                        Raylib.EndMode3D();

                        Raylib.DrawText("TEXT DRAWN IN RENDER TEXTURE", 200, 10, 30, Color.Red);

                    }
                    // End drawing to texture (now we have a texture available for next passes)
                    Raylib.EndTextureMode();

                    Raylib.BeginShaderMode(shader);
                    {
                        // NOTE: Render texture must be y-flipped due to default OpenGL coordinates (left-bottom)
                        Raylib.DrawTextureRec(
                            target.Texture,
                            new Rectangle(0, 0, target.Texture.Width, -target.Texture.Height),
                            new Vector2(0, 0),
                            Color.White
                        );
                    }
                    Raylib.EndShaderMode();

                    Raylib.DrawText(
                        "(c) Barracks 3D model by Alberto Cano",
                        screenWidth - 220,
                        screenHeight - 20,
                        10,
                        Color.Gray
                    );

                    Raylib.DrawFPS(10, 10);
                }
                Raylib.EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            Raylib.UnloadShader(shader);
            Raylib.UnloadTexture(texture);
            Raylib.UnloadModel(model);
            Raylib.UnloadRenderTexture(target);
        }
        Raylib.CloseWindow();
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
