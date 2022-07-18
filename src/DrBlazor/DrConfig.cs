namespace DrBlazor;

public class DrConfig
{
    public string Theme { get; set; } = "light";

    public delegate void ConfigChangedHandler(object sender, DrConfig e);

    public event ConfigChangedHandler? ConfigChanged;

    public void ToggleTheme()
    {
        if (Theme.Equals("light"))
        {
            Theme = "dark";
        }
        else
        {
            Theme = "light";
        }
        ConfigChanged?.Invoke(this, this);
    }
}