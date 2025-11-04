namespace RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample.ECS;

public abstract class Component : IDisposable
{
    public string Name { get; init; } = Guid.NewGuid().ToString();
    public abstract void Setup();
    public abstract void Shutdown();
    public abstract void Show();
    public abstract void Update();

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
