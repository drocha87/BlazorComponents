using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DrBlazor;

public partial class DrTeleport : DrComponentBase, IAsyncDisposable
{
    [Inject] IJSRuntime JS { get; set; } = null!;

    [Parameter] public string To { get; set; } = null!;
    [Parameter] public bool Disabled { get; set; } = false;

    [Parameter] public RenderFragment ChildContent { get; set; } = null!;

    private ElementReference _ref;

    private IJSObjectReference? _module;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/DrBlazor/DrTeleport/teleport.js");
            await _module.InvokeVoidAsync("teleport", _ref, To);
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_module is not null)
            {
                await _module.DisposeAsync();
            }
        }
        catch (JSDisconnectedException) { }
        catch (TaskCanceledException) { }
    }
}