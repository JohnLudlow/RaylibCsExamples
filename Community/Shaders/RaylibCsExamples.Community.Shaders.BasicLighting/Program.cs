
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using Raylib_cs;

namespace RaylibCsExamples.Community.Shaders.BasicLighting;

public class BasicLighting
{
    private const int _glslVersion = 330;

    public unsafe static int Main()
    {
        const int screenWidth = 800;
        const int screenHeight = 450;

        Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint);
        Raylib.InitWindow(screenWidth, screenHeight, "raylib [shaders] example - basic lighting");

        var camera = new Camera3D()
        {
            Position = new Vector3(2, 4, 6),
            Target = new Vector3(0, .5f, 6),
            Up = Vector3.UnitY,
            FovY = 45,
            Projection = CameraProjection.Perspective
        };

        var planeModel = Raylib.LoadModelFromMesh(Raylib.GenMeshPlane(10, 10, 3, 3));
        var cubeModel = Raylib.LoadModelFromMesh(Raylib.GenMeshCube(2, 4, 2));

        var processDirectory = Path.GetDirectoryName(Environment.ProcessPath) ?? throw new InvalidOperationException("Unable to get process directory");

        var shader = Raylib.LoadShader(
            Path.GetFullPath("Resources/lighting.vs", processDirectory),
            Path.GetFullPath("Resources/lighting.fs", processDirectory)
        );

        shader.Locs[(int)ShaderLocationIndex.VectorView] = Raylib.GetShaderLocation(shader, "viewPos");

        var ambientLoc = Raylib.GetShaderLocation(shader, "ambient");
        var ambient = new[] { 0.1f, 0.1f, 0.1f, 0.1f };
        Raylib.SetShaderValue(shader, ambientLoc, ambient, ShaderUniformDataType.Vec4);

        planeModel.Materials[0].Shader = shader;
        cubeModel.Materials[0].Shader = shader;

        var lights = new Light[] {
            RLights.CreateLight(0, LightType.Point, new Vector3(-2, 1, -2), Vector3.Zero, Color.Yellow  , shader),
            RLights.CreateLight(1, LightType.Point, new Vector3( 2, 1,  2), Vector3.Zero, Color.Red     , shader),
            RLights.CreateLight(2, LightType.Point, new Vector3(-2, 1,  2), Vector3.Zero, Color.Green   , shader),
            RLights.CreateLight(3, LightType.Point, new Vector3( 2, 1, -2), Vector3.Zero, Color.Blue    , shader)
        };

        Raylib.SetTargetFPS(0);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.UpdateCamera(ref camera, CameraMode.FirstPerson);

            if (Raylib.IsKeyPressed(KeyboardKey.Y))
            {
                lights[0].Enabled = !lights[0].Enabled;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.R))
            {
                lights[1].Enabled = !lights[1].Enabled;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.G))
            {
                lights[2].Enabled = !lights[2].Enabled;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.B))
            {
                lights[3].Enabled = !lights[3].Enabled;
            }

            foreach (var light in lights)
            {
                RLights.UpdateLightValues(shader, light);
            }

            Raylib.SetShaderValue(shader, shader.Locs[(int)ShaderLocationIndex.VectorView], camera.Position, ShaderUniformDataType.Vec3);

            Raylib.BeginDrawing();
            {
                Raylib.ClearBackground(Color.RayWhite);
                Raylib.BeginMode3D(camera);
                {

                    Raylib.DrawModel(planeModel, Vector3.Zero, 1, Color.White);
                    Raylib.DrawModel(cubeModel, Vector3.Zero, 1, Color.White);

                    foreach (var light in lights)
                    {
                        if (light.Enabled)
                        {
                            Raylib.DrawSphereEx(light.Position, .2f, 8, 8, light.Color);
                        }
                        else
                        {
                            Raylib.DrawSphereEx(light.Position, .2f, 8, 8, Raylib.ColorAlpha(light.Color, .3f));
                        }
                    }

                    Raylib.DrawGrid(10, 1);
                }
                Raylib.EndMode3D();

                Raylib.DrawFPS(10, 10);
                Raylib.DrawText("Use keys [Y][R][G][B] to toggle lights", 10, 40, 20, Color.DarkGray);
            }
            Raylib.EndDrawing();
        }

        Raylib.UnloadModel(planeModel);
        Raylib.UnloadModel(cubeModel);
        Raylib.UnloadShader(shader);

        Raylib.CloseWindow();

        return 0;
    }
}