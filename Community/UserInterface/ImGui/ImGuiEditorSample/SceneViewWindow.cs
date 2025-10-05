using System;

using rlImGui_cs;
using ImGuiNET;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

namespace RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample;

public class SceneViewWindow : DocumentWindow
{
    private Camera3D _camera = new();
    private Texture2D _gridTexture;

    public override void Setup()
    {
        ViewTexture = Raylib.LoadRenderTexture(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

        _camera.FovY = 45.0f;
        _camera.Up.Y = 1.0f;
        _camera.Position.Y = 3;
        _camera.Position.Z = -25;

        var img = Raylib.GenImageChecked(256, 256, 32, 32, Color.DarkGray, Color.White);
        _gridTexture = Raylib.LoadTextureFromImage(img);
        Raylib.UnloadImage(img);
        Raylib.GenTextureMipmaps(ref _gridTexture);
        Raylib.SetTextureFilter(_gridTexture, TextureFilter.Anisotropic16X);
        Raylib.SetTextureWrap(_gridTexture, TextureWrap.Clamp);
    }

    public override void Shutdown()
    {
        Raylib.UnloadRenderTexture(ViewTexture);
        Raylib.UnloadTexture(_gridTexture);
    }

    public override void Show()
    {
        ImGuiNET.ImGui.PushStyleVar(ImGuiNET.ImGuiStyleVar.WindowPadding, Vector2.Zero);
        {
            ImGuiNET.ImGui.SetNextWindowSizeConstraints(new Vector2(400, 400), new Vector2((float)Raylib.GetScreenWidth(), (float)Raylib.GetScreenHeight()));

            if (ImGuiNET.ImGui.Begin("3D View", ref _open, ImGuiNET.ImGuiWindowFlags.NoScrollbar))
            {
                Focused = ImGuiNET.ImGui.IsWindowFocused(ImGuiNET.ImGuiFocusedFlags.ChildWindows);
                rlImGui.ImageRenderTextureFit(ViewTexture, true);

                ImGuiNET.ImGui.End();
            }
        }
        ImGuiNET.ImGui.PopStyleVar();
    }

    public override void Update()
    {
        if (!_open)
        {
            return;
        }

        if (Raylib.IsWindowResized())
        {
            Raylib.UnloadRenderTexture(ViewTexture);
            ViewTexture = Raylib.LoadRenderTexture(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        }

        var period = 10.0f;
        var magnitude = 25.0f;

        _camera.Position.X = (float)(Math.Sin(Raylib.GetTime() / period) * magnitude);

        Raylib.BeginTextureMode(ViewTexture);
        {
            Raylib.ClearBackground(Color.SkyBlue);

            Raylib.BeginMode3D(_camera);
            {

                Raylib.DrawPlane(new Vector3(0, 0, 0), new Vector2(50, 50), Color.Beige);

                var spacing = 4;
                var count = 5;

                for (var x = -count * spacing; x <= count * spacing; x += spacing)
                {
                    for (var z = -count * spacing; z <= count * spacing; z += spacing)
                    {
                        DrawCubeTexture(_gridTexture, new Vector3(x, 1.5f, z), 1, 1, 1, Color.Green);
                        DrawCubeTexture(_gridTexture, new Vector3(x, 0.5f, z), .25f, 1, .25f, Color.Brown);
                    }
                }
            }
            Raylib.EndMode3D();
        }
        Raylib.EndTextureMode();
    }

    private static void DrawCubeTexture(
        Texture2D texture,
        Vector3 position,
        float width,
        float height,
        float length,
        Color color
    )
    {
        Rlgl.SetTexture(texture.Id);

        // Vertex data transformation can be defined with the commented lines,
        // but in this example we calculate the transformed vertex data directly when calling Rlgl.rlVertex3f()
        // Rlgl.rlPushMatrix();
        // NOTE: Transformation is applied in inverse order (scale -> rotate -> translate)
        // Rlgl.rlTranslatef(2.0f, 0.0f, 0.0f);
        // Rlgl.rlRotatef(45, 0, 1, 0);
        // Rlgl.rlScalef(2.0f, 2.0f, 2.0f);

        Rlgl.Begin(DrawMode.Quads);
        {
            Rlgl.Color4ub(color.R, color.G, color.B, color.A);

            // Front Face
            // Normal Pointing Towards Viewer
            Rlgl.Normal3f(0.0f, 0.0f, 1.0f);
            Rlgl.TexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.Vertex3f(position.X - width / 2, position.Y - height / 2, position.Z + length / 2);
            Rlgl.TexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.Vertex3f(position.X + width / 2, position.Y - height / 2, position.Z + length / 2);
            Rlgl.TexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.Vertex3f(position.X + width / 2, position.Y + height / 2, position.Z + length / 2);
            Rlgl.TexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.Vertex3f(position.X - width / 2, position.Y + height / 2, position.Z + length / 2);

            // Back Face
            // Normal Pointing Away From Viewer
            Rlgl.Normal3f(0.0f, 0.0f, -1.0f);
            Rlgl.TexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.Vertex3f(position.X - width / 2, position.Y - height / 2, position.Z - length / 2);
            Rlgl.TexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.Vertex3f(position.X - width / 2, position.Y + height / 2, position.Z - length / 2);
            Rlgl.TexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.Vertex3f(position.X + width / 2, position.Y + height / 2, position.Z - length / 2);
            Rlgl.TexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.Vertex3f(position.X + width / 2, position.Y - height / 2, position.Z - length / 2);

            // Top Face
            // Normal Pointing Up
            Rlgl.Normal3f(0.0f, 1.0f, 0.0f);
            Rlgl.TexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.Vertex3f(position.X - width / 2, position.Y + height / 2, position.Z - length / 2);
            Rlgl.TexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.Vertex3f(position.X - width / 2, position.Y + height / 2, position.Z + length / 2);
            Rlgl.TexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.Vertex3f(position.X + width / 2, position.Y + height / 2, position.Z + length / 2);
            Rlgl.TexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.Vertex3f(position.X + width / 2, position.Y + height / 2, position.Z - length / 2);

            // Bottom Face
            // Normal Pointing Down
            Rlgl.Normal3f(0.0f, -1.0f, 0.0f);
            Rlgl.TexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.Vertex3f(position.X - width / 2, position.Y - height / 2, position.Z - length / 2);
            Rlgl.TexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.Vertex3f(position.X + width / 2, position.Y - height / 2, position.Z - length / 2);
            Rlgl.TexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.Vertex3f(position.X + width / 2, position.Y - height / 2, position.Z + length / 2);
            Rlgl.TexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.Vertex3f(position.X - width / 2, position.Y - height / 2, position.Z + length / 2);

            // Right face
            // Normal Pointing Right
            Rlgl.Normal3f(1.0f, 0.0f, 0.0f);
            Rlgl.TexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.Vertex3f(position.X + width / 2, position.Y - height / 2, position.Z - length / 2);
            Rlgl.TexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.Vertex3f(position.X + width / 2, position.Y + height / 2, position.Z - length / 2);
            Rlgl.TexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.Vertex3f(position.X + width / 2, position.Y + height / 2, position.Z + length / 2);
            Rlgl.TexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.Vertex3f(position.X + width / 2, position.Y - height / 2, position.Z + length / 2);

            // Left Face
            // Normal Pointing Left
            Rlgl.Normal3f(-1.0f, 0.0f, 0.0f);
            Rlgl.TexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.Vertex3f(position.X - width / 2, position.Y - height / 2, position.Z - length / 2);
            Rlgl.TexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.Vertex3f(position.X - width / 2, position.Y - height / 2, position.Z + length / 2);
            Rlgl.TexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.Vertex3f(position.X - width / 2, position.Y + height / 2, position.Z + length / 2);
            Rlgl.TexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.Vertex3f(position.X - width / 2, position.Y + height / 2, position.Z - length / 2);
        }
        Rlgl.End();
    }
}
