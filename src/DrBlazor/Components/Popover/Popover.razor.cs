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

public partial class Popover : DrComponentBase, IAsyncDisposable
{
    [Inject] IJSRuntime JS { get; set; } = null!;

    [Parameter] public Func<ElementReference>? GetControl { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; } = default!;

    [Parameter] public bool Open { get; set; } = false;
    [Parameter] public EventCallback<bool> OpenChanged { get; set; }

    [Parameter] public bool Dismissible { get; set; } = false;
    [Parameter] public bool DisableArrow { get; set; } = false;

    [Parameter] public bool FlipToFit { get; set; } = true;
    [Parameter] public Direction Direction { get; set; } = Direction.Top;
    [Parameter] public int Margin { get; set; } = 2;

    public ElementReference Ref { get; set; }

    private Dictionary<string, object> _attributes =>
        new AttrBuilder()
        .AddStyle("position", "absolute")
        .AddStyle("width", "fit-content")
        .AddStyle("display", Open ? "block" : "none")
        .AddData("data-dr-popover-id", Id)
        .AddData("data-dr-popover-visible", Open)
        .AddData("data-dr-popover-arrow", !DisableArrow)
        .Build();

    private readonly string _id = Guid.NewGuid().ToString();
    public string Id => _id;

    private bool _open;

    protected override async Task OnInitializedAsync()
    {
        _open = Open;
        await base.OnInitializedAsync();
    }

    protected override void OnParametersSet()
    {
        if (_open != Open)
        {
            _open = Open;
        }
    }

    IJSObjectReference? module;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (GetControl is null)
        {
            throw new NullReferenceException(nameof(GetControl));
        }

        if (firstRender)
        {
            module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/DrBlazor/Popover/popover.js");

            var reference = DotNetObjectReference.Create(this);
            await module.InvokeVoidAsync("initializeWindowResizeObserver", reference);
        }
        await Redraw();
        await base.OnAfterRenderAsync(firstRender);
    }

    public async Task Redraw()
    {
        if (Open)
        {
            await module!.InvokeVoidAsync(
                "updatePosition",
                Ref,
                GetControl!(),
                Direction,
                FlipToFit,
                Margin)
                .ConfigureAwait(false);
        }
    }

    [JSInvokable]
    public async Task WindowResized() => await Redraw();

    public void Close()
    {
        if (_open)
        {
            Open = _open = false;
            OpenChanged.InvokeAsync(_open).ConfigureAwait(false);
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
        catch (JSException) { }
    }
}