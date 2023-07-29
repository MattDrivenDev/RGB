using Microsoft.Xna.Framework;

namespace RGB.Player;

public class G : PlayerCharacter
{
    public G(Game game) : base(game)
    {
        
    }

    protected override Color Color => Color.Green;

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}