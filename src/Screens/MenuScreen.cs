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
        SpriteBatch = Game.Services.GetService<SpriteBatch>();

        PlayerInput.OnLook += OnLook;
        PlayerInput.OnShoot += OnShoot;
    }

    public Vector2 Aim { get; private set; }
    protected SpriteBatch SpriteBatch { get; init; }
    protected PlayerInput PlayerInput { get; init; }
    protected List<MenuScreenItem> Items { get; } = new();
    public bool IsActive { get; private set; } = true;

    protected void AddMenuScreenItem(
        string text,
        Action action)
    {
        var item = new MenuScreenItem(Game, this)
        {
            Text = text,
            Action = action
        };

        var stringSize = item.Font.MeasureString(item.Text);

        item.Bounds = new Rectangle(
            100,
            100 + (int)(Items.Count * stringSize.Y),
            (int)stringSize.X,
            (int)stringSize.Y);

        Items.Add(item);
    }

    public override void Update(GameTime gameTime)
    {
        if (_look != Vector2.Zero)
        {
            Aim = _look;
            _look = Vector2.Zero;
        }

        foreach (var item in Items)
        {
            item.Update(gameTime);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        this.SpriteBatch.Begin();   
        this.SpriteBatch.DrawCircle(Aim, 2, Color.Red);
        this.SpriteBatch.End();

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