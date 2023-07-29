using Microsoft.Xna.Framework;

namespace RGB.Screens;

public class TitleScreen : SplashScreen
{
    public TitleScreen(Game game) : base(game)
    {     

    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Game.GraphicsDevice.Clear(Color.Blue);
        base.Draw(gameTime);
    }
}