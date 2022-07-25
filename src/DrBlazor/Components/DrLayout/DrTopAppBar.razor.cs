using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DrBlazor;

public enum TopAppBarStyle
{
    [Description("center-aligned")] CenterAligned, // height: 64px
    [Description("small")] Small, // height: 64px
    [Description("medium")] Medium, // height: 112px
    [Description("large")] Large, // height: 152px
}

public partial class DrTopAppBar : DrComponentBase
{
    [Inject] DrLayoutService LayoutService { get; set; } = null!;

    [Parameter] public TopAppBarStyle BarStyle { get; set; } = TopAppBarStyle.Medium;

    [Parameter] public string LeadingIcon { get; set; } = "arrow_back";
    [Parameter] public string? LeadingIconActivated { get; set; }

    [Parameter] public bool DrawerOpen { get; set; } = false;
    [Parameter] public EventCallback<bool> DrawerOpenChanged { get; set; }

    private Dictionary<string, object> Attributes =>
            new AttrBuilder()
                .AddClass("dr-topappbar")
                .AddClass($"dr-topappbar-{BarStyle.ToDescriptionString()}")
                .Build();

    protected override void OnInitialized()
    {
        LayoutService.HasTopBarApp = true;
        // TODO: set the size of the toolbar on the layout service
        LayoutService.TopBarAppHeight = 64;
    }

    public async Task ToggleDrawerAsync(MouseEventArgs ev)
    {
        DrawerOpen = !DrawerOpen;
        await DrawerOpenChanged.InvokeAsync(DrawerOpen);
    }
}