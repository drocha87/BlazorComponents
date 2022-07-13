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
                    el.Margin);
            }
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task UpdateContentHandler(object o, Popover el)
    {
        await InvokeAsync(StateHasChanged);
    }

    private void CloseDismissible(Popover el)
    {
        el.Close();
    }

    [JSInvokable]
    public async Task WindowResized()
    {
        if (PopoverSvc.VisibleElements > 0)
        {
            var fragments = PopoverSvc.Fragments.Where(x => x.Value.Open == true);

            foreach (var fragment in fragments)
            {
                var el = fragment.Value;
                await module!.InvokeVoidAsync(
                   "updatePosition",
                   el.Ref,
                   el.GetControl!(),
                   el.Direction,
                   el.FlipToFit,
                   el.Margin);
            }
        }
    }

    public void Dispose()
    {
        PopoverSvc.OnChanged -= UpdateContentHandler;
    }
}