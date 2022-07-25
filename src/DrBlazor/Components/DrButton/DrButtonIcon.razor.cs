using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DrBlazor;

public partial class DrButtonIcon : DrComponentBase
{
    [Parameter] public string Icon { get; set; } = null!;
    [Parameter] public bool Disabled { get; set; }

    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

     private Dictionary<string, object> Attributes =>
        new AttrBuilder()
            .AddClass("dr-button-root")
            .AddClass("dr-button-icon")

            .AddData("disabled", Disabled)
            .Build();
}