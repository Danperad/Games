using System.Numerics;
using Foster.Framework;

namespace PingPong.Foster;

public class Player : IStatable, IRendered
{
    private const float Size = 140;
    private readonly Batcher _batcher = new();

    private PlayerState _playerState;
    public Rect LeftPlayer => _playerState.LeftBord;
    public Rect RightPlayer => _playerState.RightBord;

    public void Render()
    {
        _batcher.Rect(_playerState.LeftBord, Color.White);
        _batcher.Rect(_playerState.RightBord, Color.White);
        _batcher.Render();
        _batcher.Clear();
    }

    public void Startup()
    {
        var leftPlayer = new Rect(7, App.HeightInPixels / 2f - (Size / 2 - 10), 20, Size);
        var rightPlayer = leftPlayer with { X = App.WidthInPixels - 27 };
        _playerState = new PlayerState
        {
            Speed = new Vector2(),
            LeftBord = leftPlayer,
            RightBord = rightPlayer
        };
    }

    public void Update()
    {
        if (Controls.Up.Down)
            _playerState = _playerState with
            {
                Speed = _playerState.Speed with { Y = _playerState.Speed.Y - PlayerState.Acceleration * Time.Delta }
            };
        if (Controls.Down.Down)
            _playerState = _playerState with
            {
                Speed = _playerState.Speed with { Y = _playerState.Speed.Y + PlayerState.Acceleration * Time.Delta }
            };
        if (!(Controls.Down.Down || Controls.Up.Down))
            _playerState = _playerState with
            {
                Speed = _playerState.Speed with
                {
                    Y = Calc.Approach(_playerState.Speed.Y, 0, Time.Delta * PlayerState.Friction)
                }
            };
        if (_playerState.Speed.Length() > PlayerState.MaxSpeed)
            _playerState.Speed = _playerState.Speed.Normalized() * PlayerState.MaxSpeed;

        _playerState.LeftBord += _playerState.Speed * Time.Delta;
        _playerState.RightBord -= _playerState.Speed * Time.Delta;
        if (_playerState.LeftBord.Top < 0)
        {
            var tmp = _playerState.LeftBord;
            tmp.Top = 0;
            _playerState.LeftBord = tmp;
            tmp = _playerState.RightBord;
            tmp.Bottom = App.HeightInPixels;
            _playerState.RightBord = tmp;
            _playerState.Speed = Vector2.Zero;
        }

        if (_playerState.LeftBord.Bottom > App.HeightInPixels)
        {
            var tmp = _playerState.LeftBord;
            tmp.Bottom = App.HeightInPixels;
            _playerState.LeftBord = tmp;
            tmp = _playerState.RightBord;
            tmp.Top = 0;
            _playerState.RightBord = tmp;
            _playerState.Speed = Vector2.Zero;
        }
    }

    public void UpdateGameState(ref GameState state)
    {
        state.Player = _playerState;
    }
}