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
            foreach (var control in Controls)
            {
                ImGuiNET.ImGui.BeginChild($"{PanelHeader}/{control.Name}", new Vector2(ImGuiNET.ImGui.GetContentRegionAvail().X, 120), ImGuiNET.ImGuiChildFlags.Borders);
                {
                    control.Show();
                }
                ImGuiNET.ImGui.EndChild();
            }
        }
    }
}
