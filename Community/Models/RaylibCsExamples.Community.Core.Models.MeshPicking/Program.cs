using System.Numerics;
using Raylib_cs;

namespace RaylibCsExamples.Community.Models.AnimationDemo;

public class MeshPicking
{
    public unsafe static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800*2;
        const int screenHeight = 450*2;

        Raylib.InitWindow(screenWidth, screenHeight, "raylib [models] example - mesh picking");

        // Define the camera to look into our 3d world
        var camera = new Camera3D
        {
            Position = new Vector3(20.0f, 20.0f, 20.0f),
            Target = new Vector3(0.0f, 8.0f, 0.0f),
            Up = new Vector3(0.0f, 1.6f, 0.0f),
            FovY = 45.0f,
            Projection = CameraProjection.Perspective
        };

        // Picking ray
        Ray ray = new();

        var tower = Raylib.LoadModel("Resources/Models/turret.obj");
        var texture = Raylib.LoadTexture("Resources/Models/turret_diffuse.png");
        Raylib.SetMaterialTexture(ref tower, 0, MaterialMapIndex.Albedo, ref texture);

        Vector3 towerPos = new(5.0f, 0.0f, 5.0f);
        var towerBBox = Raylib.GetMeshBoundingBox(tower.Meshes[0]);

        // Ground quad
        Vector3 g0 = new(-50.0f, 0.0f, -50.0f);
        Vector3 g1 = new(-50.0f, 0.0f, 50.0f);
        Vector3 g2 = new(50.0f, 0.0f, 50.0f);
        Vector3 g3 = new(50.0f, 0.0f, -50.0f);

        // Test triangle
        Vector3 ta = new(-25.0f, 0.5f, 0.0f);
        Vector3 tb = new(-4.0f, 2.5f, 1.0f);
        Vector3 tc = new(-8.0f, 6.5f, 0.0f);

        Vector3 bary = new(0.0f, 0.0f, 0.0f);

        // Test sphere
        Vector3 sp = new(-30.0f, 5.0f, 5.0f);
        var sr = 4.0f;

        Raylib.SetTargetFPS(60);
        //--------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------
        // Main game loop
        //--------------------------------------------------------------------------------------
        while (!Raylib.WindowShouldClose())
        {
            //----------------------------------------------------------------------------------
            // Update
            //----------------------------------------------------------------------------------
            if (Raylib.IsCursorHidden())
            {
                Raylib.UpdateCamera(ref camera, CameraMode.FirstPerson);
            }

            // Toggle camera controls
            if (Raylib.IsMouseButtonPressed(MouseButton.Right))
            {
                if (Raylib.IsCursorHidden())
                {
                    Raylib.EnableCursor();
                }
                else
                {
                    Raylib.DisableCursor();
                }
            }

            // Display information about closest hit
            RayCollision collision = new();
            var hitObjectName = "None";
            collision.Distance = float.MaxValue;
            collision.Hit = false;
            var cursorColor = Color.White;

            // Get ray and test against objects
            ray = Raylib.GetScreenToWorldRay(Raylib.GetMousePosition(), camera);

            // Check ray collision aginst ground quad
            var groundHitInfo = Raylib.GetRayCollisionQuad(ray, g0, g1, g2, g3);
            if (groundHitInfo.Hit && (groundHitInfo.Distance < collision.Distance))
            {
                collision = groundHitInfo;
                cursorColor = Color.Green;
                hitObjectName = "Ground";
            }

            // Check ray collision against test triangle
            var triHitInfo = Raylib.GetRayCollisionTriangle(ray, ta, tb, tc);
            if (triHitInfo.Hit && (triHitInfo.Distance < collision.Distance))
            {
                collision = triHitInfo;
                cursorColor = Color.Purple;
                hitObjectName = "Triangle";

                bary = Raymath.Vector3Barycenter(collision.Point, ta, tb, tc);
            }

            // Check ray collision against test sphere
            var sphereHitInfo = Raylib.GetRayCollisionSphere(ray, sp, sr);
            if ((sphereHitInfo.Hit) && (sphereHitInfo.Distance < collision.Distance))
            {
                collision = sphereHitInfo;
                cursorColor = Color.Orange;
                hitObjectName = "Sphere";
            }

            // Check ray collision against bounding box first, before trying the full ray-mesh test
            var boxHitInfo = Raylib.GetRayCollisionBox(ray, towerBBox);
            if (boxHitInfo.Hit && boxHitInfo.Distance < collision.Distance)
            {
                collision = boxHitInfo;
                cursorColor = Color.Orange;
                hitObjectName = "Box";

                // Check ray collision against model meshes
                RayCollision meshHitInfo = new();
                for (var m = 0; m < tower.MeshCount; m++)
                {
                    // NOTE: We consider the model.Transform for the collision check but
                    // it can be checked against any transform matrix, used when checking against same
                    // model drawn multiple times with multiple transforms
                    meshHitInfo = Raylib.GetRayCollisionMesh(ray, tower.Meshes[m], tower.Transform);
                    if (meshHitInfo.Hit)
                    {
                        // Save the closest hit mesh
                        if ((!collision.Hit) || (collision.Distance > meshHitInfo.Distance))
                        {
                            collision = meshHitInfo;
                        }
                        break;
                    }
                }

                if (meshHitInfo.Hit)
                {
                    collision = meshHitInfo;
                    cursorColor = Color.Orange;
                    hitObjectName = "Mesh";
                }
            }
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RayWhite);

            Raylib.BeginMode3D(camera);

            // Draw the tower
            Raylib.DrawModel(tower, towerPos, 1.0f, Color.White);

            // Draw the test triangle
            Raylib.DrawLine3D(ta, tb, Color.Purple);
            Raylib.DrawLine3D(tb, tc, Color.Purple);
            Raylib.DrawLine3D(tc, ta, Color.Purple);

            // Draw the test sphere
            Raylib.DrawSphereWires(sp, sr, 8, 8, Color.Purple);

            // Draw the mesh bbox if we hit it
            if (boxHitInfo.Hit)
            {
                Raylib.DrawBoundingBox(towerBBox, Color.Lime);
            }

            // If we hit something, draw the cursor at the hit point
            if (collision.Hit)
            {
                Raylib.DrawCube(collision.Point, 0.3f, 0.3f, 0.3f, cursorColor);
                Raylib.DrawCubeWires(collision.Point, 0.3f, 0.3f, 0.3f, Color.Red);

                var normalEnd = collision.Point + collision.Normal;
                Raylib.DrawLine3D(collision.Point, normalEnd, Color.Red);
            }

            Raylib.DrawRay(ray, Color.Maroon);

            Raylib.DrawGrid(10, 10.0f);

            Raylib.EndMode3D();

            // Draw some debug GUI text
            Raylib.DrawText($"Hit Object: {hitObjectName}", 10, 50, 10, Color.Black);

            if (collision.Hit)
            {
                var ypos = 70;

                Raylib.DrawText($"Distance: {collision.Distance}", 10, ypos, 10, Color.Black);

                Raylib.DrawText($"Hit Pos: {collision.Point}", 10, ypos + 15, 10, Color.Black);

                Raylib.DrawText($"Hit Norm: {collision.Normal}", 10, ypos + 30, 10, Color.Black);

                if (triHitInfo.Hit)
                {
                    Raylib.DrawText($"Barycenter: {bary}", 10, ypos + 45, 10, Color.Black);
                }
            }

            Raylib.DrawText("Right click mouse to toggle camera controls", 10, 430, 10, Color.Gray);

            Raylib.DrawText("(c) Turret 3D model by Alberto Cano", screenWidth - 200, screenHeight - 20, 10, Color.Gray);

            Raylib.DrawFPS(10, 10);

            Raylib.EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        Raylib.UnloadModel(tower);
        Raylib.UnloadTexture(texture);

        Raylib.CloseWindow();
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
