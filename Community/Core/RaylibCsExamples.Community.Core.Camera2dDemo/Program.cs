
using System.Numerics;
using Raylib_cs;

namespace RaylibCsExamples.Community.Core.Camera2dDemo;

internal sealed class Program
{
    private static void Main(string[] args)
    {
        const int MaxBuildings = 100;

        const int screenWidth = 800;
        const int screenHeight = 450;

        Raylib.InitWindow(screenWidth, screenHeight, "raylib [core] example - 2d camera");

        var player = new Rectangle(400, 280, 40, 40);
        var buildings = new (Rectangle rectangle, Color color)[MaxBuildings];

        var spacing = 0;

        for (var i = 0; i < MaxBuildings; i++)
        {
            var buildingHeight = Random.Shared.Next(50, 200);
            var buildingWidth = Random.Shared.Next(100, 800);

            buildings[i] = buildings[i] with
            {
                rectangle = new(
                    x: -6000 + spacing,
                    y: screenHeight - 130 - buildingHeight,
                    width: buildingWidth,
                    height: buildingHeight
                ),

                color = new(
                    Random.Shared.Next(200, 240),
                    Random.Shared.Next(200, 240),
                    Random.Shared.Next(200, 240)
                )
            };

            spacing += buildingWidth;
        }

        var camera = new Camera2D()
        {
            Target = new(player.X + 20, player.Y + 20),
            Offset = new(screenWidth / 2, screenHeight / 2),
            Rotation = 0.0f,
            Zoom = 1.0f
        };

        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose())
        {
            // Update
            //----------------------------------------------------------------------------------

            // Player movement
            if (Raylib.IsKeyDown(KeyboardKey.Right))
            {
                player.X += 2;
            }
            else if (Raylib.IsKeyDown(KeyboardKey.Left))
            {
                player.X -= 2;
            }

            // Camera3D target follows player
            camera.Target = new Vector2(player.X + 20, player.Y + 20);

            // Camera3D rotation controls
            if (Raylib.IsKeyDown(KeyboardKey.A))
            {
                camera.Rotation--;
            }
            else if (Raylib.IsKeyDown(KeyboardKey.S))
            {
                camera.Rotation++;
            }

            // Limit camera rotation to 80 degrees (-40 to 40)
            if (camera.Rotation > 40)
            {
                camera.Rotation = 40;
            }
            else if (camera.Rotation < -40)
            {
                camera.Rotation = -40;
            }

            // Camera3D zoom controls
            camera.Zoom += (float)Raylib.GetMouseWheelMove() * 0.05f;

            if (camera.Zoom > 3.0f)
            {
                camera.Zoom = 3.0f;
            }
            else if (camera.Zoom < 0.1f)
            {
                camera.Zoom = 0.1f;
            }

            // Camera3D reset (zoom and rotation)
            if (Raylib.IsKeyPressed(KeyboardKey.R))
            {
                camera.Zoom = 1.0f;
                camera.Rotation = 0.0f;
            }

            Raylib.BeginDrawing();
            Raylib.BeginMode2D(camera);
            Raylib.ClearBackground(Color.RayWhite);
            Raylib.DrawRectangle(-6000, 320, 13000, 8000, Color.DarkGray);

            foreach (var (rectangle, color) in buildings)
            {
                Raylib.DrawRectangleRec(rectangle, color);
            }

            Raylib.DrawRectangleRec(player, Color.Red);
            Raylib.DrawRectangle((int)camera.Target.X, -500, 1, screenHeight * 4, Color.Green);
            Raylib.DrawLine(
                -screenWidth * 10,
                (int)camera.Target.Y,
                screenWidth * 10,
                (int)camera.Target.Y,
                Color.Green
            );

            Raylib.EndMode2D();

            Raylib.DrawText("SCREEN AREA", 640, 10, 20, Color.Red);

            Raylib.DrawRectangle(posX: 0, posY: 0, width: screenWidth, height: 5, color: Color.Red);
            Raylib.DrawRectangle(posX: 0, posY: 5, width: 5, height: screenHeight - 10, color: Color.Red);
            Raylib.DrawRectangle(posX: screenWidth - 5, posY: 5, width: 5, height: screenHeight - 10, color: Color.Red);
            Raylib.DrawRectangle(posX: 0, posY: screenHeight - 5, width: screenWidth, height: 5, color: Color.Red);

            Raylib.DrawRectangle(posX: 10, posY: 10, width: 250, height: 113, color: Raylib.ColorAlpha(Color.SkyBlue, 0.5f));

            Raylib.DrawRectangleLines(10, 10, 250, 113, Color.Blue);

            Raylib.DrawText(text: "Free 2d camera controls:", posX: 20, posY: 20, fontSize: 10, color: Color.Black);
            Raylib.DrawText(text: "- Right/Left to move Offset", posX: 40, posY: 40, fontSize: 10, color: Color.DarkGray);
            Raylib.DrawText(text: "- Mouse Wheel to Zoom in-out", posX: 40, posY: 60, fontSize: 10, color: Color.DarkGray);
            Raylib.DrawText(text: "- A / S to Rotate", posX: 40, posY: 80, fontSize: 10, color: Color.DarkGray);
            Raylib.DrawText(text: "- R to reset Zoom and Rotation", posX: 40, posY: 100, fontSize: 10, color: Color.DarkGray);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}