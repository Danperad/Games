using System.Numerics;
using Foster.Framework;

namespace PingPong.Foster;

public struct GameState
{
    public BallState Ball { get; set; }
    public PlayerState Player { get; set; }
    public ulong Score { get; set; }
}

public struct PlayerState
{
    public const float Acceleration = 1200;
    public const float Friction = 500;
    public const float MaxSpeed = 800;

    public Rect LeftBord { get; set; }
    public Rect RightBord { get; set; }
    public Vector2 Speed { get; set; }
}

public struct BallState
{
    public const float MaxSpeed = 800;
    public const float StartSpeed = 200f;
    public const float SpeedMultiplier = 1.1f;
    public const float Angle = 3.1415f * 0.66f;

    public Circle Ball { get; set; }
    public Vector2 Top => Ball.Position with { Y = Ball.Position.Y + Ball.Radius };
    public Vector2 Bottom => Ball.Position with { Y = Ball.Position.Y - Ball.Radius };
    public Vector2 Left => Ball.Position with { X = Ball.Position.X - Ball.Radius };
    public Vector2 Right => Ball.Position with { X = Ball.Position.X + Ball.Radius };
    public Vector2 Speed { get; set; }
}