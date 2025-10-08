using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using Raylib_cs;

internal sealed unsafe class Program
{
    const int GOL_WIDTH = 768;
    const int MAX_TRANSFERRED_BUFFERS = 48;
    private unsafe static void Main(string[] args)
    {
        Raylib.InitWindow(GOL_WIDTH, GOL_WIDTH, "raylib [rlgl] example - compute shader - game of life");

        var resolution = new Vector2(GOL_WIDTH, GOL_WIDTH);
        var brushSize = 8;

        var golLogicShaderProgram = LoadComputeShader("resources/shaders/glsl430/game_of_life.glsl");
        var golLogicShaderTransfertProgram = LoadComputeShader("resources/shaders/glsl430/game_of_life_transfert.glsl");

        var golRenderShader = Raylib.LoadShader(null, "resources/shaders/glsl430/game_of_life_render.glsl");
        var resUniformLoc = Raylib.GetShaderLocation(golRenderShader, "resolution");

        var ssboA = Rlgl.LoadShaderBuffer(GOL_WIDTH * GOL_WIDTH * sizeof(uint), null, Rlgl.DYNAMIC_COPY);
        var ssboB = Rlgl.LoadShaderBuffer(GOL_WIDTH * GOL_WIDTH * sizeof(uint), null, Rlgl.DYNAMIC_COPY);
        var ssboTransfert = Rlgl.LoadShaderBuffer((uint)Marshal.SizeOf<GolUpdateSSBO>(), null, Rlgl.DYNAMIC_COPY);

        var transfertBuffer = new GolUpdateSSBO();
        var transfertBufferPtr = Marshal.AllocHGlobal(Marshal.SizeOf<GolUpdateSSBO>());

        var whiteImage = Raylib.GenImageColor(GOL_WIDTH, GOL_WIDTH, Color.White);
        var whiteTexture = Raylib.LoadTextureFromImage(whiteImage);
        Raylib.UnloadImage(whiteImage);

        while (!Raylib.WindowShouldClose())
        {
            brushSize += (int)Raylib.GetMouseWheelMove();

            if ((Raylib.IsMouseButtonDown(MouseButton.Left) || Raylib.IsMouseButtonDown(MouseButton.Right)) && transfertBuffer.count < MAX_TRANSFERRED_BUFFERS)
            {
                transfertBuffer.commands[transfertBuffer.count].x = (uint)(Raylib.GetMouseX() - brushSize / 2);
                transfertBuffer.commands[transfertBuffer.count].y = (uint)(Raylib.GetMouseX() - brushSize / 2);
                transfertBuffer.commands[transfertBuffer.count].w = (uint)brushSize;
                transfertBuffer.commands[transfertBuffer.count].enabled = (uint)(Raylib.IsMouseButtonDown(MouseButton.Left) ? 1 : 0);
                transfertBuffer.count++;
            }
            else if (transfertBuffer.count > 0)
            {
                Rlgl.UpdateShaderBuffer(ssboTransfert, (void*)transfertBufferPtr, (uint)Marshal.SizeOf<GolUpdateSSBO>(), 0);
                Rlgl.EnableShader(golLogicShaderTransfertProgram);
                Rlgl.BindShaderBuffer(ssboA, 1);
                Rlgl.BindShaderBuffer(ssboTransfert, 3);
                Rlgl.ComputeShaderDispatch(transfertBuffer.count, 1, 1);
                Rlgl.DisableShader();

                transfertBuffer.count = 0;
            }
            else
            {
                Rlgl.EnableShader(golLogicShaderProgram);
                Rlgl.BindShaderBuffer(ssboA, 1);
                Rlgl.BindShaderBuffer(ssboB, 2);
                Rlgl.ComputeShaderDispatch(GOL_WIDTH / 16, GOL_WIDTH / 16, 1);
                Rlgl.DisableShader();

                // Swap buffers
                (ssboB, ssboA) = (ssboA, ssboB);
            }

            Rlgl.BindShaderBuffer(ssboA, 1);
            Raylib.SetShaderValue(golRenderShader, resUniformLoc, &resolution, ShaderUniformDataType.Vec2);

            Raylib.BeginDrawing();
            {
                Raylib.ClearBackground(Color.RayWhite);

                Raylib.BeginShaderMode(golRenderShader);
                {
                    Raylib.DrawTexture(whiteTexture, 0, 0, Color.White);
                }
                Raylib.EndShaderMode();

                Raylib.DrawRectangleLines(
                    posX: Raylib.GetMouseX() - brushSize / 2,
                    posY: Raylib.GetMouseY() - brushSize / 2,
                    width: brushSize, height: brushSize,
                    color: Color.Red
                );

                Raylib.DrawText(
                    text: "Use mouse wheel to change brush size",
                    posX: 10, posY: 10, fontSize: 20,
                    color: Color.Black
                );

                Raylib.DrawText(
                    text: $"Brush size: {brushSize}",
                    posX: 10, posY: 40, fontSize: 20,
                    color: Color.Black
                );

                Raylib.DrawFPS(Raylib.GetScreenWidth() -100, 10);
            }
            Raylib.EndDrawing();

        }

        Marshal.FreeHGlobal(transfertBufferPtr);
        Rlgl.UnloadShaderProgram(golLogicShaderProgram);
    }

    private static uint LoadComputeShader(string shaderFilePath)
    {
        ArgumentNullException.ThrowIfNull(shaderFilePath);

        if (!File.Exists(shaderFilePath))
        {
            throw new FileNotFoundException($"Shader file not found: {shaderFilePath}");
        }

        fixed (byte* path = Encoding.ASCII.GetBytes(shaderFilePath))
        {
            var golLogicShaderCode = Raylib.LoadFileText((sbyte*)path);
            var golLogicShader = Rlgl.CompileShader(golLogicShaderCode, (int)ShaderType.Compute);
            var golLogicShaderProgram = Rlgl.LoadComputeShaderProgram(golLogicShader);
            Raylib.UnloadFileText(golLogicShaderCode);

            return golLogicShaderProgram;
        }
    }
    internal struct GolUpdateCommand
    {
        public uint x;
        public uint y;
        public uint w;
        public uint enabled;
    };

    internal unsafe struct GolUpdateSSBO
    {
        public GolUpdateCommand[] commands = new GolUpdateCommand[MAX_TRANSFERRED_BUFFERS];
        public uint count;

        public GolUpdateSSBO()
        {
        }
    };

}
