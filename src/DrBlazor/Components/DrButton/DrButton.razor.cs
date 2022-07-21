using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DrBlazor;

public partial class DrButton : DrComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
    [Parameter] public bool Disabled { get; set; }

    private Dictionary<string, object> Attributes =>
        new AttrBuilder()
            .AddClass("dr-button-root")
            .AddClass("dr-button-regular")
            .AddClass("dr-button-filled")
            .AddClass("dr-text-label-large")

            .AddData("disabled", Disabled)
            .Build();
}