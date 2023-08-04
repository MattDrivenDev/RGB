using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using RGB.World;

namespace RGB.Player;

public class Projectile
{
    public Projectile(Game game, WorldMap map, 
        Vector2 position, float angleInDegrees, float speed) 
    {
        Game = game;
        this.Camera = Game.Services.GetService<OrthographicCamera>();

        Map = map;
        Position = position;
        AngleInDegrees = angleInDegrees;
        Speed = speed;
        IsActive = true;
    }

    public Vector2 Position { get; set; }
    public float AngleInDegrees { get; set; }
    public float Speed { get; set; }
    protected OrthographicCamera Camera { get; init; }
    protected WorldMap Map { get; init; }
    protected Game Game { get; init; }
    public bool IsActive { get; private set; }

    public virtual void Update(GameTime gameTime)
    {
        // If the projectile is disabled and not visible, just dispose of it.
        if (!IsActive) return;

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
                        IsActive = false;
                    }
                    break;

                case CircleF circle:
                    if (circle.Contains(Position + positionDelta))
                    {
                        IsActive = false;
                    }
                    break;
            }
        }

        Position += positionDelta;
    }

    public virtual void Draw(GameTime gameTime)
    {
        if (!IsActive) return;

        var spriteBatch = new SpriteBatch(Game.GraphicsDevice);

        var viewMatrix = Camera.GetViewMatrix();
        spriteBatch.Begin(transformMatrix: viewMatrix);
        spriteBatch.DrawCircle(Position, 2, Color.Black);
        spriteBatch.End();   
    }
}