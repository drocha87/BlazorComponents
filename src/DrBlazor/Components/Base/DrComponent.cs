using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DrBlazor;

public record struct WindowSize(double Width, double Height);

public abstract class DrComponentBase : ComponentBase, IDisposable
{
    [Inject] public DrConfig Config { get; set; } = null!;
    [Inject] protected IJSRuntime JS { get; set; } = null!;

    [Parameter] public ElementReference Ref { get; set; }
    [Parameter] public EventCallback<ElementReference> RefChanged { get; set; }

    [Parameter] public string? Id { get; set; }

    [Parameter] public string? Style { get; set; }
    [Parameter] public string? Class { get; set; }

    [Parameter] public int? Width { get; set; }
    [Parameter] public int? MinWidth { get; set; }
    [Parameter] public int? MaxWidth { get; set; }

    [Parameter] public int? Height { get; set; }
    [Parameter] public int? MinHeight { get; set; }
    [Parameter] public int? MaxHeight { get; set; }

    [Parameter] public Dictionary<string, object>? UserAttributes { get; set; }

    protected override void OnInitialized()
    {
        Config.ConfigChanged += ConfigChangedHandler;
    }

    public async void ConfigChangedHandler(object sender, DrConfig config)
    {
        await InvokeAsync(StateHasChanged);
    }

    private IJSObjectReference? _jsModule;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await RefChanged.InvokeAsync(Ref);
            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/DrBlazor/Components/Base/base.js");
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    protected async Task<WindowSize> GetWindowInnerSize()
    {
        if (_jsModule is IJSObjectReference js)
        {
            var result = await js.InvokeAsync<WindowSize>("getWindowInnerSize");
            return result;
        }
        throw new NullReferenceException(nameof(_jsModule));
    }

    public void Dispose()
    {
        Config.ConfigChanged -= ConfigChangedHandler;
    }
}