using System;
using RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample.UserInterface;

namespace RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample;

public class ImGuiContainerControl : ImGuiControl
{
    public List<ImGuiControl> Controls { get; init; } = [];

    public override void Dispose()
    {
        foreach (var control in Controls)
        {
            control.Dispose();
        }

        GC.SuppressFinalize(this);
    }

    public override void Setup()
    {
        foreach (var control in Controls)
        {
            control.Setup();
        }
    }

    public override void Show()
    {
        foreach (var control in Controls)
        {
            control.Show();
        }
    }

    public override void Shutdown()
    {
        foreach (var control in Controls)
        {
            control.Shutdown();
        }
    }

    public override void Update()
    {
        foreach (var control in Controls)
        {
            control.Update();
        }
    }
}
