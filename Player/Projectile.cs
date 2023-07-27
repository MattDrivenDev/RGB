using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGB.Player;

public class Projectile : DrawableGameComponent
{
    public Projectile(Game game, Vector2 position, float angleInDegrees, float speed) 
        : base(game)
    {
        this.SpriteBatch = game.Services.GetService<SpriteBatch>();

        Position = position;
        AngleInDegrees = angleInDegrees;
        Speed = speed;
    }

    public SpriteBatch SpriteBatch { get; init; }
    public Vector2 Position { get; set; }
    public float AngleInDegrees { get; set; }
    public float Speed { get; set; }

    public override void Update(GameTime gameTime)
    {
        // Calculate the angle in radians.
        var angleInRadians = MathHelper.ToRadians(AngleInDegrees);

        // Calculate the X and Y components of the velocity.
        var velocityX = MathF.Cos(angleInRadians) * Speed;
        var velocityY = MathF.Sin(angleInRadians) * Speed;

        // Move the position of the projectile.
        Position += new Vector2(velocityX, velocityY);

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin();
        SpriteBatch.DrawCircle(Position, 2, Color.Black);
        SpriteBatch.End();   

        base.Draw(gameTime);
    }
}