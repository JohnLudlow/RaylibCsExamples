using System;

namespace RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample.ECS;

public class Entity
{
    public string Name { get; set; } = "Entity";

    public List<Component> Components { get; init; } = [];
    public List<Entity> ChildEntities { get; init; } = [];
}
