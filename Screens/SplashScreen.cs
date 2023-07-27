using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using RGB.Player;

namespace RGB.Screens;

public abstract class SplashScreen : GameScreen
{
    private Vector2 _look;
    
    protected SplashScreen(Game game) : base(game)
    {
        PlayerInput = Game.Services.GetService<PlayerInput>();
        SpriteBatch = Game.Services.GetService<SpriteBatch>();

        PlayerInput.OnLook += OnLook;
        PlayerInput.OnShoot += OnShoot;           
        PlayerInput.OnEscapeOrMenu += OnEscapeOrMenu;
    }

    public delegate void NavigateBackHandler(object sender, EventArgs e);
    public delegate void NavigateForwardHandler(object sender, EventArgs e);
    public event NavigateBackHandler OnNavigateBack;
    public event NavigateForwardHandler OnNavigateForward;

    public Vector2 Aim { get; private set; }
    protected SpriteBatch SpriteBatch { get; init; }
    protected PlayerInput PlayerInput { get; init; }
    public bool IsActive { get; private set; } = true;

    public override void Update(GameTime gameTime)
    {
        if (_look != Vector2.Zero)
        {
            Aim = _look;
            _look = Vector2.Zero;
        }
    }

    public override void Draw(GameTime gameTime)
    {
        this.SpriteBatch.Begin();   
        this.SpriteBatch.DrawCircle(Aim, 2, Color.Red);
        this.SpriteBatch.End();
    }

    private void OnEscapeOrMenu(object sender, EventArgs e)
    {
        OnNavigateBack?.Invoke(this, EventArgs.Empty);
        IsActive = false;
    }

    private void OnShoot(object sender, EventArgs e)
    {
        if (IsActive)
        {
            OnNavigateForward?.Invoke(this, EventArgs.Empty);
            IsActive = false;
        }
    }

    private void OnLook(object sender, Vector2EventArgs e)
    {
        _look = e.Value;
    }

    public override void Dispose()
    {
        PlayerInput.OnLook -= OnLook;
        PlayerInput.OnShoot -= OnShoot;
        PlayerInput.OnEscapeOrMenu -= OnEscapeOrMenu;
        base.Dispose();
    }
}