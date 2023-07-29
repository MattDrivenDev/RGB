using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGB.Screens;

public class MenuScreenItem : DrawableGameComponent
{
    private readonly SpriteBatch _spriteBatch;
    private readonly MenuScreen _menuScreen;

    public MenuScreenItem(Game game, MenuScreen menuScreen) : base(game)
    {
        _menuScreen = menuScreen;
        _spriteBatch = Game.Services.GetService<SpriteBatch>();        
        Font = Game.Content.Load<SpriteFont>("Fonts/text");
    }

    public string Text { get; set; }
    public Action Action { get; set; }
    public Rectangle Bounds { get; set; }
    public bool Highlighted { get; set; }
    public SpriteFont Font { get; set; }
    public Vector2 Position => new(Bounds.X, Bounds.Y);
    public Color Color => Highlighted ? Color.Red : Color.White;

    public override void Update(GameTime gameTime)
    {
        Highlighted = Bounds.Contains(_menuScreen.Aim);
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();
        _spriteBatch.DrawString(Font, Text, Position, Color);
        _spriteBatch.End();
    }
}