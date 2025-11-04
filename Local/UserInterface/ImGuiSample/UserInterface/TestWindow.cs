using System;

namespace RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample.UserInterface;

public class TestWindow : BaseWindow
{
    public TestWindow() : base("Test Window")
    {
        var panel1 = new ImGuiPanel("Panel 1")
        {
            Controls =
            {
                new TextControl { Text = "This is some text in Panel 1." }
            }
        };
        var panel2 = new ImGuiPanel("Panel 2");
        var panel3 = new ImGuiPanel("Panel 3");
        Controls.Add(panel1);
        Controls.Add(panel2);
        Controls.Add(panel3);
    }
}
