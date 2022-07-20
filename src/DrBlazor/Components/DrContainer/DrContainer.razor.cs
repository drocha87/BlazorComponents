using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class DrContainer : DrComponentBase
{
    [Parameter] public RenderFragment ChildContent { get; set; } = null!;

    public Dictionary<string, object> Attributes =>
        new AttrBuilder()
            // css classes
            .AddClass("dr-container")
            .AddClass("shape-medium")
            .AddClass("surface")
            .AddClasses(Class ?? "")
            // styles
            .AddStyle("padding", "24px")
            .AddStyles(Style ?? "")
            // attributes
            .AddAttributes(UserAttributes)
            .Build();
}