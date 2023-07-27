using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using RGB.Player;

namespace RGB.Screens;

public class GameplayScreen : GameScreen
{
    private WeaponSlot? _weaponSlot;

    public GameplayScreen(Game game) : base(game)
    {
        R = new R(game);
        G = new G(game);
        B = new B(game);
        Y = new Y(game);
        
        R.Position = new Vector2(100, 100);
        G.Position = new Vector2(200, 100);
        B.Position = new Vector2(100, 200);
        Y.Position = new Vector2(200, 200);

        Activate(R);

        R.SwitchWeapon += (sender, args) => QueueActivation(args.Value);
        G.SwitchWeapon += (sender, args) => QueueActivation(args.Value);
        B.SwitchWeapon += (sender, args) => QueueActivation(args.Value);
        Y.SwitchWeapon += (sender, args) => QueueActivation(args.Value);
    }

    public PlayerCharacter R { get; private init; }
    public PlayerCharacter G { get; private init; }
    public PlayerCharacter B { get; private init; }
    public PlayerCharacter Y { get; private init; } 
    public bool IsActive { get; set; } = true;

    private void QueueActivation(WeaponSlot weaponSlot)
    {
        _weaponSlot = weaponSlot;
    }

    private void Activate(WeaponSlot weaponSlot)
    {
        var character = weaponSlot switch
        {
            WeaponSlot.One => R,
            WeaponSlot.Two => G,
            WeaponSlot.Three => B,
            WeaponSlot.Four => Y,
            _ => throw new ArgumentOutOfRangeException(nameof(weaponSlot), weaponSlot, null)
        };

        Activate(character);
    }

    private void Activate(PlayerCharacter character)
    {
        // Select the previous character if this is the first activation.
        var all = new[] {R, G, B, Y};
        var previous = all.FirstOrDefault(c => c.IsActive) ?? character;

        R.Deactivate();
        G.Deactivate();
        B.Deactivate();
        Y.Deactivate();
        
        character.Activate(previous);
        _weaponSlot = null;
    }

    public override void Update(GameTime gameTime)
    {
        if (_weaponSlot.HasValue)
        {
            Activate(_weaponSlot.Value);
        }

        R.Update(gameTime);
        G.Update(gameTime);
        B.Update(gameTime);
        Y.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Game.GraphicsDevice.Clear(Color.DarkGray);
        
        R.Draw(gameTime);
        G.Draw(gameTime);
        B.Draw(gameTime);
        Y.Draw(gameTime);        
    }
}