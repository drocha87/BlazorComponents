using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DrBlazor;

public struct DOMRect
{
    public double Bottom { get; set; }
    public double Height { get; set; }
    public double Left { get; set; }
    public double Right { get; set; }
    public double Top { get; set; }
    public double Width { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
}

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
            module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/DrBlazor/DrTooltip/tooltip.js");

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