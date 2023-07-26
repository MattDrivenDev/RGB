using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RGB.Player;

namespace RGB;

public class GameEngine : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // To test
    private PlayerInput _playerInput;
    private PlayerCharacter _playerCharacter;

    public GameEngine()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _playerInput = new PlayerInput(this);

        // Add required objects to the IoC container
        Services.AddService(_playerInput);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        var settings = IniFileSettings.Create("RBG.ini");

        // Add required objects to the IoC container
        Services.AddService(_spriteBatch);
        Services.AddService(settings);

        _playerCharacter = new R(this);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _playerInput.Update(gameTime);
        _playerCharacter.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkGray);

        // TODO: Add your drawing code here
        _playerCharacter.Draw(gameTime);

        // Write the fps to the window title
        Window.Title = $"RGB - FPS: {1 / gameTime.ElapsedGameTime.TotalSeconds}";

        base.Draw(gameTime);
    }
}
