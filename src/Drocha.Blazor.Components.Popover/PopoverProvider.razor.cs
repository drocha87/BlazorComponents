using Microsoft.AspNetCore.Components;

namespace Drocha.Blazor.Components.Popover;

public partial class PopoverProvider : ComponentBase, IDisposable
{
    [Inject] PopoverService PopoverSvc { get; set; } = null!;

    protected override void OnInitialized()
    {
        PopoverSvc.OnChanged += UpdateContentHandler;
        Console.WriteLine("initialized");
    }

    private void UpdateContentHandler(object o, RenderFragment f)
    {
        InvokeAsync(StateHasChanged).ConfigureAwait(false);
    }

    public void Dispose()
    {
        PopoverSvc.OnChanged -= UpdateContentHandler;
    }
}