using Raylib_cs;

namespace RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample.UserInterface.Controls;

public abstract class BaseWindow(string title) : ImGuiContainerControl
{
    public RenderTexture2D ViewTexture { get; } = Raylib.LoadRenderTexture(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
    public string Title { get; set; } = title;
    public bool IsOpen { get; set; } = true;
    public bool IsFocused { get; protected set; } = false;

    public override void Show()
    {
        if (!IsOpen) return;

        var isOpen = IsOpen;

        ImGuiNET.ImGui.Begin(Title, ref isOpen);
        base.Show();
        ImGuiNET.ImGui.End();

        IsOpen = isOpen;        
    }

    public override void Dispose()
    {
        base.Dispose();
        GC.SuppressFinalize(this);
        Raylib.UnloadRenderTexture(ViewTexture);
    }
}
