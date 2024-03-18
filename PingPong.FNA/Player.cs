using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PingPong.FNA;

public class Player : DrawableGameComponent
{
    public SpriteBatch Batch { get; private set; } = null!;
    private Texture2D WhiteRectangle { get; set; } = null!;
    private Rectangle _rectangle;
    private bool _isRightDirection = true;
    public Player(Game game) : base(game)
    {
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        base.LoadContent();
        Batch = new SpriteBatch(GraphicsDevice);
        WhiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
        WhiteRectangle.SetData([Color.White]);
        _rectangle = new Rectangle(10, 20, 80, 30);
    }

    protected override void UnloadContent()
    {
        base.UnloadContent();
        Batch.Dispose();
        WhiteRectangle.Dispose();
    }

    public override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState()[Keys.Escape] == KeyState.Down)
        {
            Game.Exit();
        }
        if (_isRightDirection)
            _rectangle = _rectangle with { X = _rectangle.X + 1 };
        else
            _rectangle = _rectangle with { X = _rectangle.X - 1 };

        _isRightDirection = _rectangle.X switch
        {
            10 => true,
            500 => false,
            _ => _isRightDirection
        };

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        GraphicsDevice.Clear(Color.Black);
        Batch.Begin();
        Batch.Draw(WhiteRectangle, _rectangle,
            Color.Chocolate);
        Batch.End();
    }
}