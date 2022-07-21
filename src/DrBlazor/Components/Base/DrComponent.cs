using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public abstract class DrComponentBase : ComponentBase, IDisposable
{
    [Inject] public DrConfig Config { get; set; } = null!;

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

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            RefChanged.InvokeAsync(Ref);
        }
        base.OnAfterRender(firstRender);
    }

    public void Dispose()
    {
        Config.ConfigChanged -= ConfigChangedHandler;
    }
}