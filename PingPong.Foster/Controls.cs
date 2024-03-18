using Foster.Framework;

namespace PingPong.Foster;

public static class Controls
{
    public static readonly VirtualButton Up = new();
    public static readonly VirtualButton Down = new();


    public static void Init()
    {
        Up.Buffer = 0.15f;
        Up.Add(Keys.W, Keys.Up);
        Down.Buffer = 0.15f;
        Down.Add(Keys.S, Keys.Down);
    }
}