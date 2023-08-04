using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using RGB.Player;

namespace RGB.Screens;

public abstract class MenuScreen : GameScreen
{
    private Vector2 _look;

    protected MenuScreen(Game game) : base(game)
    {
        PlayerInput = Game.Services.GetService<PlayerInput>();
        Settings = Game.Services.GetService<IniFileSettings>();

        PlayerInput.OnLook += OnLook;
        PlayerInput.OnShoot += OnShoot;
    }

    public Vector2 Aim { get; private set; }
    protected PlayerInput PlayerInput { get; init; }
    protected List<MenuScreenItem> Items { get; } = new();
    public bool IsActive { get; private set; } = true;
    protected IniFileSettings Settings { get; private set; }

    protected void AddMenuScreenItem(
        string text,
        Action action)
    {
        var item = new MenuScreenItem(Game, this)
        {
            Text = text,
            Action = action,
            Bounds = new Rectangle(0, 0, 10,10)
        };

        var stringSize = item.Font.MeasureString(item.Text);

        item.Bounds = new Rectangle(
            GraphicsDevice.Viewport.X + (int)(Settings.Resolution.X / 6),
            GraphicsDevice.Viewport.Y + (int)(Settings.Resolution.Y / 6) + (int)(Items.Count * stringSize.Y),
            (int)stringSize.X,
            (int)stringSize.Y);

        Items.Add(item);
    }

    public override void Update(GameTime gameTime)
    {
        if (_look != Vector2.Zero)
        {
            Aim = _look / Settings.ScaleFactor;
            _look = Vector2.Zero;
        }

        foreach (var item in Items)
        {
            item.Update(gameTime);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        var spriteBatch = new SpriteBatch(Game.GraphicsDevice);

        spriteBatch.Begin();   
        spriteBatch.DrawCircle(Aim, 2, Color.Red);
        spriteBatch.End();

        foreach (var item in Items)
        {   
            item.Draw(gameTime);
        }
    }

    private void OnShoot(object sender, EventArgs e)
    {
        if (IsActive)
        {
            foreach (var item in Items)
            {
                if (item.Highlighted)
                {
                    item.Action();
                    IsActive = false;
                }
            }
        }
    }

    private void OnLook(object sender, Vector2EventArgs e)
    {
        _look = e.Value;
    }
}