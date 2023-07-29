using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Timers;

namespace RGB.Player;

/// <summary>
/// Base class for player characters: R, G, B, and Y.
/// </summary>
public abstract class PlayerCharacter : DrawableGameComponent
{
    private Vector2 _movement;
    private Vector2 _look;
    private bool _hasActivated;
    private PlayerCharacter _previous;
    private bool _shoot;
    private float _timeSinceLastShot;

    protected PlayerCharacter(Game game) : base(game)
    {
        PlayerInput = Game.Services.GetService<PlayerInput>();
        PlayerInput.OnMovement += OnMovement;
        PlayerInput.OnLook += OnLook;
        PlayerInput.OnShoot += OnShoot;
        PlayerInput.OnSwitchWeapon += OnSwitchWeapon;

        var settings = Game.Services.GetService<IniFileSettings>();
        PlayerSpeed = settings.PlayerSpeed;
        PlayerReloadTime = settings.PlayerReloadTime;

        SpriteBatch = Game.Services.GetService<SpriteBatch>();
        Camera = Game.Services.GetService<OrthographicCamera>();

        Position = Vector2.Zero;
    }

    public delegate void SwitchWeaponHandler(object sender, WeaponSlotEventArgs e);
    public event SwitchWeaponHandler SwitchWeapon;

    protected virtual float PlayerSpeed { get; init; }
    protected virtual float PlayerReloadTime { get; init; }
    protected PlayerInput PlayerInput { get; init; }
    public Vector2 Position { get; set; }
    protected Vector2 Aim { get; set; }
    protected SpriteBatch SpriteBatch { get; init; }
    protected abstract Color Color { get; }
    public bool IsActive => Enabled;
    protected OrthographicCamera Camera { get; init; }

    public void Activate(PlayerCharacter previous)
    {
        _previous = previous;
        _hasActivated = true;
        Enabled = true;
    }

    public void Deactivate()
    {
        Enabled = false;
    }
    
    public override void Update(GameTime gameTime)
    {
        if (!IsActive)
        {
            return;
        }

        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var totalTime = (float)gameTime.TotalGameTime.TotalSeconds;

        // It would be kind of nice to have the camera follow the player 
        // with a sort of delay, like in the original GTA games. But this
        // will do for now.
        Camera.LookAt(Position);

        Move(deltaTime);

        Look();

        Shoot(totalTime);

        base.Update(gameTime);
    }

    private void Move(float deltaTime)
    {
        if (_movement != Vector2.Zero)
        {
            Position += _movement * PlayerSpeed * deltaTime;
            _movement = Vector2.Zero;
        }
    }

    private void Look()
    {
        if (_look != Vector2.Zero)
        {
            Aim = _look;
            _look = Vector2.Zero;
        }
    }

    private void Shoot(float totalTime)
    {
        if (_shoot && totalTime - _timeSinceLastShot > PlayerReloadTime)
        {
            // Calculate the angle in degrees between the players position and the aim position.
            var angleInRadians = MathF.Atan2(Aim.Y - Position.Y, Aim.X - Position.X);
            var angleInDegrees = MathHelper.ToDegrees(angleInRadians);
            
            var projectile = new Projectile(Game, Position, angleInDegrees, 10);
            Game.Components.Add(projectile);

            // Reset the shoot flag.
            _shoot = false;

            // Set the last shot time to now.
            _timeSinceLastShot = totalTime;
        }
    }

    public override void Draw(GameTime gameTime)
    {
        var viewMatrix = Camera.GetViewMatrix();
        this.SpriteBatch.Begin(transformMatrix: viewMatrix);
        this.SpriteBatch.DrawCircle(Position, 10, this.Color);       

        // Only render the aim if the player is active.
        if (IsActive)
        {
            this.SpriteBatch.DrawCross(Aim, 3, this.Color);
        }

        if (_hasActivated)
        {
            this.SpriteBatch.DrawCircle(Position, 12, Color.White);
            _hasActivated = false;

            if (_previous != null)
            {
                this.SpriteBatch.DrawLine(_previous.Position, Position, Color.White, 2);
                _previous = null;
            }
        }
        
        this.SpriteBatch.End();

        base.Draw(gameTime);
    }

    private void OnSwitchWeapon(object sender, WeaponSlotEventArgs e)
    {
        if (IsActive)
        {
            SwitchWeapon?.Invoke(this, e);
        }
    }

    private void OnShoot(object sender, EventArgs e)
    {
        if (IsActive)
        {            
            _shoot = true;
        }
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