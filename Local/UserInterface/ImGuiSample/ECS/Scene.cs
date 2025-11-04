using System.Collections.ObjectModel;

namespace RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample.ECS;

public class Scene
{
    public ObservableCollection<Entity> RootEntities { get; init; } = [];

    
    public void Setup()
    {
        foreach (var entity in RootEntities)
        {
            entity.Setup();
        }
    }

    public void Shutdown()
    {
        foreach (var entity in RootEntities)
        {
            entity.Shutdown();
        }
    }

    public void Show()
    {
        foreach (var entity in RootEntities)
        {
            entity.Show();
        }
    }
    
    public void Update()
    {
        foreach (var entity in RootEntities)
        {
            entity.Update();
        }
    }
}
