using System.Numerics;
using Raylib_cs;

namespace RaylibCsExamples.Community.Core.Models.SolarSystem;


/*******************************************************************************************
*
*   raylib [models] example - rlgl module usage with push/pop matrix transformations
*
*   This example uses [rlgl] module funtionality (pseudo-OpenGL 1.1 style coding)
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2018 Ramon Santamaria (@raysan5)
*
********************************************************************************************/
public class SolarSystem
{
    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800 * 2;
        const int screenHeight = 450 * 2;

        const float sunRadius = 4.0f;
        const float earthRadius = 0.6f;
        const float earthOrbitRadius = 8.0f;
        const float moonRadius = 0.16f;
        const float moonOrbitRadius = 1.5f;

        Raylib.InitWindow(screenWidth, screenHeight, "raylib [models] example - rlgl module usage with push/pop matrix transformations");

        // Define the camera to look into our 3d world
        var camera = new Camera3D
        {
            Position = new Vector3(16.0f, 16.0f, 16.0f),
            Target = Vector3.Zero,
            Up = Vector3.UnitY,
            FovY = 45.0f,
            Projection = CameraProjection.Perspective
        };

        // General system rotation speed
        var rotationSpeed = 0.2f;
        // Rotation of earth around itself (days) in degrees
        var earthRotation = 0.0f;
        // Rotation of earth around the Sun (years) in degrees
        var earthOrbitRotation = 0.0f;
        // Rotation of moon around itself
        var moonRotation = 0.0f;
        // Rotation of moon around earth in degrees
        var moonOrbitRotation = 0.0f;

        Raylib.SetTargetFPS(60);
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!Raylib.WindowShouldClose())
        {
            // Update
            //----------------------------------------------------------------------------------
            Raylib.UpdateCamera(ref camera, CameraMode.Free);

            earthRotation += 5.0f * rotationSpeed;
            earthOrbitRotation += 365 / 360.0f * (5.0f * rotationSpeed) * rotationSpeed;
            moonRotation += 2.0f * rotationSpeed;
            moonOrbitRotation += 8.0f * rotationSpeed;
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RayWhite);

            Raylib.BeginMode3D(camera);

            DrawSun(sunRadius);

            Rlgl.PushMatrix();
            {
                DrawEarthOrbit(earthOrbitRadius, earthOrbitRotation);

                Rlgl.PushMatrix();
                {
                    DrawEarth(earthRadius, earthRotation);
                }
                Rlgl.PopMatrix();

                DrawMoon(moonRadius, moonOrbitRadius, moonRotation, moonOrbitRotation);
            }
            Rlgl.PopMatrix();

            // Some reference elements (not affected by previous matrix transformations)
            Raylib.DrawCircle3D(
                new Vector3(0.0f, 0.0f, 0.0f),
                earthOrbitRadius,
                new Vector3(1, 0, 0),
                90.0f,
                Raylib.ColorAlpha(Color.Red, 0.5f)
            );
            Raylib.DrawGrid(20, 1.0f);

            Raylib.EndMode3D();

            Raylib.DrawText("EARTH ORBITING AROUND THE SUN!", 400, 10, 20, Color.Maroon);
            Raylib.DrawFPS(10, 10);

            Raylib.EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        Raylib.CloseWindow();
        //--------------------------------------------------------------------------------------

        return 0;
    }

    private static void DrawMoon(float moonRadius, float moonOrbitRadius, float moonRotation, float moonOrbitRotation)
    {
        // Rotation for Moon orbit around Earth
        Rlgl.Rotatef(moonOrbitRotation, 0.0f, 1.0f, 0.0f);
        // Translation for Moon orbit
        Rlgl.Translatef(moonOrbitRadius, 0.0f, 0.0f);
        // Rotation for Moon orbit around Earth inverted
        Rlgl.Rotatef(-moonOrbitRotation, 0.0f, 1.0f, 0.0f);
        // Rotation for Moon itself
        Rlgl.Rotatef(moonRotation, 0.0f, 1.0f, 0.0f);
        // Scale Moon
        Rlgl.Scalef(moonRadius, moonRadius, moonRadius);

        // Draw the Moon
        DrawSphereBasic(Color.LightGray);
    }

    private static void DrawEarth(float earthRadius, float earthRotation)
    {
        // Rotation for Earth itself
        Rlgl.Rotatef(earthRotation, 0.25f, 1.0f, 0.0f);
        // Scale Earth
        Rlgl.Scalef(earthRadius, earthRadius, earthRadius);

        // Draw the Earth
        DrawSphereBasic(Color.Blue);
    }

    private static void DrawEarthOrbit(float earthOrbitRadius, float earthOrbitRotation)
    {
        // Rotation for Earth orbit around Sun
        Rlgl.Rotatef(earthOrbitRotation, 0.0f, 1.0f, 0.0f);
        // Translation for Earth orbit
        Rlgl.Translatef(earthOrbitRadius, 0.0f, 0.0f);
        // Rotation for Earth orbit around Sun inverted
        Rlgl.Rotatef(-earthOrbitRotation, 0.0f, 1.0f, 0.0f);
    }

    // Draw sphere without any matrix transformation
    // NOTE: Sphere is drawn in world position ( 0, 0, 0 ) with radius 1.0f
    private static void DrawSphereBasic(Color color)
    {
        var rings = 16;
        var slices = 16;

        Rlgl.Begin(DrawMode.Triangles);
        Rlgl.Color4ub(color.R, color.G, color.B, color.A);

        for (var i = 0; i < rings + 2; i++)
        {
            for (var j = 0; j < slices; j++)
            {
                Rlgl.Vertex3f(
                    MathF.Cos(Raylib.DEG2RAD * (270 + 180 / (rings + 1) * i)) * MathF.Sin(Raylib.DEG2RAD * (j * 360 / slices)),
                    MathF.Sin(Raylib.DEG2RAD * (270 + 180 / (rings + 1) * i)),
                    MathF.Cos(Raylib.DEG2RAD * (270 + 180 / (rings + 1) * i)) * MathF.Cos(Raylib.DEG2RAD * (j * 360 / slices))
                );
                Rlgl.Vertex3f(
                    MathF.Cos(Raylib.DEG2RAD * (270 + 180 / (rings + 1) * (i + 1))) * MathF.Sin(Raylib.DEG2RAD * ((j + 1) * 360 / slices)),
                    MathF.Sin(Raylib.DEG2RAD * (270 + 180 / (rings + 1) * (i + 1))),
                    MathF.Cos(Raylib.DEG2RAD * (270 + 180 / (rings + 1) * (i + 1))) * MathF.Cos(Raylib.DEG2RAD * ((j + 1) * 360 / slices))
                );
                Rlgl.Vertex3f(
                    MathF.Cos(Raylib.DEG2RAD * (270 + 180 / (rings + 1) * (i + 1))) * MathF.Sin(Raylib.DEG2RAD * (j * 360 / slices)),
                    MathF.Sin(Raylib.DEG2RAD * (270 + 180 / (rings + 1) * (i + 1))),
                    MathF.Cos(Raylib.DEG2RAD * (270 + 180 / (rings + 1) * (i + 1))) * MathF.Cos(Raylib.DEG2RAD * (j * 360 / slices))
                );

                Rlgl.Vertex3f(
                    MathF.Cos(Raylib.DEG2RAD * (270 + 180 / (rings + 1) * i)) * MathF.Sin(Raylib.DEG2RAD * (j * 360 / slices)),
                    MathF.Sin(Raylib.DEG2RAD * (270 + 180 / (rings + 1) * i)),
                    MathF.Cos(Raylib.DEG2RAD * (270 + 180 / (rings + 1) * i)) * MathF.Cos(Raylib.DEG2RAD * (j * 360 / slices))
                );
                Rlgl.Vertex3f(
                    MathF.Cos(Raylib.DEG2RAD * (270 + 180 / (rings + 1) * i)) * MathF.Sin(Raylib.DEG2RAD * ((j + 1) * 360 / slices)),
                    MathF.Sin(Raylib.DEG2RAD * (270 + 180 / (rings + 1) * i)),
                    MathF.Cos(Raylib.DEG2RAD * (270 + 180 / (rings + 1) * i)) * MathF.Cos(Raylib.DEG2RAD * ((j + 1) * 360 / slices))
                );
                Rlgl.Vertex3f(
                    MathF.Cos(Raylib.DEG2RAD * (270 + 180 / (rings + 1) * (i + 1))) * MathF.Sin(Raylib.DEG2RAD * ((j + 1) * 360 / slices)),
                    MathF.Sin(Raylib.DEG2RAD * (270 + 180 / (rings + 1) * (i + 1))),
                    MathF.Cos(Raylib.DEG2RAD * (270 + 180 / (rings + 1) * (i + 1))) * MathF.Cos(Raylib.DEG2RAD * ((j + 1) * 360 / slices))
                );
            }
        }
        Rlgl.End();
    }

    private static void DrawSun(float sunRadius)
    {
        Rlgl.PushMatrix();
        {
            Rlgl.Scalef(sunRadius, sunRadius, sunRadius);
            DrawSphereBasic(Color.Gold);
        }
        Rlgl.PopMatrix();
    }
}