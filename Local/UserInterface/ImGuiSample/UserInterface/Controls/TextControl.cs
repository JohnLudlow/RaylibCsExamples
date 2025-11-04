namespace RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample.UserInterface.Controls;

public class TextControl : ImGuiControl
{
    public string? Text { get; set; } = "Default Text";

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public override void Setup()
    {
    }

    public override void Show()
    {
        ImGuiNET.ImGui.Text(Text);
    }

    public override void Shutdown()
    {
    }

    public override void Update()
    {
    }
}
