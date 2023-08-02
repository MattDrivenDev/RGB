using Microsoft.Xna.Framework;
using RGB.World;

namespace RGB.Player;

public class R : PlayerCharacter
{
    public R(Game game, WorldMap map) : base(game, map)
    {
        
    }

    protected override Color Color => Color.Red;

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}