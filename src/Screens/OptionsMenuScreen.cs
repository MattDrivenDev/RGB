using System;
using Microsoft.Xna.Framework;

namespace RGB.Screens;

public class OptionsMenuScreen : MenuScreen
{
    public OptionsMenuScreen(Game game) : base(game)
    {
        Settings = Game.Services.GetService<IniFileSettings>();

        AddMenuScreenItem("Fullscreen", Settings.ToggleFullscreen);
        AddMenuScreenItem("Back", () => OnNavigateBack?.Invoke(this, EventArgs.Empty));
    }

    public IniFileSettings Settings { get; private set; }

    public delegate void NavigateBackHandler(object sender, EventArgs e);
    public event NavigateBackHandler OnNavigateBack;

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