using System.Numerics;
using ImGuiNET;
using Raylib_cs;
using rlImGui_cs;

namespace RaylibCsExamples.Community.Core.UserInterface.ImGui.ImGuiEditorSample;

public class ImageViewerWindow : DocumentWindow
{
    private Texture2D _imageTexture;
    private Camera2D _camera = new();

    private Vector2 _lastMousePosition = Vector2.Zero;
    private Vector2 _lastTarget = Vector2.Zero;
    private bool _dragging = false;
    private bool _dirtyScene = false;

    private enum ToolMode
    {
        None, Move,
    }

    private ToolMode _currentToolMode = ToolMode.None;

    public override void Setup()
    {
        _camera.Zoom = 1;
        _camera.Target = Vector2.Zero;
        _camera.Rotation = 0;
        _camera.Offset.X = Raylib.GetScreenWidth() / 2.0f;
        _camera.Offset.Y = Raylib.GetScreenHeight() / 2.0f;

        ViewTexture = Raylib.LoadRenderTexture(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        _imageTexture = Raylib.LoadTexture("Resources/parrots.png");

        UpdateRenderTexture();
    }

    public override void Show()
    {
        ImGuiNET.ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0, 0));
        ImGuiNET.ImGui.SetNextWindowSizeConstraints(new Vector2(400, 400), new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()));

        if (ImGuiNET.ImGui.Begin("Image Viewer", ref _open, ImGuiWindowFlags.NoScrollbar))
        {
            Focused = ImGuiNET.ImGui.IsWindowFocused(ImGuiFocusedFlags.RootAndChildWindows);
            var windowSize = ImGuiNET.ImGui.GetContentRegionAvail();
            var viewRect = new Rectangle
            {
                X = ViewTexture.Texture.Width / 2 - windowSize.X / 2,
                Y = ViewTexture.Texture.Height / 2 - windowSize.Y / 2,
                Width = windowSize.X,
                Height = windowSize.Y
            };

            if (ImGuiNET.ImGui.BeginChild("Toolbar", new Vector2(ImGuiNET.ImGui.GetContentRegionAvail().X, 25)))
            {
                ImGuiNET.ImGui.SetCursorPosX(2);
                ImGuiNET.ImGui.SetCursorPosY(3);

                if (ImGuiNET.ImGui.Button("None"))
                {
                    _currentToolMode = ToolMode.None;
                }

                ImGuiNET.ImGui.SameLine();

                if (ImGuiNET.ImGui.Button("Move"))
                {
                    _currentToolMode = ToolMode.Move;
                }

                ImGuiNET.ImGui.SameLine();

                switch (_currentToolMode)
                {
                    case ToolMode.None:
                        ImGuiNET.ImGui.TextUnformatted("No Tool");
                        break;
                    case ToolMode.Move:
                        ImGuiNET.ImGui.TextUnformatted("Move Tool");
                        break;
                    default:
                        break;
                }

                ImGuiNET.ImGui.SameLine();
                ImGuiNET.ImGui.TextUnformatted($"camera target X{_camera.Target.X} Y{_camera.Target.Y}");
                ImGuiNET.ImGui.EndChild();
            }

            rlImGui.ImageRect(ViewTexture.Texture, (int)windowSize.X, (int)windowSize.Y, viewRect);
            ImGuiNET.ImGui.End();
        }

        ImGuiNET.ImGui.PopStyleVar();
    }

    public override void Shutdown()
    {
        Raylib.UnloadRenderTexture(ViewTexture);
        Raylib.UnloadTexture(_imageTexture);
    }

    public override void Update()
    {
        if (!_open)
        {
            return;
        }

        if (Raylib.IsWindowResized())
        {
            Raylib.UnloadRenderTexture(ViewTexture);

            ViewTexture = Raylib.LoadRenderTexture(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

            _camera.Offset.X = Raylib.GetScreenWidth() / 2.0f;
            _camera.Offset.Y = Raylib.GetScreenHeight() / 2.0f;
        }

        if (Focused)
        {
            if (_currentToolMode == ToolMode.Move)
            {
                if (Raylib.IsMouseButtonDown(MouseButton.Left))
                {
                    if (!_dragging)
                    {
                        _lastMousePosition = Raylib.GetMousePosition();
                        _lastTarget = _camera.Target;
                    }

                    _dragging = true;
                    var mousePos = Raylib.GetMousePosition();
                    var mouseDelta = Raymath.Vector2Subtract(_lastMousePosition, mousePos);

                    mouseDelta.X /= _camera.Zoom;
                    mouseDelta.Y /= _camera.Zoom;

                    _camera.Target = Raymath.Vector2Add(_lastTarget, mouseDelta);
                    _dirtyScene = true;
                }
                else
                {
                    _dragging = false;
                }
            }
            else
            {
                _dragging = false;
            }

            if (_dirtyScene)
            {
                _dirtyScene = false;
                UpdateRenderTexture();
            }
        }
    }

    protected void UpdateRenderTexture()
    {
        Raylib.BeginTextureMode(ViewTexture);
        {
            Raylib.ClearBackground(Color.Blue);

            Raylib.BeginMode2D(_camera);
            {
                Raylib.DrawTexture(_imageTexture, _imageTexture.Width / -2, _imageTexture.Height / -2, Color.White);
            }
            Raylib.EndMode2D();
        }
        Raylib.EndTextureMode();
    }
}
