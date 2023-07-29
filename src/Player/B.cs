using Microsoft.Xna.Framework;

namespace RGB.Player;

public class B : PlayerCharacter
{
    public B(Game game) : base(game)
    {
        
    }

    protected override Color Color => Color.Blue;

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}