// See https://aka.ms/new-console-template for more information
using ImGuiNET;
using Raylib_cs;
using RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample;
using rlImGui_cs;

Console.WriteLine("Hello, World!");

var quit = false;
var imGuiDemoOpen = false;
var imageViewerWindow = new ImageViewerWindow();
var sceneViewWindow = new SceneViewWindow();
var DejaFont = default(ImFontPtr);

Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint | ConfigFlags.VSyncHint | ConfigFlags.ResizableWindow);
Raylib.InitWindow(1280, 800, "raylib-Extras-cs [ImGui] example - editor ImGui Demo");
Raylib.SetTargetFPS(144);

rlImGui.SetupUserFonts += imGuiIo =>
{
    imGuiIo.FontGlobalScale = 2.0f;
    DejaFont = imGuiIo.Fonts.AddFontFromFileTTF("Resources/Fonts/DejaVuSerif-Bold.ttf", 24, null, imGuiIo.Fonts.GetGlyphRangesDefault());
};

rlImGui.Setup(
    darkTheme: true,
    enableDocking: true
);

ImGui.GetIO().ConfigWindowsMoveFromTitleBarOnly = true;

imageViewerWindow = new ImageViewerWindow();
imageViewerWindow.Setup();
imageViewerWindow._open = true;

sceneViewWindow = new SceneViewWindow();
sceneViewWindow.Setup();
sceneViewWindow._open = true;

while (!Raylib.WindowShouldClose())
{
    imageViewerWindow.Update();
    sceneViewWindow.Update();

    Raylib.BeginDrawing();
    {
        Raylib.ClearBackground(Color.DarkGray);

        rlImGui.Begin();
        {
            DoMainMenu();

            if (imGuiDemoOpen)
            {
                ImGui.ShowDemoWindow(ref imGuiDemoOpen);
            }

            if (imageViewerWindow._open)
            {
                imageViewerWindow.Show();
            }

            if (sceneViewWindow._open)
            {
                sceneViewWindow.Show();
            }

            if (ImGui.Begin("Font Window"))
            {
                ImGui.PushFont(DejaFont);
                ImGui.TextUnformatted("This is DejaVuSans.ttf font");
                ImGui.PopFont();
            }

            ImGui.End();
        }
        rlImGui.End();
    }
    Raylib.EndDrawing();
}

rlImGui.Shutdown();
imageViewerWindow.Shutdown();
sceneViewWindow.Shutdown();
Raylib.CloseWindow();

void DoMainMenu()
{
    if (ImGui.BeginMainMenuBar())
    {
        if (ImGui.BeginMenu("File"))
        {
            if (ImGui.MenuItem("Exit"))
            {
                quit = true;
            }
            ImGui.EndMenu();
        }

        if (ImGui.BeginMenu("Window"))
        {
            ImGui.MenuItem("ImGui Demo", "", ref imGuiDemoOpen);
            ImGui.MenuItem("Image Viewer", "", ref imageViewerWindow._open);
            ImGui.MenuItem("3D Viewer", "", ref sceneViewWindow._open);

            ImGui.EndMenu();
        }

        ImGui.EndMainMenuBar();
    }

}