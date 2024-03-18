using Foster.Framework;

namespace PingPong.Foster;

internal class Program
{
    public static void Main(string[] args)
    {
        Time.FixedStep = false;
        App.Register<Manager>();
        App.Resizable = false;
        App.Run("Ping Pong", 800, 600);
    }
}

public class Manager : Module
{
    private readonly Batcher _batcher = new();
    private Game _game = null!;

    public override void Startup()
    {
        Console.Clear();
        Controls.Init();
        _game = new Game();
    }

    public override void Update()
    {
        if (Input.Keyboard.Down(Keys.Escape)) App.Exit();
        _game.Update();
    }

    public override void Render()
    {
        Graphics.Clear(Color.Black);
        _game.Render();
        _batcher.Render();
        _batcher.Clear();
    }
}