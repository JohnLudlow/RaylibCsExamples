using System.Diagnostics;
using System.Numerics;
using Raylib_cs;

internal sealed class Program
{
    private static void Main(string[] args)
    {
        const int screenWidth = 800 * 2;
        const int screenHeight = 450 * 2;

        Raylib.InitWindow(screenWidth, screenHeight, "raylib [shaders] example - basic lighting");
        Raylib.SetTargetFPS(30);

        var cameraPositions = new Vector2[] {
            new (0            , 0           ),
            new (screenWidth  , 0           ),
            new (0            , screenHeight),
            new (screenWidth  , screenHeight),

            new (screenWidth * .1f , screenHeight * .1f ),
            new (screenWidth * .1f , screenHeight * .2f ),
            new (screenWidth * .1f , screenHeight * .3f ),
            new (screenWidth * .1f , screenHeight * .4f ),
            new (screenWidth * .1f , screenHeight * .5f ),
            new (screenWidth * .1f , screenHeight * .6f ),
            new (screenWidth * .1f , screenHeight * .7f ),
            new (screenWidth * .1f , screenHeight * .8f ),
            new (screenWidth * .1f , screenHeight * .9f ),

            new (screenWidth * .2f , screenHeight * .1f ),
            new (screenWidth * .2f , screenHeight * .2f ),
            new (screenWidth * .2f , screenHeight * .3f ),
            new (screenWidth * .2f , screenHeight * .4f ),
            new (screenWidth * .2f , screenHeight * .5f ),
            new (screenWidth * .2f , screenHeight * .6f ),
            new (screenWidth * .2f , screenHeight * .7f ),
            new (screenWidth * .2f , screenHeight * .8f ),
            new (screenWidth * .2f , screenHeight * .9f ),

            new (screenWidth * .3f , screenHeight * .1f ),
            new (screenWidth * .3f , screenHeight * .2f ),
            new (screenWidth * .3f , screenHeight * .3f ),
            new (screenWidth * .3f , screenHeight * .4f ),
            new (screenWidth * .3f , screenHeight * .5f ),
            new (screenWidth * .3f , screenHeight * .6f ),
            new (screenWidth * .3f , screenHeight * .7f ),
            new (screenWidth * .3f , screenHeight * .8f ),
            new (screenWidth * .3f , screenHeight * .9f ),

            new (screenWidth * .4f , screenHeight * .1f ),
            new (screenWidth * .4f , screenHeight * .2f ),
            new (screenWidth * .4f , screenHeight * .3f ),
            new (screenWidth * .4f , screenHeight * .4f ),
            new (screenWidth * .4f , screenHeight * .5f ),
            new (screenWidth * .4f , screenHeight * .6f ),
            new (screenWidth * .4f , screenHeight * .7f ),
            new (screenWidth * .4f , screenHeight * .8f ),
            new (screenWidth * .4f , screenHeight * .9f ),

            new (screenWidth * .5f , screenHeight * .1f ),
            new (screenWidth * .5f , screenHeight * .2f ),
            new (screenWidth * .5f , screenHeight * .3f ),
            new (screenWidth * .5f , screenHeight * .4f ),
            new (screenWidth * .5f , screenHeight * .5f ),
            new (screenWidth * .5f , screenHeight * .6f ),
            new (screenWidth * .5f , screenHeight * .7f ),
            new (screenWidth * .5f , screenHeight * .8f ),
            new (screenWidth * .5f , screenHeight * .9f ),

            new (screenWidth * .6f , screenHeight * .1f ),
            new (screenWidth * .6f , screenHeight * .2f ),
            new (screenWidth * .6f , screenHeight * .3f ),
            new (screenWidth * .6f , screenHeight * .4f ),
            new (screenWidth * .6f , screenHeight * .5f ),
            new (screenWidth * .6f , screenHeight * .6f ),
            new (screenWidth * .6f , screenHeight * .7f ),
            new (screenWidth * .6f , screenHeight * .8f ),
            new (screenWidth * .6f , screenHeight * .9f ),

            new (screenWidth * .7f , screenHeight * .1f ),
            new (screenWidth * .7f , screenHeight * .2f ),
            new (screenWidth * .7f , screenHeight * .3f ),
            new (screenWidth * .7f , screenHeight * .4f ),
            new (screenWidth * .7f , screenHeight * .5f ),
            new (screenWidth * .7f , screenHeight * .6f ),
            new (screenWidth * .7f , screenHeight * .7f ),
            new (screenWidth * .7f , screenHeight * .8f ),
            new (screenWidth * .7f , screenHeight * .9f ),

            new (screenWidth * .8f , screenHeight * .1f ),
            new (screenWidth * .8f , screenHeight * .2f ),
            new (screenWidth * .8f , screenHeight * .3f ),
            new (screenWidth * .8f , screenHeight * .4f ),
            new (screenWidth * .8f , screenHeight * .5f ),
            new (screenWidth * .8f , screenHeight * .6f ),
            new (screenWidth * .8f , screenHeight * .7f ),
            new (screenWidth * .8f , screenHeight * .8f ),
            new (screenWidth * .8f , screenHeight * .9f ),
            
            new (screenWidth * .9f , screenHeight * .1f ),
            new (screenWidth * .9f , screenHeight * .2f ),
            new (screenWidth * .9f , screenHeight * .3f ),
            new (screenWidth * .9f , screenHeight * .4f ),
            new (screenWidth * .9f , screenHeight * .5f ),
            new (screenWidth * .9f , screenHeight * .6f ),
            new (screenWidth * .9f , screenHeight * .7f ),
            new (screenWidth * .9f , screenHeight * .8f ),
            new (screenWidth * .9f , screenHeight * .9f ),            
        };

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

        var blueCubeModel = Raylib.LoadModelFromMesh(Raylib.GenMeshCube(3, 3, 3));
        var blueCubePosition = new Vector3(5, 2, 0);

        Raylib.SetMousePosition(screenWidth / 2, screenHeight / 2);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.UpdateCamera(ref camera, CameraMode.Orbital);
            Raylib.BeginDrawing();
            {
                Raylib.ClearBackground(Color.SkyBlue);
                Raylib.BeginMode3D(camera);
                {
                    Raylib.DrawModel(planeModel, Vector3.Zero, 1, Color.Gray);
                    Raylib.DrawModelWires(planeModel, Vector3.Zero, 1, Color.LightGray);

                    Raylib.DrawModel(greenCubeModel, greenCubePosition, 1, Color.Green);
                    Raylib.DrawModelWires(greenCubeModel, greenCubePosition, 1, Color.Black);

                    Raylib.DrawModel(blueCubeModel, blueCubePosition, 1, Color.Blue);
                    Raylib.DrawModelWires(blueCubeModel, blueCubePosition, 1, Color.Black);
                }
                Raylib.EndMode3D();

                if (IsBoundingBoxVisible(camera, cameraPositions, [(greenCubeModel, greenCubePosition), (blueCubeModel, blueCubePosition)], (greenCubeModel, greenCubePosition), "green cube"))
                {
                    Raylib.DrawText("Green cube is visible", 10, 30, 30, Color.Green);
                    Raylib.TraceLog(TraceLogLevel.All, "Green cube is visible");
                }
                else
                {
                    Raylib.DrawText("Green cube is NOT visible", 10, 30, 30, Color.DarkGreen);
                    Raylib.TraceLog(TraceLogLevel.All, "Green cube is NOT visible");
                }

                if (IsBoundingBoxHovered(camera, greenCubeModel, greenCubePosition))
                {
                    Raylib.DrawText("Green cube is hovered", 10, 60, 30, Color.DarkGreen);
                    Raylib.TraceLog(TraceLogLevel.All, "Green cube is hovered");
                }

                if (IsBoundingBoxVisible(camera, cameraPositions, [(greenCubeModel, greenCubePosition), (blueCubeModel, blueCubePosition)], (blueCubeModel, blueCubePosition), "blue cube"))
                {
                    Raylib.DrawText("Blue cube is visible", 10, 120, 30, Color.Blue);
                    Raylib.TraceLog(TraceLogLevel.All, "Blue cube is visible");
                }
                else
                {
                    Raylib.DrawText("Blue cube is NOT visible", 10, 120, 30, Color.DarkBlue);
                    Raylib.TraceLog(TraceLogLevel.All, "Blue cube is NOT visible");
                }

                if (IsBoundingBoxHovered(camera, blueCubeModel, blueCubePosition))
                {
                    Raylib.DrawText("Blue cube is hovered", 10, 150, 30, Color.DarkBlue);
                    Raylib.TraceLog(TraceLogLevel.All, "Blue cube is hovered");
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

    private static bool IsBoundingBoxVisible(Camera3D camera, Vector2[] cameraPositions, (Model model, Vector3 position)[] allModels, (Model model, Vector3 position) targetModel, string id)
    {
        foreach (var cameraPosition in cameraPositions)
        {
            var ray = Raylib.GetScreenToWorldRay(cameraPosition, camera);
            var closestModel = default(Model);
            var closestCollision = (RayCollision?)null;

            foreach (var model in allModels)
            {
                var boundingBox = Raylib.GetModelBoundingBox(model.model);
                boundingBox.Min += model.position;
                boundingBox.Max += model.position;

                var collision = Raylib.GetRayCollisionBox(ray, boundingBox);

                if (collision.Hit && (collision.Distance < closestCollision?.Distance || closestCollision is null))
                {
                    Raylib.DrawBoundingBox(boundingBox, Color.Purple);
                    closestCollision = collision;
                    closestModel = model.model;
                }
            }

            if (closestCollision is not null && closestModel.Equals(targetModel.model))
            {
                Raylib.TraceLog(TraceLogLevel.All, $"{id} is visible at position {ray.Position} ({closestCollision})");
                Raylib.DrawText("X", (int)cameraPosition.X, (int)cameraPosition.Y, 10, Color.DarkGreen);

                return true;
            }
            else
            {
                Raylib.DrawText("X", (int)cameraPosition.X, (int)cameraPosition.Y, 10, Color.Red);
            }
        }

        return false;
    }

    private static bool IsBoundingBoxHovered(Camera3D camera, Model model, Vector3 position)
    {
        var ray = Raylib.GetScreenToWorldRay(Raylib.GetMousePosition(), camera);
        var boundingBox = Raylib.GetModelBoundingBox(model);
        boundingBox.Min += position;
        boundingBox.Max += position;

        var rayCollision = Raylib.GetRayCollisionBox(ray, boundingBox);
        if (rayCollision.Hit)
        {
            Raylib.DrawBoundingBox(boundingBox, Color.Purple);
            return true;
        }
        return false;
    }
}