namespace RGB;

public class IniFileSettings
{
    /// <summary>
    /// Please use the static factory method.
    /// </summary>
    private IniFileSettings() { }

    public static IniFileSettings Create(string path)
    {
        // Read and parse the ini file given the path and set the values
        // of the various settings.
        
        var settings = new IniFileSettings
        {
            PlayerSpeed = 100.0f
        };

        return settings;
    }

    public float PlayerSpeed { get; private set; }
}