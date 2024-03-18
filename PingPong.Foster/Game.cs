using System.Numerics;
using Foster.Framework;

namespace PingPong.Foster;

public class Game
{
    private readonly Batcher _batcher = new();
    private GameState _gameState;
    private readonly LinkedList<IRendered> _rendereds = new();

    private readonly LinkedList<IStatable> _updatables = new();
    public SpriteFont? Font;

    public Game()
    {
        Instant = this;
        var player = new Player();
        player.Startup();
        _updatables.AddLast(player);
        _rendereds.AddLast(player);
        var ball = new Ball();
        ball.Startup();
        _updatables.AddLast(ball);
        _rendereds.AddLast(ball);
        player.UpdateGameState(ref _gameState);
        ball.UpdateGameState(ref _gameState);
        if (File.Exists(".\\font.ttf")) Font = new SpriteFont(".\\font.ttf", 32f);
    }

    public static Game Instant { get; private set; } = null!;

    public GameState CurrentGameState => _gameState;

    public void Update()
    {
        foreach (var updatable in _updatables)
        {
            updatable.Update();
            updatable.UpdateGameState(ref _gameState);
        }
    }

    public void Render()
    {
        foreach (var rendered in _rendereds) rendered.Render();

        if (Font == null) return;

        var text = _gameState.Score.ToString();
        _batcher.Text(Font, text, new Vector2(App.WidthInPixels / 2f - Font.WidthOf(text) / 2, 0),
            Color.Yellow);
        _batcher.Render();
        _batcher.Clear();
    }
}