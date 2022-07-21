using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class DrButtonText : DrComponentBase
{
    [Parameter] public string Text { get; set; } = null!;
    [Parameter] public EventCallback OnClick { get; set; }

    private Dictionary<string, object> _attributes =>
        new AttrBuilder()
        .AddClass("dr-button-root")
        .AddClass("dr-button-regular")
        .AddClass("dr-button-text")
        .AddClass("dr-text-label-large")

        .AddClasses(Class)
        .Build();

    public void OnClickHandler()
    {
        OnClick.InvokeAsync();
    }
}