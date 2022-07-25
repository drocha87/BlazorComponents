using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class DrMainContent : DrComponentBase
{
    [Inject] DrLayoutService LayoutService { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    private Dictionary<string, object> Attributes =>
        new AttrBuilder()
            .AddClass("dr-main-content")
            .AddStyle("height", $"calc(100vh - {LayoutService.TopBarAppHeight}px)")
            .Build();
}