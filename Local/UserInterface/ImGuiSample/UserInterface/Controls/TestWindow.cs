namespace RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample.UserInterface.Controls;

public class TestWindow : BaseWindow
{
    public TestWindow() : base("Test Window")
    {
        Controls.Add(new ImGuiPanel("Panel 1") { Controls = { new TextControl { Text = "This is some text inside Panel 1." } } });
        Controls.Add(new ImGuiPanel("Panel 2") { Controls = { new TextControl { Text = "This is some text inside Panel 2." } } });
        Controls.Add(new ImGuiPanel("Panel 3") { Controls = { new TextControl { Text = "This is some text inside Panel 3." } } });
    }
}
