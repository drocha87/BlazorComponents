using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DrBlazor;

public partial class DrTooltip : DrComponentBase, IAsyncDisposable
{
    [Inject] IJSRuntime JS { get; set; } = null!;

    [Parameter] public RenderFragment ChildContent { get; set; } = null!;
    [Parameter] public RenderFragment DrTooltipContent { get; set; } = null!;

    private DrPopover? _popover;
    private ElementReference _ref;

    IJSObjectReference? module;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/DrBlazor/Components/DrTooltip/tooltip.js");

            var reference = DotNetObjectReference.Create(this);
            await module.InvokeVoidAsync("observeMouse", reference, _ref);
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    [JSInvokable]
    public async Task MouseEnter(DOMRect rect)
    {
        if (_popover is not null)
        {
            await _popover.ShowAsync(_ref);
        }
    }

    [JSInvokable]
    public async Task MouseLeave()
    {
        if (_popover is not null)
        {
            await _popover.CloseAsync();
        }
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