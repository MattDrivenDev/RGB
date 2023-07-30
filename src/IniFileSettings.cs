namespace RGB;

public class IniFileSettings
{
    /// <summary>
    /// Please use the static factory method.
    /// </summary>
    private IniFileSettings() { }

    public bool Fullscreen { get; private set; }
    public float PlayerSpeed { get; private set; }
    public float PlayerReloadTime { get; private set; }

    public static IniFileSettings Create(string path)
    {
        // Read and parse the ini file given the path and set the values
        // of the various settings.
        var parser = new IniFileParser(path);

        var settings = new IniFileSettings
        {
            // Video
            Fullscreen = bool.Parse(parser.GetSetting("Video", "Fullscreen")),

            // Player
            PlayerSpeed = float.Parse(parser.GetSetting("Player", "PlayerSpeed")),
            PlayerReloadTime = float.Parse(parser.GetSetting("Player", "PlayerReloadTime"))
        };

        return settings;
    }
}