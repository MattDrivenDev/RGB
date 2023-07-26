using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using RGB.Player;

namespace RGB.Screens;

public abstract class MenuScreen : GameScreen
{
    protected MenuScreen(Game game) : base(game)
    {
        PlayerInput = Game.Services.GetService<PlayerInput>();
    }

    protected PlayerInput PlayerInput { get; init; }

    public override void Update(GameTime gameTime)
    {
        
    }

    public override void Draw(GameTime gameTime)
    {
        
    }
}