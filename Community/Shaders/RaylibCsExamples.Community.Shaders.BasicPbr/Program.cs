using System.Numerics;
using Raylib_cs;

namespace RaylibCsExamples.Community.Shaders.BasicPbr;

public class Program
{
    private const int GLSL_VERSION = 330;

    public static unsafe int Main()
    {
        const int screenWidth = 1600;
        const int screenHeight = 900;

        Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint);
        Raylib.InitWindow(screenWidth, screenHeight, "raylib [shaders] example - basic pbr");

        var camera = new Camera3D
        {
            Position = new Vector3(2.0f, 4.0f, 6.0f),
            Target = new Vector3(0.0f, 0.5f, 0.0f),
            Up = new Vector3(0.0f, 1.0f, 0.0f),
            FovY = 45.0f,
            Projection = CameraProjection.Perspective
        };

        var shader = Raylib.LoadShader(
            "Resources/pbr.vs",
            "Resources/pbr.fs"
        );

        shader.Locs[(int)ShaderLocationIndex.MapAlbedo] = Raylib.GetShaderLocation(shader, "albedoMap");
        // WARNING: Metalness, roughness, and ambient occlusion are all packed into a MRA texture
        // They are passed as to the SHADER_LOC_MAP_METALNESS location for convenience,
        // shader already takes care of it accordingly
        shader.Locs[(int)ShaderLocationIndex.MapMetalness] = Raylib.GetShaderLocation(shader, "mraMap");
        shader.Locs[(int)ShaderLocationIndex.MapNormal] = Raylib.GetShaderLocation(shader, "normalMap");
        // WARNING: Similar to the MRA map, the emissive map packs different information
        // into a single texture: it stores height and emission data
        // It is binded to SHADER_LOC_MAP_EMISSION location an properly processed on shader
        shader.Locs[(int)ShaderLocationIndex.MapEmission] = Raylib.GetShaderLocation(shader, "emissiveMap");
        shader.Locs[(int)ShaderLocationIndex.ColorDiffuse] = Raylib.GetShaderLocation(shader, "albedoColor");

        shader.Locs[(int)ShaderLocationIndex.VectorView] = Raylib.GetShaderLocation(shader, "viewPos");

        var lightCountLoc = Raylib.GetShaderLocation(shader, "numOfLights");
        var maxLightCount = 4;

        Raylib.SetShaderValue(shader, lightCountLoc, &maxLightCount, ShaderUniformDataType.Int);

        var ambientIntensity = .02f;
        var ambientColor = new Color(26, 32, 135, 255);
        var ambientColorNormalized = new Vector3(
            ambientColor.R / 255.0f,
            ambientColor.G / 255.0f,
            ambientColor.B / 255.0f
        );

        Raylib.SetShaderValue(shader, Raylib.GetShaderLocation(shader, "ambientColor"), &ambientColorNormalized, ShaderUniformDataType.Vec3);
        Raylib.SetShaderValue(shader, Raylib.GetShaderLocation(shader, "ambient"), &ambientIntensity, ShaderUniformDataType.Float);

        var car = LoadCarModel(shader);
        var floor = LoadFloorModel(shader);

        var lights = new[] {
            PbrLights.CreatePbrLight(
                lightsCount : 0,
                type        : PbrLightType.Point,
                pos         : new Vector3(-1, 1, -2),
                target      : Vector3.Zero,
                color       : Color.Yellow,
                intensity   : 4,
                shader      : shader
            ),

            PbrLights.CreatePbrLight(
                lightsCount : 1,
                type        : PbrLightType.Point,
                pos         : new Vector3(-2, 1, 1),
                target      : Vector3.Zero,
                color       : Color.Green,
                intensity   : 3.3f,
                shader      : shader
            ),

            PbrLights.CreatePbrLight(
                lightsCount : 2,
                type        : PbrLightType.Point,
                pos         : new Vector3(-2, 1, 1),
                target      : Vector3.Zero,
                color       : Color.Red,
                intensity   : 8.3f,
                shader      : shader
            ),

            PbrLights.CreatePbrLight(
                lightsCount : 3,
                type        : PbrLightType.Point,
                pos         : new Vector3(1, 1, -2),
                target      : Vector3.Zero,
                color       : Color.Black,
                intensity   : 2,
                shader      : shader
            )
        };

        // Setup material texture maps usage in shader
        // NOTE: By default, the texture maps are always used
        var usage = 1;
        Raylib.SetShaderValue(shader, Raylib.GetShaderLocation(shader, "useTexAlbedo")   , &usage, ShaderUniformDataType.Int);
        Raylib.SetShaderValue(shader, Raylib.GetShaderLocation(shader, "useTexNormal")   , &usage, ShaderUniformDataType.Int);
        Raylib.SetShaderValue(shader, Raylib.GetShaderLocation(shader, "useTexMRA")      , &usage, ShaderUniformDataType.Int);
        Raylib.SetShaderValue(shader, Raylib.GetShaderLocation(shader, "useTexEmissive") , &usage, ShaderUniformDataType.Int);
        Raylib.SetTargetFPS(60);

        var emissiveIntensityLoc = Raylib.GetShaderLocation(shader, "emissivePower");
        var emissiveColorLoc = Raylib.GetShaderLocation(shader, "emissiveColor");
        var textureTilingLoc = Raylib.GetShaderLocation(shader, "tiling");

        while (!Raylib.WindowShouldClose())
        {
            Raylib.UpdateCamera(&camera, CameraMode.Orbital);
            Raylib.SetShaderValue(shader, shader.Locs[(int)ShaderLocationIndex.VectorView], camera.Position, ShaderUniformDataType.Vec3);

            if (Raylib.IsKeyPressed(KeyboardKey.One))
            {
                lights[2].Enabled = !lights[2].Enabled;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Two))
            {
                lights[1].Enabled = !lights[1].Enabled;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Three))
            {
                lights[3].Enabled = !lights[3].Enabled;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Four))
            {
                lights[0].Enabled = !lights[0].Enabled;
            }

            foreach (var light in lights)
            {
                UpdateLight(shader, light);
            }

            Raylib.BeginDrawing();
            {
                Raylib.ClearBackground(Color.Black);
                Raylib.BeginMode3D(camera);
                {


                    var carTextureTiling = new Vector2(.5f, .5f);
                    var floorTextureTiling = new Vector2(.5f, .5f);

                    Raylib.SetShaderValue(shader, textureTilingLoc, &floorTextureTiling, ShaderUniformDataType.Vec2);
                    var floorEmissiveColor = Raylib.ColorNormalize(floor.Materials[0].Maps[(int)MaterialMapIndex.Emission].Color);
                    Raylib.SetShaderValue(shader, emissiveColorLoc, &floorEmissiveColor, ShaderUniformDataType.Vec4);

                    Raylib.DrawModel(floor, Vector3.Zero, 5, Color.White);

                    Raylib.SetShaderValue(shader, textureTilingLoc, &carTextureTiling, ShaderUniformDataType.Vec2);
                    var carEmissiveColor = Raylib.ColorNormalize(car.Materials[0].Maps[(int)MaterialMapIndex.Emission].Color);
                    Raylib.SetShaderValue(shader, emissiveColorLoc, &carEmissiveColor, ShaderUniformDataType.Vec4);

                    var emissiveIntensity = .01f;
                    Raylib.SetShaderValue(shader, emissiveColorLoc, &emissiveIntensity, ShaderUniformDataType.Float);

                    Raylib.DrawModel(car, Vector3.Zero, .005f, Color.White);

                    foreach (var light in lights)
                    {
                        var color = light.Color;
                        var lightColor = new Color(
                            (byte)(color.X * 255),
                            (byte)(color.Y * 255),
                            (byte)(color.Z * 255),
                            (byte)(color.W * 255)
                        );

                        if (light.Enabled)
                        {
                            Raylib.DrawSphereEx(light.Position, .2f, 8, 8, lightColor);
                        }
                        else
                        {
                            Raylib.DrawSphereWires(light.Position, .2f, 8, 8, Raylib.ColorAlpha(lightColor, .3f));
                        }
                    }
                }
                Raylib.EndMode3D();

                Raylib.DrawText("ToggleLights: [1][2][3][4]", 10, 40, 20, Color.LightGray);
                Raylib.DrawText("(c) Old Rusty Car model by Renafox (https://skfb.ly/LxRy)", screenWidth - 320, screenHeight - 20, 10, Color.LightGray);
                Raylib.DrawFPS(10, 10);
            }
            Raylib.EndDrawing();
        }

        car.Materials[0].Shader = new();
        Raylib.UnloadMaterial(car.Materials[0]);
        car.Materials[0].Maps = null;
        Raylib.UnloadModel(car);

        floor.Materials[0].Shader = new();
        Raylib.UnloadMaterial(floor.Materials[0]);
        floor.Materials[0].Maps = null;
        Raylib.UnloadModel(floor);

        Raylib.UnloadShader(shader);

        Raylib.CloseWindow();

        return 0;
    }

    private static void UpdateLight(Shader shader, PbrLight light)
    {
        Raylib.SetShaderValue(shader, light.EnabledLoc, light.Enabled, ShaderUniformDataType.Int);
        Raylib.SetShaderValue(shader, light.TypeLoc, light.Type, ShaderUniformDataType.Int);
        Raylib.SetShaderValue(shader, light.PositionLoc, light.Position, ShaderUniformDataType.Vec3);
        Raylib.SetShaderValue(shader, light.ColorLoc, light.Color, ShaderUniformDataType.Vec4);
        Raylib.SetShaderValue(shader, light.IntensityLoc, light.Intensity, ShaderUniformDataType.Float);
    }

    private static unsafe Model LoadFloorModel(Shader shader)
    {
        var floor = Raylib.LoadModel("Resources/plane.glb");

        floor.Materials[0].Shader = shader;
        floor.Materials[0].Maps[(int)MaterialMapIndex.Albedo].Color = Color.White;
        floor.Materials[0].Maps[(int)MaterialMapIndex.Metalness].Value = 0;
        floor.Materials[0].Maps[(int)MaterialMapIndex.Roughness].Value = 0;
        floor.Materials[0].Maps[(int)MaterialMapIndex.Occlusion].Value = 1;
        floor.Materials[0].Maps[(int)MaterialMapIndex.Emission].Color = Color.Black;

        floor.Materials[0].Maps[(int)MaterialMapIndex.Albedo].Texture = Raylib.LoadTexture("Resources/road_a.png");
        floor.Materials[0].Maps[(int)MaterialMapIndex.Metalness].Texture = Raylib.LoadTexture("Resources/road_mra.png");
        floor.Materials[0].Maps[(int)MaterialMapIndex.Normal].Texture = Raylib.LoadTexture("Resources/road_n.png");

        return floor;
    }

    private static unsafe Model LoadCarModel(Shader shader)
    {
        var car = Raylib.LoadModel("Resources/old_car_new.glb");

        car.Materials[0].Shader = shader;

        car.Materials[0].Maps[(int)MaterialMapIndex.Albedo].Color = Color.White;
        car.Materials[0].Maps[(int)MaterialMapIndex.Metalness].Value = 0;
        car.Materials[0].Maps[(int)MaterialMapIndex.Roughness].Value = 0;
        car.Materials[0].Maps[(int)MaterialMapIndex.Occlusion].Value = 1;
        car.Materials[0].Maps[(int)MaterialMapIndex.Emission].Color = new(255, 162, 0, 255);

        car.Materials[0].Maps[(int)MaterialMapIndex.Albedo].Texture = Raylib.LoadTexture("Resources/old_car_d.png");
        car.Materials[0].Maps[(int)MaterialMapIndex.Metalness].Texture = Raylib.LoadTexture("Resources/old_car_mra.png");
        car.Materials[0].Maps[(int)MaterialMapIndex.Normal].Texture = Raylib.LoadTexture("Resources/old_car_n.png");
        car.Materials[0].Maps[(int)MaterialMapIndex.Emission].Texture = Raylib.LoadTexture("Resources/old_car_e.png");

        return car;
    }
}