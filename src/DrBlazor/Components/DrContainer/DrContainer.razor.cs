using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class DrContainer : DrComponentBase
{
    [Parameter] public RenderFragment ChildContent { get; set; } = null!;
    [Parameter] public int ElevationLevel { get; set; } = 0;

    public Dictionary<string, object> Attributes =>
        new AttrBuilder()
            // css classes
            .AddClass("dr-container")
            .AddClass("shape-medium")
            .AddClass("primary-container")
            .AddClass($"dr-elevation-{ElevationLevel}")
            .AddClasses(Class ?? "")
            // styles
            .AddStyle("padding", "24px")
            // FIXME: it should only be set if the container is fluid
            .AddStyle("height", "100%")
            .AddStyles(Style ?? "")
            // data attributes
            .AddData("id", Id, when: Id is not null)
            // attributes
            .AddAttributes(UserAttributes)
            .Build();
}