using System;
using Microsoft.Xna.Framework;

namespace RGB.Screens;

public class MainMenuScreen : MenuScreen
{
    public MainMenuScreen(Game game) : base(game)
    {
        AddMenuScreenItem("New Game", () => OnNewGame?.Invoke(this, EventArgs.Empty));
        AddMenuScreenItem("Options", () => OnOpenOptions?.Invoke(this, EventArgs.Empty));
        AddMenuScreenItem("Exit", () => OnExitGame?.Invoke(this, EventArgs.Empty));
    }
    
    public delegate void NewGameHandler(object sender, EventArgs e);
    public delegate void OpenOptionsHandler(object sender, EventArgs e);
    public delegate void ExitGameHandler(object sender, EventArgs e);
    public event NewGameHandler OnNewGame;
    public event OpenOptionsHandler OnOpenOptions;
    public event ExitGameHandler OnExitGame;

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