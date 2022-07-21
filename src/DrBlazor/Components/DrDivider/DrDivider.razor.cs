namespace DrBlazor;

public partial class DrDivider : DrComponentBase
{
    private Dictionary<string, object> Attributes =>
        new AttrBuilder()
            .AddStyle("width", "100%")
            .AddStyle("height", "1px")
            .AddStyle("background-color", "var(--md-ref-palette-neutral-variant20)")
            .AddStyle("opacity", "32%")
            .Build();
}