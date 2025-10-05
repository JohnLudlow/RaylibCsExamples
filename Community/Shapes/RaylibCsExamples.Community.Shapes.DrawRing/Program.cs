using System.Numerics;
using Raylib_cs;

const int screnWidth = 1600;
const int screenHeight = 900;

Raylib.InitWindow(screnWidth, screenHeight, "raylib [shapes] example - draw ring");
var center = new Vector2(screnWidth / 2, screenHeight / 2);

var innerRadius = 80.0f;
var outerRadius = 190.0f;

var startAngle = 0.0f;
int endAngle = 360;
int segments = 0;
int minSegments = 4;

var drawRing = true;
var drawRingLines = false;
var drawCircleLines = false;

Raylib.SetTargetFPS(60);

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    {
        Raylib.ClearBackground(Color.RayWhite);

        Raylib.DrawLine(500, 0, 500, Raylib.GetScreenHeight(), Raylib.ColorAlpha(Color.LightGray, 0.6f));
        Raylib.DrawRectangle(500, 0, Raylib.GetScreenWidth() - 500, Raylib.GetScreenHeight(), Raylib.ColorAlpha(Color.LightGray, 0.3f));

        if (drawRing)
        {
            Raylib.DrawRing(center, innerRadius, outerRadius, startAngle, endAngle, segments, Raylib.ColorAlpha(Color.Maroon, 0.6f));
        }

        if (drawRingLines)
        {
            Raylib.DrawRingLines(center, innerRadius, outerRadius, startAngle, endAngle, segments, Raylib.ColorAlpha(Color.Black, 0.4f));
        }

        if (drawCircleLines)
        {
            Raylib.DrawCircleSectorLines(center, outerRadius, startAngle, endAngle, segments, Raylib.ColorAlpha(Color.Black, 0.4f));
        }

        minSegments = (int)MathF.Ceiling(endAngle - startAngle / 90);
        var color = (segments >= minSegments) ? Color.Maroon : Color.DarkGray;
        Raylib.DrawText($"MODE: {((segments >= minSegments) ? "MANUAL" : "AUTO")} Rings segments: {segments} (min: {minSegments})", 550, 15, 20, color);
        Raylib.DrawFPS(10, 10);
    }
    Raylib.EndDrawing();
}

Raylib.CloseWindow();