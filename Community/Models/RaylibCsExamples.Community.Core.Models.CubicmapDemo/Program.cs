using System.Numerics;
using Raylib_cs;

const int screenWidth = 800 * 2;
const int screenHeight = 450 * 2;

Raylib.InitWindow(screenWidth, screenHeight, "raylib [models] example - cubesmap loading and drawing");

var camera = new Camera3D
{
    Position    = new Vector3(16, 14, 16),
    Target      = Vector3.Zero,
    Up          = Vector3.UnitY,
    FovY        = 45,
    Projection  = CameraProjection.Perspective
};

var cubicmapTextureImage = Raylib.LoadImage("Resources/Images/cubicmap.png");
var cubicmapTexture = Raylib.LoadTextureFromImage(cubicmapTextureImage);

var cubicmapMesh = Raylib.GenMeshCubicmap(cubicmapTextureImage, Vector3.One);
var cubicmapModel = Raylib.LoadModelFromMesh(cubicmapMesh);

var cubicmapAtlasTexture = Raylib.LoadTexture("Resources/Images/cubicmap_atlas.png");

Raylib.SetMaterialTexture(ref cubicmapModel, 0, MaterialMapIndex.Albedo, ref cubicmapAtlasTexture);
Raylib.UnloadImage(cubicmapTextureImage);

var mapPosition = camera.Target;

Raylib.SetTargetFPS(60);

while (!Raylib.WindowShouldClose())
{
    Raylib.UpdateCamera(ref camera, CameraMode.Orbital);

    Raylib.BeginDrawing();
    {
        Raylib.ClearBackground(Color.RayWhite);

        Raylib.BeginMode3D(camera);
        {
            Raylib.DrawModel(cubicmapModel, mapPosition, 1, Color.White);
        }
        Raylib.EndMode3D();

        var logoPosition = new Vector2(screenWidth - cubicmapTexture.Width * 4 - 20, 20);

        Raylib.DrawTextureEx(
            texture: cubicmapTexture,
            position: logoPosition,
            rotation: 0,
            scale: 4,
            tint: Color.White
        );
        Raylib.DrawRectangleLines(
            posX: (int)logoPosition.X,
            posY: 20,
            width: cubicmapTexture.Width * 4,
            height: cubicmapTexture.Height * 4,
            color: Color.Green
        );

        var message = "cubicmap image used to generate map 3d model";
        var measure = Raylib.MeasureText(message, 24);

        Raylib.DrawText(message, (int)logoPosition.X - measure - 20, 30, 24, Color.Gray);

        Raylib.DrawFPS(10, 10);
    }
    Raylib.EndDrawing();
}

Raylib.UnloadTexture(cubicmapTexture);
Raylib.UnloadTexture(cubicmapAtlasTexture);
Raylib.UnloadModel(cubicmapModel);

Raylib.CloseWindow();