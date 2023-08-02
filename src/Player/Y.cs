using Microsoft.Xna.Framework;
using RGB.World;

namespace RGB.Player;

public class Y : PlayerCharacter
{
    public Y(Game game, WorldMap map) : base(game, map)
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