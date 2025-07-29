using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Raylib_cs;

namespace RaylibCsExamples.Community.Shaders.BasicPbr;

public struct PbrLight
{
    public required PbrLightType Type { get; set; }
    public required bool Enabled { get; set; }
    public required Vector3 Position { get; set; }
    public required Vector3 Target { get; set; }
    public required Vector4 Color { get; set; }
    public required float Intensity { get; set; }

    // Shader light parameters locations
    public required int TypeLoc { get; set; }
    public required int EnabledLoc { get; set; }
    public required int PositionLoc { get; set; }
    public required int TargetLoc { get; set; }
    public required int ColorLoc { get; set; }
    public required int IntensityLoc { get; set; }
}

public enum PbrLightType
{
    Directional,
    Point,
    Spot,
}

public class PbrLights
{
    public static PbrLight CreatePbrLight(
        int lightsCount,
        PbrLightType type,
        Vector3 pos,
        Vector3 target,
        Color color,
        float intensity,
        Shader shader
    )
    {
        var light = new PbrLight
        {
            Enabled = true,
            Type = type,
            Position = pos,
            Target = target,
            Color = new Vector4(
                color.R / 255.0f,
                color.G / 255.0f,
                color.B / 255.0f,
                color.A / 255.0f
            ),
            Intensity = intensity,

            EnabledLoc = Raylib.GetShaderLocation(shader, $"lights[{lightsCount}].enabled"),
            TypeLoc = Raylib.GetShaderLocation(shader, $"lights[{lightsCount}].type"),
            PositionLoc = Raylib.GetShaderLocation(shader, $"lights[{lightsCount}].position"),
            TargetLoc = Raylib.GetShaderLocation(shader, $"lights[{lightsCount}].target"),
            ColorLoc = Raylib.GetShaderLocation(shader, $"lights[{lightsCount}].color"),
            IntensityLoc = Raylib.GetShaderLocation(shader, $"lights[{lightsCount}].intensity")
        };

        UpdateShaderValues(shader, light);

        return light;
    }

    public static void UpdateShaderValues(Shader shader, PbrLight light)
    {
        Raylib.SetShaderValue(shader, light.EnabledLoc  , light.Enabled ? 1 : 0 , ShaderUniformDataType.Int);
        Raylib.SetShaderValue(shader, light.TypeLoc     , light.Type            , ShaderUniformDataType.Int);
        Raylib.SetShaderValue(shader, light.TargetLoc   , light.Target          , ShaderUniformDataType.Vec3);
        Raylib.SetShaderValue(shader, light.PositionLoc , light.Position        , ShaderUniformDataType.Vec3);
        Raylib.SetShaderValue(shader, light.ColorLoc    , light.Color           , ShaderUniformDataType.Vec4);
        Raylib.SetShaderValue(shader, light.IntensityLoc, light.Intensity       , ShaderUniformDataType.Float);
    }
}
