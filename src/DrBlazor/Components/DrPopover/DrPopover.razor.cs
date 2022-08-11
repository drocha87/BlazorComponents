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
    [Parameter] public RenderFragment ChildContent { get; set; } = default!;

    [Parameter] public bool Dismissible { get; set; } = false;
    [Parameter] public bool DisableArrow { get; set; } = false;

    [Parameter] public bool FlipToFit { get; set; } = true;
    [Parameter] public Direction Direction { get; set; } = Direction.Top;
    [Parameter] public int Margin { get; set; } = 2;

    public record struct Position(double Top, double Left);

    private Dictionary<string, object> Attributes =>
        new AttrBuilder()
        .AddStyle("position", "absolute")
        .AddStyle("width", "fit-content")
        .AddStyle("visibility", "hidden")
        .AddStyle("top", "0")
        .AddStyle("left", "0")

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
            module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/DrBlazor/Components/DrPopover/popover.js");

            var reference = DotNetObjectReference.Create(this);
            await module.InvokeVoidAsync("initializeWindowResizeObserver", reference);
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    [JSInvokable]
    public async Task WindowResized() => await CalculatePositionOnScreen(Direction);

    public async Task CloseAsync()
    {
        _open = false;
        await InvokeAsync(StateHasChanged);
    }

    private async Task<Position> CalculatePositionOnScreen(Direction dir, bool flipping = false)
    {
        var targetRect = await _target.GetBoundingClientRectAsync(JS);
        var sourceRect = await _ref.GetBoundingClientRectAsync(JS);
        var windowSize = await GetWindowInnerSize();

        switch (dir)
        {
            case Direction.Top:
                {
                    var top = targetRect.Top - sourceRect.Height - Margin;
                    if (top < 0 && FlipToFit && !flipping)
                    {
                        return await CalculatePositionOnScreen(Direction.Bottom, true);
                    }

                    var left = targetRect.Left - (sourceRect.Width - targetRect.Width) / 2;
                    if (left < 0)
                        left = 0;

                    return new Position(top, left);
                }

            case Direction.Right:
                throw new Exception("not implemented yet");

            case Direction.Left:
                throw new Exception("not implemented yet");

            case Direction.Bottom:
                {
                    var top = targetRect.Bottom + Margin;
                    if (top > windowSize.Height && FlipToFit && !flipping)
                    {
                        return await CalculatePositionOnScreen(Direction.Top, true);
                    }

                    var left = targetRect.Left - (sourceRect.Width - targetRect.Width) / 2;
                    if (left < 0)
                        left = 0;

                    return new Position(top, left);
                }
        };
        throw new Exception("cannot place the popover");
    }

    public async Task ShowAsync(ElementReference target)
    {
        _open = true;
        _target = target;
        var position = await CalculatePositionOnScreen(Direction);
        await module!.InvokeVoidAsync("setPosition", _ref, position);
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