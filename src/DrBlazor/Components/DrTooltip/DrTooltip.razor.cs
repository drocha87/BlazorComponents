using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DrBlazor;

public partial class DrTooltip : DrComponentBase, IAsyncDisposable
{
    [Inject] IJSRuntime JS { get; set; } = null!;

    [Parameter] public RenderFragment ChildContent { get; set; } = null!;
    [Parameter] public RenderFragment DrTooltipContent { get; set; } = null!;

    private ElementReference _ref;
    private bool _open = false;

    IJSObjectReference? module;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/DrBlazor/DrTooltip/tooltip.js");

            var reference = DotNetObjectReference.Create(this);
            await module.InvokeVoidAsync("observeMouse", reference, _ref);
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    [JSInvokable]
    public async Task MouseEnter()
    {
        _open = true;
        await InvokeAsync(StateHasChanged);
    }

    [JSInvokable]
    public async Task MouseLeave()
    {
        _open = false;
        await InvokeAsync(StateHasChanged);
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (module is not null)
            {
                await module.DisposeAsync();
            }
        }
        catch (JSDisconnectedException) { }
        catch (TaskCanceledException) { }
    }
}