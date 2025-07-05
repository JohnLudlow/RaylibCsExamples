using System.Diagnostics;
using System.Numerics;
using Raylib_cs;

internal sealed unsafe class Program
{
    private static void Main(string[] args)
    {
        const int screenWidth = 800 * 2;
        const int screenHeight = 450 * 2;


        Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint);
        Raylib.InitWindow(screenWidth, screenHeight, "raylib [shaders] example - basic lighting");
        Raylib.SetTargetFPS(30);



        var camera = new Camera3D()
        {
            Position = new Vector3(20, 2, 0),
            Target = new Vector3(0, .5f, 0),
            Up = Vector3.UnitY,
            FovY = 45,
            Projection = CameraProjection.Perspective
        };

        var planeModel = Raylib.LoadModelFromMesh(Raylib.GenMeshPlane(20, 20, 3, 3));

        var greenCubeModel = Raylib.LoadModelFromMesh(Raylib.GenMeshCube(3, 3, 3));
        var greenCubePosition = new Vector3(-5, 2, 0);
        var greenCubeSize = new Vector3(3, 3, 3);
        var greenCubeBoundingBox = new BoundingBox(
            greenCubePosition - greenCubeSize / 2,
            greenCubePosition + greenCubeSize / 2
        );

        var blueCubeModel = Raylib.LoadModelFromMesh(Raylib.GenMeshCube(3, 3, 3));
        var blueCubePosition = new Vector3(5, 2, 0);
        var blueCubeSize = new Vector3(3, 3, 3);
        var blueCubeBoundingBox = new BoundingBox(
            blueCubePosition - blueCubeSize / 2,
            blueCubePosition + blueCubeSize / 2
        );

        while (!Raylib.WindowShouldClose())
        {
            var greenCubeVisible = true;
            var blueCubeVisible = true;

            Raylib.UpdateCamera(ref camera, CameraMode.Orbital);
            Raylib.BeginDrawing();
            {
                Raylib.ClearBackground(Color.SkyBlue);
                Raylib.BeginMode3D(camera);
                {
                    Raylib.DrawModel(planeModel, Vector3.Zero, 1, Color.Gray);
                    Raylib.DrawModelWires(planeModel, Vector3.Zero, 1, Color.LightGray);

                    Raylib.DrawModel(greenCubeModel, greenCubePosition, 1, Color.Green);
                    Raylib.DrawModelWires(greenCubeModel, greenCubePosition, 1, Color.Green);

                    Raylib.DrawModel(blueCubeModel, blueCubePosition, 1, Color.Blue);
                    Raylib.DrawModelWires(blueCubeModel, blueCubePosition, 1, Color.Blue);

                    greenCubeVisible = IsModelVisible(camera, screenWidth, screenHeight, greenCubeModel);
                    blueCubeVisible = IsModelVisible(camera, screenWidth, screenHeight, blueCubeModel);
                }
                Raylib.EndMode3D();


                if (greenCubeVisible)
                {
                    Raylib.DrawText("Green cube is visible", 10, 30, 30, Color.Green);
                    Raylib.TraceLog(TraceLogLevel.All, "Green cube is visible");
                    Debug.Write("Green cube is visible");
                }
                else
                {
                    Raylib.DrawText("Green cube is NOT visible", 10, 30, 30, Color.DarkGreen);
                    Raylib.TraceLog(TraceLogLevel.All, "Green cube is NOT visible");
                    Debug.Write("Green cube is NOT visible");
                }

                if (blueCubeVisible)
                {
                    Raylib.DrawText("Blue cube is visible", 10, 90, 30, Color.Blue);
                    Raylib.TraceLog(TraceLogLevel.All, "Blue cube is visible");
                }
                else
                {
                    Raylib.DrawText("Blue cube is NOT visible", 10, 90, 30, Color.DarkBlue);
                    Raylib.TraceLog(TraceLogLevel.All, "Blue cube is NOT visible");
                }

                Raylib.DrawFPS(10, 1);
            }
            Raylib.EndDrawing();
        }

        Raylib.UnloadModel(greenCubeModel);
        Raylib.UnloadModel(blueCubeModel);
        Raylib.UnloadModel(planeModel);

        Raylib.CloseWindow();
    }

    private static bool IsModelVisible(Camera3D camera, int screenWidth, int screenHeight, Model model)
    {
        var ray = Raylib.GetScreenToWorldRay(new Vector2 (screenWidth / 2, screenHeight / 2), camera);
        var rayCollision = Raylib.GetRayCollisionMesh(ray, model.Meshes[0], model.Transform);
        if (rayCollision.Hit)
        {
            return true;
        }
        return false;
    }
}