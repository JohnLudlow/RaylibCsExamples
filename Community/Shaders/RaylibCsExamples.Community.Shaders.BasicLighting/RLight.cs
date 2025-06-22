using System.Numerics;
using Raylib_cs;

namespace RaylibCsExamples.Community.Shaders.BasicLighting;

public record struct Light(
    bool Enabled,
    LightType Type,
    Vector3 Position,
    Vector3 Target,
    Color Color,

    int EnabledLoc,
    int TypeLoc,
    int PositionLoc,
    int TargetLoc,
    int ColorLoc
);

public enum LightType
{
    Directional,
    Point,
}

public static class RLights
{
    public static Light CreateLight(int lightsCount, LightType type, Vector3 position, Vector3 target, Color color, Shader shader)
    {
        var light = new Light
        {
            Enabled = true,
            Type = type,
            Position = position,
            Target = target,
            Color = color,

            EnabledLoc = Raylib.GetShaderLocation(shader, $"lights[{lightsCount}].enabled"),
            TypeLoc = Raylib.GetShaderLocation(shader, $"lights[{lightsCount}].type"),
            PositionLoc = Raylib.GetShaderLocation(shader, $"lights[{lightsCount}].position"),
            TargetLoc = Raylib.GetShaderLocation(shader, $"lights[{lightsCount}].target"),
            ColorLoc = Raylib.GetShaderLocation(shader, $"lights[{lightsCount}].color"),
        };

        UpdateLightValues(shader, light);

        return light;
    }

    public static void UpdateLightValues(Shader shader, Light light)
    {
        Raylib.SetShaderValue(shader, light.EnabledLoc  , light.Enabled ? 1 : 0 , ShaderUniformDataType.Int);
        Raylib.SetShaderValue(shader, light.TypeLoc     , light.Type            , ShaderUniformDataType.Int);
        Raylib.SetShaderValue(shader, light.PositionLoc , light.Position        , ShaderUniformDataType.Vec3);
        Raylib.SetShaderValue(shader, light.TargetLoc   , light.Target          , ShaderUniformDataType.Vec3);
        Raylib.SetShaderValue(shader, light.ColorLoc    ,
            new float[] {
                light.Color.R / 255,
                light.Color.G / 255,
                light.Color.B / 255,
                light.Color.A / 255
            },

            ShaderUniformDataType.Vec4
        );
    }
}