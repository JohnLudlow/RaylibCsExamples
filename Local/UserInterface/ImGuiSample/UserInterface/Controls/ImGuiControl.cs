namespace RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample.UserInterface.Controls;

public abstract class ImGuiControl : IDisposable
{
    public string Name { get; init; } = Guid.NewGuid().ToString();
    public abstract void Setup();
    public abstract void Shutdown();
    public abstract void Show();
    public abstract void Update();
    public abstract void Dispose();
}
