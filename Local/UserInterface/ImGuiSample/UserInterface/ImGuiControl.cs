using System;

namespace RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample.UserInterface;

public abstract class ImGuiControl : IDisposable
{
    public abstract void Setup();
    public abstract void Shutdown();
    public abstract void Show();
    public abstract void Update();
    public abstract void Dispose();
}
