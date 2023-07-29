using Microsoft.Xna.Framework;

namespace RGB.Player;

public class Y : PlayerCharacter
{
    public Y(Game game) : base(game)
    {
        
    }

    protected override Color Color => Color.Yellow;

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}