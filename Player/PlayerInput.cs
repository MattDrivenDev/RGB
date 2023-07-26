using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RGB.Player;

/// <summary>
/// Encapsulates keyboard, mouse, and gamepad input for the player character(s)
/// in the Gameplay screen(s).
/// </summary>
public class PlayerInput : GameComponent
{
    public PlayerInput(Game game) : base(game)
    {
        
    }
    
    public delegate void MovementHandler(object sender, Vector2EventArgs e);
    
    public delegate void LookHandler(object sender, Vector2EventArgs e);

    public delegate void ShootHandler(object sender, EventArgs e);

    public delegate void SwitchWeaponHandler(object sender, WeaponSlotEventArgs e);
    
    public event MovementHandler OnMovement;
    
    public event LookHandler OnLook;

    public event ShootHandler OnShoot;

    public event SwitchWeaponHandler OnSwitchWeapon;

    public override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        var movement = GetMovement(keyboardState);
        if (movement != Vector2.Zero)
        {
            OnMovement?.Invoke(this, new Vector2EventArgs(movement));
        }
        
        var mouseState = Mouse.GetState();
        var look = new Vector2(mouseState.X, mouseState.Y);
        if (look != Vector2.Zero)
        {
            OnLook?.Invoke(this, new Vector2EventArgs(look));
        }

        base.Update(gameTime);
    }

    private Vector2 GetMovement(KeyboardState keyboardState)
    {
        var movement = Vector2.Zero;
        if (keyboardState.IsKeyDown(Keys.W))
        {
            movement.Y -= 1;
        }

        if (keyboardState.IsKeyDown(Keys.S))
        {
            movement.Y += 1;
        }

        if (keyboardState.IsKeyDown(Keys.A))
        {
            movement.X -= 1;
        }

        if (keyboardState.IsKeyDown(Keys.D))
        {
            movement.X += 1;
        }

        return movement;
    }
}