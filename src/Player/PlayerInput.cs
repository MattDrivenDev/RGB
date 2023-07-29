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

    public delegate void EscapeOrMenuHandler(object sender, EventArgs e);
    
    public event MovementHandler OnMovement;
    
    public event LookHandler OnLook;

    public event ShootHandler OnShoot;

    public event SwitchWeaponHandler OnSwitchWeapon;

    public event EscapeOrMenuHandler OnEscapeOrMenu;

    public override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        var movement = GetMovement(keyboardState);
        if (movement != Vector2.Zero)
        {
            OnMovement?.Invoke(this, new Vector2EventArgs(movement));
        }

        if (EscapeOrMenu(keyboardState))
        {
            OnEscapeOrMenu?.Invoke(this, EventArgs.Empty);
        }
        
        var mouseState = Mouse.GetState();
        var look = new Vector2(mouseState.X, mouseState.Y);
        if (look != Vector2.Zero)
        {
            OnLook?.Invoke(this, new Vector2EventArgs(look));
        }

        if (Shooting(mouseState))
        {
            OnShoot?.Invoke(this, EventArgs.Empty);
        }

        if (SwitchingWeapons(keyboardState, out var weaponSlot))
        {
            OnSwitchWeapon?.Invoke(this, new WeaponSlotEventArgs(weaponSlot));
        }

        base.Update(gameTime);
    }

    private bool SwitchingWeapons(KeyboardState keyboardState, out WeaponSlot weaponSlot)
    {
        weaponSlot = WeaponSlot.One;
        if (keyboardState.IsKeyDown(Keys.D1))
        {
            weaponSlot = WeaponSlot.One;
            return true;
        }

        if (keyboardState.IsKeyDown(Keys.D2))
        {
            weaponSlot = WeaponSlot.Two;
            return true;
        }

        if (keyboardState.IsKeyDown(Keys.D3))
        {
            weaponSlot = WeaponSlot.Three;
            return true;
        }

        if (keyboardState.IsKeyDown(Keys.D4))
        {
            weaponSlot = WeaponSlot.Four;
            return true;
        }

        return false;
    }

    private bool Shooting(MouseState mouseState)
    {
        return mouseState.LeftButton == ButtonState.Pressed;
    }

    private bool EscapeOrMenu(KeyboardState keyboardState)
    {
        return keyboardState.IsKeyDown(Keys.Escape);
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