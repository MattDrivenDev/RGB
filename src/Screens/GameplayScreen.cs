using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using RGB.Player;

namespace RGB.Screens;

public class GameplayScreen : GameScreen
{
    private WeaponSlot? _weaponSlot;

    public GameplayScreen(Game game) : base(game)
    {
        Camera = Game.Services.GetService<OrthographicCamera>();
        Map = Content.Load<TiledMap>("Maps/RGB01");
        MapRenderer = new TiledMapRenderer(GraphicsDevice, Map);

        var tiledPlayerLayer = Map.GetLayer<TiledMapObjectLayer>("Spawn");

        R = new R(game);
        G = new G(game);
        B = new B(game);
        Y = new Y(game);

        Game.Components.Add(R);
        Game.Components.Add(G);
        Game.Components.Add(B);
        Game.Components.Add(Y);
        
        R.Position = tiledPlayerLayer.Objects.FirstOrDefault(x => x.Name == "R").Position;
        G.Position = tiledPlayerLayer.Objects.FirstOrDefault(x => x.Name == "G").Position;
        B.Position = tiledPlayerLayer.Objects.FirstOrDefault(x => x.Name == "B").Position;
        Y.Position = tiledPlayerLayer.Objects.FirstOrDefault(x => x.Name == "Y").Position;

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
    public TiledMap Map { get; private set; }
    public TiledMapRenderer MapRenderer { get; private set; }
    public OrthographicCamera Camera { get; private set; }

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
        MapRenderer.Update(gameTime);

        if (_weaponSlot.HasValue)
        {
            Activate(_weaponSlot.Value);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        Game.GraphicsDevice.Clear(Color.DarkGray);

        var viewMatrix = Camera.GetViewMatrix();
        MapRenderer.Draw(viewMatrix: viewMatrix);    
    }
}