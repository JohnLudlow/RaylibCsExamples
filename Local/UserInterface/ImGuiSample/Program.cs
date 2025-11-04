using ImGuiNET;
using Raylib_cs;
using RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample.ECS;
using RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample.UserInterface.Controls;
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

var rootScene = new Scene() { RootEntities = {
    new Entity()
    {
        Name = "Entity 1",
        Components =
        {
            new MeshComponent()
            {
                Mesh = Raylib.GenMeshCube(1.0f, 1.0f, 1.0f)
            }
        },
    },
} };

while (!Raylib.WindowShouldClose())
{
    Setup(rootScene);
    Update(rootScene);
    Draw();
}

static void Setup(Scene scene)
{
    scene.Setup();
}

static void Update(Scene scene)
{
    scene.Update();
}

static void Draw()
{
    Raylib.BeginDrawing();
    {
        Raylib.ClearBackground(Color.RayWhite);

        DrawUserInterface();

        Raylib.DrawFPS(10, 10);
    }
    Raylib.EndDrawing();
}

static void DrawUserInterface()
{
    rlImGui.Begin();
    {
        var testWindow = new TestWindow();
        testWindow.Show();
    }
    rlImGui.End();
}