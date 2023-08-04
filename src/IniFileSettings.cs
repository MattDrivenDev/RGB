using Microsoft.Xna.Framework;
using System;

namespace RGB;

public class IniFileSettings
{
    /// <summary>
    /// Please use the static factory method.
    /// </summary>
    private IniFileSettings() { }

    public Vector2 Resolution { get; private set; }
    public int ScaleFactor { get; private set; }
    public bool Fullscreen { get; private set; }
    public float PlayerSpeed { get; private set; }
    public float PlayerReloadTime { get; private set; }
    public float PlayerSize { get; private set; }

    public delegate void VideoSettingsHandler(object sender, EventArgs e);
    public event VideoSettingsHandler OnVideoSettingsChanged;

    public void ToggleFullscreen()
    {
        Fullscreen = !Fullscreen;
        OnVideoSettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetScreenResolution(Vector2 resolution)
    {
        Resolution = resolution;
        OnVideoSettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetScaleFactor(int scaleFactor)
    {
        ScaleFactor = scaleFactor;
        OnVideoSettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    public static IniFileSettings Create(string path)
    {
        try
        {
            // Read and parse the ini file given the path and set the values
            // of the various settings.
            var parser = new IniFileParser(path);

            var settings = new IniFileSettings
            {
                // Video
                Fullscreen = bool.Parse(parser.GetSetting("Video", "Fullscreen")),
                Resolution = ParseResolution(parser.GetSetting("Video", "Resolution")),
                ScaleFactor = int.Parse(parser.GetSetting("Video", "ScaleFactor")),

                // Player
                PlayerSpeed = float.Parse(parser.GetSetting("Player", "PlayerSpeed")),
                PlayerReloadTime = float.Parse(parser.GetSetting("Player", "PlayerReloadTime")),
                PlayerSize = float.Parse(parser.GetSetting("Player", "PlayerSize"))
            };

            return settings;
        }
        catch (Exception ex)

        {
            // If there is an error, log it and return the default settings.
            Console.WriteLine(ex.Message);

            return new IniFileSettings
            {
                // Video
                Fullscreen = false,
                Resolution = new Vector2(320, 180),

                // Player
                PlayerSpeed = 100f,
                PlayerReloadTime = 0.5f,
                PlayerSize = 10f
            };
        }
    }

    private static Vector2 ParseResolution(string resolution)
    {
        var split = resolution.Split(new[] { 'x', 'X' });
        var x = int.Parse(split[0]);
        var y = int.Parse(split[1]);

        return new Vector2(x, y);
    }
}