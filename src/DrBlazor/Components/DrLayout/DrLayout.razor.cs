using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class DrLayout : DrComponentBase, IDisposable
{
    [Inject] DrLayoutService LayoutService { get; set; } = null!;
    [Inject] DrTeleportService TeleportService { get; set; } = null!;

    [Parameter] public DrawerType Drawer { get; set; } = DrawerType.Standard;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    private Dictionary<string, object> Attributes =>
        new AttrBuilder()
            .AddClass("dr-layout")
            .Build();

    protected override void OnInitialized()
    {
        TeleportService.OnChanged += OnTeleportFragmentsChanged;
    }

    private async void OnTeleportFragmentsChanged()
    {
        await InvokeAsync(StateHasChanged);
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            TeleportService.HasChanged();
        }
    }

    public new void Dispose()
    {
        TeleportService.OnChanged -= OnTeleportFragmentsChanged;
        base.Dispose();
    }
}