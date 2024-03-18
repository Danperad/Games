using Microsoft.Xna.Framework;

namespace PingPong.FNA;

public class PingPongGame : Game
{
    public PingPongGame()
    {
        var r = new GraphicsDeviceManager(this);
        r.PreferredBackBufferHeight = 600;
        r.PreferredBackBufferWidth = 800;
        Window.Title = "Ping Pong";
        Content.RootDirectory = "Content";
        Components.Add(new Player(this));
    }
}