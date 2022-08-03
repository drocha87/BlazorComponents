using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DrBlazor;

public partial class DrDialog : DrComponentBase, IAsyncDisposable
{
    [Inject] IJSRuntime JS { get; set; } = null!;

    [Parameter] public RenderFragment ChildContent { get; set; } = default!;
    [Parameter] public bool Dismissible { get; set; } = true;

    private ElementReference _ref;

    private Dictionary<string, object> _attributes =>
        new AttrBuilder()
        // material design classes
        .AddClass("dr-dialog")

        // styles
        .AddStyle("padding", "24px")

        // data attributes
        .AddData("data-dr-dialog-id", Id)
        .Build();

    private bool _open;
    public bool IsVisible => _open;

    IJSObjectReference? module;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/DrBlazor/Components/DrDialog/dialog.js");

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
            await module!.InvokeVoidAsync("updatePosition", _ref).ConfigureAwait(false);
        }
    }

    public async Task CloseAsync()
    {
        _open = false;
        await InvokeAsync(StateHasChanged);
    }

    public async Task ShowAsync()
    {
        _open = true;
        await InvokeAsync(StateHasChanged);
    }

    [JSInvokable]
    public async Task WindowResized() => await Redraw();

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
        GC.SuppressFinalize(this);
    }
}