using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class DrIcon : DrComponentBase
{
    [Parameter] public string Icon { get; set; } = default!;
    [Parameter] public int Size { get; set; } = 24;

    private Dictionary<string, object> Attributes =>
       new AttrBuilder()
            .AddClass("material-symbols-outlined")
            .AddClass("dr-icon")
            .AddStyle("font-size", $"{Size}px")
            .Build();
}