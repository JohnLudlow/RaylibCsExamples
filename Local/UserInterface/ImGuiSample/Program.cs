using ImGuiNET;
using Raylib_cs;
using RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample.UserInterface;
using rlImGui_cs;

var DejaFont = default(ImFontPtr);

Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint | ConfigFlags.VSyncHint | ConfigFlags.ResizableWindow);
Raylib.InitWindow(1900, 1600, "raylib-Extras-cs [ImGui] example - editor ImGui Demo");
Raylib.SetTargetFPS(144);

rlImGui.SetupUserFonts += imGuiIo =>
{
    imGuiIo.FontGlobalScale = 2.0f;
    DejaFont = imGuiIo.Fonts.AddFontFromFileTTF("Resources/Fonts/DejaVuSerif-Bold.ttf", 64, null, imGuiIo.Fonts.GetGlyphRangesDefault());
};

rlImGui.Setup(    
    darkTheme: true,
    enableDocking: true
);

var testWindow = new TestWindow();

while (!Raylib.WindowShouldClose())
{
    testWindow.Update();

    Raylib.BeginDrawing();
    {
        Raylib.ClearBackground(Color.RayWhite);

        rlImGui.Begin();
        {
            testWindow.Show();
        }
        rlImGui.End();

        Raylib.DrawFPS(10, 10);
    }
    Raylib.EndDrawing();
}