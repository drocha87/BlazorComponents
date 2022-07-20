using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DrBlazor;

public partial class DrDialog : DrComponentBase, IAsyncDisposable
{
    [Inject] IJSRuntime JS { get; set; } = null!;

    [Parameter] public RenderFragment ChildContent { get; set; } = default!;

    [Parameter] public bool Open { get; set; } = false;
    [Parameter] public EventCallback<bool> OpenChanged { get; set; }

    [Parameter] public bool Dismissible { get; set; } = true;

    public ElementReference Ref { get; set; }

    private Dictionary<string, object> _attributes =>
        new AttrBuilder()
        // material design classes
        .AddClass("shape-medium")
        .AddClass("surface")
        .AddClass("elevation-3")
        // styles
        .AddStyle("position", "absolute")
        .AddStyle("width", "fit-content")
        .AddStyle("padding", "24px")
        // data attributes
        .AddData("data-dr-dialog-id", Id)
        .AddData("data-dr-dialog-visible", Open)
        .AddData("data-dr-dialog-theme", Config.Theme)
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
        if (firstRender)
        {
            module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/DrBlazor/DrDialog/dialog.js");

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
            await module!.InvokeVoidAsync("updatePosition", Ref).ConfigureAwait(false);
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
        catch (TaskCanceledException) { }
    }
}