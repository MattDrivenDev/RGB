using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.ViewportAdapters;
using RGB.Player;
using RGB.World;

namespace RGB.Screens;

public class GameplayScreen : GameScreen
{
    private WeaponSlot? _weaponSlot;

    public GameplayScreen(Game game) : base(game)
    {
        Settings = Services.GetService<IniFileSettings>();

        Camera = new OrthographicCamera(
            new BoxingViewportAdapter(
                Game.Window,
                GraphicsDevice,
                (int)Settings.Resolution.X,
                (int)Settings.Resolution.Y));
        Services.AddService(Camera);

        Map = new WorldMap(Game, "Maps/RGB01", Camera);  

        R = new R(game, Map);
        G = new G(game, Map);
        B = new B(game, Map);
        Y = new Y(game, Map);
        
        R.Position = Map.GetSpawnPoint("R");
        G.Position = Map.GetSpawnPoint("G");
        B.Position = Map.GetSpawnPoint("B");
        Y.Position = Map.GetSpawnPoint("Y");

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
    public OrthographicCamera Camera { get; private set; }
    public WorldMap Map { get; init; }
    protected IniFileSettings Settings { get; init; }

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
        Map.Draw(gameTime);
        R.Draw(gameTime);
        G.Draw(gameTime);
        B.Draw(gameTime);
        Y.Draw(gameTime);
    }
}