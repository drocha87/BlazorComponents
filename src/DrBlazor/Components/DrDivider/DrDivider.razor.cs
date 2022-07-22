using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class DrDivider : DrComponentBase
{
    [Parameter] public bool Vertical { get; set; } = false;
    private Dictionary<string, object> Attributes =>
        new AttrBuilder()
            .AddClass("dr-divider-horizontal", !Vertical)
            .AddClass("dr-divider-vertical", Vertical)
            .Build();
}