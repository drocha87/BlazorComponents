using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public enum DrawerType
{
    [Description("standard")] Standard,
    [Description("modal")] Modal,
}

public partial class DrDrawer : DrComponentBase
{
    [Inject] DrLayoutService LayoutService { get; set; } = null!;

    [Parameter] public bool Visible { get; set; } = false;
    [Parameter] public EventCallback<bool> VisibleChanged { get; set; }

    private Dictionary<string, object> Attributes =>
            new AttrBuilder()
                .AddClass("dr-drawer")
                .AddStyle("height", $"calc(100vh - {LayoutService.TopBarAppHeight}px)")
                .Build();

    protected override void OnInitialized()
    {
        LayoutService.HasDrawer = true;
    }

    public async Task ToggleVisibility()
    {
        Visible = !Visible;
        await VisibleChanged.InvokeAsync(Visible);
    }
}