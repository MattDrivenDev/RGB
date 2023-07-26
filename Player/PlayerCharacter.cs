using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGB.Player;

/// <summary>
/// Base class for player characters: R, G, B, and Y.
/// </summary>
public abstract class PlayerCharacter : DrawableGameComponent
{
    private Vector2 _movement;
    private Vector2 _look;

    protected PlayerCharacter(Game game) : base(game)
    {
        PlayerInput = Game.Services.GetService<PlayerInput>();
        PlayerInput.OnMovement += OnMovement;
        PlayerInput.OnLook += OnLook;
        PlayerInput.OnShoot += OnShoot;
        PlayerInput.OnSwitchWeapon += OnSwitchWeapon;

        var settings = Game.Services.GetService<IniFileSettings>();
        PlayerSpeed = settings.PlayerSpeed;

        SpriteBatch = Game.Services.GetService<SpriteBatch>();

        Position = Vector2.Zero;
    }

    protected float PlayerSpeed { get; init; }
    protected PlayerInput PlayerInput { get; init; }
    protected Vector2 Position { get; set; }
    protected Vector2 Aim { get; set; }
    protected float AngleInDegrees { get; set; }
    protected SpriteBatch SpriteBatch { get; init; }
    protected abstract Color Color { get; }
    
    public override void Update(GameTime gameTime)
    {
        if (_movement != Vector2.Zero)
        {
            Position += _movement * PlayerSpeed * (float) gameTime.ElapsedGameTime.TotalSeconds;
            _movement = Vector2.Zero;
        }

        if (_look != Vector2.Zero)
        {
            Aim = _look;
            _look = Vector2.Zero;
        }

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        this.SpriteBatch.Begin();
        this.SpriteBatch.DrawCircle(Position, 10, this.Color);        
        this.SpriteBatch.DrawCircle(Aim, 2, this.Color);
        this.SpriteBatch.End();

        base.Draw(gameTime);
    }

    private void OnSwitchWeapon(object sender, WeaponSlotEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnShoot(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnLook(object sender, Vector2EventArgs e)
    {
        _look = e.Value;
    }

    private void OnMovement(object sender, Vector2EventArgs e)
    {
        _movement = e.Value;
    }
}