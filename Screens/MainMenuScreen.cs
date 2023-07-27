using System;
using Microsoft.Xna.Framework;

namespace RGB.Screens;

public class MainMenuScreen : MenuScreen
{
    public MainMenuScreen(Game game) : base(game)
    {
        AddMenuScreenItem("New Game", () => OnNewGame?.Invoke(this, EventArgs.Empty));
    }
    
    public delegate void NewGameHandler(object sender, EventArgs e);
    public event NewGameHandler OnNewGame;

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);        
    }

    public override void Draw(GameTime gameTime)
    {
        Game.GraphicsDevice.Clear(Color.Green);
        base.Draw(gameTime);        
    }
}