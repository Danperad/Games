using System.Diagnostics;
using System.Numerics;
using Foster.Framework;

namespace PingPong.Foster;

public class Ball : IStatable, IRendered
{
    private readonly Batcher _batcher = new();

    private BallState _ballState;
    private ulong _lastFrame;
    private ulong _localScore;

    private Rng _random = new(DateTime.Now.Nanosecond);
    private float CheckCollisionDistance => _ballState.Speed.Length() / 100;

    public void Render()
    {
        _batcher.Circle(_ballState.Ball, 16, Color.Red);
        _batcher.Render();
        _batcher.Clear();
    }

    public void Startup()
    {
        _localScore = 0;
        _ballState = new BallState
        {
            Ball = new Circle(new Vector2(App.WidthInPixels / 2f, App.HeightInPixels / 2f), 12),
            Speed = RandomVector2(BallState.Angle, 3.1415f * (0.66f + (_random.Boolean() ? 1 : 0))) *
                    BallState.StartSpeed
        };
    }

    public void Update()
    {
        CheckLose();
        CheckCollision();
        UpdatePosition();
    }

    public void UpdateGameState(ref GameState state)
    {
        state.Ball = _ballState;
        state.Score = _localScore;
    }

    private void UpdatePosition()
    {
        _ballState = _ballState with
        {
            Ball = _ballState.Ball + _ballState.Speed * Time.Delta
        };
        if (_ballState.Bottom.Y < 0 ||
            _ballState.Top.Y > App.HeightInPixels)
            _ballState.Speed = _ballState.Speed with { Y = -_ballState.Speed.Y };
    }

    private void CheckCollision()
    {
        if (Time.Frame - _lastFrame < 2) return;
        var leftPlayer = Game.Instant.CurrentGameState.Player.LeftBord;
        var rightPlayer = Game.Instant.CurrentGameState.Player.RightBord;

        if (leftPlayer.RightLine.Distance(_ballState.Left) <= CheckCollisionDistance)
        {
            _lastFrame = Time.Frame;
            _localScore++;
            var currentSpeed = _ballState.Speed.Length();
            currentSpeed *= BallState.SpeedMultiplier;
            var newSpeed = RandomVector2(BallState.Angle, 3.1415f * 1.66f);
            _ballState.Speed = newSpeed * currentSpeed;
            if (_ballState.Speed.Length() > BallState.MaxSpeed)
                _ballState.Speed = _ballState.Speed.Normalized() * BallState.MaxSpeed;
        }

        if (rightPlayer.LeftLine.Distance(_ballState.Right) <= CheckCollisionDistance)
        {
            _lastFrame = Time.Frame;
            _localScore++;
            var currentSpeed = _ballState.Speed.Length();
            currentSpeed *= BallState.SpeedMultiplier;
            var newSpeed = RandomVector2(BallState.Angle, 3.1415f * 0.66f);
            _ballState.Speed = newSpeed * currentSpeed;
            if (_ballState.Speed.Length() > BallState.MaxSpeed)
                _ballState.Speed = _ballState.Speed.Normalized() * BallState.MaxSpeed;
        }

        if (leftPlayer.BottomLine.Distance(_ballState.Top) <= CheckCollisionDistance || rightPlayer.BottomLine.Distance(
                _ballState.Top) <= CheckCollisionDistance)
        {
            _lastFrame = Time.Frame;
            _ballState.Speed = _ballState.Speed with { Y = float.Abs(_ballState.Speed.Y) };
        }

        if (leftPlayer.TopLine.Distance(_ballState.Bottom) <= CheckCollisionDistance || rightPlayer.TopLine.Distance(
                _ballState.Bottom) <= CheckCollisionDistance)
        {
            _lastFrame = Time.Frame;
            _ballState.Speed = _ballState.Speed with { Y = -float.Abs(_ballState.Speed.Y) };
        }
    }

    private void CheckLose()
    {
        if (_ballState.Right.X < 0 ||
            _ballState.Left.X > App.WidthInPixels)
            Startup();
    }

    public Vector2 RandomVector2(float angle, float angleMin)
    {
        var random = _random.Float() * angle + angleMin;
        return new Vector2(float.Cos(random), float.Sin(random)).Normalized();
    }
}