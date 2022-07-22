using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DrBlazor;

public enum Direction
{
    Top,
    Right,
    Left,
    Bottom,
}

public partial class DrPopover : DrComponentBase, IAsyncDisposable
{
    [Inject] IJSRuntime JS { get; set; } = null!;

    [Parameter] public RenderFragment ChildContent { get; set; } = default!;

    [Parameter] public bool Dismissible { get; set; } = false;
    [Parameter] public bool DisableArrow { get; set; } = false;

    [Parameter] public bool FlipToFit { get; set; } = true;
    [Parameter] public Direction Direction { get; set; } = Direction.Top;
    [Parameter] public int Margin { get; set; } = 2;


    private Dictionary<string, object> Attributes =>
        new AttrBuilder()
        .AddStyle("position", "absolute")
        .AddStyle("width", "fit-content")

        .AddData("data-dr-popover-arrow", !DisableArrow)
        .Build();

    private ElementReference _ref;
    private ElementReference _target;

    private bool _open = false;

    public bool IsVisible => _open;

    IJSObjectReference? module;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/DrBlazor/DrPopover/popover.js");

            var reference = DotNetObjectReference.Create(this);
            await module.InvokeVoidAsync("initializeWindowResizeObserver", reference);
        }
        await Redraw();
        await base.OnAfterRenderAsync(firstRender);
    }

    public async Task Redraw()
    {
        if (_open)
        {
            try
            {
                await module!.InvokeVoidAsync(
                    "updatePosition",
                    _ref,
                    _target,
                    Direction,
                    FlipToFit,
                    Margin);
            }
            catch (JSException) { }
        }
    }

    [JSInvokable]
    public async Task WindowResized() => await Redraw();

    public async Task CloseAsync()
    {
        _open = false;
        await InvokeAsync(StateHasChanged);
    }

    public async Task ShowAsync(ElementReference target)
    {
        _open = true;
        _target = target;
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
            await CloseAsync();
        }
        catch (JSDisconnectedException) { }
        catch (TaskCanceledException) { }

        GC.SuppressFinalize(this);
    }
}