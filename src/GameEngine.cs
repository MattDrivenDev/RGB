using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.ViewportAdapters;
using RGB.Player;
using RGB.Screens;

namespace RGB;

public class GameEngine : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private PlayerInput _playerInput;
    private ScreenManager _screenManager;
    private OrthographicCamera _camera;

    public GameEngine()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;

        _screenManager = new ScreenManager();
        Components.Add(_screenManager);
    }

    protected override void Initialize()
    {
        _camera = new OrthographicCamera(
            new BoxingViewportAdapter(
                Window,
                GraphicsDevice,
                800, 480));

        // TODO: Add your initialization logic here
        _playerInput = new PlayerInput(this);

        // Add required objects to the IoC container
        Services.AddService(_playerInput);
        Services.AddService(_camera);

        base.Initialize();

        LoadTitleSreen();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        var settings = IniFileSettings.Create("RBG.ini");

        // Add required objects to the IoC container
        Services.AddService(_spriteBatch);
        Services.AddService(settings);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _playerInput.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // TODO: Add your drawing code here

        // Write the fps to the window title
        Window.Title = $"RGB - FPS: {1 / gameTime.ElapsedGameTime.TotalSeconds}";

        base.Draw(gameTime);
    }

    private void LoadTitleSreen()
    {
        var screen = new TitleScreen(this);
        screen.OnNavigateForward += (sender, args) => LoadMainMenuScreen();
        screen.OnNavigateBack += (sender, args) => Exit();

        _screenManager.LoadScreen(screen, new FadeTransition(GraphicsDevice, Color.Black, 0.5f));
    }

    private void LoadMainMenuScreen()
    {
        var screen = new MainMenuScreen(this);
        screen.OnNewGame += (sender, args) => LoadGameplayScreen();

        _screenManager.LoadScreen(screen, new FadeTransition(GraphicsDevice, Color.Black, 0.5f));
    }

    private void LoadGameplayScreen()
    {
        var screen = new GameplayScreen(this);

        _screenManager.LoadScreen(screen, new FadeTransition(GraphicsDevice, Color.Black, 0.5f));
    }
}
