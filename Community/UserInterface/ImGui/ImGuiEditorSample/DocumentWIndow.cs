using Raylib_cs;

namespace RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample;

public abstract class DocumentWindow
{
    internal bool _open = false;

    public RenderTexture2D ViewTexture { get; protected set; }

    public abstract void Setup();
    public abstract void Shutdown();
    public abstract void Show();
    public abstract void Update();

    public bool Focused { get; protected set; } = false;
}
