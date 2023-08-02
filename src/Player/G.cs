using Microsoft.Xna.Framework;
using RGB.World;

namespace RGB.Player;

public class G : PlayerCharacter
{
    public G(Game game, WorldMap map) : base(game, map)
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