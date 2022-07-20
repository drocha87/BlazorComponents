using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class DrButtonText : DrComponentBase
{
    [Parameter] public string Text { get; set; } = null!;
    [Parameter] public EventCallback OnClick { get; set; }

    private Dictionary<string, object> _attributes =>
        new AttrBuilder()
        .AddClass("label-large")
        .AddStyle("color", "var(--md-sys-color-primary)")
        .Build();

    public void OnClickHandler()
    {
        OnClick.InvokeAsync();
    }
}