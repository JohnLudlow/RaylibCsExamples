using Raylib_cs;

namespace RaylibExamples.Core.BasicScreenManager;

internal enum GameScreen
{
    Logo, Title, Gameplay, Ending
}

public class Program
{
    public static int Main()
    {
        const int screenWidth = 800;
        const int screenHeight = 450;

        Raylib.InitWindow(screenWidth, screenHeight, "raylib [core] example - basic screen manaager");

        var currentScreen = GameScreen.Logo;

        var frameCounter = 0;

        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose())
        {
            switch (currentScreen)
            {
                case GameScreen.Logo:
                    {
                        frameCounter++;

                        if (frameCounter >= 120)
                        {
                            currentScreen = GameScreen.Title;
                        }
                    }
                    break;

                case GameScreen.Title:
                    {
                        if (Raylib.IsKeyPressed(KeyboardKey.Enter) || Raylib.IsGestureDetected(Gesture.Tap))
                        {
                            currentScreen = GameScreen.Gameplay;
                        }
                    }
                    break;
                case GameScreen.Gameplay:
                    {
                        if (Raylib.IsKeyPressed(KeyboardKey.Enter) || Raylib.IsGestureDetected(Gesture.Tap))
                        {
                            currentScreen = GameScreen.Ending;
                        }
                    }
                    break;
                case GameScreen.Ending:
                    {
                        if (Raylib.IsKeyPressed(KeyboardKey.Enter) || Raylib.IsGestureDetected(Gesture.Tap))
                        {
                            currentScreen = GameScreen.Title;
                        }
                    }
                    break;

                default:
                    break;
            }

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RayWhite);

            switch (currentScreen)
            {
                case GameScreen.Logo:
                    {
                        // TODO: Draw LOGO screen here!
                        Raylib.DrawText("LOGO SCREEN", 20, 20, 40, Color.LightGray);
                        Raylib.DrawText("WAIT for 2 SECONDS...", 290, 220, 20, Color.Gray);

                    }
                    break;
                case GameScreen.Title:
                    {
                        // TODO: Draw TITLE screen here!
                        Raylib.DrawRectangle(0, 0, screenWidth, screenHeight, Color.Green);
                        Raylib.DrawText("TITLE SCREEN", 20, 20, 40, Color.DarkGreen);
                        Raylib.DrawText("PRESS ENTER or TAP to JUMP to GAMEPLAY SCREEN", 120, 220, 20, Color.DarkGreen);

                    }
                    break;
                case GameScreen.Gameplay:
                    {
                        // TODO: Draw GAMEPLAY screen here!
                        Raylib.DrawRectangle(0, 0, screenWidth, screenHeight, Color.Purple);
                        Raylib.DrawText("GAMEPLAY SCREEN", 20, 20, 40, Color.Maroon);
                        Raylib.DrawText("PRESS ENTER or TAP to JUMP to ENDING SCREEN", 130, 220, 20, Color.Maroon);

                    }
                    break;
                case GameScreen.Ending:
                    {
                        // TODO: Draw ENDING screen here!
                        Raylib.DrawRectangle(0, 0, screenWidth, screenHeight, Color.Blue);
                        Raylib.DrawText("ENDING SCREEN", 20, 20, 40, Color.DarkBlue);
                        Raylib.DrawText("PRESS ENTER or TAP to RETURN to TITLE SCREEN", 120, 220, 20, Color.DarkBlue);
                    }
                    break;
            }
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();

        return 0;
    }
}