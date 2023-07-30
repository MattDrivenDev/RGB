using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        Settings = IniFileSettings.Create("RGB.ini");
        Services.AddService(Settings);

        _graphics = new GraphicsDeviceManager(this);
        InitializeGraphics();

        Content.RootDirectory = "Content";
        IsMouseVisible = false;

        _screenManager = new ScreenManager();
        Components.Add(_screenManager);

        Settings.OnVideoSettingsChanged += Settings_OnVideoSettingsChanged;
    }

    public IniFileSettings Settings { get; private set; }

    private void Settings_OnVideoSettingsChanged(object sender, System.EventArgs e)
    {
        InitializeGraphics();
    }

    protected override void Initialize()
    {
        _playerInput = new PlayerInput(this);
        Components.Add(_playerInput);

        // Add required objects to the IoC container
        Services.AddService(_playerInput);

        base.Initialize();

        LoadTitleSreen();
    }

    private void InitializeGraphics()
    {
        _graphics.IsFullScreen = Settings.Fullscreen;
        _graphics.PreferredBackBufferWidth = (int)Settings.ScreenResolution.X;
        _graphics.PreferredBackBufferHeight = (int)Settings.ScreenResolution.Y;
        _graphics.ApplyChanges();

        _camera = new OrthographicCamera(
            new BoxingViewportAdapter(
                Window,
                GraphicsDevice,
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight));
        Services.AddService(_camera);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        // Add required objects to the IoC container
        Services.AddService(_spriteBatch);
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
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
        screen.OnOpenOptions += (sender, args) => LoadOptionsMenuScreen();
        screen.OnExitGame += (sender, args) => Exit();

        _screenManager.LoadScreen(screen, new FadeTransition(GraphicsDevice, Color.Black, 0.5f));
    }

    private void LoadOptionsMenuScreen()
    {
        var screen = new OptionsMenuScreen(this);
        screen.OnNavigateBack += (sender, args) => LoadMainMenuScreen();

        _screenManager.LoadScreen(screen, new FadeTransition(GraphicsDevice, Color.Black, 0.5f));
    }

    private void LoadGameplayScreen()
    {
        var screen = new GameplayScreen(this);

        _screenManager.LoadScreen(screen, new FadeTransition(GraphicsDevice, Color.Black, 0.5f));
    }
}
