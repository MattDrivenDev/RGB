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
    private RenderTarget2D _renderTarget;

    public GameEngine()
    {
        Settings = IniFileSettings.Create("RGB.ini");
        Services.AddService(Settings);

        _graphics = new GraphicsDeviceManager(this);
        InitializeGraphics();

        Content.RootDirectory = "Content";
        IsMouseVisible = false;

        ScreenManager = new ScreenManager();

        Settings.OnVideoSettingsChanged += Settings_OnVideoSettingsChanged;
    }

    public IniFileSettings Settings { get; private set; }
    public ScreenManager ScreenManager { get ; private set; }
    public PlayerInput PlayerInput { get; private set; }

    private void Settings_OnVideoSettingsChanged(object sender, System.EventArgs e)
    {
        InitializeGraphics();
    }

    protected override void Initialize()
    {
        PlayerInput = new PlayerInput(this);

        // Add required objects to the IoC container
        Services.AddService(PlayerInput);

        base.Initialize();

        LoadTitleSreen();
    }

    private void InitializeGraphics()
    {
        var renderWidth = (int)Settings.Resolution.X * Settings.ScaleFactor;
        var renderHeight = (int)Settings.Resolution.Y * Settings.ScaleFactor;

        _graphics.IsFullScreen = Settings.Fullscreen;
        _graphics.PreferredBackBufferWidth = renderWidth;
        _graphics.PreferredBackBufferHeight = renderHeight;
        _graphics.ApplyChanges();

        _renderTarget = new RenderTarget2D(
            GraphicsDevice,
            (int)Settings.Resolution.X,
            (int)Settings.Resolution.Y,
            false,
            SurfaceFormat.Color,
            DepthFormat.Depth24Stencil8);
    }

    protected override void LoadContent()
    {

    }

    protected override void Update(GameTime gameTime)
    {
        PlayerInput.Update(gameTime);
        ScreenManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Write the fps to the window title
        Window.Title = $"RGB - FPS: {1 / gameTime.ElapsedGameTime.TotalSeconds}";
        
        GraphicsDevice.SetRenderTarget(_renderTarget);
        GraphicsDevice.Clear(Color.Black);
        ScreenManager.Draw(gameTime);
        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);

        var spriteBatch = new SpriteBatch(GraphicsDevice);  

        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullCounterClockwise);

        spriteBatch.Draw(
            _renderTarget, 
            new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) / 2,
            null,
            Color.White, 
            0f,
            new Vector2(_renderTarget.Width, _renderTarget.Height) / 2,
            Settings.ScaleFactor,
            SpriteEffects.None,
            1);

        spriteBatch.End();

        base.Draw(gameTime);
    }

    private void LoadTitleSreen()
    {
        var screen = new TitleScreen(this);
        screen.OnNavigateForward += (sender, args) => LoadMainMenuScreen();
        screen.OnNavigateBack += (sender, args) => Exit();

        ScreenManager.LoadScreen(screen, new FadeTransition(GraphicsDevice, Color.Black, 0.5f));
    }

    private void LoadMainMenuScreen()
    {
        var screen = new MainMenuScreen(this);
        screen.OnNewGame += (sender, args) => LoadGameplayScreen();
        screen.OnOpenOptions += (sender, args) => LoadOptionsMenuScreen();
        screen.OnExitGame += (sender, args) => Exit();

        ScreenManager.LoadScreen(screen, new FadeTransition(GraphicsDevice, Color.Black, 0.5f));
    }

    private void LoadOptionsMenuScreen()
    {
        var screen = new OptionsMenuScreen(this);
        screen.OnNavigateBack += (sender, args) => LoadMainMenuScreen();

        ScreenManager.LoadScreen(screen, new FadeTransition(GraphicsDevice, Color.Black, 0.5f));
    }

    private void LoadGameplayScreen()
    {
        var screen = new GameplayScreen(this);

        ScreenManager.LoadScreen(screen, new FadeTransition(GraphicsDevice, Color.Black, 0.5f));
    }
}
