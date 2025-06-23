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
    public static Light CreateLight(
        int lightsCount,
        LightType type,
        Vector3 pos,
        Vector3 target,
        Color color,
        Shader shader
    )
    {
        Light light = new()
        {
            Enabled = true,
            Type = type,
            Position = pos,
            Target = target,
            Color = color,

            EnabledLoc  = Raylib.GetShaderLocation(shader, $"lights[{lightsCount}].enabled"),
            TypeLoc     = Raylib.GetShaderLocation(shader, $"lights[{lightsCount}].type"),
            PositionLoc = Raylib.GetShaderLocation(shader, $"lights[{lightsCount}].position"),
            TargetLoc   = Raylib.GetShaderLocation(shader, $"lights[{lightsCount}].target"),
            ColorLoc    = Raylib.GetShaderLocation(shader, $"lights[{lightsCount}].color")
        };

        UpdateLightValues(shader, light);

        return light;
    }

    public static void UpdateLightValues(Shader shader, Light light)
    {
        // Send to shader light enabled state and type
        Raylib.SetShaderValue(
            shader,
            light.EnabledLoc,
            light.Enabled ? 1 : 0,
            ShaderUniformDataType.Int
        );
        Raylib.SetShaderValue(shader, light.TypeLoc, (int)light.Type, ShaderUniformDataType.Int);

        // Send to shader light target position values
        Raylib.SetShaderValue(shader, light.PositionLoc, light.Position, ShaderUniformDataType.Vec3);

        // Send to shader light target position values
        Raylib.SetShaderValue(shader, light.TargetLoc, light.Target, ShaderUniformDataType.Vec3);

        // Send to shader light color values
        var color = new[]
        {
            light.Color.R / 255.0f, // force to float because otherwise it's integer division, and always truncated to 0
            light.Color.G / 255.0f,
            light.Color.B / 255.0f,
            light.Color.A / 255.0f
        };
        Raylib.SetShaderValue(shader, light.ColorLoc, color, ShaderUniformDataType.Vec4);
    }
}
