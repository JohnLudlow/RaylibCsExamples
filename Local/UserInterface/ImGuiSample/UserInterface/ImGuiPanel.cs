using System;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample.UserInterface;

public class ImGuiPanel : ImGuiContainerControl
{
    public string PanelHeader { get; set; }
    public bool IsOpen { get; set; } = true;

    public ImGuiPanel(string header)
    {
        PanelHeader = header;
    }

    public override void Show()
    {
        if (ImGuiNET.ImGui.CollapsingHeader(PanelHeader, ImGuiNET.ImGuiTreeNodeFlags.DefaultOpen))
        {
            ImGuiNET.ImGui.BeginChild(PanelHeader + "Child", new Vector2(ImGuiNET.ImGui.GetContentRegionAvail().X, 120), ImGuiNET.ImGuiChildFlags.Borders);
            {
                ImGuiNET.ImGui.Text($"This is the {PanelHeader} panel.");
            }
            ImGuiNET.ImGui.EndChild();
        }
    }

    public override void Update()
    {
    }
}
