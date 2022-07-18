using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class DrContainer : DrComponentBase
{
    [Parameter] public RenderFragment ChildContent { get; set; } = null!;

    public Dictionary<string, object> Attributes =>
        new AttrBuilder()
            .AddClass("dr-container")
            .AddClass($"dr-container-{Config.Theme}")
            .AddClasses(Class ?? "")
            .AddStyles(Style ?? "")
            .AddAttributes(UserAttributes)
            .Build();
}