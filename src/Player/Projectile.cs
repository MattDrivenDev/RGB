using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using RGB.World;

namespace RGB.Player;

public class Projectile : DrawableGameComponent
{
    public Projectile(Game game, WorldMap map, 
        Vector2 position, float angleInDegrees, float speed) 
        : base(game)
    {
        this.SpriteBatch = game.Services.GetService<SpriteBatch>();
        this.Camera = game.Services.GetService<OrthographicCamera>();

        Map = map;
        Position = position;
        AngleInDegrees = angleInDegrees;
        Speed = speed;
    }

    public SpriteBatch SpriteBatch { get; init; }
    public Vector2 Position { get; set; }
    public float AngleInDegrees { get; set; }
    public float Speed { get; set; }
    protected OrthographicCamera Camera { get; init; }
    protected WorldMap Map { get; init; }

    public override void Update(GameTime gameTime)
    {
        // If the projectile is disabled and not visible, just dispose of it.
        if (!Enabled && !Visible) Dispose();

        // Calculate the angle in radians.
        var angleInRadians = MathHelper.ToRadians(AngleInDegrees);

        // Calculate the X and Y components of the velocity.
        var velocityX = MathF.Cos(angleInRadians) * Speed;
        var velocityY = MathF.Sin(angleInRadians) * Speed;

        // Move the position of the projectile.
        var positionDelta = new Vector2(velocityX, velocityY);

        foreach (var x in Map.CollisionActors)
        {
            switch(x.Bounds)
            {
                case RectangleF rect:
                    if (rect.Contains(Position + positionDelta))
                    {
                        Enabled = false;
                        Visible = false;
                    }
                    break;

                case CircleF circle:
                    if (circle.Contains(Position + positionDelta))
                    {
                        Enabled = false;
                        Visible = false;
                    }
                    break;
            }
        }

        Position += positionDelta;

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        var viewMatrix = Camera.GetViewMatrix();
        SpriteBatch.Begin(transformMatrix: viewMatrix);
        SpriteBatch.DrawCircle(Position, 2, Color.Black);
        SpriteBatch.End();   

        base.Draw(gameTime);
    }
}