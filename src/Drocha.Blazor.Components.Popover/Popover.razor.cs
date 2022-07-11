using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Drocha.Blazor.Components.Popover;

public partial class Popover : ComponentBase, IAsyncDisposable
{
    [Inject] IJSRuntime JS { get; set; } = default!;
    [Inject] PopoverService PopoverSvc { get; set; } = null!;

    [Parameter] public ElementReference TargetElement { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; } = default!;
    [Parameter] public bool Open { get; set; } = false;

    public ElementReference RootElement { get; set; }

    public string Class => "drocha-popover-" + (Open ? "open" : "close");

    private readonly string _id = Guid.NewGuid().ToString();
    public string Id => $"popover-{_id}";

    IJSObjectReference? module;

    protected override async Task OnInitializedAsync()
    {
        module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/Drocha.Blazor.Components.Popover/popover.js");
        await base.OnInitializedAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        PopoverSvc.AddFragment(_id, ChildContent);
        await InvokeAsync(StateHasChanged);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (module is not null)
            {
                await module.InvokeVoidAsync("popover.initialize", RootElement, TargetElement);
            }
        }
        await base.OnAfterRenderAsync(firstRender);
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