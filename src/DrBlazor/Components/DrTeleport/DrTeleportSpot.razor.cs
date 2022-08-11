using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class DrTeleportSpot : DrComponentBase, IDisposable
{
    [Inject] DrTeleportService TeleportService { get; set; } = null!;

    [Parameter] public string Name { get; set; } = null!;

    private IEnumerable<RenderFragment>? _fragments;

    protected override void OnInitialized()
    {
        TeleportService.OnChanged += OnFragmentsChanged;
    }

    protected override async Task OnParametersSetAsync()
    {
        await UpdateFragmentsAsync();
    }

    protected async Task UpdateFragmentsAsync()
    {
        _fragments = TeleportService.GetRenderFragments(Name);
        await InvokeAsync(StateHasChanged);
        Console.WriteLine("diego");
    }

    private async void OnFragmentsChanged()
    {
        await UpdateFragmentsAsync();
    }

    public new void Dispose()
    {
        TeleportService.OnChanged -= OnFragmentsChanged;
        base.Dispose();
    }
}