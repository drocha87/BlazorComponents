using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Drocha.Blazor.Components.Popover;

public partial class PopoverProvider : ComponentBase, IDisposable
{
    [Inject] IJSRuntime JS { get; set; } = null!;
    [Inject] PopoverService PopoverSvc { get; set; } = null!;

    private readonly Dictionary<string, object> _attributes = new();

    protected override void OnInitialized()
    {
        PopoverSvc.OnChanged += UpdateContentHandler;
    }

    IJSObjectReference? module;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/Drocha.Blazor.Components.Popover/popover.js");

            var reference = DotNetObjectReference.Create(this);
            await module.InvokeVoidAsync("initializeWindowResizeObserver", reference);
        }

        await RedrawPopovers();
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task RedrawPopovers()
    {
        foreach (var fragment in PopoverSvc.Fragments)
        {
            var el = fragment.Value;
            if (el.Open)
            {
                await module!.InvokeVoidAsync(
                    "updatePosition",
                    el.Ref,
                    el.GetControl!(),
                    el.Direction,
                    el.FlipToFit,
                    el.Margin)
                    .ConfigureAwait(false);
            }
        }
    }

    private async Task UpdateContentHandler(object o, Popover el)
    {
        await InvokeAsync(StateHasChanged);
    }

    private void CloseDismissible(Popover el) => el.Close();

    [JSInvokable]
    public async Task WindowResized() => await RedrawPopovers();

    public void Dispose()
    {
        PopoverSvc.OnChanged -= UpdateContentHandler;
    }
}