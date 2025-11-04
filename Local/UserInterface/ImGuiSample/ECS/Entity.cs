using System.Collections.ObjectModel;

namespace RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample.ECS;

public class Entity
{
    public string Name { get; set; } = new Guid().ToString();

    public void Setup()
    {
        foreach (var component in Components)
        {
            component.Setup();
        }

        foreach (var child in ChildEntities)
        {
            child.Setup();
        }
    }

    public void Shutdown()
    {
        foreach (var component in Components)
        {
            component.Shutdown();
        }

        foreach (var child in ChildEntities)
        {
            child.Shutdown();
        }
    }

    public void Show()
    {
        foreach (var component in Components)
        {
            component.Show();
        }

        foreach (var child in ChildEntities)
        {
            child.Show();
        }
    }
    
    public void Update()
    {
        foreach (var component in Components)
        {
            component.Update();
        }

        foreach (var child in ChildEntities)
        {
            child.Update();
        }
    }

    public ObservableCollection<Component> Components { get; init; } = [];
    public ObservableCollection<Entity> ChildEntities { get; init; } = [];
}
