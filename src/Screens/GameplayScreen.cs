using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using RGB.Player;
using RGB.World;

namespace RGB.Screens;

public class GameplayScreen : GameScreen
{
    private WeaponSlot? _weaponSlot;

    public GameplayScreen(Game game) : base(game)
    {
        Camera = Game.Services.GetService<OrthographicCamera>();

        Map = new WorldMap(Game, "Maps/RGB01", Camera);  
        Game.Components.Add(Map);

        R = new R(game, Map);
        G = new G(game, Map);
        B = new B(game, Map);
        Y = new Y(game, Map);

        Game.Components.Add(R);
        Game.Components.Add(G);
        Game.Components.Add(B);
        Game.Components.Add(Y);
        
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
    }

    public override void Draw(GameTime gameTime)
    {
        Game.GraphicsDevice.Clear(Color.DarkGray);
    }
}